namespace StarWars.Models
{
    public class Common
    {
        public static string FilmApi = "films/";

        public static string StarShipApi = "starships/";
        public static string StarWarServiceBaseUrl = "https://swapi.dev/api/";

        public static string FilmFullUrl = $"{StarWarServiceBaseUrl}{FilmApi}";
        public static string StarShipFullUrl = $"{StarWarServiceBaseUrl}{StarShipApi}";


    }
}
