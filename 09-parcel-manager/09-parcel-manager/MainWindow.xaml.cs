using System.IO;
using System.Windows;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

using AcadApp =
    Autodesk.AutoCAD.ApplicationServices.Application;

namespace _09_parcel_manager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrowseFolder_Click(
            object sender,
            RoutedEventArgs e)
        {
            MessageBox.Show(
                "Browse button works!");
        }

        private void OpenMasterFile_Click(
            object sender,
            RoutedEventArgs e)
        {
            string masterFolder =
                MasterFolderTextBox.Text;

            if (string.IsNullOrWhiteSpace(masterFolder))
            {
                MessageBox.Show(
                    "Please select a Master Root Folder first.");

                return;
            }

            string masterPath =
                System.IO.Path.Combine(
                    masterFolder,
                    "MASTER.dwg");


            // MASTER.dwg already exists
            if (File.Exists(masterPath))
            {
                AcadApp.DocumentManager.Open(
                    masterPath,
                    false);

                return;
            }


            // MASTER.dwg does not exist
            MessageBoxResult result =
                MessageBox.Show(
                    "MASTER.dwg does not exist.\n\n" +
                    "Do you want to create it?",
                    "Create MASTER.dwg",
                    MessageBoxButton.YesNo);


            if (result != MessageBoxResult.Yes)
            {
                return;
            }


            // Create a new AutoCAD document
            Autodesk.AutoCAD.ApplicationServices.Document document =
                AcadApp.DocumentManager.Add(
                    "acad.dwt");


            // Save it as MASTER.dwg
            document.Database.SaveAs(
                masterPath,
                DwgVersion.Current);


            MessageBox.Show(
                "MASTER.dwg created successfully.");
        }
    }
}

