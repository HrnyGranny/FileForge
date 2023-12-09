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
        static string pathEnd = string.Empty;
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

        // Method to compress files
        static void CompressFile(String sourcePath, String targetPath) {

            try
            {
                using (var zip = ZipFile.Open(targetPath, ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(sourcePath, System.IO.Path.GetFileName(sourcePath)); // Provide a valid entry name
                }

                Console.WriteLine("File compressed successfully.");
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

                Console.WriteLine("Archive compressed successfully.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error compressing files:" + ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        static void SelectFolder()
        {
            string selectedPath = null;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Seleccionar Carpeta";
            folderBrowserDialog.ShowNewFolderButton = false; // Evita la opción de crear nueva carpeta

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
            }

            if (!string.IsNullOrEmpty(selectedPath))
            {
                string userInput = Microsoft.VisualBasic.Interaction.InputBox("Name to the compress file:", "Name", "");
               
                if (!string.IsNullOrEmpty(userInput))
                {
                    pathEnd = System.IO.Path.Combine(selectedPath, userInput);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("No directory selected");
            }
        }
        private void Compress_bt_Click(object sender, RoutedEventArgs e)
        {
            SelectFolder();

            if (File.Exists(pathStart))
            {
                CompressFile(pathStart,pathEnd);
            }
            else if (Directory.Exists(pathStart))
            {
                CompressArchive(pathStart,pathEnd);
            }
            else
            {
                System.Windows.MessageBox.Show("The Path does not exists");
            }
        }

        private void Choose_bt_directory_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = null;

            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.Description = "Select Folder to compress";
            folderBrowserDialog.ShowNewFolderButton = false; // Evita la opción de crear nueva carpeta

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPath = folderBrowserDialog.SelectedPath;
            }

            if (!string.IsNullOrEmpty(selectedPath))
            {
                pathStart = selectedPath;
            }
            else
            {
                System.Windows.MessageBox.Show("No directory selected");
            }
        }

        private void Choose_bt_archive_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = null;

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "Select file to compress";
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
            }
            else
            {
                System.Windows.MessageBox.Show("No file selected");
            }
        }
    }
}

