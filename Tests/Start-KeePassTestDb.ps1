# Ensure KeePass test instance is running with --kph-ev-config.
$keePassExe = Join-Path "${env:ProgramFiles(x86)}\KeePass Password Safe 2" "KeePass.exe"
$dbPath = (Get-Item .\test\test.kdbx).FullName

function Start-KeePassTest {
    param(
        [hashtable]$Environment = @{}
    )
    if (-not $Environment.ContainsKey("KPH_AlwaysAllowUpdates")) {
        $Environment["KPH_AlwaysAllowUpdates"] = "1"
    }
    if (-not $Environment.ContainsKey("KPH_ReceiveCredentialNotification")) {
        $Environment["KPH_ReceiveCredentialNotification"] = "0"
    }

    $envStrings = $Environment.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }
    Write-Host "Starting KeePass test instance... @{$($envStrings -join '; ')}"

    Start-Process -FilePath $keePassExe `
        -ArgumentList "$dbPath", "-pw:test", "--kph-ev-config" `
        -WindowStyle Minimized `
        -Environment $Environment
}

function Restart-KeePassTest {
    param(
        [hashtable]$Environment = @{ "KPH_AlwaysAllowUpdates" = "1" }
    )
    $existing = Get-CimInstance Win32_Process -Filter "Name='KeePass.exe'" -ErrorAction SilentlyContinue
    if ($existing) {
        foreach ($proc in $existing) {
            try {
                Stop-Process -Id $proc.ProcessId -Force -ErrorAction Stop
            }
            catch {
                Write-Warning "Failed to stop KeePass (PID $($proc.ProcessId)): $_"
            }
        }
        Start-Sleep -Milliseconds 200
    }
    Start-KeePassTest -Environment $Environment
}

# Get existing KeePass processes (if any)
$existing = Get-CimInstance Win32_Process -Filter "Name='KeePass.exe'" -ErrorAction SilentlyContinue

if (-not $existing) {
    Start-KeePassTest
    return
}

# Check if any existing process was started with the required flag
$hasTestConfig = $false
foreach ($proc in $existing) {
    if ($proc.CommandLine -match '--kph-ev-config') {
        $hasTestConfig = $true
        break
    }
}

if ($hasTestConfig) {
    Write-Host "KeePass test instance already running with --kph-ev-config."
    return
}

# No process has the test config; ask user before killing
Write-Host "KeePass is running without --kph-ev-config."
$answer = Read-Host "Kill existing KeePass process(es) and restart with test config? (y/n)"
if ($answer -match '^[Yy]') {
    foreach ($proc in $existing) {
        try {
            Stop-Process -Id $proc.ProcessId -Force -ErrorAction Stop
            Write-Host "Stopped KeePass (PID $($proc.ProcessId))."
        }
        catch {
            Write-Warning "Failed to stop KeePass (PID $($proc.ProcessId)): $_"
        }
    }
    Start-KeePassTest
}
else {
    Write-Host "Leaving existing KeePass instance running."
}