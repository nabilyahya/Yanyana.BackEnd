using Microsoft.VisualBasic;
using Xunit;

namespace Yanyana.BackEnd.Api.Test.TestComments
{
    [CollectionDefinition("WIZapp", DisableParallelization = true)]
    public class CommentControllerTests
    {
        [Collection("WIZapp")]
        public class WhenAddingCommetn 
        {
            private readonly HttpResponseMessage _response;
            public WhenAddingCommetn()
            {
                //example:
                //_response = client.GetAsync($"/api/v1/Indiener/{Constants.RoutingConstants.GetAanvrager}?meldingGuid={melding.MeldingGuid}").Result;
            }
            [Fact]
            public void ShouldBeOK()
            {
                //example:
                //_response.IsSuccessStatusCode.Should().BeTrue();
            }
        }
    }
}
