using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using dynamodb_converters;
using dynamodb_converters.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Addding DynamoDB
var dynamoDbClient = new AmazonDynamoDBClient(FallbackCredentialsFactory.GetCredentials(),
                                                Amazon.RegionEndpoint.USEast1);
builder.Services.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);
var context = new DynamoDBContext(dynamoDbClient);
context.ConverterCache.Add(typeof(WeatherType), new WeatherTypeDynamoDbConverter());
context.ConverterCache.Add(typeof(Temperature), new TemperatureDynamoDbConverter());
builder.Services.AddSingleton<IDynamoDBContext>(context);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
