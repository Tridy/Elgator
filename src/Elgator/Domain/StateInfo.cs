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

        public StateInfo()
        {
            NumberOfLights = 0;
            Changes = new T[0];
        }
    }
}