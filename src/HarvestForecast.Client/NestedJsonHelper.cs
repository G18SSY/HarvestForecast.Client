using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HarvestForecast.Client;

internal static class NestedJsonHelper
{
    public static async ValueTask<T> UnwrapNestedJsonContentAsync<T>(HttpContent content, string containerPropertyName)
    {
        await using var contentStream = await content.ReadAsStreamAsync();
        var document = await JsonDocument.ParseAsync(contentStream);
        var entity = document.RootElement.GetProperty(containerPropertyName).Deserialize<T>();

        if (entity is null) throw new InvalidOperationException("Unable to deserialize response");

        return entity;
    }

    public static HttpContent CreateNestedJsonContent<T>(T content, string containerPropertyName)
    {
        var rawContent = JsonSerializer.Serialize(content, typeof(T));
        var json = "{\"" + containerPropertyName + "\":" + rawContent + "}";
        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}