param(
    [Parameter(Mandatory = $true)]
    [string]$Owner,

    [Parameter(Mandatory = $true)]
    [string]$Repository
)

$ErrorActionPreference = "Stop"

function Set-BranchProtection {
    param(
        [Parameter(Mandatory = $true)]
        [string]$BranchName,

        [Parameter(Mandatory = $true)]
        [int]$RequiredApprovingReviewCount
    )

    $bodyObject = @{
        required_status_checks = @{
            strict   = $true
            contexts = @(
                "Pull Request Flow / Validate branch flow",
                ".NET CI / test"
            )
        }
        enforce_admins = $true
        restrictions = $null
        required_linear_history = $false
        allow_force_pushes = $false
        allow_deletions = $false
        block_creations = $false
        required_conversation_resolution = $true
        lock_branch = $false
        allow_fork_syncing = $false
        required_pull_request_reviews = @{
            dismiss_stale_reviews           = $true
            require_code_owner_reviews      = $false
            required_approving_review_count = $RequiredApprovingReviewCount
        }
    }

    $body = $bodyObject | ConvertTo-Json -Depth 10

    $body | gh api `
        --method PUT `
        -H "Accept: application/vnd.github+json" `
        "/repos/$Owner/$Repository/branches/$BranchName/protection" `
        --input -
}

Set-BranchProtection -BranchName "develop" -RequiredApprovingReviewCount 0
Set-BranchProtection -BranchName "master" -RequiredApprovingReviewCount 1
