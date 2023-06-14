using Abp.Dependency;
using Abp.Domain.Repositories;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using SoftGrid.Configuration;
using SoftGrid.Url;

using System;
using System.IO;
using System.Threading.Tasks;

namespace SoftGrid.Storage
{
    public class DbBinaryObjectManager : IBinaryObjectManager, ITransientDependency
    {
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;
        private readonly IRepository<FileBinaryData, Guid> _fileBinaryDataRepository;
        private readonly IAppFolders _appFolders;
        private readonly IWebUrlService _webUrlService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _env;

        public DbBinaryObjectManager(IRepository<BinaryObject, Guid> binaryObjectRepository,
            IRepository<FileBinaryData, Guid> fileBinaryDataRepository, IAppFolders appFolders, IWebUrlService webUrlService, IWebHostEnvironment env)
        {
            _binaryObjectRepository = binaryObjectRepository;
            _fileBinaryDataRepository = fileBinaryDataRepository;
            _appFolders = appFolders;
            _webUrlService = webUrlService;
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public Task<BinaryObject> GetOrNullAsync(Guid id)
        {
            var item = _fileBinaryDataRepository.FirstOrDefaultAsync(id);
            return ConvertFileBinaryDataToBinaryObject(item.Result);
            //return _binaryObjectRepository.FirstOrDefaultAsync(id);
        }

        public Task SaveAsync(BinaryObject file)
        {
            var item = ConvertBinaryObjectToFileBinaryData(file);
            _fileBinaryDataRepository.InsertAsync(item.Result);
            file.Bytes = null;
            return _binaryObjectRepository.InsertAsync(file);
        }

        public Task DeleteAsync(Guid id)
        {
            _fileBinaryDataRepository.DeleteAsync(id);
            return _binaryObjectRepository.DeleteAsync(id);
        }

        //return the image path for products
        public async Task<string> GetProductPictureUrlAsync(Guid id, string extension)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{Path.DirectorySeparatorChar}Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}Products");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string pictureUrl = string.Empty;

            string rootPath = _webUrlService.GetServerRootAddress(null);
            //rootPath = rootPath.Replace("https://", "https://www.");

            var filepath = Path.Combine(_appFolders.ProductImagesFolder, id + extension);

            if (_env.IsDevelopment())
            {
                rootPath = _appConfiguration["App:ServerRootAddress"];
                filepath = Path.Combine(directoryPath, id + extension);
            }

            if (File.Exists(filepath))
            {
                pictureUrl = rootPath + _appFolders.ProductImagesFolderUrl + "/" + id + extension;
            }
            else
            {
                BinaryObject data = await GetOrNullAsync(id);
                var tempFilePath = filepath;
                if (data != null)
                {
                    File.WriteAllBytes(tempFilePath, data.Bytes);
                    pictureUrl = rootPath + _appFolders.ProductImagesFolderUrl + "/" + id + extension;
                }
            }
            return pictureUrl;
        }

        //return the image path for stores
        public async Task<string> GetStorePictureUrlAsync(Guid id, string extension)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{Path.DirectorySeparatorChar}Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}Stores");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string pictureUrl = string.Empty;

            string rootPath = _webUrlService.GetServerRootAddress(null);
            //rootPath = rootPath.Replace("https://", "https://www.");

            var filepath = Path.Combine(_appFolders.StoreImagesFolder, id + extension);

            if (_env.IsDevelopment())
            {
                rootPath = _appConfiguration["App:ServerRootAddress"];
                filepath = Path.Combine(directoryPath, id + extension);
            }

            if (File.Exists(filepath))
            {
                pictureUrl = rootPath + _appFolders.StoreImagesFolderUrl + "/" + id + extension;
            }
            else
            {
                BinaryObject data = await GetOrNullAsync(id);
                var tempFilePath = filepath;
                if (data != null)
                {
                    File.WriteAllBytes(tempFilePath, data.Bytes);
                    pictureUrl = rootPath + _appFolders.StoreImagesFolderUrl + "/" + id + extension;
                }
            }
            return pictureUrl;
        }

        //return the image path for others
        public async Task<string> GetOthersPictureUrlAsync(Guid id, string extension)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{Path.DirectorySeparatorChar}Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}Others");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string pictureUrl = string.Empty;

            string rootPath = _webUrlService.GetServerRootAddress(null);
            //rootPath = rootPath.Replace("https://", "https://www.");

            var filepath = Path.Combine(_appFolders.OthersImagesFolder, id + extension);

            if (_env.IsDevelopment())
            {
                rootPath = _appConfiguration["App:ServerRootAddress"];
                filepath = Path.Combine(directoryPath, id + extension);
            }

            if (File.Exists(filepath))
            {
                pictureUrl = rootPath + _appFolders.OthersImagesFolderUrl + "/" + id + extension;
            }
            else
            {
                BinaryObject data = await GetOrNullAsync(id);
                var tempFilePath = filepath;
                if (data != null)
                {
                    File.WriteAllBytes(tempFilePath, data.Bytes);
                    pictureUrl = rootPath + _appFolders.OthersImagesFolderUrl + "/" + id + extension;
                }
            }
            return pictureUrl;
        }

        //return the file path
        public async Task<string> GetFileUrlAsync(Guid id, string fileName)
        {
            string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{Path.DirectorySeparatorChar}Common{Path.DirectorySeparatorChar}Files");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var extension = Path.GetExtension(fileName);
            string fileUrl = string.Empty;

            string rootPath = _webUrlService.GetServerRootAddress(null);
            //rootPath = rootPath.Replace("https://", "https://www.");

            var filepath = Path.Combine(_appFolders.FileFolder, id + extension);

            if (_env.IsDevelopment())
            {
                rootPath = _appConfiguration["App:ServerRootAddress"];
                filepath = Path.Combine(directoryPath, id + extension);
            }

            if (File.Exists(filepath))
            {
                fileUrl = rootPath + _appFolders.FileFolderUrl + "/" + id + extension;
            }
            else
            {
                BinaryObject data = await GetOrNullAsync(id);
                var tempFilePath = filepath;
                if (data != null)
                {
                    File.WriteAllBytes(tempFilePath, data.Bytes);
                    fileUrl = rootPath + _appFolders.FileFolderUrl + "/" + id + extension;
                }
            }
            return fileUrl;
        }



        public string GetPictureUrl(Guid id, string extension)
        {
            if (id == Guid.Empty) return string.Empty;
            //var directory = new DirectoryInfo(_appFolders.TempFileDownloadFolder);
            string pictureUrl = string.Empty;

            var filepath = Path.Combine(_appFolders.TempFileDownloadFolder, id + extension);
            // var files = directory.GetFiles(id + ".*", SearchOption.AllDirectories).ToList();
            //if (files.Count > 0)
            if (File.Exists(filepath))
            {
                pictureUrl = _webUrlService.GetServerRootAddress(null) + _appFolders.TempUrlFileDownloadFolder + "/" + id + extension;
            }
            else
            {
                BinaryObject data = _binaryObjectRepository.Get(id);
                var tempFilePath = Path.Combine(_appFolders.TempFileDownloadFolder, id + extension);
                if (data != null)
                {
                    System.IO.File.WriteAllBytes(tempFilePath, data.Bytes);
                    pictureUrl = _webUrlService.GetServerRootAddress(null) + _appFolders.TempUrlFileDownloadFolder + "/" + id + extension;
                }
            }
            return pictureUrl;
        }

        private async Task<BinaryObject> ConvertFileBinaryDataToBinaryObject(FileBinaryData input)
        {
            var output = new BinaryObject();
            if (input != null)
            {
                output.TenantId = input.TenantId;
                output.Id = input.Id;
                output.Bytes = input.Bytes;
            }

            return output;
        }

        private async Task<FileBinaryData> ConvertBinaryObjectToFileBinaryData(BinaryObject input)
        {
            var output = new FileBinaryData();
            if (input != null)
            {
                output.TenantId = input.TenantId;
                output.Id = input.Id;
                output.Bytes = input.Bytes;
            }
            return output;
        }
    }
}