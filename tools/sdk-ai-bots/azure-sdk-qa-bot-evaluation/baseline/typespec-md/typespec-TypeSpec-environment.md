# tsp compile failure

## question 
Hi All, the 'tsp compile .' command is throwing the following error. Could anyone kindly help with resolving the error? 
error file-load: Package subpath './experimental' is not defined by "exports" in C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\http\package.json imported from C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\rest\dist\src\rest.js
error file-load: Package subpath './experimental' is not defined by "exports" in C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\http\package.json imported from C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\rest\dist\src\rest.js
error file-load: Package subpath './typekit' is not defined by "exports" in C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\compiler\package.json imported from C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@azure-tools\typespec-azure-core\dist\src\decorators.js
error import-not-found: Couldn't resolve import "@azure-tools/typespec-autorest"

## answer
This seems like you have an issue with the dependencies you have installed. Did you get any error when using npm install? What versions are you using? Try a fresh install (delete node_modules first).

These are not compatible. Please upgrade to the latest or make sure you are using versions that match if you can't for now.

For the latest, you can look on npm: https://www.npmjs.com/package/@typespec/compiler
Or at the bottom of the repo: https://github.com/microsoft/typespec

There are 3 version groups you need to know since we are at 1.0:

Stable packages: 1.x (compiler, http, openapi, openapi3)

Core preview packages: 0.y (rest, versioning, stream, see)

Azure packages: 0.z (all Azure packages)
All packages in the same group have the same major.minor version.

If you are in the spec repo, read this doc for next steps:
https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/typespec-rest-api-dev-process.md

# TSP Install fails with below error

## question 
I am trying to install TSP and its dependencies and it fails with below error. I have latest node.js Can you please help?

Error: spawn EINVAL
    at ChildProcess.spawn (node:internal/child_process:420:11)
    at spawn (node:child_process:753:9)
    at installTypeSpecDependencies (file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/dist/src/core/install.js:4:19)
    at file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/dist/src/core/cli/cli.js:152:95
    at Object.handler (file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/dist/src/core/cli/utils.js:16:16)
    at file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/node_modules/yargs/build/lib/command.js:206:54
    at maybeAsyncResult (file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/node_modules/yargs/build/lib/utils/maybe-async-result.js:9:15)
    at CommandInstance.handleValidationAndGetResult (file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/node_modules/yargs/build/lib/command.js:205:25)
    at CommandInstance.applyMiddlewareAndGetResult (file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/node_modules/yargs/build/lib/command.js:245:20)
    at CommandInstance.runCommand (file:///C:/Users/anponnet/AppData/Roaming/npm/node_modules/@typespec/compiler/node_modules/yargs/build/lib/command.js:128:20) {
  errno: -4071,
  code: 'EINVAL',
  syscall: 'spawn'
}

## answer
1. which version of the compiler did you install globally, this looks liek quite an old one?
2. where are you trying to use typespec, if its the azure spec repo please follow the docs there https://github.com/Azure/azure-rest-api-specs/blob/7fc6689d84858b1c71b786526b04c014c4589968/documentation/typespec-rest-api-dev-process.md

# How to properly update the TypeSpec environment?

## question 
I tried inferring steps from the various installation documents but just managed to break my environment and have no idea how to fix it.
 
I saw a recent post where it was said to run `npm install -g @typespec/compiler` to get the latest (0.66) but it looks like it did not work for me. My compiler is still 0.64.
```
NORTHAMERICA+darkoa@darkoa-ws MINGW64 /d/Dev/Projects/git/github/azure-rest-api-specs-pr (RPSaaSMaster)
$ npm install -g @typespec/compiler

changed 268 packages in 11s

34 packages are looking for funding
  run `npm fund` for details

NORTHAMERICA+darkoa@darkoa-ws MINGW64 /d/Dev/Projects/git/github/azure-rest-api-specs-pr (RPSaaSMaster)
$ tsp compile specification/deviceupdate/DeviceUpdate.Edge.Management/
TypeSpec compiler v0.64.0

Diagnostics were reported during compilation:
```
I get a bunch of errors, although we made no changes recently. I am guessing those changes were made by the TypeSpec team and I am also guessing that if I manage to properly update tools, the errors should go away.
 
So, is there a single document that describes how to update the environment to the latest?

## answer
The issue you're facing is related to updating your local TypeSpec environment. Here's a summary of the solution:

Update Local Dependencies:

Running npm install -g @typespec/compiler only updates the global TypeSpec compiler, which is not typically used for local development unless you need access to TypeSpec commands globally.

To properly update your local environment, navigate to your repository and run npm ci. This command installs the exact versions of dependencies specified in the package-lock.json, ensuring consistency.

TypeSpec Versions in Repositories:

When working with repositories like azure-rest-api-specs, you should always install dependencies based on the local package.json and package-lock.json at the root of your branch. This ensures you're using the correct version of TypeSpec and associated tools for your current project.

Dealing with Configuration Warnings:

The warnings you're seeing (such as missing options for SDK emitters) are configuration-related, not errors. These warnings appear when SDK emitters for specific languages (like Go, Python, C#) are not installed, but they do not affect your immediate work with TypeSpec validation. These can be safely ignored unless you need to work with SDK generation.

Suppression and Documentation:

If you want to suppress specific warnings, you can modify the suppressions.yaml file, but it's important to follow the TypeSpec guidelines to ensure proper environment configuration.
