
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Bloggie.Web.Repositories
{
    public class UploadRepository : IUploadRepository
    {
        private readonly IConfiguration configuration;
        private readonly Account account;

        public UploadRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            account = new Account(
                configuration.GetSection("Cloudinary")["CloudName"],
                configuration.GetSection("Cloudinary")["ApiKey"],
                configuration.GetSection("Cloudinary")["ApiSecret"]
                );
            
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var cloudinary = new Cloudinary(account);
            var image = new ImageUploadParams
            {
                File = new FileDescription(file.FileName,file.OpenReadStream()),
                DisplayName = file.FileName.Split(".")[0],

            };
            var uploadResult = await cloudinary.UploadAsync(image);
            if(uploadResult != null)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
            
        }
    }
}
