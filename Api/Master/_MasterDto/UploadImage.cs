public class ImageUploadModel
{
    public IFormFile Image { get; set; }
    public string Title { get; set; }
    public string? Name { get; set; }
}

public class ImageModel 
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public byte[]? ImageData { get; set; }
    }

public class ImageUploadViewModel
    {
         public IFormFile? ImageFile { get; set; }
         public string? Name { get; set; }

    }