using System;
using System.Windows;
using System.Windows.Controls;
using HIPA.Statics;
using Microsoft.Win32;
using System.IO;
using HIPA.Classes.InputFile;
using HIPA.Services.SettingsHandler;
using System.Diagnostics;

namespace HIPA {
    /// <summary>
    /// Interaktionslogik für General.xaml
    /// </summary>
    public partial class CalculationsPage : Page
    {
        public CalculationsPage()
        {
            InitializeComponent();
            CustomFolderOutputCheckBox.IsChecked = Settings.Default.CustomOutputPathActive;

            if (CustomFolderOutputCheckBox.IsChecked.Value)
                OutputpathBox.Text = Settings.Default.CustomOutputPath;
            

           NormalizationMethodComboBox.ItemsSource = SettingsHandler.GetStringNormalizationMethods();
           NormalizationMethodComboBox.SelectedItem = SettingsHandler.LoadStoredNormalizationMethod().Item2;
         
        }

        private void NormalizationMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.Print("Test");
            Console.WriteLine((sender as ComboBox).SelectedItem.ToString() as string);
            Settings.Default.DefaultNormalization = Convert.ToInt32(SettingsHandler.GetNormalizationMethodEnumValue((sender as ComboBox).SelectedItem.ToString() as string));

            Console.WriteLine("Settings {0}", Settings.Default.DefaultNormalization);
            Settings.Default.Save();
        }

    

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            Settings.Default.CustomOutputPath = OutputpathBox.Text;
            Settings.Default.CustomOutputPathActive = CustomFolderOutputCheckBox.IsChecked.Value;
            Settings.Default.Save();
        }

        private void ChoosePath(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog browserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            browserDialog.ShowDialog();
            OutputpathBox.Text = browserDialog.SelectedPath;
        }

     
    }
}
