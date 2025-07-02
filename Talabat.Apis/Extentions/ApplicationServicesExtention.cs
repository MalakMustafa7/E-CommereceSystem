using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Errors;
using Talabat.Apis.Helper;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Services;

namespace Talabat.Apis.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));

            #region  ErrorHandling
            Services.Configure<ApiBehaviorOptions>(Options =>
                {
                    Options.InvalidModelStateResponseFactory = (actioncontext) =>
                    {
                        var errors = actioncontext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                  .SelectMany(p => p.Value.Errors)
                                                   .Select(p => p.ErrorMessage)
                                                   .ToArray();
                        var validationErrorResponse = new ApiValidationResponse()
                        {
                            Errors = errors
                        };
                        return new BadRequestObjectResult(validationErrorResponse);
                    };
                }); 
            #endregion
            return Services;
        }
    }
}
