using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystemCore.Models;
using ProjectManagementSystemRepository;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class FileService
    {
        private readonly AppDbContext _appDbContext;
        private readonly CacheService _cacheService;
        public FileService(AppDbContext appDbContext, CacheService cacheService)
        {
            _appDbContext = appDbContext;
            _cacheService = cacheService;

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
        public async Task<List<FileUpload>> GetFilesOfUser(Guid id) {

            return await _appDbContext.FileUploads.Where(x => x.UserIdentityId == id).ToListAsync();
        }
        public async Task<List<FileUpload>> GetFilesOfManager(int id)
        {
            return await _appDbContext.FileUploads.Where(x => x.ManagerId == id).ToListAsync();
        }
        public async Task<FileUpload> GetFile(Guid id)
        {
            FileUpload? file =  await _appDbContext.FileUploads.FindAsync(id);
            if (file != null)
            {
                return file;
            }
            throw new Exception("Dosya bulunamadı");
        }
        



    }
}
