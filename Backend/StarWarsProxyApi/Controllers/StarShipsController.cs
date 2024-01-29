using Microsoft.AspNetCore.Mvc;
using StarWars.Models;
using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;
using StarWarsServices.Interfaces;
using System.IO;
using System.Net;

namespace StarWarsProxyApi.Controllers
{
    [ApiController]
    [Tags("StarShips")]
    [Route("proxyapi/starships")]
    public class StarShipController : Controller
    {
        private readonly ILogger<StarShipController> _logger;
        private readonly IStarWarService<StarshipModel> _starShipService;
        public StarShipController(ILogger<StarShipController> logger,IStarWarService<StarshipModel> starShipService)
        {
            _logger = logger;
            _starShipService = starShipService;
        }

        /// <summary>
        /// Fetch all star wars Starships
        /// </summary>
        /// <remarks>This api use for to get all star wars starships along with basic deatils</remarks>
        /// <response code="200">Success</response>
        /// <response code="204">No contect</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<StarShipViewModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetStarShips()
        {
            try
            {
                (HttpStatusCode, List<StarshipModel>?) result = await _starShipService.FetchAll(Common.StarShipApi);
                if (result.Item1 == HttpStatusCode.OK && result.Item2 != null)
                {
                    List<StarShipViewModel> viewModels = result.Item2.Select(starship => starship.Map()).ToList();
                    return Ok(viewModels);
                }
                return StatusCode((int)result.Item1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Internal Server Error",
                    Detail = ex.Message
                };
                return new ObjectResult(problemDetails)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }

        /// <summary>
        /// Fetch a star wars starships
        /// </summary>
        /// <remarks>Api for particular starship details</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal server error</response>

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(StarShipViewModel), 200)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetStarShipById(int id)
        {
            try
            {
                (HttpStatusCode, StarshipModel?) result = await _starShipService.FetchById(Common.StarShipApi, id);
                if (result.Item1 == HttpStatusCode.OK && result.Item2 != null)
                {
                    StarShipViewModel viewModel = result.Item2.Map();
                    return Ok(viewModel);
                }
                return StatusCode((int)result.Item1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Fetch selective films
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetStarShipsByIds([FromBody] int[] ids)
        {
            try
            {
                var response = await _starShipService.FetchByIds(Common.StarShipApi, ids);
                if (response == null)
                {
                    return NotFound();
                }

                List<StarShipViewModel> viewModels = response.Select(starship => starship.Map()).ToList();
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

    }
}

