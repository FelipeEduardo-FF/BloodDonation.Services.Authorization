using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Shared.Infra.DTO;
using Microsoft.AspNetCore.Http;



namespace Shared.Infra.Filters
{

    public class ApiResponseFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                ApiResponse<object> apiResponse;
                int? statusCode=200;

                if (objectResult.Value is not ApiResponse<object> problemDetails)
                {
                    apiResponse = new ApiResponse<object>
                    {
                        Success = true,
                        Message = "Request is successful",
                        Data = objectResult.Value
                    };
                    statusCode = objectResult.StatusCode;


                    context.Result = new ObjectResult(apiResponse)
                    {
                        StatusCode = statusCode
                    };
                }

            }
        }
    }
}
