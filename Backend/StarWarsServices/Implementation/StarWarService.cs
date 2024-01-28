using Newtonsoft.Json;
using StarWars.Models.ServiceModel;
using StarWarsServices.Interfaces;
using System.Collections.Concurrent;
using System.Net;
namespace StarWars.Services.Implementation
{
    /// <summary>
    /// Generic service to reterive Films, StarShips and People
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StarWarService<T> : IStarWarService<T> where T : ReturnModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public StarWarService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("starWarsClient");
        }

        #region Public methods
        /// <summary>
        /// To Fetch all record from respective entity api
        /// </summary>
        /// <param name="api">Entity name</param>
        /// <returns>(HttpStatusCode, List<T>?)</returns>
        public async Task<(HttpStatusCode, List<T>?)> FetchAll(string api)
        {
            (HttpStatusCode, string) reponse = await GetHttp(api);
            if (reponse.Item1 == HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<Entitys<T>>(reponse.Item2);
                return (HttpStatusCode.OK, data?.results);
            }

            return (reponse.Item1, null);
        }

        /// <summary>
        /// To fetch one record based on id from respective entity api
        /// </summary>
        /// <param name="api">Entity name</param>
        /// <param name="id">id</param>
        /// <returns>(HttpStatusCode,T?)</returns>
        public async Task<(HttpStatusCode,T?)> FetchById(string api, int id)
        {
            var reponse = await GetHttp($"{api}/{id}");
            if (reponse.Item1 == HttpStatusCode.OK)
            {
                var data = JsonConvert.DeserializeObject<T>(reponse.Item2);
                return (HttpStatusCode.OK, data);
            }
           return (reponse.Item1,null);
        }

        /// <summary>
        /// To fetch multiple records based on ids from respective entity api
        /// </summary>
        /// <param name="api">entity name</param>
        /// <param name="arrayOfId">array of id</param>
        /// <returns>List<T></returns>
        public async Task<List<T>> FetchByIds(string api, int[] arrayOfId)
        {
            var bag = new ConcurrentBag<T>();
            var tasks = arrayOfId.Select(async id =>
            {
                var response = await FetchById(api, id);
                if(response.Item1 == HttpStatusCode.OK && response.Item2 != null)
                {
                    bag.Add(response.Item2);
                }
               
            });
            await Task.WhenAll(tasks);
            var count = bag.Count;
            return bag.ToList<T>();
        }

        #endregion

        #region Private methods
        private async Task<(HttpStatusCode, string)> GetHttp(string api)
        {
            using (var response = await _httpClient.GetAsync(api, HttpCompletionOption.ResponseHeadersRead))
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                return (response.StatusCode, responseStr);
            }

        }
        #endregion
    }
}
