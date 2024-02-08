using CQRS.Core.Events;
using System.Text.Json.Serialization;
using System.Text.Json;
using Lease.Common.Events;

namespace Lease.Query.Infrastructure.Convertors
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type type)
        {
            return type.IsAssignableFrom(typeof(BaseEvent));
        }

        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
            }

            if (!doc.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException("Could not detect the Type discriminator property!");
            }

            var typeDiscriminator = type.GetString();
            var json = doc.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                nameof(LeaseCreatedEvent) => JsonSerializer.Deserialize<LeaseCreatedEvent>(json, options),
                nameof(LeaseSentEvent) => JsonSerializer.Deserialize<LeaseSentEvent>(json, options),
                nameof(LeaseSignedEvent) => JsonSerializer.Deserialize<LeaseSignedEvent>(json, options),
                nameof(LeaseEditedEvent) => JsonSerializer.Deserialize<LeaseEditedEvent>(json, options),
                nameof(LeaseCancelledEvent) => JsonSerializer.Deserialize<LeaseCancelledEvent>(json, options),
                _ => throw new JsonException($"{typeDiscriminator} is not supported yet!")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
