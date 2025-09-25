using FluentValidation;
using HIS.Contracts.Responses;

namespace HIS.Api.Mappers
{
    public class ValidationErrorMappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationErrorMappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                ValidationErrorResponse response = new ValidationErrorResponse
                {
                    Errors = ex.Errors.Select(x => new ValidationError
                    {
                        PropertyName = x.PropertyName,
                        ErrorMessage = x.ErrorMessage
                    })
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
