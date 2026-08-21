# Git Flow

## Branches

- `master`: production-ready code only.
- `develop`: integration branch for completed features.
- `feature/*`: feature development branches created from `develop`.
- `release/*`: release preparation branches created from `develop`.

## Expected flow

1. Create a branch from `develop` using the pattern `feature/<name>`.
2. Open a pull request from `feature/<name>` to `develop`.
3. After one or more features are merged into `develop`, create a branch from `develop` using the pattern `release/<name>`.
4. Open a pull request from `release/<name>` to `master`.

## GitHub protection rules

The repository includes a workflow that validates this pull request route:

- only `feature/*` can target `develop`;
- only `release/*` can target `master`;
- pull requests to other target branches fail.

To fully enforce the flow, configure branch protection in GitHub for both `master` and `develop`.

## Recommended branch protection

### `master`

- Require a pull request before merging.
- Require approvals.
- Require status checks to pass before merging.
- Select at least:
  - `.NET CI / test`
  - `Pull Request Flow / Validate branch flow`
- Dismiss stale pull request approvals when new commits are pushed.
- Restrict who can push to matching branches.
- Do not allow force pushes.
- Do not allow deletions.

### `develop`

- Require a pull request before merging.
- Require status checks to pass before merging.
- Select at least:
  - `.NET CI / test`
  - `Pull Request Flow / Validate branch flow`
- Restrict who can push to matching branches if you want to block direct pushes there too.
- Do not allow force pushes.
- Do not allow deletions.

## Local examples

```bash
git checkout develop
git pull origin develop
git checkout -b feature/create-character-endpoint
```

```bash
git checkout develop
git pull origin develop
git checkout -b release/v1.0.0
```
