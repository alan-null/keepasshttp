Set-StrictMode -Version Latest

function New-Nonce {
    $bytes = New-Object byte[] 16
    [Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)
    [Convert]::ToBase64String($bytes)
}

function New-Aes {
    param(
        [Parameter(Mandatory)][string]$Key,
        [Parameter(Mandatory)][string]$Nonce
    )
    $aes = [Security.Cryptography.Aes]::Create()
    $aes.Mode    = 'CBC'
    $aes.Padding = 'PKCS7'
    $aes.Key     = [Convert]::FromBase64String($Key)
    $aes.IV      = [Convert]::FromBase64String($Nonce)
    $aes
}

function New-KphContext {
    [CmdletBinding()]
    param(
        [Parameter(Mandatory)][string]$Key,
        [Parameter(Mandatory)][string]$Id,
        [Parameter(Mandatory)][string]$Endpoint
    )
    [pscustomobject]@{
        PSTypeName = 'KeePassHttp.Context'
        Key       = $Key
        Id        = $Id
        Endpoint  = $Endpoint
    }
}

function Protect-Field {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [Parameter(Mandatory)][string]$PlainText,
        [Parameter(Mandatory)][string]$Nonce
    )
    $aes = New-Aes -Key $Context.Key -Nonce $Nonce
    $enc = $aes.CreateEncryptor()
    try {
        $bytes  = [Text.Encoding]::UTF8.GetBytes($PlainText)
        $cipherBytes = $enc.TransformFinalBlock($bytes,0,$bytes.Length)
        [Convert]::ToBase64String($cipherBytes)
    } finally {
        $enc.Dispose(); $aes.Dispose()
    }
}

function Unprotect-Field {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [Parameter(Mandatory)][string]$Cipher,
        [Parameter(Mandatory)][string]$Nonce
    )
    $aes = New-Aes -Key $Context.Key -Nonce $Nonce
    $dec = $aes.CreateDecryptor()
    try {
        $cipherBytes = [Convert]::FromBase64String($Cipher)
        $plain  = $dec.TransformFinalBlock($cipherBytes,0,$cipherBytes.Length)
        [Text.Encoding]::UTF8.GetString($plain)
    } finally {
        $dec.Dispose(); $aes.Dispose()
    }
}

function New-VerifierPair {
    param(
        [Parameter(Mandatory)][psobject]$Context
    )
    $n = New-Nonce
    [pscustomobject]@{
        Nonce    = $n
        Verifier = Protect-Field -Context $Context -PlainText $n -Nonce $n
    }
}

function Invoke-TestAssociate {
    param(
        [Parameter(Mandatory)][psobject]$Context
    )
    $p = New-VerifierPair -Context $Context
    $body = @{
        RequestType = "test-associate"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
    } | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $body
}

function Invoke-GetLogins {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [Parameter(Mandatory)][string]$Url,
        [string]$SubmitUrl,
        [string]$Realm
    )
    $p = New-VerifierPair -Context $Context
    $req = @{
        RequestType = "get-logins"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
        Url         = Protect-Field -Context $Context -PlainText $Url -Nonce $p.Nonce
    }
    if ($SubmitUrl) { $req.SubmitUrl = Protect-Field -Context $Context -PlainText $SubmitUrl -Nonce $p.Nonce }
    if ($Realm)     { $req.Realm     = Protect-Field -Context $Context -PlainText $Realm     -Nonce $p.Nonce }
    $json = $req | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $json
}

Export-ModuleMember -Function `
    New-KphContext, New-Nonce, New-VerifierPair, `
    Invoke-TestAssociate, Invoke-GetLogins, `
    Protect-Field, Unprotect-Field
