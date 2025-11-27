using System.Text;
using System.Text.Json.Serialization;
using Octokit;

namespace Azure.Sdk.Tools.Cli.Models.Responses.TypeSpec
{
    public class TypeSpecAuthoringResponse: CommandResponse
    {
        [JsonPropertyName("is_successful")]
        public bool IsSuccessful { get; set; }
        [JsonPropertyName("task")]
        public string Task { get; set; }
        [JsonPropertyName("steps")]
        public List<string> Steps { get; set; }
        protected override string Format()
        {
            var output = new StringBuilder();
            if (Steps?.Count > 0)
            {
                output.AppendLine("[Follow this process:]");
                foreach (var step in Steps)
                {
                    output.AppendLine(step);
                }
            }
            if (NextSteps?.Count > 0)
            {
                output.AppendLine("[Follow this process:]");
                foreach (var step in NextSteps)
                {
                    output.AppendLine(step);
                }
            }
            return output.ToString();
        }
    }
}
