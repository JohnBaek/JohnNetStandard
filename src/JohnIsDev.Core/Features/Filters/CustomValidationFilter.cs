using System.Text;
using JohnIsDev.Core.Models.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JohnIsDev.Core.Features.Filters;

/// <summary>
/// Validation Filter
/// </summary>
public class CustomValidationFilter : IActionFilter
{
    /// <summary>
    /// OnActionExecuting
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            string? errorMessage = context.ModelState
                .Where(ms => ms is { Value.Errors.Count: > 0, Key: "Password" })
                .Select(ms => ms.Value?.Errors.First().ErrorMessage)
                .FirstOrDefault();


            StringBuilder errorBuilder = new StringBuilder();
            foreach (var modelStateValue in context.ModelState.Values)
            {
                foreach (var modelError in modelStateValue.Errors)
                {
                    errorBuilder.AppendLine(modelError.ErrorMessage);
                }
            }
            
            var errorResponse = new
            {
                Code = "" ,
                Result = EnumResponseResult.Error ,
                Message = errorBuilder.ToString()
            };

            context.Result = new JsonResult(errorResponse)
            {
                StatusCode = 200
            };
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}