using System.Net;

namespace StarWarsServices.Interfaces
{
    public interface IStarWarService<T>
    {
        public Task<(HttpStatusCode, List<T>?)> FetchAll(string api);
        public Task<(HttpStatusCode, T?)> FetchById(string api, int id);

        public Task<List<T>> FetchByIds(string api, int[] arrayOfId);


    }
}
