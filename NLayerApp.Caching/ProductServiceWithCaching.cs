using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayerApp.Core.Dtos;
using NLayerApp.Core.Models;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;
using System.Linq.Expressions;

namespace NLayerApp.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;

            //trygetvalue verilen keye karşılık outda tuttuğu datayı döner. out keywordü ile birden fazla değer dönülebilir.
            //burada CacheProductKey bunda herhangi bir bilgi olup olmadığı bilgisini alacağız çünkü bu trygetvalue bool döner out _ olup olmadığını döner direk olarak bool.
            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                
                //eğer yoksa al tüm productları set et demektir bu.
                //constructor içinde asenkron metod dönemez unutma senkrona dönüştürmek zorundasın
                _memoryCache.Set(CacheProductKey, _productRepository.GetProductsWithCategory().Result);
            }
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entities;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            if (_memoryCache.Get<List<Product>>(CacheProductKey).Any(expression.Compile()))
                return Task.FromResult(true);
            return Task.FromResult(false);

        }

        public Task<IEnumerable<Product>> GetAll()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey).Where(i => i.Id == id).FirstOrDefault();
            if (product == null)
                throw new Exception($"{typeof(Product).Name} does not exist");

            //burada cacheden okuduğumuz için bir asenkron işlem yapılması gerekmemektedir. o yüzden Task.FromResult ile dönüş yapıyoruz.
            //burada bir await kullanılmıyor ama task dönülmesi gerekiyor bu sebeple fromresult kullanılır unutma!
            return Task.FromResult(product);
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productwithcategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productwithcategoryDto));
        }

        public async Task RemoveAsync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public async Task RemoveRangeAsync(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            //burada ilk olarak cacheden datayı alıyoruz. sonrasında where yazıyoruz ki compile metodunu bunu func. a çeviriyor 
            //zaten bizim where koşulumuzda bir fonksiyon istiyor.
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }



        public async Task CacheAllProducts()
        {
            _memoryCache.Set(CacheProductKey, await _productRepository.GetProductsWithCategory());
        }
    }
}
