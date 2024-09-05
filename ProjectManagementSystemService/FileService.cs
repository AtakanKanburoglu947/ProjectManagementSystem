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
   
        public async Task<Guid> Upload(IFormFile file, string[] extensions,Guid? userIdentityId, int? managerId )
        {
            MemoryStream memoryStream = new MemoryStream();
            if (file == null)
            {
                return Guid.Empty;
            }
            string fileName = file.FileName;
            using (memoryStream)
            {
                await file.CopyToAsync(memoryStream);
                bool validate = ValidateFile(fileName, extensions);
                if (!validate)
                {
                    throw new Exception("Yanlış dosya formatı");
                    
                }
                int fileSize = 50 * 1024 * 1024;
                if (memoryStream.Length < fileSize)
                {
                    FileUpload fileUpload = new FileUpload()
                    {
                        Id = Guid.NewGuid(),
                        Data = memoryStream.ToArray(),
                        Name = fileName,
                        UserIdentityId = userIdentityId!,
                        ManagerId = managerId!
                    };

                    _appDbContext.FileUploads.Add(fileUpload);
                    await _appDbContext.SaveChangesAsync();
                    return  fileUpload.Id;
                    
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
        public List<FileUpload> GetFilesOfUser(Guid id) {

            return _appDbContext.FileUploads.Where(x=>x.UserIdentityId == id).ToList();
        }
        public List<FileUpload> GetFilesOfManager(int id)
        {
            return _appDbContext.FileUploads.Where(x=>x.ManagerId == id).ToList();
        }



    }
}
