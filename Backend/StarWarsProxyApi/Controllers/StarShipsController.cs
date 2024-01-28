using Microsoft.AspNetCore.Mvc;
using StarWars.Models;
using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;
using StarWarsServices.Interfaces;
using System.IO;

namespace StarWarsProxyApi.Controllers
{
    [ApiController]
    [Tags("StarShips")]
    [Route("proxyapi/starships")]
    public class StarShipController : Controller
    {
        private readonly IStarWarService<StarshipModel> _starShipService;
        public StarShipController(IStarWarService<StarshipModel> starShipService)
        {
            _starShipService = starShipService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStarShips()
        {
            var starshipModels = await _starShipService.FetchAll(Common.StarShipApi);
            if (starshipModels == null)
            {
                return NoContent();
            }

            List<StarShipViewModel> viewModels = starshipModels.Select(starship => starship.Map()).ToList();
            return Ok(viewModels);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetStarShipById(int id)
        {
            var response = await _starShipService.FetchById(Common.StarShipApi, id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetStarShipsByIds([FromBody] int[] ids)
        {
            var response = await _starShipService.FetchByIds(Common.StarShipApi, ids);
            if (response == null)
            {
                return NoContent();
            }

            List<StarShipViewModel> viewModels = response.Select(starship => starship.Map()).ToList();
            return Ok(viewModels);
        }

    }
}

