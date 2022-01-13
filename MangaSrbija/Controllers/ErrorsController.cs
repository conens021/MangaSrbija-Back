using MangaSrbija.BLL.exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorHandlingController : ControllerBase
    {

        [Route("/error-dev")]
        public ActionResult GetDevError([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            Exception error = context?.Error;

            if (error is BusinessException)
            {
                BusinessException bussinesException = (BusinessException)error;
                return Problem(
                                  detail: bussinesException.StackTrace,
                                  title: bussinesException.Message,
                                  statusCode: bussinesException.StatusCode
                               );
            }

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }
    }
}
