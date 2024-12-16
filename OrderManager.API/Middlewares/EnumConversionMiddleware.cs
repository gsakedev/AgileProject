using System.Text;
using System.Text.Json;

namespace OrderManager.API.Middlewares
{
    public class EnumConversionMiddleware
    {
        private readonly RequestDelegate _next;

        public EnumConversionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the request is JSON
            if (context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();

                // Read the request body
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

                if (!string.IsNullOrEmpty(requestBody))
                {
                    // Deserialize into a dynamic object to detect and convert enums
                    var jsonDocument = JsonDocument.Parse(requestBody);
                    var rootElement = jsonDocument.RootElement;

                    // Convert delivery option to enum, if applicable
                    if (rootElement.TryGetProperty("DeliveryOption", out var deliveryOptionProp))
                    {
                        var deliveryOptionString = deliveryOptionProp.GetString();
                        if (!Enum.TryParse<Domain.Enums.DeliveryOption>(deliveryOptionString, true, out var deliveryOption))
                        {
                            throw new ArgumentException($"Invalid DeliveryOption: {deliveryOptionString}");
                        }

                        // Modify the JSON with the converted enum
                        var modifiedBody = JsonSerializer.Serialize(new
                        {
                            rootElement,
                            DeliveryOption = deliveryOption
                        });

                        var bytes = Encoding.UTF8.GetBytes(modifiedBody);

                        // Replace the request body with the modified JSON
                        context.Request.Body = new MemoryStream(bytes);
                    }
                }

                // Reset the request body position so the next middleware can read it
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            await _next(context);
        }
    }
}
