using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace PM.Infrastructure.FileSytem
{
    public interface IFileSystemClient
    {
        Task SaveImage(IFormFile file, string name);

        FileStream GetImage(string name);
    }
}