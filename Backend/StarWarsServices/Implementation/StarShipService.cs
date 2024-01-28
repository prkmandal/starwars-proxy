using StarWars.Models;
using StarWarsServices.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StarWars.Services.Implementation
{
    public class StarShipService : IStarWarService<StarshipModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public StarShipService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("starWarsClient");
        }


        private async Task<ConcurrentBag<StarshipModel>> ParallelGetCal(int[] ids)
        {
            var bag = new ConcurrentBag<StarshipModel>();
            var tasks = ids.Select(async item =>
            {
                var response = await FetchById(item);
                bag.Add(response);
            });
            await Task.WhenAll(tasks);
            var count = bag.Count;
            return bag;


        }

        public async Task<StarshipModel> FetchById(int id)
        {
            using (var response = await _httpClient.GetAsync($"films/{id}", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<StarshipModel>(stream);
            }
        }

        public async Task<List<StarshipModel>> FetchByIds(int[] arrayOfId)
        {
            var bag = await ParallelGetCal(arrayOfId);
            return bag.ToList<StarshipModel>();
        }

        public async Task<List<StarshipModel>> FetchAll()
        {
            using (var response = await _httpClient.GetAsync("films", HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var data = await JsonSerializer.DeserializeAsync<Entitys<StarshipModel>>(stream);
                return data.entity;
            }
        }
    }
}
