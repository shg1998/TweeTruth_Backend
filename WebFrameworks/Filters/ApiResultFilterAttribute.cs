using System.Linq;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebFrameworks.Api;

namespace WebFrameworks.Filters
{
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            switch (context.Result)
            {
                case OkObjectResult okObjectResult:
                    {
                        var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, okObjectResult.Value);
                        context.Result = new JsonResult(apiResult) { StatusCode = okObjectResult.StatusCode };
                        break;
                    }
                case OkResult okResult:
                    {
                        var apiResult = new ApiResult(true, ApiResultStatusCode.Success);
                        context.Result = new JsonResult(apiResult) { StatusCode = okResult.StatusCode };
                        break;
                    }
                case BadRequestResult badRequestResult:
                    {
                        var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest);
                        context.Result = new JsonResult(apiResult) { StatusCode = badRequestResult.StatusCode };
                        break;
                    }
                case BadRequestObjectResult badRequestObjectResult:
                    {
                        var message = badRequestObjectResult.Value.ToString();
                        if (badRequestObjectResult.Value is SerializableError errors)
                        {
                            var errorMessages = errors.SelectMany(p => (string[])p.Value).Distinct();
                            message = string.Join(" | ", errorMessages);
                        }
                        var apiResult = new ApiResult(false, ApiResultStatusCode.BadRequest, message);
                        context.Result = new JsonResult(apiResult) { StatusCode = badRequestObjectResult.StatusCode };
                        break;
                    }
                case ContentResult contentResult:
                    {
                        var apiResult = new ApiResult(true, ApiResultStatusCode.Success, contentResult.Content);
                        context.Result = new JsonResult(apiResult) { StatusCode = contentResult.StatusCode };
                        break;
                    }
                case NotFoundResult notFoundResult:
                    {
                        var apiResult = new ApiResult(false, ApiResultStatusCode.NotFound);
                        context.Result = new JsonResult(apiResult) { StatusCode = notFoundResult.StatusCode };
                        break;
                    }
                case NotFoundObjectResult notFoundObjectResult:
                    {
                        var apiResult = new ApiResult<object>(false, ApiResultStatusCode.NotFound, notFoundObjectResult.Value);
                        context.Result = new JsonResult(apiResult) { StatusCode = notFoundObjectResult.StatusCode };
                        break;
                    }
                case ObjectResult { StatusCode: null } objectResult when !(objectResult.Value is ApiResult):
                    {
                        var apiResult = new ApiResult<object>(true, ApiResultStatusCode.Success, objectResult.Value);
                        context.Result = new JsonResult(apiResult) { StatusCode = objectResult.StatusCode };
                        break;
                    }
            }

            base.OnResultExecuting(context);
        }
    }
}
