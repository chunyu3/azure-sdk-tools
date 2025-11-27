using System.CommandLine;
using System.ComponentModel;
using Azure.Sdk.Tools.Cli.Models;
using Azure.Sdk.Tools.Cli.Models.Responses.TypeSpec;
using Azure.Sdk.Tools.Cli.Services;
using ModelContextProtocol.Server;

namespace Azure.Sdk.Tools.Cli.Tools.TypeSpec.authoring
{
    /// <summary>
    /// This tool is designed to guide the user to define and update TypeSpec based API spec for an azure service
    /// It connects to an AI agent that can answer questions about TypeSpec, Azure SDK guidelines, and API best practices.
    /// </summary>
    public class TypeSpecAuthoringTool : MCPTool
    {
        private readonly ILogger<TypeSpecAuthoringTool> _logger;
        private readonly AiCompletionTool _aiCompletionTool;
        private const string TypeSpecAuthoringToolCommandName = "typespec_authoring";
        // Command line options and arguments
        private readonly Argument<string> _taskArgument = new("task")
        {
            Description = "The task to perform"
        };
        public TypeSpecAuthoringTool(ILogger<TypeSpecAuthoringTool> logger, AiCompletionTool aiCompletionTool)
        {
            _logger = logger;
            _aiCompletionTool = aiCompletionTool ?? throw new ArgumentNullException(nameof(aiCompletionTool)); ;
        }

        protected override Command GetCommand()
        {
            var command = new Command(TypeSpecAuthoringToolCommandName, "Guide the user to define and update TypeSpec based API spec for an azure service.")
            {
                _taskArgument
            };

            return command;
        }

        public override async Task<CommandResponse> HandleCommand(ParseResult parseResult, CancellationToken ct)
        {
            var task = parseResult.GetValue(_taskArgument);

            if (string.IsNullOrWhiteSpace(task))
            {
                _logger.LogError("Task cannot be empty");
                return new DefaultCommandResponse() { ResponseError = "Task cannot be empty" };
            }

            var response = await GeneratePlansAsync(task);
            if (!response.IsSuccessful)
            {
                return new DefaultCommandResponse() { ResponseError = $"Tool excution failed: {response.ResponseError}" };
            }
            return response;
        }

        [McpServerTool(Name = "azsdk_typespec_authoring")]
        [Description(@"Guide the user to define and update TypeSpec based API spec for an azure service.
        ## 🧩 Context
        This tool applies to all tasks involving **TypeSpec**, including:
        - Writing new TypeSpec definitions: service, api version, resource, models, operations
        - Editing or refactoring existing TypeSpec files, editing api version, service, resource, models, or operations
        - versioning evolution: 
            Making a preview api stable, 
            Replacing a preview API with a new preview API
            Replacing a preview API with a stable API
            Replacing a preview API with a stable API and a new preview API
            Adding a preview API version
            Adding a stable API version
            and so on
        - review and analyze provided TypeSpec (.tsp) sources, verify compilation and linting status, detect misalignment with Azure API guidelines, identify and confirm correct usage of **typespec-azure** and **typespec-azure-resource-manager** templates (resource models & decorators), and propose minimal, standards‑compliant fixes.
        - Resolving TypeSpec-related issues")]
        public async Task<TypeSpecAuthoringResponse> GeneratePlansAsync(string task)
        {
            try 
            {
                if (string.IsNullOrWhiteSpace(task))
                {
                    _logger.LogError("Task cannot be empty");
                    return new TypeSpecAuthoringResponse() { ResponseError = "Task cannot be empty" };
                }
                _logger.LogInformation("Generating plans for task: {task}", task);
                var response = await _aiCompletionTool.QueryAzureSDKDocumentation(
                $"Provide validated solutions, suggestions, or fixes for TypeSpec issues. Combine the user’s question with all provided metadata (such as context, role, service name, decorators, versioning details, and any additional parameters) to construct a comprehensive query. Ensure the query is precise, includes relevant technical terms, and targets authoritative sources for TypeSpec and Azure API guidance. User question: {task}",
                ct: CancellationToken.None);
                if (!response.IsSuccessful)
                {
                    return new TypeSpecAuthoringResponse() { ResponseError = $"AI completion tool failed: {response.ResponseError}" };
                }
                return new TypeSpecAuthoringResponse
                {
                    IsSuccessful = true,
                    NextSteps = new List<string>() {
                        "Step 1: Call the `azsdk_ai_qa_completion` tool to query how to perform the task : " +$"{task}",
                        "Step 2: Extract the solution from the tool result: Parse the response from `azure-sdk-mcp/azsdk_ai_qa_completion` to identify the recommended fix or implementation.",
                        "Step 3: Execute the solution: Apply the extracted solution to update the TypeSpec file accordingly.",
                        "tep 4: Summary the solution: Summary the solution taken, and display the reference doc url from the response from `azure-sdk-mcp/azsdk_ai_qa_completion` tool",
                    },
                };
                //return new TypeSpecAuthoringResponse
                //{
                //    IsSuccessful = response.IsSuccessful,
                //    Task = task,
                //    Steps = new List<string>(),
                //    NextSteps = new List<string>{ $"Parse the following response to identify the recommended fix or implementation.\n{response.Answer}",
                //    "Apply the extracted solution to update the TypeSpec file accordingly.",
                //    $"Summary the solution taken, and display the reference doc url from the response source: {response.References}"},
                //};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging task information");
                return new TypeSpecAuthoringResponse() { ResponseError = $"Failed to generate plans: {ex.Message}" };
            }
            
            //return new TypeSpecAuthoringResponse
            //{
            //    IsSuccessful = true,
            //    NextSteps = new List<string>() {
            //        "Step 1: Call the `azsdk_ai_qa_completion` tool to query how to perform the task : " +$"{task}",
            //        "Step 2: Extract the solution from the tool result: Parse the response from `azure-sdk-mcp/azsdk_ai_qa_completion` to identify the recommended fix or implementation.",
            //        "Step 3: Execute the solution: Apply the extracted solution to update the TypeSpec file accordingly.",
            //        "tep 4: Summary the solution: Summary the solution taken, and display the reference doc url from the response from `azure-sdk-mcp/azsdk_ai_qa_completion` tool",
            //    },
            //};
        }
    }
}
