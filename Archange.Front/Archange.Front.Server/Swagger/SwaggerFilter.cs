using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Mime;
using System.Reflection;

namespace ArchAnge.Front.Server.Swagger;

/// <summary>
/// Swagger filter
/// </summary>
public class SwaggerFilter(IList<OpenApiSecurityRequirement> requirements) : IDocumentFilter, ISchemaFilter, IParameterFilter
{
    #region Apply

    /// <summary>
    /// IDocumentFilter implementation
    /// </summary>
    /// <param name="swaggerDoc"> Swagger json document </param>
    /// <param name="context"> Document context </param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        CleanRequests(swaggerDoc.Paths.Values);
    }

    /// <summary>
    /// ISchemaFilter implementation
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        FilterEnum(context, schema);
        FilterProperties(schema);
    }

    /// <summary>
    /// Filter implementation
    /// </summary>
    /// <param name="parameter">  Rest parameter </param>
    /// <param name="context"> parameter info </param>
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        var exampleAttributes = new List<SwaggerExampleAttribute>();

        if (parameter.Name == SwaggerExampleAttribute.VersionAttributes.First().Name)
        {
            exampleAttributes.AddRange(SwaggerExampleAttribute.VersionAttributes);
        }
        AddFrom(exampleAttributes, context.PropertyInfo);
        AddFrom(exampleAttributes, context.ParameterInfo);
        AddExample(parameter, exampleAttributes);
    }

    #endregion

    #region IDocumentFilter

    private void CleanRequests(ICollection<OpenApiPathItem> requests)
    {
        foreach (var request in requests)
        {
            CleanOperations(request.Operations.Values);
        }
    }

    private void CleanOperations(ICollection<OpenApiOperation> operations)
    {
        foreach (var operation in operations)
        {
            operation.Security = requirements;
            CleanRequestBody(operation.RequestBody);
            CleanReponses(operation.Responses.Values);
        }
    }

    private static void CleanReponses(ICollection<OpenApiResponse> responses)
    {
        foreach (var response in responses)
        {
            CleanReponseContent(response);
        }
    }

    private static void CleanRequestBody(OpenApiRequestBody? requestBody)
    {
        if (requestBody != null)
        {
            CleanMediaType(requestBody.Content);
        }
    }

    private static void CleanReponseContent(OpenApiResponse? response)
    {
        if (response != null)
        {
            CleanMediaType(response.Content);
        }
    }

    private static void CleanMediaType(IDictionary<string, OpenApiMediaType> contents)
    {
        var appJson = MediaTypeNames.Application.Json;
        var jsonresult = contents.Where(content => content.Key == appJson).Select(content => content.Value).FirstOrDefault();

        if (jsonresult != null)
        {
            contents.Clear();
            contents.Add(appJson, jsonresult);
        }
    }

    #endregion

    #region ISchemaFilter

    private static void FilterEnum(SchemaFilterContext context, OpenApiSchema schema)
    {
        if (!context.Type.IsEnum)
            return;

        schema.Enum = [.. context.Type.GetEnumNames().Select(value => (IOpenApiAny)new OpenApiString(value))];
        schema.Type = "string";
        schema.Format = null;
    }

    private static void FilterProperties(OpenApiSchema schema)
    {
        var requiredProperties = schema.Required;

        foreach (var item in schema.Properties.Where(property => requiredProperties.Contains(property.Key)))
        {
            item.Value.Nullable = false;
        }
    }

    #endregion

    #region IParameterFilter

    private static void AddFrom<TAttribute>(List<TAttribute> attributes, ICustomAttributeProvider member)
        where TAttribute : Attribute
    {
        if (member != null)
        {
            attributes.AddRange(member.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>());
        }
    }

    private static void AddExample(OpenApiParameter parameter, IEnumerable<SwaggerExampleAttribute> exampleAttributes)
    {
        foreach (var item in exampleAttributes)
        {
            if (parameter.Examples.ContainsKey(item.Name))
            {
                continue;
            }
            var exemple = new OpenApiExample { Value = new OpenApiString(item.Value) };

            parameter.Examples.Add(item.Name, exemple);
        }
    }

    #endregion
}
