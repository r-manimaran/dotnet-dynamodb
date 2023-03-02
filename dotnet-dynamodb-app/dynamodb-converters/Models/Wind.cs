using System.Text.Json.Serialization;

namespace dynamodb_converters.Models;

public class Wind
{
    public decimal Speed { get; set; }
    public string Direction { get; set; }
    //Adding this Property to check the Dynamodb Converter defined in Context level
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public WeatherType WeatherType { get; set; }
}
