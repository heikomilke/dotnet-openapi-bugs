using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


// Add services to the container.
builder.Services.AddControllers() .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));


var app = builder.Build();
////////////////
app.MapOpenApi();
app.MapControllers();

app.UseHttpsRedirection();


app.Run();