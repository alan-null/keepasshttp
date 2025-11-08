# Requires: KeePass running with KeePassHttp, test.kdbx unlocked
# Start / validate KeePass test instance (ensures --kph-ev-config)
. "$PSScriptRoot\Start-KeePassTestDb.ps1"

# wait for KeePassHttp endpoint to respond before starting tests
$Endpoint = "http://localhost:19455"
$maxWaitSeconds = 30
$sw = [Diagnostics.Stopwatch]::StartNew()
while ($sw.Elapsed.TotalSeconds -lt $maxWaitSeconds) {
    try {
        Invoke-WebRequest -Uri $Endpoint -Method Post -Body '{"RequestType": "test-associate"}' -UseBasicParsing -TimeoutSec 2 | Out-Null
        break
    }
    catch {
        Start-Sleep -Milliseconds 500
    }
}
if ($sw.Elapsed.TotalSeconds -ge $maxWaitSeconds) {
    Write-Warning "KeePassHttp endpoint $Endpoint not reachable after $maxWaitSeconds seconds."
}

Import-Module Pester -ErrorAction Stop
Import-Module (Join-Path $PSScriptRoot 'KeePassHttp.psm1') -Force

Describe "KeePassHttp protocol" {
    BeforeAll {
        $Id = "Test Key"
        $Key = "Lgh8xMEkV2j10bG7O42GjCibsUEpM80T7Db+skKGiNc="
        $Endpoint = "http://localhost:19455"
        $Context = New-KphContext -Key $Key -Id $Id -Endpoint $Endpoint
    }

    It "associates (test-associate ok)" {
        $r = Invoke-TestAssociate -Context $Context
        $r.Success | Should -BeTrue
    }

    Context "get-logins" {

        It "returns empty for unknown host" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.doesnotexist.com/"
            $r.Entries.Count | Should -Be 0
        }

        It "matches partial title google" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.google.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "google-user"
        }

        It "matches exact title www.yahoo.com" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.yahoo.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "www.yahoo-user"
        }

        It "matches partial title yahoo.com" {
            $r = Invoke-GetLogins -Context $Context -Url "http://yahoo.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "yahoo-user"
        }

        It "matches host via URL field citi.com" {
            $r = Invoke-GetLogins -Context $Context -Url "http://citi.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "citi-user"
        }

        It "matches real URL field citi1.com" {
            $r = Invoke-GetLogins -Context $Context -Url "http://citi1.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "citi1-user"
        }

        It "matches title and URL field cititest.com" {
            $r = Invoke-GetLogins -Context $Context -Url "https://cititest.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "cititest-user"
        }

        It "matches title urltitle mismatch bogustest.com" {
            $r = Invoke-GetLogins -Context $Context -Url "https://bogustest.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest-user"
        }

        It "matches url urltitle mismatch www.bogustest.com" {
            $r = Invoke-GetLogins -Context $Context -Url "https://www.bogustest.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest-user"
        }

        It "matches title urltitle mismatch2 bogustest1.com" {
            $r = Invoke-GetLogins -Context $Context -Url "https://bogustest1.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest1-user"
        }

        It "matches url urltitle mismatch2 www.bogustest1.com" {
            $r = Invoke-GetLogins -Context $Context -Url "https://www.bogustest1.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest1-user"
        }

        It "matches subpath user1" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.host.com/" -SubmitUrl "http://www.host.com/path1"
            $r.Entries.Count | Should -Be 2
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "user1"
        }

        It "matches subpath user2" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.host.com" -SubmitUrl "http://www.host.com/path2?param=value"
            $r.Entries.Count | Should -Be 2
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "user2"
        }
    }
}