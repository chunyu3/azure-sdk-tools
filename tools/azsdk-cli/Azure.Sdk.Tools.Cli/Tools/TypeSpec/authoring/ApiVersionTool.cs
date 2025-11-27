using System.CommandLine;
using System.ComponentModel;
using Azure.Sdk.Tools.Cli.Commands;
using Azure.Sdk.Tools.Cli.Models;
using Azure.Sdk.Tools.Cli.Models.AiCompletion;
using Azure.Sdk.Tools.Cli.Models.Responses.TypeSpec;
using ModelContextProtocol.Server;

namespace Azure.Sdk.Tools.Cli.Tools.TypeSpec.authoring
{
    [McpServerToolType, Description("This type contains the tool to add or update api versions for a TypeSpec project based on documentation, guidelines, and best practices.")]
    public class ApiVersionTool: MCPTool
    {
        public override CommandGroup[] CommandHierarchy { get; set; } = [SharedCommandGroups.TypeSpecAuthoring];
        private readonly ILogger<ApiVersionTool> _logger;

        private const string ApiVersionToolCommandName = "api-version";
        // Command line options and arguments
        private readonly Argument<string> _taskArgument = new("task")
        {
            Description = "The task to perform"
        };
        public ApiVersionTool(ILogger<ApiVersionTool> logger)
        {
            _logger = logger;
        }

        protected override Command GetCommand()
        {
            var command = new Command(ApiVersionToolCommandName, "add or update an api version for a service.")
            {
                _taskArgument
            };

            return command;
        }

        public override Task<CommandResponse> HandleCommand(ParseResult parseResult, CancellationToken ct)
        {
            return Task.FromResult<CommandResponse>(new TypeSpecAuthoringResponse
            {
                IsSuccessful = true,
                NextSteps = new List<string>() {
                    "identify current api versions which are stable version, which are preview version ( the version with suffix '-preview', For preview versions, the format should be YYYY-MM-DD-preview; For stable versions, the format should be YYYY-MM-DD (e.g., 2025-12-01)). Give out summary of the service api version and put it as meta data.",
                    "prompt user to provide the api version name and choose if is stable version or preview version.",
                    "verify if the api version name match the azure version rule: preview version suffix '-preview'. If not, prompt user to suggest an api version name",
                    "when add a new preview api version, and the current latest api version is preview api version, prompt user to ask if it want to replace the previous preview api version or add a new one",
                    "when add a new stable api version, and the current latest api version is preview version, prompt user to ask if it want to replace the preview api version or not",
                    "step 1: Call the `azsdk_ai_qa_completion` tool to retrieve validated solutions, suggestions, or fixes for TypeSpec issues. Combine the user’s question with all provided metadata (such as context, role, service name, decorators, versioning details, and any additional parameters) to construct a comprehensive query. Ensure the query is precise, includes relevant technical terms, and targets authoritative sources for TypeSpec and Azure API guidance.",
                    "step 2: Parse the response from `azsdk_ai_qa_completion` to identify the recommended fix or implementation.",
                    "step 3: Apply the extracted solution to update the TypeSpec file accordingly.",
                    "step 4: Summary the solution taken, and display the reference doc url from the response from `azure-sdk-mcp/azsdk_ai_qa_completion` tool"
                },
            });
        }

        [McpServerTool(Name = "azsdk_typespec_api_version")]
        [Description(@"add or update api version of typepsec project for a service.")]
        public Task<CommandResponse> UpdateApiVersionForService(
            [Description("The task to perform")]
            string task,
            CancellationToken ct = default)
        {
            try
            {
                // Validate inputs
                //if (string.IsNullOrWhiteSpace(task))
                //{
                //    return new AiCompletionToolResponse
                //    {
                //        ResponseError = "Task cannot be empty"
                //    };
                //}

                return Task.FromResult<CommandResponse>(new TypeSpecAuthoringResponse
                {
                    IsSuccessful = true,
                    NextSteps = new List<string>() {
                    "identify current api versions which are stable version, which are preview version ( the version with suffix '-preview', For preview versions, the format should be YYYY-MM-DD-preview; For stable versions, the format should be YYYY-MM-DD (e.g., 2025-12-01)). Give out summary of the service api version and put it as meta data.",
                    "prompt user to provide the api version name and choose if is stable version or preview version.",
                    "verify if the api version name match the azure version rule: preview version suffix '-preview'. If not, prompt user to suggest an api version name",
                    "when add a new preview api version, and the current latest api version is preview api version, prompt user to ask if it want to replace the previous preview api version or add a new one",
                    "when add a new stable api version, and the current latest api version is preview version, prompt user to ask if it want to replace the preview api version or not",
                    "step 1: Call the `azsdk_ai_qa_completion` tool to retrieve validated solutions, suggestions, or fixes for TypeSpec issues. Combine the user’s question with all provided metadata (such as context, role, service name, decorators, versioning details, and any additional parameters) to construct a comprehensive query. Ensure the query is precise, includes relevant technical terms, and targets authoritative sources for TypeSpec and Azure API guidance.",
                    "step 2: Parse the response from `azsdk_ai_qa_completion` to identify the recommended fix or implementation.",
                    "step 3: Apply the extracted solution to update the TypeSpec file accordingly.",
                    "step 4: Summary the solution taken, and display the reference doc url from the response from `azure-sdk-mcp/azsdk_ai_qa_completion` tool"
                },
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying AI agent");
                //return new AiCompletionToolResponse
                //{
                //    ResponseError = $"Failed to query AI agent: {ex.Message}"
                //};
                return Task.FromResult<CommandResponse>(new TypeSpecAuthoringResponse
                {
                    IsSuccessful = false,
                    NextSteps = new List<string>() {
                    "Check the running logs for details about the error",
                    "Resolve the issue",
                    "Re-run the tool",
                    "Run verify setup tool if the issue is environment related"
                }
                });
            }

            
        }
    }
}
