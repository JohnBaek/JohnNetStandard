using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JohnIsDev.Core.Features.Filters;


/// <summary>
/// 스웨거에서 [JsonIgnore] 를 처리하기위한 필터 
/// </summary>
public class IgnorePropertyFilter : IOperationFilter 
{
    /// <summary>
    /// 필터적용
    /// </summary>
    /// <param name="operation">실행</param>
    /// <param name="context">실행 필터</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 값이 유효하지 않은경우 
        if (context.ApiDescription == null || operation.Parameters == null)
            return;

        // 값이 유효하지 않은경우 
        if (!context.ApiDescription.ParameterDescriptions.Any())
            return;
        
        context.ApiDescription.ParameterDescriptions.Where(p => 
                p.Source.Equals(BindingSource.Form) && 
                p.CustomAttributes().Any(p => p.GetType().Equals(typeof(JsonIgnoreAttribute)))
            )
            .ToList()
            .ForEach(p => operation.RequestBody.Content.Values
                .Single(v => v.Schema.Properties.ContainsKey(p.Name)) 
                .Schema.Properties.Remove(p.Name)); 

        context.ApiDescription.ParameterDescriptions.Where(p => 
                p.Source.Equals(BindingSource.Query) && 
                p.CustomAttributes().Any(p => p.GetType().Equals(typeof(JsonIgnoreAttribute))))
            .ToList()
            .ForEach(p => 
                operation.Parameters.Remove(operation.Parameters.Single(w => w.Name.Equals(p.Name))));
    }
}