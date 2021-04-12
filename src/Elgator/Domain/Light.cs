using System.Text.Json.Serialization;

namespace Elgator
{
    public class LightOn
    {
        [JsonPropertyName("on")]
        public int Value { get; } = 1;
    }

    public class LightOff
    {
        [JsonPropertyName("on")]
        public int Value { get; } = 0;
    }

    public class Brightness
    {
        [JsonPropertyName("brightness")]
        public int Value { get; set; } = 50;
    }

    public class Temperature
    {
        [JsonPropertyName("temperature")]
        public int Value { get; set; } = 200;
    }
}
