# OpenAPI Enum Schema Generation Issue

This project demonstrates an issue with how .NET's OpenAPI schema generator handles enum types and why a schema transformer is required to fix it.


** IMPORTANT **

We do not encourage you to use this fix. This is just meant as a workaround and to show the problem exists!

## The Problem

When .NET generates an OpenAPI schema for an enum type that is used in both nullable and non-nullable contexts, it includes `null` in the enum value definition for all usages of that enum.

### Example

Given these C# types:

```csharp
public enum Color
{
    Green,
    Yellow
}

public class ColorRequest
{
    public Color? Here { get; set; }  // Nullable enum
}

public class ColorResponse
{
    public required Color There { get; init; }  // Non-nullable enum
}
```

**Without the transformer**, the generated OpenAPI schema incorrectly produces:

```json
"Color": {
  "enum": ["Green", "Yellow", null]  // ❌ null should not be here
}
```

This means the OpenAPI contract allows `null` for the `Color` enum in **all** contexts, including the `ColorResponse.There` field which should be required and non-nullable. This is misleading for API clients.

## The Solution

A schema transformer in `Program.cs` filters out `null` values from enum definitions:

```csharp
builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, type, _) =>
    {
        // Remove null from enum values
        if (schema.Enum?.Count > 0)
        {
            var filtered = schema.Enum
                .Where(e => e is not null)
                .ToList();
            schema.Enum = filtered;
        }
        return Task.CompletedTask;
    });
});
```

**With the transformer**, the schema correctly produces:

```json
"Color": {
  "enum": ["Green", "Yellow"]  // ✅ null removed
}
```

Nullable contexts are still properly handled using OpenAPI's `oneOf` pattern:

```json
"ColorRequest": {
  "properties": {
    "here": {
      "oneOf": [
        { "type": "null" },
        { "$ref": "#/components/schemas/Color" }
      ]
    }
  }
}
```

This correctly indicates that `ColorRequest.Here` can be either `null` OR a valid enum value, while `ColorResponse.There` must be a valid enum value.

## Additional Configuration

The enum is also configured to serialize as a string in JSON using the `[JsonConverter]` attribute:

```csharp
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Color
{
    Green,
    Yellow
}
```

This ensures enum values are serialized as `"Green"` instead of `0` in JSON responses.
