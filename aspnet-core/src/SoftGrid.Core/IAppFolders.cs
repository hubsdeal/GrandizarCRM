namespace SoftGrid
{
    public interface IAppFolders
    {
        string SampleProfileImagesFolder { get; }
        string WebLogsFolder { get; set; }
        string ProductImagesFolder { get; set; }
        string ProductImagesFolderUrl { get; set; }
        string StoreImagesFolder { get; set; }
        string StoreImagesFolderUrl { get; set; }
        string OthersImagesFolder { get; set; }
        string OthersImagesFolderUrl { get; set; }
        string FileFolder { get; set; }
        string FileFolderUrl { get; set; }
        string TempFileDownloadFolder { get; set; }
        string TempUrlFileDownloadFolder { get; set; }
    }
}