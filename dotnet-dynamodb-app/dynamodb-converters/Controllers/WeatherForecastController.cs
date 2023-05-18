using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using Amazon.Util;
using dynamodb_converters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace dynamodb_converters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        private readonly ILogger<WeatherForecastController> _logger;
        public readonly IDynamoDBContext _dynamoDBContext;
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                        IDynamoDBContext dynamoDBContext,IAmazonDynamoDB dynamoDBClient)
        {
            _logger = logger;
            _dynamoDBContext = dynamoDBContext;
            _dynamoDbClient = dynamoDBClient;
        }        

        [HttpGet("get-by-date")]
        public async Task<WeatherForecast> GetByDateAsync(string cityName,DateTime dateTime)
        {
            return await _dynamoDBContext.LoadAsync<WeatherForecast>(cityName, dateTime);
        }

        [HttpGet("all-city")]
        public async Task<IEnumerable<WeatherForecast>> GetAllForCity(string cityName)
        {
            return await _dynamoDBContext.QueryAsync<WeatherForecast>(cityName).GetRemainingAsync();
        }
        //[HttpGet(Name ="get-with-projected-expression")]
        //public async Task<WeatherForecast> GetDataWithProjExp(string cityName, DateTime date)
        //{
        //    return await _dynamoDBContext.LoadAsync<WeatherForecast>(cityName,
        //        date, new DynamoDBOperationConfig()
        //        {
        //            OverrideTableName=nameof(WeatherForecast)
        //        });
        //}



        [HttpPost]
        public async Task Post(WeatherForecast weatherForecast)
        {
            weatherForecast.Date = weatherForecast.Date.Date;
            await _dynamoDBContext.SaveAsync(weatherForecast);
        }

        //Using Conditional Expression
        [HttpPost("post-if-latest")]
        public async Task PostIfLatest(WeatherForecast data)
        {
            data.Date = data.Date.Date;
            var item = Document.FromJson(JsonSerializer.Serialize(data));
            PutItemRequest putItemRequest = new PutItemRequest()
            {
                TableName = nameof(WeatherForecast),
                Item = item.ToAttributeMap(),
                ConditionExpression = "attribute_not_exists(LastUpdated) OR (LastUpdated < :LastUpdated)",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {
                        ":LastUpdated", new AttributeValue( data.LastUpdated.ToString(AWSSDKUtils.ISO8601DateFormat))
                    }
                }
            };
            await _dynamoDbClient.PutItemAsync(putItemRequest);
            
        }

        [HttpPost("post-if-not-exists")]
        public async Task PostIfNotExists(WeatherForecast data)
        {
            data.Date = data.Date.Date;
            var item = Document.FromJson(JsonSerializer.Serialize(data));
            PutItemRequest putItemRequest = new PutItemRequest()
            {
                TableName = nameof(WeatherForecast),
                Item = item.ToAttributeMap(),
                ConditionExpression = "attribute_not_exists(CityName) AND attribute_not_exists(#Date)",
                ExpressionAttributeNames = new Dictionary<string, string>
                {
                    {"#Date", "Date" }
                }
            };
            await _dynamoDbClient.PutItemAsync(putItemRequest);
        }

        [HttpDelete]
        public async Task DeleteIfGt20(string cityName, string date)
        {
            await _dynamoDbClient.DeleteItemAsync(new DeleteItemRequest()
            {
                TableName = nameof(WeatherForecast),
                Key = new Dictionary<string, AttributeValue>
                {
                    { "CityName", new AttributeValue(cityName) },
                    { "Date", new AttributeValue(date) }
                },
                ConditionExpression = "TemperatureC > :limit",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":limit", new AttributeValue() { N = "20" } }
                }
            });
        }
    }
}