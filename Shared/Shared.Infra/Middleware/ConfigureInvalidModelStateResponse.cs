using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared.Infra.DTO;

public class ConfigureInvalidModelStateResponse : IConfigureOptions<ApiBehaviorOptions>
{
    public void Configure(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .SelectMany(kvp => kvp.Value!.Errors.Select(error => new ModelStateError
                {
                    Field = kvp.Key,
                    Message = error.ErrorMessage
                }))
                .ToList();

            var apiResponse = new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid Request",
                Error = errors.Any() ? errors : new List<ModelStateError>()
            };

            return new BadRequestObjectResult(apiResponse);
        };
    }
}
