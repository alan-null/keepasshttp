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
        [Parameter(Mandatory)][AllowEmptyString()][string]$PlainText,
        [Parameter(Mandatory)][string]$Nonce
    )
    if ($null -eq $PlainText) { $PlainText = '' }
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

function Invoke-GetLoginsByName {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [Parameter(Mandatory)][string[]]$Names,
        [string]$SubmitUrl,
        [string]$Realm
    )
    $p = New-VerifierPair -Context $Context
    $req = @{
        RequestType = "get-logins-by-names"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
        Names       = @()
    }
    if ($SubmitUrl) { $req.SubmitUrl = Protect-Field -Context $Context -PlainText $SubmitUrl -Nonce $p.Nonce }
    if ($Realm) { $req.Realm = Protect-Field -Context $Context -PlainText $Realm     -Nonce $p.Nonce }
    $Names | ForEach-Object {
        $protectedName = Protect-Field -Context $Context -PlainText $_ -Nonce $p.Nonce
        $req.Names += $protectedName
    }
    $json = $req | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $json
}

function Invoke-GetLoginByUuid {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [Parameter(Mandatory)][string]$Uuid
    )

    $p = New-VerifierPair -Context $Context
    $body = @{
        RequestType = "get-login-by-uuid"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
        Uuid        = Protect-Field -Context $Context -PlainText $Uuid -Nonce $p.Nonce
    } | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $body
}

function Invoke-GetLoginsCount {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [Parameter(Mandatory)][string]$Url
    )
    $p = New-VerifierPair -Context $Context
    $body = @{
        RequestType = "get-logins-count"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
        Url         = Protect-Field -Context $Context -PlainText $Url -Nonce $p.Nonce
    } | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $body
}

function Invoke-GeneratePassword {
    param(
        [Parameter(Mandatory)][psobject]$Context
    )
    $p = New-VerifierPair -Context $Context
    $body = @{
        RequestType = "generate-password"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
    } | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $body
}

function Invoke-GetAllLogins {
    param(
        [Parameter(Mandatory)][psobject]$Context
    )
    $p = New-VerifierPair -Context $Context
    $body = @{
        RequestType = "get-all-logins"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
    } | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $body
}

function Invoke-SetLogin {
    param(
        [Parameter(Mandatory)][psobject]$Context,
        [string]$Name,
        [string]$Login,
        [string]$Password,
        [string]$Url,
        [string]$Uuid,
        [string]$SubmitUrl,
        [string]$Realm,
        $StringFields
    )
    $p = New-VerifierPair -Context $Context
    $body = @{
        RequestType = "set-login"
        Id          = $Context.Id
        Nonce       = $p.Nonce
        Verifier    = $p.Verifier
    }
    if ($Name)      { $body.Name        = Protect-Field -Context $Context -PlainText $Name      -Nonce $p.Nonce }
    if ($Login)     { $body.Login       = Protect-Field -Context $Context -PlainText $Login     -Nonce $p.Nonce }
    if ($Password)  { $body.Password    = Protect-Field -Context $Context -PlainText $Password  -Nonce $p.Nonce }
    if ($Url)       { $body.Url         = Protect-Field -Context $Context -PlainText $Url       -Nonce $p.Nonce }
    if ($Uuid)      { $body.Uuid        = Protect-Field -Context $Context -PlainText $Uuid      -Nonce $p.Nonce }
    if ($SubmitUrl) { $body.SubmitUrl   = Protect-Field -Context $Context -PlainText $SubmitUrl -Nonce $p.Nonce }
    if ($Realm)     { $body.Realm       = Protect-Field -Context $Context -PlainText $Realm     -Nonce $p.Nonce }
    if ($StringFields){
        $protectedStringFields = @{}
        foreach ($key in $StringFields.Keys) {
            $protectedKey   = Protect-Field -Context $Context -PlainText $key                   -Nonce $p.Nonce
            if ($null -eq $StringFields[$key]) {
                $protectedStringFields[$protectedKey] = $null
            }else {
                $protectedValue = Protect-Field -Context $Context -PlainText $StringFields[$key]    -Nonce $p.Nonce
                $protectedStringFields[$protectedKey] = $protectedValue
            }
        }
        $body.StringFields = $protectedStringFields
    }
    $json = $body | ConvertTo-Json
    Invoke-RestMethod -Uri $Context.Endpoint -Method Post -ContentType "application/json" -Body $json
}

Export-ModuleMember -Function `
    New-KphContext, New-Nonce, New-VerifierPair, `
    Invoke-TestAssociate, Invoke-GetLogins, Invoke-GetLoginsByName, Invoke-GetLoginByUuid, Invoke-GetLoginsCount, Invoke-GetAllLogins, Invoke-GeneratePassword, Invoke-SetLogin, `
    Protect-Field, Unprotect-Field
