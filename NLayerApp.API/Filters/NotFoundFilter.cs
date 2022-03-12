using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;
using NLayerApp.Core.Services;

namespace NLayerApp.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {

        // ctorda bir implement yapıyorsan ve generic kullanmak istiyorsan IAsyncActionFilter kullanmak zorundasın.
        // data var mı diye bakmak için bunu baseentity den türetiyorsun unutma!

        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //zaten 1 tane olduğu için direk id geliyor.
            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next.Invoke();
                return;
            }

            var id = (int)idValue;

            var anyEntity = await _service.AnyAsync(i => i.Id == id);

            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Error(404, $"{typeof(T).Name} not found!"));

        }
    }
}
