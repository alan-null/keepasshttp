# ---------------------------------------------
#   LOCAL HTTPS SETUP FOR KEEPASSHTTP LISTENER
# ---------------------------------------------

$ErrorActionPreference = "Stop"

# --- CONFIG ---
$dns = "localhost"
$port = 19456
$friendlyName = "KeePassLocalHttpsCert"

$storeLocationRoot = "LocalMachine"
$storeNameRoot = "Root"

$storeLocationMy = "LocalMachine"
$storeNameMy = "My"

$prefix = "https://$dns`:$port/"

Write-Host "=============================================="
Write-Host "   HTTPS Setup for Local Secure Communication  "
Write-Host "=============================================="
Write-Host ""

# Ensure administrator
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {

    Write-Host "[ERROR] This script must be run as Administrator." -ForegroundColor Red
    exit 1
}

# ---------------------------------------------
# Step 1 — Check/create certificate
# ---------------------------------------------
Write-Host "🔍 Checking for existing certificate..." -ForegroundColor Cyan

$myStore = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeNameMy, $storeLocationMy)
$myStore.Open("ReadWrite")

$existingCert = $myStore.Certificates | ? { $_.FriendlyName -eq $friendlyName }

if ($existingCert) {
    Write-Host "✔ Found existing certificate: $friendlyName" -ForegroundColor Green
    $cert = $existingCert
}
else {
    Write-Host "⏳ Creating new self-signed certificate for '$dns'..." -ForegroundColor Yellow

    $cert = New-SelfSignedCertificate `
        -DnsName $dns, "127.0.0.1" `
        -FriendlyName $friendlyName `
        -CertStoreLocation "Cert:\$storeLocationMy\$storeNameMy" `
        -KeyExportPolicy Exportable `
        -NotAfter (Get-Date).AddYears(5)

    Write-Host "✔ Certificate created successfully!" -ForegroundColor Green
}

$myStore.Close()

# ---------------------------------------------
# Step 2 — Ensure certificate is trusted
# ---------------------------------------------
Write-Host ""
Write-Host "🔐 Ensuring certificate is trusted..." -ForegroundColor Cyan

$rootStore = New-Object System.Security.Cryptography.X509Certificates.X509Store($storeNameRoot, $storeLocationRoot)
$rootStore.Open("ReadWrite")

$alreadyInRoot = $rootStore.Certificates |
Where-Object { $_.Thumbprint -eq $cert.Thumbprint }

if ($alreadyInRoot) {
    Write-Host "✔ Certificate already trusted (Root store)." -ForegroundColor Green
}
else {
    Write-Host "⏳ Adding certificate to Trusted Root..." -ForegroundColor Yellow
    $rootStore.Add($cert)
    Write-Host "✔ Certificate trusted successfully!" -ForegroundColor Green
}

$rootStore.Close()

# ---------------------------------------------
# Step 3 — Clean old HTTPS bindings
# ---------------------------------------------
Write-Host ""
Write-Host "🧹 Cleaning old HTTPS bindings on port $port..." -ForegroundColor Cyan

try {
    netsh http delete sslcert ipport=0.0.0.0:$port | Out-Null
    Write-Host "✔ Old bindings removed." -ForegroundColor Green
}
catch {
    Write-Host "ℹ No old bindings found." -ForegroundColor DarkGray
}

# ---------------------------------------------
# Step 4 — Bind certificate to port
# ---------------------------------------------
Write-Host ""
Write-Host "🔗 Binding certificate to port $port..." -ForegroundColor Cyan

$thumb = $cert.Thumbprint
$appid = [guid]::NewGuid()

netsh http add sslcert `
    ipport=0.0.0.0:$port `
    certhash=$thumb `
    certstorename=$storeNameMy `
    appid="{$appid}" | Out-Null

Write-Host "✔ HTTPS binding added successfully!" -ForegroundColor Green

# ---------------------------------------------
# Step 5 — Summary
# ---------------------------------------------
Write-Host ""
Write-Host "=============================================="
Write-Host "              SETUP COMPLETE 🎉"
Write-Host "=============================================="
Write-Host ""
Write-Host "🔒 HTTPS is now enabled for your local server."
Write-Host ""
Write-Host "📜 Details:"
Write-Host " - Hostname:   $dns"
Write-Host " - Port:       $port"
Write-Host " - Prefix:     $prefix"
Write-Host " - Thumbprint: $thumb"
Write-Host " - Cert Name:  $friendlyName"
Write-Host ""
Write-Host "🧪 Test it in your browser:"
Write-Host "   $prefix" -ForegroundColor Yellow
Write-Host ""
Write-Host "✔ Your Chrome extension should now work without certificate errors."
Write-Host ""
Write-Host "=============================================="
Write-Host ""
