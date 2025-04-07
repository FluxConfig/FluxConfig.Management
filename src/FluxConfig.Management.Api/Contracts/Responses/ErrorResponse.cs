using System.Net;

namespace FluxConfig.Management.Api.Contracts.Responses;

public record ErrorResponse(
    HttpStatusCode StatusCode,
    string? Message,
    IEnumerable<string> Exceptions
);