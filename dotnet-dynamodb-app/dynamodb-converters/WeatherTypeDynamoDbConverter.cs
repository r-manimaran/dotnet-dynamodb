using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using dynamodb_converters.Models;

namespace dynamodb_converters
{
    public class WeatherTypeDynamoDbConverter : IPropertyConverter
    {
        /// <summary>
        /// Convert Back String in DynamoDB to Enum Type
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public object FromEntry(DynamoDBEntry entry)
        {
            return Enum.Parse<WeatherType>(entry.AsString());
        }

        /// <summary>
        /// Convert Enum Type to String to Store in DynamoDB
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DynamoDBEntry ToEntry(object value)
        {
            return new Primitive(value.ToString());
        }
    }
}
