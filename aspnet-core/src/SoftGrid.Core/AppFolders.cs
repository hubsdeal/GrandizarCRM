using Abp.Dependency;

namespace SoftGrid
{
    public class AppFolders : IAppFolders, ISingletonDependency
    {
        public string SampleProfileImagesFolder { get; set; }

        public string WebLogsFolder { get; set; }

        //custom folder path properties for storing images and files
        public string ProductImagesFolder { get; set; }
        public string ProductImagesFolderUrl { get; set; }
        public string StoreImagesFolder { get; set; }
        public string StoreImagesFolderUrl { get; set; }
        public string OthersImagesFolder { get; set; }
        public string OthersImagesFolderUrl { get; set; }
        public string FileFolder { get; set; }
        public string FileFolderUrl { get; set; }
        public string TempFileDownloadFolder { get; set; }
        public string TempUrlFileDownloadFolder { get; set; }
    }
}