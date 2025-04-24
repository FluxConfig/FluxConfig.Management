using System.Globalization;
using FluxConfig.Management.Domain.Contracts.ISC.Interfaces;
using FluxConfig.Management.Domain.Exceptions.Domain.Config;
using FluxConfig.Management.Domain.Exceptions.Infrastructure;
using FluxConfig.Management.Domain.Models.Configuration;
using FluxConfig.Management.Domain.Models.Enums;
using FluxConfig.Management.Infrastructure.ISC.Extensions;
using FluxConfig.Management.Infrastructure.ISC.GrpcContracts.Storage;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace FluxConfig.Management.Infrastructure.ISC.Clients;

public class FluxConfigStorageClient : IFluxConfigStorageClient
{
    private readonly Storage.StorageClient _client;

    public FluxConfigStorageClient(Storage.StorageClient client)
    {
        _client = client;
    }

    public async Task CreateConfiguration(string key, string tag, CancellationToken cancellationToken)
    {
        try
        {
            await CreateConfigurationUnsafe(key, tag, cancellationToken);
        }
        catch (RpcException ex)
        {
            InfrastructureException iscException = ex.MapExceptionToIscException();
            throw iscException;
        }
    }

    private async Task CreateConfigurationUnsafe(string key, string tag, CancellationToken cancellationToken)
    {
        await _client.CreateServiceConfigurationAsync(
            request: new CreateServiceConfigRequest
            {
                ConfigurationKey = key,
                ConfigurationTag = tag
            },
            cancellationToken: cancellationToken
        );
    }

    public async Task DeleteConfiguration(string key, List<string> tags, CancellationToken cancellationToken)
    {
        try
        {
            await DeleteConfigurationUnsafe(key, tags, cancellationToken);
        }
        catch (RpcException ex)
        {
            InfrastructureException iscException = ex.MapExceptionToIscException();
            throw iscException;
        }
    }

    public async Task<IReadOnlyList<ConfigurationKeyValueType>> LoadConfigData(string key, string tag,
        ConfigurationDataType dataType, CancellationToken cancellationToken)
    {
        try
        {
            return await LoadConfigDataUnsafe(key, tag, dataType, cancellationToken);
        }
        catch (RpcException ex)
        {
            InfrastructureException iscException = ex.MapExceptionToIscException();
            throw iscException;
        }
    }

    private async Task<IReadOnlyList<ConfigurationKeyValueType>> LoadConfigDataUnsafe(string key, string tag,
        ConfigurationDataType dataType, CancellationToken cancellationToken)
    {
        LoadConfigResponse loadResponse;
        if (dataType == ConfigurationDataType.RealTime)
        {
            loadResponse = await _client.LoadRealTimeConfigAsync(
                request: new LoadConfigRequest
                {
                    ConfigurationKey = key,
                    ConfigurationTag = tag
                },
                cancellationToken: cancellationToken
            );
        }
        else
        {
            loadResponse = await _client.LoadVaultConfigAsync(
                request: new LoadConfigRequest
                {
                    ConfigurationKey = key,
                    ConfigurationTag = tag
                },
                cancellationToken: cancellationToken
            );
        }

        return ConvertValueToTypedList(loadResponse.ConfigurationData);
    }

    public async Task UpdateConfigData(string key, string tag, IReadOnlyList<ConfigurationKeyValueType> updatedData,
        ConfigurationDataType dataType,
        CancellationToken cancellationToken)
    {
        try
        {
            await UpdateConfigDataUnsafe(key, tag, updatedData, dataType, cancellationToken);
        }
        catch (RpcException ex)
        {
            InfrastructureException iscException = ex.MapExceptionToIscException();
            throw iscException;
        }
    }

    private async Task UpdateConfigDataUnsafe(string key, string tag,
        IReadOnlyList<ConfigurationKeyValueType> updatedData, ConfigurationDataType dataType,
        CancellationToken cancellationToken)
    {
        Value updatedValue = ConvertTypedListToValue(updatedData);

        UpdateConfigRequest updateRequest = new UpdateConfigRequest
        {
            ConfigurationKey = key,
            ConfigurationTag = tag,
            ConfigurationData = updatedValue
        };

        if (dataType == ConfigurationDataType.RealTime)
        {
            await _client.UpdateRTConfigAsync(
                request: updateRequest,
                cancellationToken: cancellationToken
            );
        }
        else
        {
            await _client.UpdateVaultConfigAsync(
                request: updateRequest,
                cancellationToken: cancellationToken
            );
        }
    }

    private async Task DeleteConfigurationUnsafe(string key, List<string> tags, CancellationToken cancellationToken)
    {
        DeleteServiceConfigRequest request = new DeleteServiceConfigRequest
        {
            ConfigurationKey = key
        };
        request.ConfigurationTags.AddRange(tags);

        await _client.DeleteServiceConfigurationAsync(
            request: request,
            cancellationToken: cancellationToken
        );
    }

    private IReadOnlyList<ConfigurationKeyValueType> ConvertValueToTypedList(Value value)
    {
        var list = new List<ConfigurationKeyValueType>();

        if (value.KindCase == Value.KindOneofCase.StructValue)
        {
            foreach (var field in value.StructValue.Fields)
            {
                list.Add(ProcessField(field.Key, field.Value));
            }
        }
        else
        {
            list.Add(ProcessField("value", value));
        }

        return list;
    }

    private Value ConvertTypedListToValue(IReadOnlyList<ConfigurationKeyValueType> list)
    {
        var structValue = new Struct();

        foreach (var item in list)
        {
            structValue.Fields[item.Key] = CreateValue(item);
        }

        return Value.ForStruct(structValue);
    }

    private Value CreateValue(ConfigurationKeyValueType item)
    {
        switch (item.Type)
        {
            case ConfigurationValueType.String:
                return Value.ForString(item.Value);

            case ConfigurationValueType.Number:
                if (!double.TryParse(item.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
                {
                    throw new ConfigurationDataInvalidTypeException(
                        message: "Invalid value.",
                        key: item.Key,
                        value: item.Value,
                        type: item.Type
                    );
                }
                
                return Value.ForNumber(number);

            case ConfigurationValueType.Bool:
                if (!bool.TryParse(item.Value, out var boolVal))
                {
                    throw new ConfigurationDataInvalidTypeException(
                        message: "Invalid value",
                        key: item.Key,
                        value: item.Value,
                        type: item.Type
                    );
                }
                
                return Value.ForBool(boolVal);
            
            case ConfigurationValueType.Null:
                return Value.ForNull();

            default:
                return Value.ForString(item.Value);
        }

        return new Value();
    }

    private ConfigurationKeyValueType ProcessField(string key, Value value)
    {
        return new ConfigurationKeyValueType
        {
            Key = key,
            Value = GetValueString(value),
            Type = GetTypeString(value.KindCase)
        };
    }

    private string GetValueString(Value value)
    {
        switch (value.KindCase)
        {
            case Value.KindOneofCase.StringValue:
                return value.StringValue;

            case Value.KindOneofCase.NumberValue:
                return value.NumberValue.ToString(CultureInfo.InvariantCulture);

            case Value.KindOneofCase.BoolValue:
                return value.BoolValue.ToString().ToLower();

            case Value.KindOneofCase.NullValue:
                return "null";

            default:
                return string.Empty;
        }
    }

    private ConfigurationValueType GetTypeString(Value.KindOneofCase kind)
    {
        return kind switch
        {
            Value.KindOneofCase.StringValue => ConfigurationValueType.String,
            Value.KindOneofCase.NumberValue => ConfigurationValueType.Number,
            Value.KindOneofCase.BoolValue => ConfigurationValueType.Bool,
            Value.KindOneofCase.NullValue => ConfigurationValueType.Null,
            _ => ConfigurationValueType.String
        };
    }
}