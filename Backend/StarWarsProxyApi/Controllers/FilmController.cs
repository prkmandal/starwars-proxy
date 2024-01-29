using Microsoft.AspNetCore.Mvc;
using StarWars.Models;
using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;
using StarWarsServices.Interfaces;
using System.Net;

namespace StarWarsProxyApi.Controllers
{
    [ApiController]
    [Tags("Flims")]
    [Route("proxyapi/films")]
    public class FilmController : ControllerBase
    {
        private readonly ILogger<FilmController> _logger;
        private readonly IStarWarService<FilmModel> _filmService;
        public FilmController(ILogger<FilmController> logger,IStarWarService<FilmModel> filmService)
        {
            _logger = logger;
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
        [ProducesResponseType(typeof(List<FilmViewModel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string),500)]
        public async Task<IActionResult> GetFilms()
        {
            try
            {
                (HttpStatusCode,List<FilmModel>?) result = await _filmService.FetchAll(Common.FilmApi);
                if(result.Item1 == HttpStatusCode.OK && result.Item2 != null)
                {
                    List<FilmViewModel> viewModels = result.Item2.Select(film => film.Map()).ToList();
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
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> GetFilmById(int id)
        {
            try
            {
                (HttpStatusCode,FilmModel?) result = await _filmService.FetchById(Common.FilmApi, id);
                if (result.Item1==HttpStatusCode.OK && result.Item2 != null)
                {
                    FilmViewModel viewModel = result.Item2.Map();
                    return Ok(viewModel);
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
        /// Fetch selective films
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetFilmsByIds([FromBody] int[] ids)
        {
            try
            {
                var response = await _filmService.FetchByIds(Common.FilmApi, ids);

                return Ok(response);
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

    }

}
