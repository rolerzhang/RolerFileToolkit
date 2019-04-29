using System;
using System.IO;
using Roler.Toolkit.File.Epub;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FileToolkitSample.UWP
{
    public sealed partial class MainPage : Page
    {
        Stream fileStream;
        EpubReader epubReader;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OpenEpubButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };
            picker.FileTypeFilter.Add(".epub");
            var file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                this.fileStream = await file.OpenStreamForReadAsync();
                {
                    epubReader = new EpubReader(fileStream);
                    {
                        if (epubReader.TryRead(out Epub epub))
                        {
                            this.listView.ItemsSource = epub.ReadingFiles;
                        }
                    }
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ContentFile contentFile)
            {
                if (this.epubReader != null)
                {
                    var content = this.epubReader.ReadContentAsText(contentFile.FilePath);
                    this.textBlock.Text = content;
                }
            }
        }

        ~MainPage()
        {
            this.epubReader?.Dispose();
            this.fileStream?.Dispose();
        }
    }
}
