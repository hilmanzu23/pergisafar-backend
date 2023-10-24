using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IMongoCollection<ImageModel> dataUser;
        private readonly IWebHostEnvironment _environment;
        private readonly string key;

        public ImageController(IWebHostEnvironment environment, IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<ImageModel>("image");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
            _environment = environment;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetImage(string id)
        {
            var image = await dataUser.Find(i => i.Id == id).FirstOrDefaultAsync();
            if (image != null)
            {
                return File(image.ImageData, "image/jpeg");
                
            }

            return NotFound();
            
        }

    }
}