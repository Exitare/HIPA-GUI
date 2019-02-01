using System;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using HIPA;
using HIPA.Files;
using System.Windows.Input;
using System.IO;
using System.IO.IsolatedStorage;
using HIPA.Calculations;
using HIPA.Statics;
using HIPA.Screens;
using HIPA.Updates;
using HIPA.Log;

namespace HIPA {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 



    public partial class MainWindow : Window {




        public MainWindow()
        {
           
            InitializeComponent();
            Create.CreateFiles();
            if (Settings.Default.Main_Window_Location_Left != 0 && Settings.Default.Main_Window_Location_Top != 0)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Left = Settings.Default.Main_Window_Location_Left;
                Top = Settings.Default.Main_Window_Location_Top;
            }

            UpdateMenu.IsEnabled = Globals.ConnectionSuccessful ? true : false;
            if (!Globals.ConnectionSuccessful) {
                MessageBox.Show("There was a problem reaching the Remote Server\nUpdates will be disabled!\nCheck your internet and proxy settings!");
                Logging.WriteLog("Error reaching the Remote Server. Disable Updates", LogLevel.Error); ;
            }
            progressBar.Value = 0;
            Globals.MyCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            Globals.InitializeNormalization();
            ComboBoxColumn.ItemsSource = Globals.NormalizationMethods.Keys;
            
        }



        private void Calculate(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            double step = 100 / Globals.Files.Count;
            Thread Calculations = new Thread(() =>
            {

                foreach (InputFile file in Globals.Files)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        StatusBarLabel.Text = file.Name;
                    });

                    InputFile.PrepareFiles();
                    Debug.Print(file.Normalization_Method);
                    Mean.Calculate_Baseline_Mean(file);
                    TimeFrameNormalization.Execute_Chosen_Normalization(file);
                    MinimumMaximum.FindTimeFrameMaximum(file);
                    MinimumMaximum.CalculateThreshold(file);
                    HighIntensity.Detect_Above_Below_Threshold(file);
                    HighIntensity.Count_High_Intensity_Peaks_Per_Minute(file);
                    bool hic_written = Write.Export_High_Intensity_Counts(file);
                    bool nt_written = Write.Export_Normalized_Timesframes(file);
                    this.Dispatcher.Invoke(() =>
                    {
                        if(!hic_written || !nt_written)
                        {
                            MessageBox.Show("There was a problem writing to the original source path.\nThe concerned files are placed in the programm execution folder", "Attention");
                        }
                        progressBar.Value = progressBar.Value + step;
                    });

                }

                this.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("All Files processed.", "Done");
                    StatusBarLabel.Text = "Done";
                });
            })
            {
                IsBackground = true
            };
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
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            bool updateAvailable = Update.CheckForUpdates(version);
            if (updateAvailable)
            {           
                if (MessageBox.Show("Updates available!\nDo you want to start the Update?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    // user clicked yes
                    Debug.Print("Yes clicked");
                    Update.StartUpdates();
                }
            } else
            {
                MessageBox.Show("No Update available", "Update");
            }


            //try
            //{

            //    Process firstProc = new Process();
            //    firstProc.StartInfo.FileName = "notepad.exe";
            //    firstProc.EnableRaisingEvents = true;

            //    firstProc.Start();

            //    firstProc.WaitForExit();

            //    //You may want to perform different actions depending on the exit code.
            //    Console.WriteLine("First process exited: " + firstProc.ExitCode);

            //    Process secondProc = new Process();
            //    secondProc.StartInfo.FileName = "mspaint.exe";
            //    secondProc.Start();

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("An error occurred!!!: " + ex.Message);
            //    return;
            //} 
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsScreen settingsScreen = new SettingsScreen();
            settingsScreen.Show();
        }

        private void WindowLocationChanged(object sender, EventArgs e)
        {
            Console.WriteLine(Application.Current.MainWindow.Top);

            Settings.Default.Main_Window_Location_Top = Application.Current.MainWindow.Top;
            Settings.Default.Main_Window_Location_Left = Application.Current.MainWindow.Left;
            Settings.Default.Save();

        }
    }
}


//lblTick.Text = "\u2713";