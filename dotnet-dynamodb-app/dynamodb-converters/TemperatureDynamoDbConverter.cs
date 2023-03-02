using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using dynamodb_converters.Models;

namespace dynamodb_converters
{
    public class TemperatureDynamoDbConverter : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            if(entry!= null){
                var tempString = entry.ToString();
                var tempStrings = tempString.Split("*",StringSplitOptions.RemoveEmptyEntries);
                var unit = tempStrings[1] switch
                {
                    "C" => TemperatureType.Celcius,
                    "F" => TemperatureType.Farenheit,
                    _ => throw new Exception("Invalid Temperature Type")
                };
                return new Temperature(Decimal.Parse(tempStrings[0]), unit);
            
            }
            return null;
        }

        public DynamoDBEntry ToEntry(object value)
        {
            if(value is Temperature temperature )
            {
                var unit = temperature.TemperatureType == TemperatureType.Celcius ? "C" : "F";
                return new Primitive($"{temperature.Degree}*{unit}");
            }
            return null;
        }
    }
}
