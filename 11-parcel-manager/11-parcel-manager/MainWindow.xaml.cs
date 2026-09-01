using System.Diagnostics;
using System.IO;
using System.Windows;

namespace DwgManager;

public partial class MainWindow : Window
{
    private string? _dwgPath;

    public MainWindow() => InitializeComponent();

    private void Browse_Click(object sender, RoutedEventArgs e)
    {
        using var dialog = new System.Windows.Forms.FolderBrowserDialog
        {
            Description = "Select the root folder containing TagumCity.dwg"
        };

        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        {
            return;
        }

        var found = Directory.GetFiles(dialog.SelectedPath, "*.dwg")
            .FirstOrDefault(f => Path.GetFileNameWithoutExtension(f)
                .Equals("tagumcity", StringComparison.OrdinalIgnoreCase));

        if (found != null)
        {
            _dwgPath = found;
            StatusText.Text = $"Found: {Path.GetFileName(found)}";
            OpenBtn.IsEnabled = true;
        }
        else
        {
            _dwgPath = null;
            StatusText.Text = "TagumCity.dwg not found in that folder.";
            OpenBtn.IsEnabled = false;
        }
    }

    private void Open_Click(object sender, RoutedEventArgs e)
    {
        if (_dwgPath == null)
        {
            return;
        }
        Process.Start(new ProcessStartInfo { FileName = _dwgPath, UseShellExecute = true });
    }
}