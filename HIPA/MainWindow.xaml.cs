using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;
using System.Data;
using System.Threading;
using System.ComponentModel;
using System.Collections.ObjectModel;
using HIPA.Windows;
using FileService;
using System.Windows.Input;

namespace HIPA {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window {




        public MainWindow()
        {



            InitializeComponent();
            progressBar.Value = 0;
            Globals.MyCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            Globals.InitializeNormalization();
            //Globals.Files.Add(new File(0, "", "Test", 0, new List<Cell>(), 0, 0, 0, new string[0], 0, "Kaya"));
            ComboBoxColumn.ItemsSource = Globals.NormalizationMethods.Keys;
            RefreshFilesDataGrid();
        }




        private void Calculate(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            double step = 100 / Globals.Files.Count;
            Thread Calculations = new Thread(() =>
            {

                foreach (InputFile file in Globals.Files)
                {
                   
                    InputFile.PrepareFiles();
                    Debug.Print(file.Normalization_Method);
                    Mean.Calculate_Baseline_Mean(file);
                    TimeFrameNormalization.Execute_Chosen_Normalization(file);
                    MinimumMaximum.FindTimeFrameMaximum(file);
                    MinimumMaximum.CalculateThreshold(file);
                    HighIntensity.Detect_Above_Below_Threshold(file);
                    HighIntensity.Count_High_Intensity_Peaks_Per_Minute(file);
                    Write.Export_High_Stimulus_Counts(file);
                    this.Dispatcher.Invoke(() =>
                    {
                        progressBar.Value = progressBar.Value + step;
                    });
                   
                }

            });
            Calculations.Start();
        }

        private void RefreshFilesDataGrid()
        {
            selectedFilesDataGrid.ItemsSource = null;
            selectedFilesDataGrid.ItemsSource = Globals.Files;
        }



        private void OpenFiles(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Set the file dialog to filter for graphics files.
                Filter = "Text|*.txt|All|*.*",
                // Allow the user to select multiple images.
                Multiselect = true,
                Title = "Select your TimeFrame Files"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Thread Prepare = new Thread(() =>
                {
                    try
                    {
                        InputFile.AddFilesToList(openFileDialog);
                        Read.ReadFileContent();
                        InputFile.PrepareFiles();
                    }
                    finally
                    {
                        this.Dispatcher.Invoke(() =>
                       {
                           if (Globals.Files.Count > 0)
                           {
                               ClearButton.IsEnabled = true;
                               CalculateButton.IsEnabled = true;
                           }
                           RefreshFilesDataGrid();
                       });
                    }
                });
                Prepare.Start();



            }
        }




        private void CloseApplication(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenHelpWindow(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.Show();
        }

    

        private void Clear(object sender, RoutedEventArgs e)
        {
            Globals.Files.Clear();

            selectedFilesDataGrid.ItemsSource = null;
            CalculateButton.IsEnabled = false;
            ClearButton.IsEnabled = false;

        }

  
        private void OpenExportWindow(object sender, RoutedEventArgs e)
        {
            ExportFiles exportFiles = new ExportFiles();
            exportFiles.Show();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            foreach (InputFile file in Globals.Files)
            {
                Debug.Print(file.Normalization_Method);
                Debug.Print(file.Stimulation_Timeframe.ToString());
            }
        }

        private void CheckForUpdates(object sender, RoutedEventArgs e)
        {
            try
            {

                Process firstProc = new Process();
                firstProc.StartInfo.FileName = "notepad.exe";
                firstProc.EnableRaisingEvents = true;

                firstProc.Start();

                firstProc.WaitForExit();

                //You may want to perform different actions depending on the exit code.
                Console.WriteLine("First process exited: " + firstProc.ExitCode);

                Process secondProc = new Process();
                secondProc.StartInfo.FileName = "mspaint.exe";
                secondProc.Start();

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred!!!: " + ex.Message);
                return;
            } 
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}


//lblTick.Text = "\u2713";