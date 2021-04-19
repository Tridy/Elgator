//using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Elgator
{
    public class StateInfo<T>
    {
        [JsonPropertyName("numberOfLights")]
        public int NumberOfLights { get; set; }

        [JsonPropertyName("lights")]
        public T[] Changes { get; set; }
    }
}