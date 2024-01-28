using StarWarsServices.Interfaces;
using System.Net.Http.Json;
using Microsoft.Extensions.Http;
using System.Net.Http;

using System.Collections.Concurrent;
using System.IO;
using Newtonsoft.Json;
using StarWars.Models.ServiceModel;
namespace StarWars.Services.Implementation
{
    public class StarWarService<T> : IStarWarService<T> where T : ReturnModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public StarWarService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("starWarsClient");
        }


        public async Task<List<T>> FetchAll(string api)
        {
            var reponse = await GetHttp(api);
            var data = JsonConvert.DeserializeObject<Entitys<T>>(reponse);
            return data.results;

        }

        public async Task<T> FetchById(string api, int id)
        {
            var reponse = await GetHttp($"{api}/{id}");
            var data = JsonConvert.DeserializeObject<T>(reponse);
            return data;
        }

        public async Task<List<T>> FetchByIds(string api, int[] arrayOfId)
        {
            var bag = new ConcurrentBag<T>();
            var tasks = arrayOfId.Select(async id =>
            {
                var response = await FetchById(api, id);
                bag.Add(response);
            });
            await Task.WhenAll(tasks);
            var count = bag.Count;
            return bag.ToList<T>();
        }

        private async Task<string> GetHttp(string api)
        {
            using (var response = await _httpClient.GetAsync(api, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStringAsync();
                return stream;
            }
        }
    }
}
