using System.Text.Json.Serialization;

namespace dynamodb_converters.Models;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WeatherType
{
    None,
    Sunny,
    Rainy,
    Snow,
    Stromy,
    Cloudy
}
