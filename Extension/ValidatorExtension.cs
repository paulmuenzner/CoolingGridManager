using CoolingGridManager.Validators;
using FluentValidation; // Import the validators folder

namespace CoolingGridManager.Extensions
{
    public static class ValidatorExtension
    {
        public static void AddValidators(this IServiceCollection services)
        {
            // Interface for validators
            var validatorType = typeof(IValidator);

            // Get all validator classes in the assembly
            var validatorTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => p.IsClass && !p.IsAbstract && validatorType.IsAssignableFrom(p));

            // Register each validator with the service collection
            foreach (var validator in validatorTypes)
            {
                services.AddScoped(validator);
            }
        }
    }
}
