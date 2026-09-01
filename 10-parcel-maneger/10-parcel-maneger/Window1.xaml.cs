using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace _10_parcel_maneger
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        // ==========================================
        // BROWSE ROOT FOLDER
        // ==========================================
        private void BrowseRootFolder_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.FolderBrowserDialog();

            dialog.Description = "Select the Parcel Manager root folder";
            dialog.UseDescriptionForTitle = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RootFolderTextBox.Text = dialog.SelectedPath;

                StatusTextBlock.Text =
                    $"Root folder selected:\n{dialog.SelectedPath}";
            }
        }


        // ==========================================
        // BROWSE MASTER DWG
        // ==========================================
        private void BrowseMasterDwg_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Master DWG",
                Filter = "AutoCAD Drawing (*.dwg)|*.dwg",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                MasterDwgTextBox.Text = dialog.FileName;

                StatusTextBlock.Text =
                    $"Master DWG selected:\n{dialog.FileName}";
            }
        }


        // ==========================================
        // OPEN MASTER DWG
        // ==========================================
        private void OpenMasterDwg_Click(object sender, RoutedEventArgs e)
        {
            string dwgPath = MasterDwgTextBox.Text;

            // No DWG selected
            if (string.IsNullOrWhiteSpace(dwgPath))
            {
                MessageBox.Show(
                    "Please select a Master DWG first.",
                    "Master DWG",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // File no longer exists
            if (!File.Exists(dwgPath))
            {
                MessageBox.Show(
                    "The selected DWG file does not exist.",
                    "Master DWG",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            try
            {
                // Open the DWG using Windows file association.
                // If DWG is associated with AutoCAD,
                // AutoCAD will open the drawing.
                Process.Start(new ProcessStartInfo
                {
                    FileName = dwgPath,
                    UseShellExecute = true
                });

                StatusTextBlock.Text =
                    $"Opening Master DWG:\n{dwgPath}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not open the Master DWG.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}

