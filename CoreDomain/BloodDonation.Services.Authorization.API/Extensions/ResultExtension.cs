using Microsoft.AspNetCore.Mvc;
using Shared.Infra.DTO;
using Shared.Infra.Results;

namespace BloodDonation.Services.Authorization.API.Extensions;

public static class ResultExtension
{
    public static IActionResult ToProblemDetails(this IResultBase result)
    {
        if (result.Success)
            throw new InvalidOperationException("Result is a success!");

        var error = result.Errors[0];         

        var apiResponse = new ApiResponse<object>
        {
            Success = false,
            Message = error.Message,
            Error = result.Errors
        };

        return new ObjectResult(apiResponse) {StatusCode=error.Code};
    }

}