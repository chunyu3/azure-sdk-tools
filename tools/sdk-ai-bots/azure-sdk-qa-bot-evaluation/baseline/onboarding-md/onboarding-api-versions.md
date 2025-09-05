# Are beta sdks the only option for preview api-versions?

## question 
Hi,
 
I'm from the ArcHCIVm team.
 
We recently started working on releasing our sdks. I am working on releasing beta sdks for our latest preview api-version (almost done). Can I convert that to a stable sdk once its done? Can a preview api-version have a stable sdk?

## answer
No, you cannot release a stable (GA) SDK for a preview API version. Stable SDKs are expected to target stable (GA) API versions. Preview API versions (those with a `-preview` suffix) are considered early-in-development and are not final. The Azure SDK process recommends that SDKs generated for preview API versions are released as beta packages only. Once the API version is finalized and promoted to GA (stable), you can then release a stable SDK for that GA API version.
If you have already released a beta SDK for a preview API version and want to convert it to a stable SDK, you should wait until your API version is finalized as GA. At that point, you can update the SDK version from beta to GA and release it as a stable SDK. This process is outlined in the FAQ: teams are encouraged to release beta SDKs for preview API versions, gather feedback, and only release stable SDKs for stable (GA) API versions. If you have special circumstances, you can request an exception, but this is rarely granted for stable SDKs targeting preview API versions.
In summary:
Beta SDKs are the standard for preview API versions.
Stable SDKs should only be released for stable (GA) API versions.
Convert your beta SDK to stable only after the API version is GA.
References
[Azure SDK frequently asked questions | Azure-sdk-docs-eng](https://eng.ms/docs/products/azure-developer-experience/faqs/faqs)
[Glossary terms for Azure SDKs - Preview (Service) | Azure-sdk-docs-eng](https://eng.ms/docs/products/azure-developer-experience/faqs/glossary)
