using Microsoft.AspNetCore.Http;

namespace newplgapi.model
{
    public class PriceInputUpload : PriceInput
    {
        public IFormFile file { get; set; }
    }
}