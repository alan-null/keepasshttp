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

    Context "TEST_ASSOCIATE" {
        It "returns success for test-associate request" {
            $r = Invoke-TestAssociate -Context $Context
            $r.Success | Should -BeTrue
        }
    }

    Context "ASSOCIATE" {
        It "fails test-associate with invalid key" {
            $badCtx = New-KphContext -Key "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=" -Id $Id -Endpoint $Endpoint
            $r = Invoke-TestAssociate -Context $badCtx
            $r.Success | Should -BeFalse
        }
    }

    Context "GET_LOGINS" {
        It "returns no entries for unknown host" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.doesnotexist.com/"
            $r.Entries.Count | Should -Be 0
        }

        It "matches entry by partial title (google)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.google.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "google-user"
        }

        It "matches entry by exact title (www.yahoo.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.yahoo.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "www.yahoo-user"
        }

        It "matches entry by partial title (yahoo.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://yahoo.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "yahoo-user"
        }

        It "matches entry via URL field (citi.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://citi.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "citi-user"
        }

        It "matches entry via URL field (citi1.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://citi1.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "citi1-user"
        }

        It "matches entry by title and URL field (cititest.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "https://cititest.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "cititest-user"
        }

        It "matches entry where title and URL differ (bogustest.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "https://bogustest.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest-user"
        }

        It "matches entry via URL where title and URL differ (www.bogustest.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "https://www.bogustest.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest-user"
        }

        It "matches entry where title and URL differ (bogustest1.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "https://bogustest1.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest1-user"
        }

        It "matches entry via URL where title and URL differ (www.bogustest1.com)" {
            $r = Invoke-GetLogins -Context $Context -Url "https://www.bogustest1.com/"
            $r.Entries.Count | Should -Be 1
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "bogustest1-user"
        }

        It "matches first subpath (user1)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.host.com/" -SubmitUrl "http://www.host.com/path1"
            $r.Entries.Count | Should -Be 2
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "user1"
        }

        It "matches second subpath (user2)" {
            $r = Invoke-GetLogins -Context $Context -Url "http://www.host.com" -SubmitUrl "http://www.host.com/path2?param=value"
            $r.Entries.Count | Should -Be 2
            (Unprotect-Field -Context $Context -Cipher $r.Entries[0].Login -Nonce $r.Nonce) | Should -Be "user2"
        }
    }

    Context "GET_LOGINS_COUNT" {
        It "get-logins-count equals get-logins count for google.com" {
            $logins = Invoke-GetLogins -Context $Context -Url "http://www.google.com/"
            $count = Invoke-GetLoginsCount -Context $Context -Url "http://www.google.com/"
            $count.Count | Should -Be $logins.Entries.Count
        }

        It "get-logins-count equals get-logins count for multi-entry host www.host.com (2)" {
            $logins = Invoke-GetLogins -Context $Context -Url "http://www.host.com/"
            $count = Invoke-GetLoginsCount -Context $Context -Url "http://www.host.com/"
            $count.Count | Should -Be 2
            $count.Count | Should -Be $logins.Entries.Count
        }

        It "get-logins-count returns 0 for unknown host" {
            $logins = Invoke-GetLogins -Context $Context -Url "http://no-such-host.example/"
            $count = Invoke-GetLoginsCount -Context $Context -Url "http://no-such-host.example/"
            $logins.Entries.Count | Should -Be 0
            $count.Count | Should -Be 0
        }
    }

    Context "GET_ALL_LOGINS" {
        It "get-all-logins returns all expected entries (includes google-user)" {
            $all = Invoke-GetAllLogins -Context $Context
            $all.Entries.Count | Should -BeExactly 11
            $found = $false
            foreach ($e in $all.Entries) {
                $name = Unprotect-Field -Context $Context -Cipher $e.Name -Nonce $all.Nonce
                if ($name -like "*google*") {
                    $login = Unprotect-Field -Context $Context -Cipher $e.Login -Nonce $all.Nonce
                    if ($login -eq "google-user") { $found = $true; break }
                }
            }
            $found | Should -BeTrue
        }
    }

    Context "SET_LOGIN" {
        It "set-login creates a new entry" {
            $domain = [guid]::NewGuid().ToString("n") + ".example"
            $username = "new_user_$domain"
            $password = "new_pass_$domain"

            $create = Invoke-SetLogin -Context $Context -Url "https://$domain/path/page" -Login $username -Password $password
            $create.Success | Should -BeTrue
            $lookup = Invoke-GetLogins -Context $Context -Url "https://$domain/"
            $lookup.Entries | % {
                (Unprotect-Field -Context $Context -Cipher $_.Login -Nonce $lookup.Nonce) -eq $username
                (Unprotect-Field -Context $Context -Cipher $_.Password -Nonce $lookup.Nonce) -eq $password
                (Unprotect-Field -Context $Context -Cipher $_.Name -Nonce $lookup.Nonce) -eq $domain
            }
            $lookup.Entries.Count | Should -Be 1
        }

        It "set-login updates an existing entry" {
            $domain = [guid]::NewGuid().ToString("n") + ".example"
            $username = "new_user_$domain"
            $password = "new_pass_$domain"

            # Create initial entry
            $create = Invoke-SetLogin -Context $Context -Url "https://$domain/path/page" -Login $username -Password $password
            $create.Success | Should -BeTrue

            # fetch Uuid
            $lookup = Invoke-GetLogins -Context $Context -Url "https://$domain/"
            $uuid = Unprotect-Field -Context $Context -Cipher $lookup.Entries[0].Uuid -Nonce $lookup.Nonce

            # Update entry
            $newUsername = "updated_user_$domain"
            $newPassword = "updated_pass_$domain"
            $update = Invoke-SetLogin -Context $Context -Url "https://$domain/path/page" -Login $newUsername -Password $newPassword -Uuid $uuid
            $update.Success | Should -BeTrue

            # Verify update
            $lookup = Invoke-GetLogins -Context $Context -Url "https://$domain/"
            $lookup.Entries.Count | Should -Be 1
            $lookup.Entries | % {
                (Unprotect-Field -Context $Context -Cipher $_.Login -Nonce $lookup.Nonce) -eq $newUsername
                (Unprotect-Field -Context $Context -Cipher $_.Password -Nonce $lookup.Nonce) -eq $newPassword
                (Unprotect-Field -Context $Context -Cipher $_.Name -Nonce $lookup.Nonce) -eq $domain
            }
        }
    }

    Context "GENERATE_PASSWORD" {
        It "generate-password returns one entry with entropy bits" {
            $gen = Invoke-GeneratePassword -Context $Context
            $gen.Success | Should -BeTrue
            $gen.Entries.Count | Should -Be 1
            $gen.Id | Should -Be $Context.Id
            $loginDec = Unprotect-Field -Context $Context -Cipher $gen.Entries[0].Login -Nonce $gen.Nonce
            $passDec = Unprotect-Field -Context $Context -Cipher $gen.Entries[0].Password -Nonce $gen.Nonce
            $nameDec = Unprotect-Field -Context $Context -Cipher $gen.Entries[0].Name -Nonce $gen.Nonce
            [int]::TryParse($loginDec, [ref]$null) | Should -BeTrue   # login holds bits
            $passDec.Length | Should -BeGreaterThan 0
            $nameDec | Should -Be "generate-password"
        }
    }
}