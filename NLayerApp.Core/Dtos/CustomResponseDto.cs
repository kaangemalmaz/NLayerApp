using System.Text.Json.Serialization;

namespace NLayerApp.Core.Dtos
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public List<String> Errors { get; set; }


        //bu yapı factory design patterdan gelir.
        //bir sınıf içinde kendi kendinin static olarak set edip ona göre dönüş yapıyorsa factory yani bir fabrika tarzı ile kendi işlemini kendi yapar.
        //new anahtar sözcüğü kullanma kısmını sadece kendi içinde yapar.
        //design patter amacı nesne üretmeyi azaltmak clienttan tamamen bağımsız hale getirmektir.
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }
        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Data = data };
        }

        public static CustomResponseDto<T> Error(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }
        public static CustomResponseDto<T> Error(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }

    }
}
