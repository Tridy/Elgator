using System.Text.Json.Serialization;

namespace Elgator
{
    public class StateChangeResult
    {
        [JsonPropertyName("numberofLights")]
        public int NumberOfLights { get; set; }

        [JsonPropertyName("lights")]
        public Light[] Lights { get; set; }

        public StateChangeResult(Light[] lights)
        {
            Lights = lights;
            NumberOfLights = 0;
        }
    }

    public class Light
    {
        [JsonPropertyName("on")]
        public int IsOn { get; set; }

        [JsonPropertyName("brightness")]
        public int Brightness { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }

        public Light()
        {
            IsOn = 0;
            Brightness = 0;
            Temperature = 0;
        }
    }
}