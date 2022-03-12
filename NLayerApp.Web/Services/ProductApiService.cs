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
    }
}
