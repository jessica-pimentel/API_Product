using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult CustomResponse(object result = null)
        {
            if (ModelState.IsValid)
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = GetModelErrors()
            });
        }

        protected ActionResult CustomResponseError(string errorMessage)
        {
            return BadRequest(new
            {
                success = false,
                errors = new List<string> { errorMessage }
            });
        }

        private IEnumerable<string> GetModelErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return CustomResponseError("Model state is invalid.");
            }

            return CustomResponse();
        }
}