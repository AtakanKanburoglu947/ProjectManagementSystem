using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class FileService
    {
        private readonly AppDbContext _appDbContext;
        public FileService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public string GetUploadedFileExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            int index = fileName.IndexOf('.');
            if (index >= 0 && index < fileName.Length - 1)
            {
                return fileName.Substring(index);
            }
            return string.Empty;
        }
        public async Task Upload(IFormFile file, string[] extensions,Guid UserIdentityId)
        {
            MemoryStream memoryStream = new MemoryStream();
            string fileName = file.FileName;
            using (memoryStream)
            {
                await file.CopyToAsync(memoryStream);
                bool validate = ValidateFile(fileName, extensions);
                if (!validate)
                {
                    throw new Exception("Yanlış dosya formatı");
                    
                }
                if (memoryStream.Length < 2097152)
                {
                    FileUpload fileUpload = new FileUpload()
                    {
                        Data = memoryStream.ToArray(),
                        Name = fileName,
                        UserIdentityId = UserIdentityId
                    };

                    _appDbContext.Files.Add(fileUpload);
                    await _appDbContext.SaveChangesAsync();

                }
                else
                {
                    throw new Exception("Dosya boyutu çok büyük");
                  

                }

            }
        }
        bool ValidateFile(string fileName, string[] permittedExtensions)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            {
                return false;
            }
            return true;
        }


    }
}
