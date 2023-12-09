using System.IO;
using System.IO.Compression;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace FileForge
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathStart = string.Empty;
       
        static string pathName = string.Empty;
        static string pathNoFinal = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            CenterWindowOnScreen();
            Compress_bt.IsEnabled = true;
        }
        private void CenterWindowOnScreen()
        {
            // Calcula la posición para centrar la ventana
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowWidth = Width;
            double windowHeight = Height;
            Left = (screenWidth - windowWidth) / 2;
            Top = (screenHeight - windowHeight) / 2;
        }

        private void VisibilityObjects1()
        {
            Name_bt.Visibility = Visibility.Visible;
            Name_bt.Content = "Name";
            Destination_bt.Visibility = Visibility.Visible;
            Destination_bt.Content = "Destination";
            Compress_bt.Visibility = Visibility.Visible;
            Choose_bt_archive.Visibility = Visibility.Collapsed;
            Choose_bt_directory.Visibility = Visibility.Collapsed;
            Final_lab.Visibility = Visibility.Collapsed;
        }

        private void VisibilityObjects2()
        {
            Name_bt.Visibility = Visibility.Collapsed;
            Destination_bt.Visibility = Visibility.Collapsed;
            Compress_bt.Visibility = Visibility.Collapsed;
            Choose_bt_archive.Visibility = Visibility.Visible;
            Choose_bt_directory.Visibility = Visibility.Visible;
            Final_lab.Visibility = Visibility.Visible;
        }

        // Method to compress files
        static void CompressFile(String sourcePath, String targetPath) {

            try
            {
                using (var zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(sourcePath, System.IO.Path.GetFileName(sourcePath)); // Provide a valid entry name
                }

                System.Windows.MessageBox.Show("File compressed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error compressing files:" + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Method to compress Archives
        static void CompressArchive(String sourcePath, String targetPath)
        {

            try
            {
                ZipFile.CreateFromDirectory(sourcePath, targetPath);

                System.Windows.MessageBox.Show("Archive compressed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error compressing files:" + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SelectFolder()
        {
            string selectedPath = null;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the Folder";
            folderBrowserDialog.ShowNewFolderButton = false; // Evita la opción de crear nueva carpeta

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
            }

            if (!string.IsNullOrEmpty(selectedPath))
            {
                pathNoFinal = selectedPath;
            }
            else
            {
                System.Windows.MessageBox.Show("No directory selected.");
            }
        }
        private void Compress_bt_Click(object sender, RoutedEventArgs e)
        {

            string pathEnd = System.IO.Path.Combine(pathNoFinal, pathName);

            if (File.Exists(pathStart))
            {
                CompressFile(pathStart,pathEnd);
                VisibilityObjects2();
                Final_lab.Content = pathEnd;
                Compress_bt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCA491C"));
                Compress_bt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6D250B"));
            }
            else if (Directory.Exists(pathStart))
            {
                CompressArchive(pathStart,pathEnd);
                VisibilityObjects2();
                Final_lab.Content = pathEnd;
                Compress_bt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFCA491C"));
                Compress_bt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6D250B"));
            }
            else
            {
                System.Windows.MessageBox.Show("The Path does not exists.");
            }
        }

        private void Choose_bt_directory_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = null;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select the Folder to compress";
            folderBrowserDialog.ShowNewFolderButton = false; // Evita la opción de crear nueva carpeta

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
            }

            if (!string.IsNullOrEmpty(selectedPath))
            {
                pathStart = selectedPath;

                VisibilityObjects1();
            }
            else
            {
                System.Windows.MessageBox.Show("No directory selected.");
            }
        }

        private void Choose_bt_archive_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = null;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Select the file to compress";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.FileName = "Select"; // Este texto es solo un placeholder en el diálogo

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPath = openFileDialog.FileName;
            }

            if (!string.IsNullOrEmpty(selectedPath))
            {
                pathStart = selectedPath;

                VisibilityObjects1();
            }
            else
            {
                System.Windows.MessageBox.Show("No file selected.");
            }
        }

        private void Name_bt_Click(object sender, RoutedEventArgs e)
        {
            string userInput = Microsoft.VisualBasic.Interaction.InputBox("Name to the compress file:", "Name", "");
            if (!string.IsNullOrEmpty(userInput))
            {
                pathName = userInput + ".zip";
                Name_bt.Content = userInput;
            }
        }

        private void Destination_bt_Click(object sender, RoutedEventArgs e)
        {
            SelectFolder();
            Destination_bt.Content = pathNoFinal;
            
            Compress_bt.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF"));
            Compress_bt.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEC4C15"));
        }
    }
}

