using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ConvertToPdfs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSelectedFiles_Click(object sender, RoutedEventArgs e)
        {
            lblSummary.Content = string.Empty;
            var fileNames = new List<string>();

            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;

                var result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (var fileName in dialog.FileNames)
                    {
                        fileNames.Add(fileName);
                    }
                }
            }

            listView.ItemsSource = fileNames;
        }

        private void btnConvertedFilePath_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.IsFolderPicker = true;

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    txtConvertedFilePath.Text = dialog.FileName;
                }
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                var fileName = txtFileName.Text;
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = "ConvertedFile";
                }

                fileName = txtConvertedFilePath.Text + "\\" + fileName + ".pdf";

                var msg = PDFWrapper.CreatePDF(listView.Items, fileName);

                lblSummary.Content = msg;

                if (msg == "success")
                {
                    lblSummary.Content = "Files converted successfully.";
                    lblSummary.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (listView.Items != null)
            {
                listView.ItemsSource = null;
            }

            txtConvertedFilePath.Text = string.Empty;
            txtFileName.Text = string.Empty;
        }

        private bool ValidateInput()
        {
            var isSuccess = true;
            var errorMsg = new StringBuilder();

            if (listView.Items.Count <= 0)
            {
                errorMsg.AppendLine("Please enter files to convert to pdf.");
                isSuccess = false;
            }

            if (string.IsNullOrEmpty(txtConvertedFilePath.Text))
            {
                errorMsg.AppendLine("Please enter the location");
                isSuccess = false;
            }

            lblSummary.Content = errorMsg.ToString();
            lblSummary.Foreground = new SolidColorBrush(Colors.Red);

            return isSuccess;
        }
    }
}
