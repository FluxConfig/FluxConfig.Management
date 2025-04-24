using FluxConfig.Management.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FluxConfig.Management.Api.FiltersAttributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class ErrorResponseTypeAttribute : ProducesResponseTypeAttribute
{
    public ErrorResponseTypeAttribute(int statusCode) : base(typeof(ErrorResponse), statusCode)
    {
    }
}