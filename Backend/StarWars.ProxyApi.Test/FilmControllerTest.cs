using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using StarWars.Models;
using StarWars.Models.ServiceModel;
using StarWars.Models.ViewModel;
using StarWarsProxyApi.Controllers;
using StarWarsServices.Interfaces;
using System.Net;

namespace StarWars.ProxyApi.Test
{
    public class FilmControllerTests
    {
        private  Mock<IStarWarService<FilmModel>> _filmServiceMock;
        private  FilmController _filmController;
        private Mock<ILogger<FilmController>> _mockLogger ;

        public void Setup()
        {
            _filmServiceMock = new Mock<IStarWarService<FilmModel>>();
            _mockLogger = new Mock<ILogger<FilmController>>();
            _filmController = new FilmController(_mockLogger.Object,_filmServiceMock.Object );
            
        }


        [Fact]
        public async Task GetFilms_ReturnsOkWithFilmViewModels()
        {
            // Arrange
            Setup();
            var filmModels = new List<FilmModel>() { new FilmModel{ Title="Test Film", Director = "Director", Starships = new List<string> { } } };
            _filmServiceMock.Setup(s => s.FetchAll(Common.FilmApi)).ReturnsAsync((HttpStatusCode.OK, filmModels));

            // Act
            var result = await _filmController.GetFilms();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<List<FilmViewModel>>(okResult.Value);
         
        }

        [Fact]
        public async Task GetFilmById_ReturnsOkWithFilmViewModel()
        {
            // Arrange
            Setup();
            var filmId = 1;
            var filmModel = new FilmModel { Title = "Test Film", Director = "Director", Starships = new List<string> { } };
            _filmServiceMock.Setup(s => s.FetchById(Common.FilmApi, filmId)).ReturnsAsync((HttpStatusCode.OK, filmModel));

            // Act
            var result = await _filmController.GetFilmById(filmId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<FilmViewModel>(okResult.Value);
        }

        [Fact]
        public async Task GetFilmById_ReturnsNotFound()
        {
            // Arrange
            Setup();
            var filmId = 1;
            _filmServiceMock.Setup(s => s.FetchById(Common.FilmApi, filmId)).ReturnsAsync((HttpStatusCode.NotFound, (FilmModel?)null));

            // Act
            var result = await _filmController.GetFilmById(filmId);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }
    }
}