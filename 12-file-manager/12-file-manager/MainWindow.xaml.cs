using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace _12_file_manager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            LoadBarangays();
        }


        // =========================================
        // LOAD BARANGAYS
        // =========================================

        private void LoadBarangays()
        {
            var barangays = new List<Barangay>
            {
                new Barangay("Barangay 01", "Ready"),
                new Barangay("Barangay 02", "Ready"),
                new Barangay("Barangay 03", "Ready"),
                new Barangay("Barangay 04", "Ready"),
                new Barangay("Barangay 05", "Ready"),
                new Barangay("Barangay 06", "Ready"),
                new Barangay("Barangay 07", "Ready"),
                new Barangay("Barangay 08", "Ready"),
                new Barangay("Barangay 09", "Ready"),
                new Barangay("Barangay 10", "Ready"),
                new Barangay("Barangay 11", "Ready"),
                new Barangay("Barangay 12", "Ready"),
                new Barangay("Barangay 13", "Ready"),
                new Barangay("Barangay 14", "Ready"),
                new Barangay("Barangay 15", "Ready"),
                new Barangay("Barangay 16", "Ready"),
                new Barangay("Barangay 17", "Ready"),
                new Barangay("Barangay 18", "Ready"),
                new Barangay("Barangay 19", "Ready"),
                new Barangay("Barangay 20", "Ready"),
                new Barangay("Barangay 21", "Ready"),
                new Barangay("Barangay 22", "Ready"),
                new Barangay("Barangay 23", "Ready")
            };

            BarangayList.ItemsSource = barangays;
        }


        // =========================================
        // BROWSE ROOT FOLDER
        // =========================================

        private void BrowseRoot_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog
            {
                Title = "Select Project Root Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                RootFolderTextBox.Text = dialog.FolderName;

                LoadBarangaysFromFolder(dialog.FolderName);
            }
        }


        // =========================================
        // LOAD BARANGAYS FROM ROOT
        // =========================================

        private void LoadBarangaysFromFolder(string rootFolder)
        {
            if (!Directory.Exists(rootFolder))
            {
                MessageBox.Show(
                    "The selected folder does not exist.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            MessageBox.Show(
                $"Root folder selected:\n\n{rootFolder}",
                "Project Root",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================
        // VIEW MASTER DRAWING
        // =========================================

        private void ViewMaster_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RootFolderTextBox.Text) ||
                RootFolderTextBox.Text == "No folder selected")
            {
                MessageBox.Show(
                    "Please select the project root folder first.",
                    "Master Drawing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            string masterDrawing = Path.Combine(
                RootFolderTextBox.Text,
                "MASTER.dwg");


            if (!File.Exists(masterDrawing))
            {
                MessageBox.Show(
                    "MASTER.dwg was not found in the project root folder.",
                    "Master Drawing",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            try
            {
                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = masterDrawing,
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not open the master drawing.\n\n{ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        // =========================================
        // SYNC
        // =========================================

        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RootFolderTextBox.Text) ||
                RootFolderTextBox.Text == "No folder selected")
            {
                MessageBox.Show(
                    "Please select the project root folder first.",
                    "Sync",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }


            MessageBox.Show(
                "Sync started...",
                "Sync",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =========================================
        // LOGOUT
        // =========================================

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to logout?",
                "Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }


    // =============================================
    // BARANGAY MODEL
    // =============================================

    public class Barangay
    {
        public string Name { get; set; }

        public string Status { get; set; }


        public Barangay(
            string name,
            string status)
        {
            Name = name;
            Status = status;
        }
    }
}

