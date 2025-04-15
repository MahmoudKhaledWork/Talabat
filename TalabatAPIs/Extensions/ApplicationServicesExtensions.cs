using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Services;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            
            Services.AddScoped(typeof(IPaymentServiec), typeof(PaymentService));
            Services.AddScoped(typeof(IOrderServices), typeof(OrderServices));
            Services.AddScoped(typeof(IUnitOFWork), typeof(UnitOfWork));
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GeneriecRepository<>));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    /// ModelState = dectionary [ Key , Value ]
                    /// Key => Name Of Parameter 
                    /// Value => Error
                    /// Select The Values That have Errors
                    /// That Its Count Of Error is Bigger than 0
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                              .SelectMany(P => P.Value.Errors)
                                              .Select(M => M.ErrorMessage)
                                              .ToList();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });
            return Services;
        }
    }
}
