using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using StarWars.Models.ServiceModel;
using StarWars.Services.Implementation;
using System.Net;

namespace StarWars.Services.Test
{
    public class StarWarServiceTest
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        public StarWarServiceTest()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        }


        [Fact]
        public async Task FetchAll_Films_ValidApiResponse_ReturnsData()
        {
            // Arrange
            var api = "films";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new Entitys<FilmModel> { results = new List<FilmModel> { new FilmModel() } }))
            };

            var _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                  ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(_httpMessageHandler.Object);

            httpClient.BaseAddress = new Uri("http://nonexisting.domain"); 

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var starWarService = new StarWarService<FilmModel>(_httpClientFactoryMock.Object);

            // Act
            var result = await starWarService.FetchAll(api);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Item1);
            Assert.NotNull(result.Item2);
            Assert.Single(result.Item2);
        }

        [Fact]
        public async Task FetchById_Film_ValidApiResponse_ReturnsData()
        {
            // Arrange
            var api = "films";
            var id = 1;
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new FilmModel()))
            };

            var _httpMessageHandler = new Mock<HttpMessageHandler>();
            _httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                  ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                  ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(_httpMessageHandler.Object);

            httpClient.BaseAddress = new Uri("http://nonexisting.domain");

            _httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var starWarService = new StarWarService<FilmModel>(_httpClientFactoryMock.Object);


            // Act
            var result = await starWarService.FetchById(api, id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.Item1);
            Assert.NotNull(result.Item2);
        }

    }
}