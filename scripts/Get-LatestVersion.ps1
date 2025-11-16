param (
    [string]$LatestVersionPath = ""
)

$fileExist = Test-Path -Path $LatestVersionPath -ErrorAction Stop
if (-not $fileExist) {
    Write-Error "The specified file does not exist: $LatestVersionPath"
    exit 1
}

$content = Get-Content -Path $LatestVersionPath -Encoding UTF8

if ($content.Count -gt 0) {
    $firstLine = $content[0]

    # Extract the separator character used in the first line
    $separator = $firstLine.Trim()
    $separator = $separator[0]

    $plugins = @()
    # Iterate through each line in the content (starting from the second line)
    for ($i = 1; $i -lt $content.Count; $i++) {
        $line = $content[$i]

        if ($line -match "^(.*?${separator})(.*)$") {
            $pluginName = $matches[1].TrimEnd($separator)
            $version = $matches[2].Trim()

            $plugin = [PSCustomObject]@{
                Name    = $pluginName
                Version = $version
            }

            $plugins += $plugin
        }
    }
    $plugins
}
else {
    Write-Output "The file is empty."
}