// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace Microsoft.Marketplace.Metering.Models
{
    public partial class BatchUsageEventOkResponse
    {
        internal static BatchUsageEventOkResponse DeserializeBatchUsageEventOkResponse(JsonElement element)
        {
            Optional<IReadOnlyList<UsageBatchEventOkMessage>> result = default;
            Optional<int> count = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("result"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    List<UsageBatchEventOkMessage> array = new List<UsageBatchEventOkMessage>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(UsageBatchEventOkMessage.DeserializeUsageBatchEventOkMessage(item));
                    }
                    result = array;
                    continue;
                }
                if (property.NameEquals("count"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    count = property.Value.GetInt32();
                    continue;
                }
            }
            return new BatchUsageEventOkResponse(Optional.ToList(result), Optional.ToNullable(count));
        }
    }
}
