using System.IO;
using System.Windows;
using System.Windows.Forms;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

using AcadApp =
    Autodesk.AutoCAD.ApplicationServices.Application;

namespace _08_parcel_maneger
{
    public partial class MainWindow : Window
    {
        private string? masterFolder;

        public MainWindow()
        {
            InitializeComponent();
        }


        // ============================================
        // SELECT MASTER ROOT FOLDER
        // ============================================

        private void BrowseFolder_Click(
            object sender,
            RoutedEventArgs e)
        {
            using FolderBrowserDialog dialog = new();

            dialog.Description =
                "Select Master Root Folder";

            dialog.UseDescriptionForTitle = true;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                masterFolder = dialog.SelectedPath;

                MasterFolderTextBox.Text =
                    masterFolder;

                LoadBarangays();
            }
        }


        // ============================================
        // OPEN OR CREATE MASTER.DWG
        // ============================================

        private void OpenMasterFile_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(masterFolder))
            {
                MessageBox.Show(
                    "Please select a master folder first.",
                    "Parcel Manager");

                return;
            }


            string masterPath =
                Path.Combine(
                    masterFolder,
                    "MASTER.dwg");


            // ========================================
            // MASTER.DWG EXISTS
            // ========================================

            if (File.Exists(masterPath))
            {
                AcadApp.DocumentManager.Open(
                    masterPath,
                    false);

                return;
            }


            // ========================================
            // MASTER.DWG DOES NOT EXIST
            // ========================================

            MessageBoxResult result =
                MessageBox.Show(
                    "MASTER.dwg does not exist.\n\n" +
                    "Do you want to create it?",
                    "Create Master Drawing",
                    MessageBoxButton.YesNo);


            if (result != MessageBoxResult.Yes)
            {
                return;
            }


            // Create a new AutoCAD document
            Document document =
                AcadApp.DocumentManager.Add(
                    "acad.dwt");


            // Save it as MASTER.dwg
            document.Database.SaveAs(
                masterPath,
                true);
        }


        // ============================================
        // LOAD BARANGAYS
        // ============================================

        private void LoadBarangays()
        {
            BarangayListBox.Items.Clear();

            if (string.IsNullOrEmpty(masterFolder))
            {
                return;
            }


            string barangayFolder =
                Path.Combine(
                    masterFolder,
                    "Barangays");


            if (!Directory.Exists(barangayFolder))
            {
                return;
            }


            string[] files =
                Directory.GetFiles(
                    barangayFolder,
                    "*.dwg");


            foreach (string file in files)
            {
                BarangayListBox.Items.Add(
                    Path.GetFileNameWithoutExtension(file));
            }
        }
    }
}