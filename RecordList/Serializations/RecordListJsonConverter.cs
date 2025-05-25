using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecordList.Serializations;

public class RecordListJsonConverter<T> : JsonConverter<RecordList<T>>
{
    public override RecordList<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var items = JsonSerializer.Deserialize<IEnumerable<T>>(ref reader, options);
        return items is null ? null : [.. items];
    }

    public override void Write(Utf8JsonWriter writer, RecordList<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.AsEnumerable(), options);
    }
}
