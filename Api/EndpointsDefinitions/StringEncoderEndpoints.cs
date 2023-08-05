using Api.Abstractions;
using Infrastructure.Abstractions;

namespace Api.EndpointsDefinitions;

public class StringEncoderEndpoints : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var stroke = app.MapGroup("api/v1/string-encode");

        stroke.MapDelete("/{sessionId}", CancelEncodeRequest);
    }

    public async Task<IResult> CancelEncodeRequest(ISessionOperationService sessionService, string sessionId)
    {
        if (await sessionService.EndOperationSession(sessionId: Guid.Parse(sessionId)))
        {
            return TypedResults.Ok();
        }

        return TypedResults.BadRequest("Cannot end session");
    }
}