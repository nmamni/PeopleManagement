using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PM.Infrastructure.FileSytem
{
    public class FileSystemClient : IFileSystemClient
    {
        private readonly string _fsPath;

        public FileSystemClient(string fsPath)
        {
            _fsPath = fsPath;
        }

        public async Task SaveImage(IFormFile file, string name)
        {
            var photosDir = Path.Combine(_fsPath, "photos");
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(photosDir, name), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }

        public FileStream GetImage(string name)
        {
            var photosDir = Path.Combine(_fsPath, "photos");
            return File.OpenRead(Path.Combine(photosDir, name));
        }
    }
}
