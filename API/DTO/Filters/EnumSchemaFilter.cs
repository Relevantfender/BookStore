﻿
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
//necessary filter so the enum isn't shown as the integer, but as its string enum values
public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum.Clear(); 
            schema.Type = "string";
            schema.Format = null;

            var enumValues = Enum.GetNames(context.Type);
            foreach (var value in enumValues)
                schema.Enum.Add(new OpenApiString(value));
        }
    }
}
