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
        [bool]$RequireReviews
    )

    $bodyObject = @{
        required_status_checks = @{
            strict   = $true
            contexts = @(
                "Validate branch flow",
                "test"
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
    }

    if ($RequireReviews) {
        $bodyObject.required_pull_request_reviews = @{
            dismiss_stale_reviews           = $true
            require_code_owner_reviews      = $false
            required_approving_review_count = 1
        }
    }
    else {
        $bodyObject.required_pull_request_reviews = $null
    }

    $body = $bodyObject | ConvertTo-Json -Depth 10

    $body | gh api `
        --method PUT `
        -H "Accept: application/vnd.github+json" `
        "/repos/$Owner/$Repository/branches/$BranchName/protection" `
        --input -
}

Set-BranchProtection -BranchName "develop" -RequireReviews $false
Set-BranchProtection -BranchName "master" -RequireReviews $false
