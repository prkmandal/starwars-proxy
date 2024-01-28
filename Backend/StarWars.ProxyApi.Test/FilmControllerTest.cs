using Microsoft.AspNetCore.Mvc;
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
        private  Mock<IStarWarService<FilmModel>> filmServiceMock;
        private  FilmController filmController;

        public void Setup()
        {
            filmServiceMock = new Mock<IStarWarService<FilmModel>>();
            filmController = new FilmController(filmServiceMock.Object);
        }


        [Fact]
        public async Task GetFilms_ReturnsOkWithFilmViewModels()
        {
            // Arrange
            Setup();
            var filmModels = new List<FilmModel>() { new FilmModel{ Title="Test Film", Director = "Director", Starships = new List<string> { } } };
            filmServiceMock.Setup(s => s.FetchAll(Common.FilmApi)).ReturnsAsync((HttpStatusCode.OK, filmModels));

            // Act
            var result = await filmController.GetFilms();

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
            filmServiceMock.Setup(s => s.FetchById(Common.FilmApi, filmId)).ReturnsAsync((HttpStatusCode.OK, filmModel));

            // Act
            var result = await filmController.GetFilmById(filmId);

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
            filmServiceMock.Setup(s => s.FetchById(Common.FilmApi, filmId)).ReturnsAsync((HttpStatusCode.NotFound, (FilmModel?)null));

            // Act
            var result = await filmController.GetFilmById(filmId);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }
    }
}