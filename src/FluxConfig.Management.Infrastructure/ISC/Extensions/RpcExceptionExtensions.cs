using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Exceptions.Infrastructure.ISC;
using Google.Rpc;
using Grpc.Core;

namespace FluxConfig.Management.Infrastructure.ISC.Extensions;

internal static class RpcExceptionExtensions
{
    internal static InfrastructureException MapExceptionToIscException(this RpcException ex)
    {
        InfrastructureException exception = ex.StatusCode switch
        {
            StatusCode.AlreadyExists => MapToAlreadyExistsException(ex),
            StatusCode.Unauthenticated => MapToAuthException(ex),
            StatusCode.NotFound => MapToNotFoundException(ex),
            StatusCode.InvalidArgument => MapToBrException(ex),
            _ => new FcStorageResponseException(
                message: "Unexpected response from FC Storage service",
                statusCode: (int)ex.StatusCode,
                innerException: ex
            )
        };

        return exception;
    }

    private static FcStorageBadRequestException MapToBrException(RpcException ex)
    {
        var badRequest = ex.GetRpcStatus()?.GetDetail<BadRequest>();
        var rpcViolations = badRequest?.FieldViolations.ToDictionary(x => x.Field, x => x.Description) ??
                            new Dictionary<string, string>();

        return new FcStorageBadRequestException(
            message: ex.Message,
            filedViolations: rpcViolations,
            innerException: ex
        );
    }

    private static FcStorageNotFoundException MapToNotFoundException(RpcException ex)
    {
        var detailedException = ex.GetRpcStatus()?.GetDetail<ErrorInfo>();

        return new FcStorageNotFoundException(
            message: ex.Message,
            key: detailedException?.Metadata["key"] ?? "",
            tag: detailedException?.Metadata["tag"] ?? "",
            innerException: ex
        );
    }

    private static FcStorageInternalApiKeyUnauthenticatedException MapToAuthException(RpcException ex)
    {
        var detailedException = ex.GetRpcStatus()?.GetDetail<ErrorInfo>();

        return new FcStorageInternalApiKeyUnauthenticatedException(
            message: ex.Message,
            invalidApiKey: detailedException?.Metadata["x-api-key"] ?? "",
            innerException: ex
        );
    }

    private static FcStorageAlreadyExistsException MapToAlreadyExistsException(RpcException ex)
    {
        var detailedException = ex.GetRpcStatus()?.GetDetail<ErrorInfo>();

        return new FcStorageAlreadyExistsException(
            message: ex.Message,
            tag: detailedException?.Metadata["tag"] ?? "",
            innerException: ex
        );
    }
}