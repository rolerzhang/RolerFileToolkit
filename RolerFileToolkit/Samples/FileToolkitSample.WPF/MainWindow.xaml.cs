using System.IO;
using System.Windows;
using Microsoft.Win32;
using Roler.Toolkit.File.Epub;

namespace FileToolkitSample.WPF
{
    public partial class MainWindow : Window
    {
        FileStream fileStream;
        EpubReader epubReader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenEpubButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "epub file|*.epub"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                this.fileStream?.Dispose();
                this.epubReader?.Dispose();

                var filePath = openFileDialog.FileName;
                fileStream = File.OpenRead(filePath);
                {
                    epubReader = new EpubReader(fileStream);
                    {
                        var epub = epubReader.Read();
                        this.listView.ItemsSource = epub.ReadingFiles;
                    }
                }
            }
        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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

        ~MainWindow()
        {
            this.epubReader?.Dispose();
            this.fileStream?.Dispose();
        }
    }
}
