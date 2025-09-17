# run tsp-client convert fail

## question 
Hi All, when I run the tsp-client convert --swagger-readme .\spec.json command, it throws this error
 
error   | Autorest completed with an error. If you think the error message is unclear, or is a bug, please declare an issues at https://github.com/Azure/autorest/issues with the error message you are seeing.
error   | invalidType | Expected a boolean but got object: { title: 'Purview Data Catalog Client', version: '2023-10-01-preview' }
    - file:///C:/VSTS/PDG/ActiveGlossary/typespec/spec.json:3:3


## answer
You should use the `readme.md`, not the swagger file, as input.
Refer to the migration guide: https://azure.github.io/typespec-azure/docs/migrate-swagger/01-get-started/

Do you have a `readme.md` or another autorest markdown configuration file?
Is that spec part of the Azure REST API spec repo?
Did you actually run the command using the `readme.md` as input?

Try running `autorest <readme>`.
Also, your `spec.json` must be a valid Swagger file — from the error, you likely have a name mismatch (casing issue).

You can validate the Swagger file at: https://editor.swagger.io/ — that’s a good place to start.
But be aware: Autorest imposes additional restrictions, so you may need to keep running `autorest` or `tsp-client convert` repeatedly until the spec passes.

# Brownfield TypeSpec migration

## question 
Hi, Is there a timeframe for existing brownfield RPs to move from OpenAPI swagger to TypeSpec.  Is it possible to mix TypeSpec with handwritten swagger and migrate in phases. Say for example migrate one resource type at a time to minimize risk. 

## answer
Migration to TypeSpec for existing services is not yet mandatory, but it is suggested, and teams should be planning for it in Bromine and Krypton
Services must wholly switch to TypeSpec, there is no allowed mixing of hand-written and generated swagger
Servicesmust conform to a single, unified api-version for their service, servicesthat currently use different api-versions for parts of their service are going to need to plan for conformance -this either means SDK splitting or version uniformity.  Teams that use this 'different api-versions for different resources in the same sdk' pattern are not good candidates for conversion at the moment
In generally, the more compliant your service is to the RPC and best practices, the easier conversion will be
There is documentation on converting here: [Getting started | TypeSpec Azure](https://azure.github.io/typespec-azure/docs/migrate-swagger/01-get-started/)

# Typespec -> Autorest generation : multiple specs per service

## question 
Currently while we are able to organize and manage multiple typespec files per service easily, the final generated swagger is a single file.
I was asked during my API review to check if there is feasibility to produce multiple specs per service for organizational purposes considering the generated file is huge. 
I see prior posts on this indicating this is not supported, but looking for any latest update/guidance here.
Secondly, if the above is in fact supported, any idea if the SDK generation part can handle multiple specs per service?

## answer
No, since the swagger is now just an emitted artifact, there is no real reason to organize it.  There is no mechanism for splitting a TypeSpec spec into multiple OpenAPI files.
