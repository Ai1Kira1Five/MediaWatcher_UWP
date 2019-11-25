using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaWatcher
{
    public class VideoDataObject{
        public string Title { get; set; }
        public string VideoLocation { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }

        public VideoProperties videoProperties { get; set; }
        public BitmapImage Display { get; set; }
        public StorageFile VideoFile { get; set; }

        public VideoDataObject(StorageFile file) {
            VideoFile = file;
            Initialize();
        }

        private async void Initialize()
        {
            Title = VideoFile.DisplayName;
            videoProperties = await VideoFile.Properties.GetVideoPropertiesAsync();
            var dur = videoProperties.Duration;
            Duration = dur.Hours.ToString() + " : " + dur.Minutes.ToString() + " : " + dur.Seconds.ToString();
            Display = await GetDisplay();
        }

        private async Task<BitmapImage> GetDisplay()
        {
            var bitMap = new BitmapImage();
            using (var imgSource = await VideoFile.GetScaledImageAsThumbnailAsync(ThumbnailMode.VideosView))
            {
                if (imgSource != null)
                    bitMap.SetSource(imgSource);
                else
                {
                    var storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
                    var storageFile = await storageFolder.GetFileAsync("Logo.png");
                    bitMap.UriSource = new Uri(storageFile.Path);
                }
            }
            return bitMap;
        }
    }

    public sealed partial class VideosWatcher : Page
    {
        public VideosWatcher()
        {
            this.InitializeComponent();
        }
    }
}
