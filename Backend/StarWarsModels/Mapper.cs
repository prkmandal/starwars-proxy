using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;

namespace StarWars.Models
{
    public static class Mapper
    {
        /// <summary>
        /// Extension method to convert FilmModel to FilmViewModel
        /// </summary>
        /// <param name="film">FilmModel</param>
        /// <returns>FilmViewModel</returns>
        public static FilmViewModel Map(this FilmModel film)
        {
            return new FilmViewModel
            {
                Id = int.TryParse(film.Url?.Replace(Common.FilmFullUrl, "").TrimEnd('/'), out int id)? id: 0,
                Title = film.Title,
                Episode_id = film.Episode_id,
                Opening_crawl = film.Opening_crawl,
                Director = film.Director,
                Producer = film.Producer,
                //Rlease_date = DateTime.TryParseExact(film.Rlease_date, "dd/MM/yyyy HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime) ? dateTime : null,

                Starships = film.Starships.Select(url =>
                {
                    string? modifiedUrl = url?.Replace(Common.StarShipFullUrl, "").TrimEnd('/');
                    return int.TryParse(modifiedUrl, out int itemId) ? itemId : 0;
                }).ToArray()
            };
        }

        /// <summary>
        /// Extension method to convert StarshipModel to StarShipViewModel
        /// </summary>
        /// <param name="starship">StarshipModel</param>
        /// <returns>StarShipViewModel</returns>
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
