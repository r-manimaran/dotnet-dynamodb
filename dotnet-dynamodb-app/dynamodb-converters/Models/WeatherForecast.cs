using Amazon.DynamoDBv2.DataModel;
using System.Text.Json.Serialization;

namespace dynamodb_converters.Models;

public class WeatherForecast
{
    public string CityName { get; set; }
    public DateTime Date { get; set; }
    public Temperature Temperature { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    public Wind Wind { get; set; }
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //Instead of setting up here, we can set in DynamoDB context so that if 
    //the same enum type is used in another entity that will be used.
   // [DynamoDBProperty(Converter = typeof(WeatherTypeDynamoDbConverter))]
    public WeatherType WeatherType { get; set; }
}