using OrderManager.Domain.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OrderManager.Application.Helpers
{
    public class DeliveryOptionJsonConverter : JsonConverter<DeliveryOption>
    {
        public override DeliveryOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();

            if (Enum.TryParse<DeliveryOption>(stringValue, true, out var deliveryOption))
            {
                return deliveryOption;
            }

            throw new JsonException($"Invalid value '{stringValue}' for enum '{nameof(DeliveryOption)}'");
        }

        public override void Write(Utf8JsonWriter writer, DeliveryOption value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
