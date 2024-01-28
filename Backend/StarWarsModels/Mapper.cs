using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWars.Models
{
    public static class Mapper
    {
        public static FilmViewModel Map(this FilmModel film)
        {
            return new FilmViewModel
            {
                Id = int.TryParse(film.Url.Replace(Common.FilmFullUrl, "").TrimEnd('/'), out int id)? id: 0,
                Title = film.Title,
                Episode_id = film.Episode_id,
                Opening_crawl = film.Opening_crawl,
                Director = film.Director,
                Producer = film.Producer,
                Rlease_date = DateTime.TryParseExact(film.Rlease_date, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime) ? dateTime : null,

                Starships = film.Starships.Select(url =>
                {
                    string modifiedUrl = url.Replace(Common.StarShipFullUrl, "").TrimEnd('/');
                    return int.TryParse(modifiedUrl, out int itemId) ? itemId : 0;
                }).ToArray()
            };
        }

        public static StarShipViewModel Map(this StarshipModel starship)
        {
            return new StarShipViewModel
            {
                Id = Convert.ToInt16(starship.Url.Replace(Common.StarShipFullUrl, "").TrimEnd('/')),
                Name = starship.Name,
                Model = starship.Model,
                Manufacturer = starship.Manufacturer,
                Consumables = starship.Consumables,
                Films = starship.Films.Select(url =>
                {
                    string modifiedUrl = url.Replace(Common.FilmFullUrl, "").TrimEnd('/');
                    return int.TryParse(modifiedUrl, out int itemId) ? itemId : 0;
                }).ToArray()
            };
        }
    }
}
