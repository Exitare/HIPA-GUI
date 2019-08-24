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
using HIPA.Services.SettingsHandler;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms.Design;
using System.IO;

enum DGColumnIDs
{
    PERCENTAGE_LIMIT = 7,
    STIMULATION_TIMEFRAME = 8
}


namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
           
           
            SettingsHandler.InitializeNormalizationMethods();
            if (!FileMgr.CheckCustomPath())
                MessageBox.Show("Could not write to given output path. Using own directory!", "Path");

            selectedFilesDataGrid.CellEditEnding += DataGrid_CellEditEnding;

            double locationLeft = Settings.Default.Main_Window_Location_Left;
            double locationTop = Settings.Default.Main_Window_Location_Top;
           
            if (locationLeft != 0 && locationTop != 0)
            {
                if(locationLeft < -1000 || locationLeft > 1000 ||locationTop < -1000 ||locationTop > 1000)
                    WindowStartupLocation = WindowStartupLocation.CenterScreen;                
                else
                {
                    WindowStartupLocation = WindowStartupLocation.Manual;
                    Left = Settings.Default.Main_Window_Location_Left;
                    Top = Settings.Default.Main_Window_Location_Top;
                }

            }
          

            versionLabel.Text = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
            UpdateMenu.IsEnabled = Globals.ConnectionSuccessful ? true : false;
            if (!Globals.ConnectionSuccessful)
                MessageBox.Show("There was a problem reaching the Remote Server\nUpdates will be disabled!\nCheck your internet and/or proxy settings!");

            if (Globals.UpdateAvailable)
                if (MessageBox.Show("Updates available!\nDo you want to start the Update?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    UpdateHandler.StartUpdate();


            InitializeUIState();
            ComboBoxColumn.ItemsSource = SettingsHandler.GetStringNormalizationMethods();

        }

        private async void Calculate(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 0;
            double step = 100 / Globals.GetFiles().Count;


            Task Calculations = Task.Run(() =>
            {

                foreach (InputFile file in Globals.GetFiles())
                {
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            StatusBarLabel.Text = file.Name;
                        });
                        Console.WriteLine("Using timeframe {0}", file.StimulationTimeframe);
                        file.CalculateBaselineMean();
                        file.ExecuteChosenNormalization();
                        file.FindTimeFrameMaximum();
                        file.CalculateThreshold();
                        file.DetectAboveBelowThreshold();
                        file.CountHighIntensityPeaksPerMinute();
                        file.ExportHighIntensityCounts();
                        file.ExportNormalizedTimesframes();

                        Dispatcher.Invoke(() =>
                        {
                            progressBar.Value += step;
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {

                            if (MessageBox.Show(ex.Message + "\nError occured.\nWould you like to proceed to process remaining files?\nYou could find more information about the error in the logs", "Error occured!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                return;

                        });
                    }


                }

            });
            await Calculations;

            MessageBox.Show("All Files processed.", "Done");
            StatusBarLabel.Text = "Done";
        }

        private void RefreshFilesDataGrid()
        {
            selectedFilesDataGrid.ItemsSource = null;
            selectedFilesDataGrid.ItemsSource = Globals.GetFiles();
        }

        //TODO Implement checks for user input
        void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            DataGridColumn column = e.Column;
            //DataGridRow row = e.Row;
            //int rowID = ((DataGrid)sender).ItemContainerGenerator.IndexFromContainer(row);
            int colummID = column.DisplayIndex;

            if (e.EditAction == DataGridEditAction.Commit)
            {
                if (column != null)
                {
                    switch (colummID)
                    {
                        case (int)DGColumnIDs.STIMULATION_TIMEFRAME:

                            break;

                        case (int)DGColumnIDs.PERCENTAGE_LIMIT:

                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private async void OpenFiles(object sender, RoutedEventArgs e)
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
                Task Preparation = Task.Run(() =>
                {

                    InputFile.AddFilesToList(openFileDialog);

                    foreach (InputFile file in Globals.GetFiles())
                    {
                        try
                        {
                            if (!file.Prepared)
                                file.PrepareFile();
                        }
                        catch (Exception ex)
                        {

                            file.Invalid = true;
                            if (MessageBox.Show(ex.Message + "\nError occured. Would you like to process all remaining files?", "Error occured!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                                return;
                        }
                    }


                });

                await Preparation;

                Task Cleanup = Task.Run(() =>
                {
                    Globals.GetFiles().RemoveAll(file => file.Invalid == true);
                });

                await Cleanup;

                if (Globals.GetFiles().Count != 0)
                    StatusBarLabel.Text = "Prepared all remaining files";
                else
                    StatusBarLabel.Text = "No files to process";

                if (Globals.GetFiles().Count > 0)
                {
                    ClearButton.IsEnabled = true;
                    CalculateButton.IsEnabled = true;
                }

                RefreshFilesDataGrid();


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
            UpdateHandler.CheckForUpdate();
            if (Globals.UpdateAvailable)
            {
                if (MessageBox.Show("Updates available!\nDo you want to start the Update?", "Update", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    UpdateHandler.StartUpdate();
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