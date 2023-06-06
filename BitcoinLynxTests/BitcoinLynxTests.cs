using BitcoinLynx;
using Moq;
using Moq.Protected;
using System.Net;

namespace BitcoinLynxTests
{
    [TestClass]
    public class BitcoinLynxTests
    {       
        [TestMethod]
        public async Task testQueryEngine()
        {
            var content = new StringContent("[{'id':1,'value':'1'}]");
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = content,
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };
            QueryEngine queryEngine = new QueryEngine("http://www.google.com");
            queryEngine.client = httpClient;
            string apiReturns = await queryEngine.queryApi();
            Assert.AreEqual(apiReturns, "[{'id':1,'value':'1'}]");
        }
        
    }
}
