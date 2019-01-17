using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;
using System.Data;

namespace HIPA {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

     
        private void PrepareData(object sender, RoutedEventArgs e)
        {

            try {
                FileService.ReadFiles();
               
                
                foreach (File file in Globals.Files) {
                    CellService.CreateCells(file);
                    CellService.PopulateCells(file);
                    CellService.CalculateMinutes(file);
                    Debug.Print("FileName: " + file.FileName);
                    Debug.Print("Limit: " + file.Limit.ToString());
                    Debug.Print("Cells: " + file.Cells.Count.ToString());
                    Debug.Print("TimeFrames: " + file.Cells[0].Timeframes.Count.ToString());
                    Debug.Print("Time Frame Test: " + file.Cells[0].Timeframes[16].Minute + " " + file.Cells[0].Timeframes[16].ID + " " + file.Cells[0].Timeframes[16].Value);
                   
                }
                selectedFilesDataGrid.ItemsSource = null;
                selectedFilesDataGrid.ItemsSource = Globals.Files;
                CalculateButton.IsEnabled = true;

            }
            catch (Exception ex) {
                Debug.Print(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        private void Calculate(object sender, RoutedEventArgs e) {

           

        }


        private void OpenFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog {
                // Set the file dialog to filter for graphics files.
                Filter = "Text|*.txt|All|*.*",
                // Allow the user to select multiple images.
                Multiselect = true,
                Title = "Select your TimeFrame Files"
            };
            int id = 0;
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    Debug.Print(file);
                    Globals.Files.Add(new File(id, file, FileService.GetFileName(file), 0, false, false, new List<Cell>(),0,0,0, new string[0]));
                    id++;
                }
                selectedFilesDataGrid.ItemsSource = Globals.Files;
                if(Globals.Files.Count > 0){
                    PrepareButton.IsEnabled = true;
                    ClearButton.IsEnabled = true;
                }

            }
        }

        private void CloseApplication(object sender, RoutedEventArgs e) {
           
        }

        private void OpenHelpWindow(object sender, RoutedEventArgs e) {

        }

        private void Clear(object sender, RoutedEventArgs e) {
            Globals.Files.Clear();
            Globals.Cells.Clear();
            selectedFilesDataGrid.ItemsSource = null;
            CalculateButton.IsEnabled = false;
            PrepareButton.IsEnabled = false;
            ClearButton.IsEnabled = false;

        }
    }
}
