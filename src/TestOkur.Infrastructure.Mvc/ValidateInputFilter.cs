namespace TestOkur.Infrastructure.Mvc
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidateInputFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var errors = from kvp in context.ModelState
                         from e in kvp.Value.Errors
                         select e.ErrorMessage;

            if (errors.Any())
            {
                throw new ValidationException(errors.First());
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
	        // Do nothing for now
		}
    }
}
