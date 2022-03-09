using Microsoft.AspNetCore.Diagnostics;
using NLayerApp.Core.Dtos;
using NLayerApp.Service.Exceptions;
using System.Text.Json;

namespace NLayerApp.API.Extensions.Middlewares
{
    public static class UseCustomExceptionHandler 
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                //run sonlandırıcı bir middlewaredir. request buraya girdikten sonra geri döner.
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";


                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();



                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        _ => 500 //else demektir.
                    };


                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDto<NoContentDto>.Error(statusCode, exceptionFeature.Error.Message);


                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));

                });
            });
        }
    }
}
