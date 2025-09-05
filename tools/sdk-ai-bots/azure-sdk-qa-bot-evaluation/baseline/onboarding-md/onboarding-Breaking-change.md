# Breaking change (bug fix) flow for previous versions

## question 
Hi Azure SDK Onboarding,
Our team found an invalid type specification in our previous 2 API releases (1 preview, 1 stable).  I've read through (lightly) the breaking change deep dive, which looks like I'll have to reach out to the review board to get their approval, however, I was wondering what is the recommended way to implement a fix for existing APIs, up to the point where it can be reviewed by ARM and Breaking changes board.
Normally, I would create a branch, commit a base version for review, then create a release plan against the new branch (then add in changes, get ARM sign off, then request the sdk release).
How do I regenerate the SDKs once I have a fix in place for the previously released verisons?  Will they require manual updates (e.g. create a PR in each sdk repo with a fix)?
Any guidance would be appreciated!

## answer
I recommend starting here: [Deep-dive into breaking changes on spec PRs](https://eng.ms/docs/products/azure-developer-experience/design/specs-pr-guides/pr-brch-deep)
