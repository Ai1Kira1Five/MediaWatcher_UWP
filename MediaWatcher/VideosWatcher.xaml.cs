using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MediaWatcher
{
    public sealed partial class VideosWatcher : Page
    {
        public VideosWatcher()
        {
            this.InitializeComponent();
        }
    }

    public class VideoWatcherViewModel
    {
        public VideoWatcherViewModel(StorageFolder stFolder)
        {
            VideoItems = new ObservableCollection<VideoDataObject>();
            MainFolder = stFolder;
        }

        public ObservableCollection<VideoDataObject> VideoItems { get; set; }

        private StorageFolder MainFolder;
        private IEnumerable<StorageFile> Videos;
        private char[] sep = new char[] { '/' };

        private async void Initialize()
        {
            Videos = await MainFolder.GetFilesAsync();
            Videos = Videos.Where(a => a.ContentType.Split(sep)[0] == "video");
            FillUp();
        }

        private void FillUp()
        {
            foreach(var file in Videos)
            {
                VideoItems.Add(new VideoDataObject(file));
            }
        }
    }
}
