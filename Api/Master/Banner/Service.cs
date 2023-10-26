using MongoDB.Driver;
using pergisafar.Shared.Models;

namespace RepositoryPattern.Services.BannerService
{
    public class BannerService : IBannerService
    {
        private readonly IMongoCollection<Banner> dataUser;
        private readonly IMongoCollection<ImageModel> dataImage;
        private readonly IWebHostEnvironment _environment;
        private readonly string key;

        public BannerService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("ConnectionURI"));
            IMongoDatabase database = client.GetDatabase("testprod");
            dataUser = database.GetCollection<Banner>("banner");
            dataImage = database.GetCollection<ImageModel>("image");
            this.key = configuration.GetSection("AppSettings")["JwtKey"];
            _environment = environment;
        }
        public async Task<Object> Get()
        {
            try
            {
                var items = await dataUser.Find(_ => _.IsActive == true).ToListAsync();
                return new { code = 200, data = items, message = "Data Add Complete" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
        public async Task<object> Post(ImageUploadViewModel model)
        {
            try
            {
                ////upload image sections
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    byte[] imageData;
                    using (var memoryStream = new MemoryStream())
                    {
                        await model.ImageFile.CopyToAsync(memoryStream);
                        imageData = memoryStream.ToArray();
                    }

                    var image = new ImageModel()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ImageData = imageData,
                        Name = model.ImageFile.FileName,
                    };

                    var filter = Builders<Banner>.Filter.Eq(u => u.Name, model.Name);
                    var user = await dataUser.Find(filter).SingleOrDefaultAsync();
                    if (user != null)
                    {
                        throw new CustomException(400, "Name", "Nama sudah digunakan.");
                    }

                    var BannerData = new Banner()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = image.Name,
                        Image = image.Id,
                        IsActive = true,
                        IsVerification = false,
                        CreatedAt = DateTime.Now
                    };
                    await dataImage.InsertOneAsync(image);
                    await dataUser.InsertOneAsync(BannerData);
                    return new { code = 200, id = BannerData.Id, message = "Data Add Complete" };
                }
                else
                {
                    throw new CustomException(400, "Error", "Failed");
                }
            }
            catch (CustomException)
            {
                throw;
            }
        }

        public async Task<object> Put(string id, ImageUploadViewModel item)
        {
            try
            {
                var BannerData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (BannerData == null)
                {
                    throw new CustomException(400, "Error", "Data Not Found");
                }
                BannerData.Name = item.Name;
                await dataUser.ReplaceOneAsync(x => x.Id == id, BannerData);
                return new { code = 200, id = BannerData.Id.ToString(), message = "Data Updated" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
        public async Task<object> Delete(string id)
        {
            try
            {
                var BannerData = await dataUser.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (BannerData == null)
                {
                    throw new CustomException(400, "Error", "Data Not Found");
                }
                BannerData.IsActive = false;
                await dataUser.ReplaceOneAsync(x => x.Id == id, BannerData);
                return new { code = 200, id = BannerData.Id.ToString(), message = "Data Deleted" };
            }
            catch (CustomException)
            {
                throw;
            }
        }
    }
}