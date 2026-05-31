using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Servces.FileService
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
    }
}
