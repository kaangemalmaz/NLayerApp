using NLayerApp.Core.Dtos;

namespace NLayerApp.Web.Services
{
    public class ProductApiService
    {
        //burada _httpclient için asla kendin instance üretmeye çalışma burada di container yani program.cs tarafında yazdığın kısımdan üretilmeli
        //bu performansı arttırır ve bir çok hatadan korur bizi.
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<ProductWithCategoryDto>>>("products/ProductWithCategory");
            return response.Data;
        }

        public async Task<ProductDto> GetById(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<ProductDto>>($"products/{id}");
            return response.Data;
        }

        public async Task<ProductDto> Save(ProductDto newProduct)
        {
            var response = await _httpClient.PostAsJsonAsync<ProductDto>("products", newProduct);

            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<ProductDto>>();

            return responseBody.Data;
        }

        public async Task<bool> Update(ProductDto product)
        {
            var response = await _httpClient.PutAsJsonAsync<ProductDto>("products", product);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");

            return response.IsSuccessStatusCode;
        }





    }
}
