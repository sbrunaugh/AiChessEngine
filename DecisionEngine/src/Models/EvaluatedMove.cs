using System.Text.Json;
using System.Text.Json.Serialization;

namespace DecisionEngine.Models
{
    public class EvaluatedMove
    {
        [JsonPropertyName("move")]
        public Move Move { get; set; }
        [JsonPropertyName("evaluation")]
        public float Evaluation { get; set; }
    }
}
