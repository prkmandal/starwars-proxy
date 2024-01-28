using Microsoft.AspNetCore.Mvc;
using StarWars.Models;
using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;
using StarWars.Services;
using StarWarsServices.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarWarsProxyApi.Controllers
{
    [ApiController]
    [Tags("Flims")]
    [Route("proxyapi/films")]
    public class FilmController: ControllerBase
    {
        private readonly IStarWarService<FilmModel> _filmService;
        public FilmController(IStarWarService<FilmModel> filmService)
        {
            _filmService = filmService;
        }

        /// <summary>
        /// Fetch all star wars films
        /// </summary>
        /// <remarks>This api use for to get all star wars films along with basic deatils</remarks>
        /// <response code="200">Success</response>
        /// <response code="204">No contect</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        public async Task<IActionResult> GetFilms()
        {
            try
            {
                List<FilmModel> filmModels = await _filmService.FetchAll(Common.FilmApi);
                if (filmModels == null)
                {
                    return NoContent();
                }

                List<FilmViewModel> viewModels = filmModels.Select(film=> film.Map()).ToList();
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Fetch a star wars film
        /// </summary>
        /// <remarks>Api for particular film details</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(FilmViewModel), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFilmById(int id)
        {
            try
            {
                FilmModel filmModel = await _filmService.FetchById(Common.FilmApi, id);
                FilmViewModel viewModel = filmModel.Map();
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> GetFilmsByIds([FromBody] int[] ids)
        {
            var response = await _filmService.FetchByIds(Common.FilmApi, ids);
            return Ok(response);
        }

    }

}
