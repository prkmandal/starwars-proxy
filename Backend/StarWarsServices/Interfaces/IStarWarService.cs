using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsServices.Interfaces
{
    public interface IStarWarService<T> 
        {
        public Task<List<T>> FetchAll(string api);
        public Task<T> FetchById(string api, int id);

        public Task<List<T>> FetchByIds(string api,int[] arrayOfId);
       

    }
}
