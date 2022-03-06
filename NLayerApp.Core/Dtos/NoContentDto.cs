using System.Text.Json.Serialization;

namespace NLayerApp.Core.Dtos
{
    public class NoContentDto
    {
        [JsonIgnore]
        public int StatusCode { get; set; }

        public List<String> Errors { get; set; }


        //bu yapı factory design patterdan gelir.
        //bir sınıf içinde kendi kendinin static olarak set edip ona göre dönüş yapıyorsa factory yani bir fabrika tarzı ile kendi işlemini kendi yapar.
        //new anahtar sözcüğü kullanma kısmını sadece kendi içinde yapar.
        //design patter amacı nesne üretmeyi azaltmak clienttan tamamen bağımsız hale getirmektir.
        public static NoContentDto Success(int statusCode)
        {
            return new NoContentDto { StatusCode = statusCode };
        }

        public static NoContentDto Error(int statusCode, List<string> errors)
        {
            return new NoContentDto { StatusCode = statusCode, Errors = errors };
        }
        public static NoContentDto Error(int statusCode, string error)
        {
            return new NoContentDto { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}
