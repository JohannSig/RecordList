using System.Text.Json.Serialization;
using System.Text.Json;

namespace RecordList.Serializations
{
    public class RecordListJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(RecordList<>);

        public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
        {
            var elementType = type.GetGenericArguments()[0];
            var converterType = typeof(RecordListJsonConverter<>).MakeGenericType(elementType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }
}
