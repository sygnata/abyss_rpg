param(
    [string]$BaseBranch = "develop",
    [int]$Major = 0,
    [switch]$Preview,
    [switch]$Push
)

$ErrorActionPreference = "Stop"

function Get-GitOutput {
    param(
        [Parameter(Mandatory = $true)]
        [string[]]$Arguments
    )

    $processStartInfo = New-Object System.Diagnostics.ProcessStartInfo
    $processStartInfo.FileName = "git"
    $processStartInfo.Arguments = ($Arguments | ForEach-Object {
        if ($_ -match '\s') {
            '"' + $_.Replace('"', '\"') + '"'
        }
        else {
            $_
        }
    }) -join " "
    $processStartInfo.RedirectStandardOutput = $true
    $processStartInfo.RedirectStandardError = $true
    $processStartInfo.UseShellExecute = $false
    $processStartInfo.CreateNoWindow = $true

    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $processStartInfo

    [void]$process.Start()

    $standardOutput = $process.StandardOutput.ReadToEnd()
    $standardError = $process.StandardError.ReadToEnd()

    $process.WaitForExit()

    if ($process.ExitCode -ne 0) {
        $combinedOutput = @($standardOutput, $standardError) -join [Environment]::NewLine
        throw $combinedOutput.Trim()
    }

    if ([string]::IsNullOrWhiteSpace($standardOutput)) {
        return @()
    }

    return $standardOutput -split "\r?\n" | Where-Object { $_ -ne "" }
}

function Get-NextReleaseBranchName {
    param(
        [Parameter(Mandatory = $true)]
        [int]$ReleaseMajor
    )

    $pattern = '^release/(?<major>\d+)\.(?<minor>\d+)$'

    $references = @()
    $references += Get-GitOutput -Arguments @("for-each-ref", "--format=%(refname:short)", "refs/heads", "refs/remotes/origin")

    $matchingBranches =
        $references |
        Where-Object { $_ -match $pattern } |
        ForEach-Object {
            [PSCustomObject]@{
                Name = $_
                Major = [int]$Matches["major"]
                Minor = [int]$Matches["minor"]
            }
        } |
        Where-Object { $_.Major -eq $ReleaseMajor }

    if (-not $matchingBranches) {
        return "release/$ReleaseMajor.1"
    }

    $nextMinor =
        ($matchingBranches | Measure-Object -Property Minor -Maximum).Maximum + 1

    return "release/$ReleaseMajor.$nextMinor"
}

Get-GitOutput -Arguments @("fetch", "origin", $BaseBranch) | Out-Null

$releaseBranch = Get-NextReleaseBranchName -ReleaseMajor $Major

if ($Preview.IsPresent) {
    Write-Output $releaseBranch
    exit 0
}

$status = Get-GitOutput -Arguments @("status", "--porcelain")
if ($status.Count -gt 0) {
    throw "Working tree is not clean. Commit or stash your changes before creating a release branch."
}

Get-GitOutput -Arguments @("switch", "--create", $releaseBranch, "origin/$BaseBranch") | Out-Null

if ($Push.IsPresent) {
    Get-GitOutput -Arguments @("push", "--set-upstream", "origin", $releaseBranch) | Out-Null
}

Write-Output $releaseBranch
