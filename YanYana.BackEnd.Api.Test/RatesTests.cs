using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Yanyana.BackEnd.Api.Controllers;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;
using Yanyana.BackEnd.Data.Context;

namespace Yanyana.BackEnd.Api.Tests.Controllers
{
    public class RatesControllerTests
    {
        private readonly Mock<IRateManager> _mockManager;

        public RatesControllerTests()
        {
            _mockManager = new Mock<IRateManager>();
        }

        [Fact]
        public async Task CreateRate_ReturnsCreatedAtAction()
        {
            var testRates = new List<Rate>()
            {
                new Rate {RateId = 1,Value = 3},
                new Rate {RateId = 2,Value = 3},

            };
            _mockManager.Setup(m => m.GetAllRatesAsync()).ReturnsAsync(testRates);
            var controller = new RatesController(_mockManager.Object);
            var result = await controller.GetAllRates();

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetRateById_ReturnsNotFound_WhenInvalidId()
        {

            _mockManager.Setup(m => m.GetRateByIdAsync(1)).ReturnsAsync((Rate)null);
            var controller = new RatesController(_mockManager.Object);
            var result = await controller.GetRateById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateRate_ReturnsBadRequest_WhenIdMismatch()
        {
            var controller = new RatesController(_mockManager.Object);
            var result = await controller.UpdateRate(2, new Rate { RateId = 1 });

            Assert.IsType<BadRequestResult>(result);
        }

        //[Fact]
        //public async Task GetRatesByPlaceId_ReturnsCorrectRates()
        //{
        //    var rates = new List<Rate> { new Rate { PlaceId = 1 } };
        //    var mockSet = MockDbSetHelper.CreateMockDbSet(rates);
        //    _mockContext.Setup(c => c.Rates).Returns(mockSet.Object);

        //    var result = await _controller.GetRatesByPlaceId(1);

        //    var okResult = Assert.IsType<OkObjectResult>(result.Result);
        //    Assert.Single((IEnumerable<Rate>)okResult.Value);
        //}
    }
}