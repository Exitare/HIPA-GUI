using System;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using HIPA.Statics;
using HIPA.Screens;
using HIPA.Services.Updater;
using HIPA.Services.FileMgr;
using HIPA.Services.Log;
using System.Windows.Threading;
using System.Collections.Generic;
using HIPA.Classes.InputFile;
using HIPA.Services.Misc;

namespace HIPA {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window {



        public MainWindow()
        {
            InitializeComponent();
            FileMgr.CreateFiles();

            //if (Settings.Default.Main_Window_Location_Left != 0 && Settings.Default.Main_Window_Location_Top != 0)
            //{
            //    WindowStartupLocation = WindowStartupLocation.Manual;
            //    Left = Settings.Default.Main_Window_Location_Left;
            //    Top = Settings.Default.Main_Window_Location_Top;
            //} 

            versionLabel.Text = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
            UpdateMenu.IsEnabled = Globals.ConnectionSuccessful ? true : false;
            if (!Globals.ConnectionSuccessful)
                MessageBox.Show("There was a problem reaching the Remote Server\nUpdates will be disabled!\nCheck your internet and/or proxy settings!");




            if (Globals.UpdateAvailable)
                if (MessageBox.Show("Updates available!\nDo you want to start the Update?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    UpdateHandler.StartUpdates();


            InitializeUIState();          
            ComboBoxColumn.ItemsSource = InputFile.GetNormalizationMethods();
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            double step = 100 / Globals.GetFiles().Count;
            Thread Calculations = new Thread(() =>
            {

                foreach (InputFile file in Globals.GetFiles())
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        StatusBarLabel.Text = file.Name;
                    });

                    file.CalculateBaselineMean();
                    file.ExecuteChosenNormalization();
                    file.FindTimeFrameMaximum();
                    file.CalculateThreshold();
                    file.DetectAboveBelowThreshold();
                    file.CountHighIntensityPeaksPerMinute();

                    bool hic_written = file.ExportHighIntensityCounts();
                    bool nt_written = file.ExportNormalizedTimesframes();

                    Dispatcher.Invoke(() =>
                    {
                        if (!hic_written || !nt_written)
                            MessageBox.Show("There was a problem writing to the original source path.\nThe concerned files are placed in the program execution folder", "Attention");

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
            selectedFilesDataGrid.ItemsSource = Globals.GetFiles();
        }


        private void OpenFiles(object sender, RoutedEventArgs e)
        {
           
            List<InputFile> errorList = new List<InputFile>();
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
                        foreach (InputFile file in Globals.GetFiles())
                        {
                         
                            if (!file.PrepareFile())
                                Dispatcher.Invoke(() =>
                                {
                                    errorList.Add(file);
                                  
                                });

                        }
                    }
                    finally
                    {
                        Dispatcher.Invoke(() =>
                       {
                           if (Globals.GetFiles().Count > 0)
                           {
                              
                                                        
                               if (errorList.Count != 0)
                               {
                                   string errorMessage = "There were error(s) in those files:\n ";
                                   foreach (InputFile file in errorList)
                                   {
                                       errorMessage = errorMessage + "\n " + file.Name;
                                       Globals.GetFiles().Remove(file);
                                   }
                                   errorMessage = errorMessage + "\n\n More information can be found @ " + Globals.ErrorLog;
                                   errorList.Clear();
                                   MessageBox.Show(errorMessage, "Could not prepare files!");
                               }
                             
                           }

                           if(Globals.GetFiles().Count > 0)
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
            Globals.GetFiles().Clear();

            selectedFilesDataGrid.ItemsSource = null;
            CalculateButton.IsEnabled = false;
            ClearButton.IsEnabled = false;

        }

        private void CheckForUpdates(object sender, RoutedEventArgs e)
        {
            if (UpdateHandler.CheckForUpdate())
            {
                if (MessageBox.Show("Updates available!\nDo you want to start the Update?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    UpdateHandler.StartUpdates();
            }
            else
                MessageBox.Show("No Update available", "Update");
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsScreen settingsScreen = new SettingsScreen();
            settingsScreen.Show();
        }

        private void WindowLocationChanged(object sender, EventArgs e)
        {
            Settings.Default.Main_Window_Location_Top = Application.Current.MainWindow.Top;
            Settings.Default.Main_Window_Location_Left = Application.Current.MainWindow.Left;
            Settings.Default.Save();

        }
    }
}




//lblTick.Text = "\u2713";