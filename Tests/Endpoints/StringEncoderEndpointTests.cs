using Api.EndpointsDefinitions;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Tests.Endpoints;

public class StringEncoderEndpointTests
{
    [Fact]
    public async Task CancelEncodeRequest_ValidSessionId_ReturnsStatusCode200()
    {
        //Arrange
        var expectedResult = StatusCodes.Status200OK;
        var sessionService = Substitute.For<ISessionOperationService>();
        var sessionId = Guid.NewGuid();
        sessionService.EndOperationSession(Arg.Any<Guid>()).Returns(true);

        //Act
        var stringEncoder = new StringEncoderEndpoints();
        var actualResult = (Ok)await stringEncoder.CancelEncodeRequest(sessionService, sessionId.ToString());

        //Arrange
        Assert.Equal(expectedResult, actualResult.StatusCode);
        
    }
    
    [Fact]
    public async Task CancelEncodeRequest_ValidSessionId_ReturnsStatusCode400()
    {
        //Arrange
        var expectedResult = StatusCodes.Status400BadRequest;
        var sessionService = Substitute.For<ISessionOperationService>();
        var sessionId = Guid.NewGuid();
        sessionService.EndOperationSession(Arg.Any<Guid>()).Returns(false);

        //Act
        var stringEncoder = new StringEncoderEndpoints();
        var actualResult = (BadRequest<string>)await stringEncoder.CancelEncodeRequest(sessionService, sessionId.ToString());

        //Arrange
        Assert.Equal(expectedResult, actualResult.StatusCode);
        
    }
    
    
}