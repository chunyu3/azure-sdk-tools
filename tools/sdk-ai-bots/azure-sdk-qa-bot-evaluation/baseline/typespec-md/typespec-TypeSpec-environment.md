# tsp compile failure

## question 
Hi All, the 'tsp compile .' command is throwing the following error. Could anyone kindly help with resolving the error? 
error file-load: Package subpath './experimental' is not defined by "exports" in C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\http\package.json imported from C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\rest\dist\src\rest.js
error file-load: Package subpath './experimental' is not defined by "exports" in C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\http\package.json imported from C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\rest\dist\src\rest.js
error file-load: Package subpath './typekit' is not defined by "exports" in C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@typespec\compiler\package.json imported from C:\VSTS\PDG\ActiveGlossary\typespec\node_modules\@azure-tools\typespec-azure-core\dist\src\decorators.js
error import-not-found: Couldn't resolve import "@azure-tools/typespec-autorest"

## answer
The errors you're seeing are caused by incompatible TypeSpec package versions in your environment. These specific errors occur when package versions don't align correctly across your dependencies.

To fix this issue when working in the Azure REST API specs repository:

1. **Clean your environment**:
   - Delete your `node_modules` folder
   - Clear npm cache if needed

2. **Install the correct dependencies**:
   - Navigate to the repository root
   - Run `npm ci` (not npm install) to ensure exact versions from package-lock.json
   - Verify installation: `npx tsp --version`

3. **Version compatibility**:
   Since TypeSpec is now at 1.0, you need to maintain version alignment within these groups:
   - **Stable packages (1.x)**: compiler, http, openapi, openapi3
   - **Core preview packages (0.y)**: rest, versioning, stream, see
   - **Azure packages (0.z)**: all Azure packages

   All packages within the same group must have matching major.minor versions.

4. **For Azure REST API specs repositories**:
   - Always use the dependencies defined in the repository's package.json
   - Don't mix global and local TypeSpec installations when working on specs
   - If issues persist, try: `npx tsp init https://aka.ms/typespec/azure-init` to set up a fresh project

For latest references:
- Official guide: [TypeSpec REST API Dev Process](https://github.com/Azure/azure-rest-api-specs/blob/main/documentation/typespec-rest-api-dev-process.md)
- TypeSpec versions: [npm/@typespec/compiler](https://www.npmjs.com/package/@typespec/compiler)

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
The error you're encountering (`spawn EINVAL`) typically occurs when there's an issue with Node.js processes being unable to spawn child processes correctly. Here's how to address this issue:

1. **Check your Node.js version**: 
   - Make sure you're using Node.js LTS version 18 or above as recommended by the TypeSpec documentation
   - You can verify your version with `node --version`

2. **Proper installation process for TypeSpec**:
   - If you're working with Azure REST API specs repositories, don't rely solely on global installation
   - Navigate to the repository root folder and run `npm ci` to install all required packages based on the package-lock.json
   - Verify installation with `npx tsp --version`

3. **Use the TypeSpec Azure template for new projects**:
   - When creating a new TypeSpec project, use: `npx tsp init https://aka.ms/typespec/azure-init`
   - This ensures you have the correct project structure and dependencies

4. **Visual Studio Code integration**:
   - Install TypeSpec VS Code extension for better development experience: `npx tsp code install`

5. **Consider using containerized options**:
   - VSCode with local Docker .devcontainer
   - GitHub Codespaces for browser-based development

If you continue experiencing issues, you might need to:
- Check for any proxy or networking issues that could interfere with npm processes
- Verify you have proper permissions to execute processes on your system
- Try clearing npm cache with `npm cache clean --force`

For complete TypeSpec setup and usage documentation, please refer to the official guide: [TypeSpec REST API Dev Process](https://github.com/Azure/azure-rest-api-specs/blob/7fc6689d84858b1c71b786526b04c014c4589968/documentation/typespec-rest-api-dev-process.md)

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
