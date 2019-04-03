using System;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using HIPA;
using HIPA.FileMgr;
using System.Windows.Input;
using System.IO;
using System.IO.IsolatedStorage;
using HIPA.Calculations;
using HIPA.Statics;
using HIPA.Screens;
using HIPA.Updates;
using HIPA.Log;
using System.Windows.Threading;
using HIPA.Queue;

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
            //if (Settings.Default.Main_Window_Location_Left != 0 && Settings.Default.Main_Window_Location_Top != 0)
            //{
            //    WindowStartupLocation = WindowStartupLocation.Manual;
            //    Left = Settings.Default.Main_Window_Location_Left;
            //    Top = Settings.Default.Main_Window_Location_Top;
            //} 

            UpdateMenu.IsEnabled = Globals.ConnectionSuccessful ? true : false;
            if (!Globals.ConnectionSuccessful) {
                MessageBox.Show("There was a problem reaching the Remote Server\nUpdates will be disabled!\nCheck your internet and proxy settings!");
                Logging.WriteLog("Error reaching the Remote Server. Disable Updates", LogLevel.Warning); ;
            }
            InitializeUIState();
            Globals.MyCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));
            Globals.InitializeNormalization();
            ComboBoxColumn.ItemsSource = Globals.NormalizationMethods.Keys;
            
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            Logging.WriteLog(e.ToString(), LogLevel.Error);
            Logging.WriteLog(sender.ToString(), LogLevel.Error);
            // Prevent default unhandled exception processing
            e.Handled = true;
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

                    file.Calculate_Baseline_Mean();
                    file.Execute_Chosen_Normalization();
                    file.FindTimeFrameMaximum();
                    file.CalculateThreshold();
                    file.Detect_Above_Below_Threshold();
                    file.Count_High_Intensity_Peaks_Per_Minute();

                    bool hic_written = file.Export_High_Intensity_Counts();
                    bool nt_written = file.Export_Normalized_Timesframes();

                    Dispatcher.Invoke(() =>
                    {
                        if (!hic_written || !nt_written)
                            MessageBox.Show("There was a problem writing to the original source path.\nThe concerned files are placed in the programm execution folder", "Attention");

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
                Title = "Select your TimeFrame Files..."
            };

            if (openFileDialog.ShowDialog() == true)
            {
                Thread Prepare = new Thread(() =>
                {
                    try
                    {
                        InputFile.AddFilesToList(openFileDialog);
                        foreach (InputFile file in Globals.Files)
                        {
                            if (!file.PrepareFile())
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.CalculateButton.IsEnabled = false;
                                });
                        }

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

        private void InitializeUIState()
        {
            progressBar.Value = 0;
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
#if DEBUG
                    Debug.Print("Yes clicked");
#endif
                    Update.StartUpdates();
                }
            } else
            {
                MessageBox.Show("No Update available", "Update");
            }
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsScreen settingsScreen = new SettingsScreen();
            settingsScreen.Show();
        }

        private void WindowLocationChanged(object sender, EventArgs e)
        {
#if DEBUG
            //Console.WriteLine("Application height is {0}", Application.Current.MainWindow.Top);
#endif

            Settings.Default.Main_Window_Location_Top = Application.Current.MainWindow.Top;
            Settings.Default.Main_Window_Location_Left = Application.Current.MainWindow.Left;
            Settings.Default.Save();

        }
    }
}


//lblTick.Text = "\u2713";