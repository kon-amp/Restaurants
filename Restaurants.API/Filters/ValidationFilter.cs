using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Restaurants.API.Filters;


/// <summary>
/// A global ASP.NET Core action filter that automatically performs 
/// FluentValidation validation on controller action parameters.
/// </summary>
/// <remarks>
/// <para>
/// This filter runs before a controller action executes. It uses the built-in 
/// dependency injection container (<see cref="IServiceProvider"/>) to locate any 
/// registered <see cref="IValidator{T}"/> that matches the action parameter types.
/// </para>
/// <para>
/// If validation fails, the filter short-circuits the pipeline and returns a 
/// <see cref="BadRequestObjectResult"/> containing a standard 
/// <see cref="ValidationProblemDetails"/> response.
/// </para>
/// 
/// <h3>How to register / instantiate</h3>
/// <para>
/// To enable this filter globally for all controllers, register it in 
/// <c>Program.cs</c> using <c>options.Filters.Add&lt;ValidationFilter&gt;()</c>:
/// </para>
/// <code>
/// services
///     .AddControllers(options =>
///     {
///         options.Filters.Add&lt;ValidationFilter&gt;();
///     });
/// </code>
/// 
/// <para>
/// Note: This approach fits traditional controller-based validation.  
/// In Clean Architecture with the Mediator pattern, prefer validation via 
/// MediatR pipeline behaviors instead.
/// </para>
/// </remarks>
[Obsolete("Not recommended for Clean Architecture. Prefer MediatR pipeline validation instead.")]
public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
        // Iterate through all arguments passed to the controller action.
        // Example: a POST action with parameter "CreateRestaurantDto dto" will appear here.
        foreach (var arg in context.ActionArguments.Values) {
            // Skip null parameters (e.g., optional ones)
            if (arg == null) continue;

            // Dynamically create the validator type based on the argument’s runtime type.
            // For example, typeof(IValidator<CreateRestaurantDto>)
            var validatorType = typeof(IValidator<>).MakeGenericType(arg.GetType());

            // Try to resolve a matching validator from the dependency injection container.
            if (serviceProvider.GetService(validatorType) is IValidator validator) {
                // Create a FluentValidation context for the current argument object.
                var validationContext = new ValidationContext<object>(arg);

                // Run asynchronous validation.
                var result = await validator.ValidateAsync(validationContext);

                // If any validation errors were found:
                if (!result.IsValid) {
                    // Group all validation errors by property name 
                    // and convert them into a dictionary format that 
                    // can be serialized in a standard validation response.
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    // Return a 400 Bad Request response with validation details.
                    // This prevents the controller action from executing.
                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors));
                    return; 
                }
            }
        }

        // Continue the request pipeline if no validation errors occurred.
        await next(); 
    }
}
