namespace Bloggie.Web.Repositories
{
    public interface IUploadRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
