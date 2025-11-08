# Ensure KeePass test instance is running with --kph-ev-config.
$keePassExe = Join-Path "${env:ProgramFiles(x86)}\KeePass Password Safe 2" "KeePass.exe"
$dbPath = (Get-Item .\test\test.kdbx).FullName

function Start-KeePassTest {
    Write-Host "Starting KeePass test instance..."
    & $keePassExe $dbPath -pw:test --kph-ev-config
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
        } catch {
            Write-Warning "Failed to stop KeePass (PID $($proc.ProcessId)): $_"
        }
    }
    Start-KeePassTest
} else {
    Write-Host "Leaving existing KeePass instance running."
}