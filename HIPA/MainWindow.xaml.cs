using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using HIPA;

namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

     
        private void Calculate(object sender, RoutedEventArgs e)
        {
            
        }

        private void testDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effects = DragDropEffects.All;
            }
        }

        private void testDragEnter(object sender, DragEventArgs e)
        {
            calculationBox.Text = "Enter";
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                calculationBox.Text = files[0];
            }
        }

        private void testDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            calculationBox.Text = "Entered";
            Console.WriteLine("DragEnter");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // Set the file dialog to filter for graphics files.
            openFileDialog.Filter = "Text|*.txt|All|*.*";

            // Allow the user to select multiple images.
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select your TimeFrame Files";
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    calculationBox.AppendText(openFileDialog.FileName + "\n");
                    HIPA.Globals.Files.Add(new SelectedFiles(0, openFileDialog.FileName));
                }
            }
               
        }
    }
}
