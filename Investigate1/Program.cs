using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, type, _) =>
    {
        // Remove null from enum values
        if (schema.Enum?.Count > 0)
        {
            var hasNull = schema.Enum.Any(e => e is null);
            if (hasNull)
            {
                var filtered = schema.Enum
                    .Where(e => e is not null)
                    .ToList();
                schema.Enum = filtered;
            }
        }
        return Task.CompletedTask;
    });
});


// Add services to the container.
builder.Services
    .AddControllers();
    //.AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
//builder.Services.Configure<JsonOptions>(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));


var app = builder.Build();
////////////////
app.MapOpenApi();
app.MapControllers();

app.UseHttpsRedirection();


app.Run();