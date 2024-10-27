<#
    .SYNOPSIS 
    Modify XAML files to adhere to XAML Styler settings.

    .DESCRIPTION
    The Apply XAML Stying Script can be used to check or modify XAML files with the repo's XAML Styler settings.
    Learn more about XAML Styler at https://github.com/Xavalon/XamlStyler

    By default, uses git status to check all new or modified files.

    Use "PS> Help .\ApplyXamlStyling.ps1 -Full" for more details on parameters.

    .PARAMETER LastCommit
    Runs against last commit vs. current changes

    .PARAMETER Unstaged
    Runs against unstaged changed files

    .PARAMETER Staged
    Runs against staged files vs. current changes

    .PARAMETER Main
    Runs against main vs. current branch

    .PARAMETER Passive
    Runs a passive check against all files in the repo for the CI

    .EXAMPLE
    PS> .\ApplyXamlStyling.ps1 -Main
#>
param(
    [switch]$LastCommit = $false,
    [switch]$Unstaged = $false,
    [switch]$Staged = $false,
    [switch]$Main = $false,
    [switch]$Passive = $false
)

Write-Output "Use 'Help .\ApplyXamlStyling.ps1' for more info or '-Main' to run against all files."
Write-Output ""
Write-Output "Restoring dotnet tools..."
dotnet tool restore

if (-not $Passive)
{
    # Look for unstaged changed files by default
    $gitDiffCommand = "git status -s --porcelain"

    if ($Main)
    {
        Write-Output 'Checking Current Branch against `main` Files Only'
        $branch = git status | Select-String -Pattern "On branch (?<branch>.*)$"
        if ($null -eq $branch.Matches)
        {
            $branch = git status | Select-String -Pattern "HEAD detached at (?<branch>.*)$"
            if ($null -eq $branch.Matches)
            {
                Write-Error 'Don''t know how to fetch branch from `git status`:'
                git status | Write-Error
                exit 1
            }
        }
        $branch = $branch.Matches.groups[1].Value
        $gitDiffCommand = "git diff origin/main $branch --name-only --diff-filter=ACM"
    }
    elseif ($Unstaged)
    {
        # Look for unstaged files
        Write-Output "Checking Unstaged Files"
        $gitDiffCommand = "git diff --name-only --diff-filter=ACM"
    }
    elseif ($Staged)
    {
        # Look for staged files
        Write-Output "Checking Staged Files Only"
        $gitDiffCommand = "git diff --cached --name-only --diff-filter=ACM"
    }
    elseif ($LastCommit)
    {
        # Look at last commit files
        Write-Output "Checking the Last Commit's Files Only"
        $gitDiffCommand = "git diff HEAD^ HEAD --name-only --diff-filter=ACM"
    }
    else 
    {
        Write-Output "Checking Git Status Files Only"    
    }

    Write-Output "Running Git Diff: $gitDiffCommand"
    $files = Invoke-Expression $gitDiffCommand | Select-String -Pattern "\.xaml$"

    if (-not $Passive -and -not $Main -and -not $Unstaged -and -not $Staged -and -not $LastCommit)
    {
        # Remove 'status' column of 3 characters at beginning of lines
        $files = $files | ForEach-Object { $_.ToString().Substring(3) }
    }

    if ($files.count -gt 0)
    {
        dotnet tool run xstyler -c .\settings.xamlstyler -f $files
    }
    else
    {
        Write-Output "No XAML Files found to style..."
    }
}
else 
{
    Write-Output "Checking all files (passively)"
    $files = Get-ChildItem *.xaml -Recurse | Select-Object -ExpandProperty FullName | Where-Object { $_ -notmatch "(\\obj\\)|(\\bin\\)" }

    if ($files.count -gt 0)
    {
        dotnet tool run xstyler -p -c .\settings.xamlstyler -f $files

        if ($lastExitCode -eq 1)
        {
            Write-Error 'XAML Styling is incorrect, please run `ApplyXamlStyling.ps1 -Main` locally.'
        }

        # Return XAML Styler Status
        exit $lastExitCode
    }
    else
    {
        exit 0
    }
}

# SIG # Begin signature block
# MIIFwwYJKoZIhvcNAQcCoIIFtDCCBbACAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUbqQvEDeDREoEUVCBkc2H+p8n
# SvCgggNCMIIDPjCCAiagAwIBAgIQFQ8Ol7gj6pBJY3Zhli0Q+jANBgkqhkiG9w0B
# AQsFADA2MTQwMgYDVQQDDCtQb3dlclNoZWxs44K544Kv44Oq44OX44OI572y5ZCN
# 55So6Ki85piO5pu4MCAXDTI0MTAyNTE0NTEwNloYDzIwOTgxMjMxMTUwMDAwWjA2
# MTQwMgYDVQQDDCtQb3dlclNoZWxs44K544Kv44Oq44OX44OI572y5ZCN55So6Ki8
# 5piO5pu4MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsmAALCJFJ1mE
# J5u+41wXGmJOs6/YwZqh/Zc1W5gFO6iO9fyHzTi9y2u9ONxXFrgt07JKwL03nH/1
# 0Y4WDM94W5XwaSod3KZFlelisR4VZpsr65TZBMv38i+6BUfq6w6lV4HB2HCUjlQE
# t7LqvwOAolLXNcpT3YqEYz+kQmkwDsQDgKqtVNR+A1+d+sEq/z7Ty8cwckz9KW6A
# QUaWHe6izbgb3LjW3mui/iVSigOr2HkPlZhH94S6HLMgQcFAvKjCUxgEFcKH4Y36
# GWzNefwBN7hy6f3XBex95Hass6z2hXROAGXxKWF8zKf9p08Zp1SUPKlaqUMow2ET
# ToLciLgsfQIDAQABo0YwRDAOBgNVHQ8BAf8EBAMCB4AwEwYDVR0lBAwwCgYIKwYB
# BQUHAwMwHQYDVR0OBBYEFDEzufuEJqjElLkhgl4v/nSq/3nAMA0GCSqGSIb3DQEB
# CwUAA4IBAQAVsomvcc6A90iik0Bwqnwgyok/FiF6ZGBw/kDJhaIGj6lFM6zg+JI/
# eb2brgLdxFuHBED62U8Y/DZBnGeTa2oO0UlBBfZUMP7mvLVp8o9ZAkOtOf8uyGDc
# oN7frSa4DuEaR4+Jh9MfYQPorw6Qj01dWoWnCKUXS1IL70X1XaX05Kqlbalq68Nv
# BoIPbUZno1rlJyd2OhhX89fzVvM4d5K6HrOTT3d73xKBrQ0tSNYnMc286iTOU/HG
# KQRS4Zds2ZX12cK+oia8ys9AwMd7Miu9FJid5hYMb0Dp8lAjSLgWRkXjwtUHRajn
# t9VqsS9QiE8AUqrbt/E7wyT4eHqHqHvuMYIB6zCCAecCAQEwSjA2MTQwMgYDVQQD
# DCtQb3dlclNoZWxs44K544Kv44Oq44OX44OI572y5ZCN55So6Ki85piO5pu4AhAV
# Dw6XuCPqkEljdmGWLRD6MAkGBSsOAwIaBQCgeDAYBgorBgEEAYI3AgEMMQowCKAC
# gAChAoAAMBkGCSqGSIb3DQEJAzEMBgorBgEEAYI3AgEEMBwGCisGAQQBgjcCAQsx
# DjAMBgorBgEEAYI3AgEVMCMGCSqGSIb3DQEJBDEWBBRLlZC7WgaqGj3CX6wp6A20
# l9qlBDANBgkqhkiG9w0BAQEFAASCAQAJE4mjrYWz2xQV4d/RQ1W5lXqfDE5RcTxL
# 0e9bR156HypLg+xNvX5uUF5pPnoWcm6m83GOMDgbWd1Au6MD/x63yOgjodvQ9ZaU
# pH8W6pkimiZNhi0TnNvWq9SgID4RCorxSEsH6ZdubiyQh/AM5kZ8UxZ/I28ua33a
# juAk5KbqlkiESaL4e6oQpbScpWMC+UgD30gLRYlG/kpb6sjpTNOSbTxTnz96hvY0
# JGHlTeoJ9S/7aO8YsmKn6YTYX8OQbJWKBMqz09Gnlxc3a+9id0QwXyH1qYAKUj/E
# wBW1wxtqg+xW9uqCkQBo0H9OU8572crsSjeJ6F6yhGTQNvkD5wsD
# SIG # End signature block
