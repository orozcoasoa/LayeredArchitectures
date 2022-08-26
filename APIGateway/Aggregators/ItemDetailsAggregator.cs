using System.Net;
using System.Text.Json;
using CatalogService.BLL.Entities;
using Microsoft.AspNetCore.Http;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

namespace APIGateway.Aggregators
{
    public class ItemDetailsAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            if (responses.Count != 2)
                throw new ArgumentException(nameof(ItemDetailsAggregator) +
                    " only allows to reponses, wrong setup in ocelot.json", nameof(responses));

            var contentDict = new Dictionary<string, object>();
            var headers = new List<Header>();
            foreach (var response in responses)
            {
                var content = await GetResponseContent(response);
                var baseRoute = response.Items.DownstreamRoute();
                object baseObj;
                if (baseRoute.Key.Contains("details"))
                    baseObj = JsonSerializer.Deserialize<ItemDetails>(content,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                else
                    baseObj = JsonSerializer.Deserialize<Item>(content,
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                contentDict[baseRoute.Key] = baseObj;
                MergeHeaders(response, headers);

            }
            headers.Add(new Header("Content-Type", new List<string>() { "application/json" }));
            return new DownstreamResponse(
                new StringContent(JsonSerializer.Serialize(contentDict)),
                HttpStatusCode.OK,
                headers,
                "reason");

        }
        private async Task<string> GetResponseContent(HttpContext context)
        {
            var body = "";
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                var req = context.Items.DownstreamRequest().ToHttpRequestMessage();
                using (var client = new HttpClient())
                {
                    var tempResponse = await client.GetAsync(req.RequestUri);
                    if (tempResponse.IsSuccessStatusCode)
                        body = await tempResponse.Content.ReadAsStringAsync();
                }
            }
            else
                body = await context.Items.DownstreamResponse().Content.ReadAsStringAsync();
            return body;
        }
        private void MergeHeaders(HttpContext context, List<Header> headers)
        {
            var resp = context.Items.DownstreamResponse();
            foreach (var hdr in resp.Headers)
            {
                if (headers.Exists(h => h.Key == hdr.Key))
                {
                    var currHdrIdx = headers.FindIndex(h => h.Key == hdr.Key);
                    var values = headers[currHdrIdx].Values;
                    foreach (var val in hdr.Values)
                        if (!values.Contains(val))
                            values = values.Append(val);

                    headers[currHdrIdx] = new Header(hdr.Key, values);
                }
                else
                    headers.Add(hdr);
            }
        }

    }
}
