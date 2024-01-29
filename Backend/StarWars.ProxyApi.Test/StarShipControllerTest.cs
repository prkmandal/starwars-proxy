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
    public class StarShipControllerTest
    {
        private  Mock<IStarWarService<StarshipModel>> _starShipServiceMock ;
        private Mock<ILogger<StarShipController>> _mockLogger;
        private  StarShipController _starShipController ;

        public void Setup()
        {
            _starShipServiceMock = new Mock<IStarWarService<StarshipModel>>();
            _mockLogger = new Mock<ILogger<StarShipController>>();
            _starShipController = new StarShipController(_mockLogger.Object,_starShipServiceMock.Object);
        }


        [Fact]
        public async Task GetStarShip_ReturnsOkWithStarShipsViewModels()
        {
            // Arrange
            Setup();
            var starshipModels = new List<StarshipModel>() { new StarshipModel { Name = "Test Film", Model = "Director", Films = new List<string> { } } };
            _starShipServiceMock.Setup(s => s.FetchAll(Common.StarShipApi)).ReturnsAsync((HttpStatusCode.OK, starshipModels));

            // Act
            var result = await _starShipController.GetStarShips();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<List<StarShipViewModel>>(okResult.Value);

        }

        [Fact]
        public async Task GetStarShipById_ReturnsOkWithStarShipViewModel()
        {
            // Arrange
            Setup();
            var shipId = 1;
            var filmModel = new StarshipModel { Name = "Test ", Model = "TEst model", Films = new List<string> { } };
            _starShipServiceMock.Setup(s => s.FetchById(Common.StarShipApi, shipId)).ReturnsAsync((HttpStatusCode.OK, filmModel));

            // Act
            var result = await _starShipController.GetStarShipById(shipId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<StarShipViewModel>(okResult.Value);
        }

        [Fact]
        public async Task GetFilmById_ReturnsNotFound()
        {
            // Arrange
            Setup();
            var starshipId = 1;
            _starShipServiceMock.Setup(s => s.FetchById(Common.StarShipApi, starshipId)).ReturnsAsync((HttpStatusCode.NotFound, (StarshipModel?)null));

            // Act
            var result = await _starShipController.GetStarShipById(starshipId);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var objectResult = (StatusCodeResult)result;
            Assert.Equal((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }
    }
}
