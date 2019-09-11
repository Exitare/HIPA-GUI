using System;
using System.Windows;
using System.Windows.Controls;
using HIPA.Statics;
using Microsoft.Win32;
using System.IO;
using HIPA.Classes.InputFile;
using HIPA.Services.SettingsHandler;
using System.Diagnostics;
using HIPA.Services.FileMgr;

namespace HIPA {
    /// <summary>
    /// Interaktionslogik für General.xaml
    /// </summary>
    public partial class FilePage : Page
    {
        public FilePage()
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
            Debug.WriteLine((sender as ComboBox).SelectedItem.ToString() as string);
            Settings.Default.DefaultNormalization = Convert.ToInt32(SettingsHandler.GetNormalizationMethodEnumValue((sender as ComboBox).SelectedItem.ToString() as string));

            Debug.WriteLine("Settings {0}", Settings.Default.DefaultNormalization);
            Settings.Default.Save();
        }

    

        private void SaveSettings(object sender, RoutedEventArgs e)
        {

            if (CustomFolderOutputCheckBox.IsChecked.Value && OutputpathBox.Text == "")
            {
                MessageBox.Show("You cannot save an empty output directory. Please choose a valid path!");
                CustomFolderOutputCheckBox.IsChecked = false;
                return;
            }
             
            if(CustomFolderOutputCheckBox.IsChecked.Value && !FileMgr.CheckCustomPath(OutputpathBox.Text))
            {
                MessageBox.Show("Directory is not writeable! Please choose another directory");
                CustomFolderOutputCheckBox.IsChecked = false;
                OutputpathBox.Text = "";
                return;
            }

            Settings.Default.CustomOutputPath = OutputpathBox.Text;
            Settings.Default.CustomOutputPathActive = CustomFolderOutputCheckBox.IsChecked.Value;
            Settings.Default.Save();
            MessageBox.Show("Settings saved", "Settings", MessageBoxButton.OK);
        }

        private void ChoosePath(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog browserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            browserDialog.ShowDialog();
            OutputpathBox.Text = browserDialog.SelectedPath;
        }

        private void CopySourceFileCheckboxChecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.CopySourceFile = CopySourceFileCheckbox.IsChecked.Value;
            Settings.Default.Save();
        }

        private void CustomFolderOutputCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.CustomOutputPathActive = CustomFolderOutputCheckBox.IsChecked.Value;
            Settings.Default.Save();

            if (CustomFolderOutputCheckBox.IsChecked.Value)
                SelectFolderButton.IsEnabled = true;
            else
                SelectFolderButton.IsEnabled = false;
        }
    }
}
