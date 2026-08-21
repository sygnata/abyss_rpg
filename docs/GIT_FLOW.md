# Git Flow

## Branches

- `master`: production-ready code only.
- `develop`: integration branch for completed features.
- `feature/*`: feature development branches created from `develop`.
- `release/<major>.<minor>`: release preparation branches created from `develop`.

## Expected flow

1. Create a branch from `develop` using the pattern `feature/<name>`.
2. Open a pull request from `feature/<name>` to `develop`.
3. After one or more features are merged into `develop`, create a branch from `develop` using the pattern `release/<major>.<minor>`.
4. Open a pull request from `release/<name>` to `master`.

## Release naming convention

- Releases follow `release/<major>.<minor>`.
- The default sequence starts at `release/0.1`.
- The next releases become `release/0.2`, `release/0.3`, and so on.
- When you want to start a new major line, create the next series explicitly, for example `release/1.1`.

To automate this, use:

```powershell
.\scripts\github\create-next-release-branch.ps1
```

This script:

- fetches `origin/develop`;
- finds existing local and remote branches matching `release/<major>.<minor>`;
- creates the next branch for the selected major;
- starts from `origin/develop`.

Examples:

```powershell
.\scripts\github\create-next-release-branch.ps1
```

Creates `release/0.1`, then `release/0.2`, then `release/0.3`.

```powershell
.\scripts\github\create-next-release-branch.ps1 -Push
```

Creates the next release branch and publishes it to `origin`.

```powershell
.\scripts\github\create-next-release-branch.ps1 -Major 1 -Push
```

Creates the next release in the `1.x` line, for example `release/1.1`.

## GitHub protection rules

The repository includes a workflow that validates this pull request route:

- only `feature/*` can target `develop`;
- only `release/*` can target `master`;
- pull requests to other target branches fail.

To fully enforce the flow, configure branch protection in GitHub for both `master` and `develop`.

## Recommended branch protection

### `master`

- Require a pull request before merging.
- Require status checks to pass before merging.
- Select at least:
  - `test`
  - `Validate branch flow`
- Do not require approvals when working solo.
- Restrict who can push to matching branches.
- Do not allow force pushes.
- Do not allow deletions.

### `develop`

- Require a pull request before merging.
- Require status checks to pass before merging.
- Select at least:
  - `test`
  - `Validate branch flow`
- Do not require approvals when working solo.
- Restrict who can push to matching branches if you want to block direct pushes there too.
- Do not allow force pushes.
- Do not allow deletions.

## Local examples

```bash
git checkout develop
git pull origin develop
git checkout -b feature/create-character-endpoint
```

```powershell
.\scripts\github\create-next-release-branch.ps1 -Push
```
