using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using dynamodb_converters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace dynamodb_converters.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        
        private readonly ILogger<WeatherForecastController> _logger;
        public readonly IDynamoDBContext _dynamoDBContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
                                        IDynamoDBContext dynamoDBContext)
        {
            _logger = logger;
            _dynamoDBContext = dynamoDBContext;
        }        

        [HttpGet(Name = "get-by-date")]
        public async Task<WeatherForecast> GetByDateAsync(string cityName,DateTime dateTime)
        {
            return await _dynamoDBContext.LoadAsync<WeatherForecast>(cityName, dateTime);
        }

        [HttpPost]
        public async Task Post(WeatherForecast weatherForecast)
        {
            weatherForecast.Date = weatherForecast.Date.Date;
            await _dynamoDBContext.SaveAsync(weatherForecast);
        }
    }
}