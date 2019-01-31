using System;
using System.Windows;
using System.Windows.Controls;
using HIPA.Statics;
using Microsoft.Win32;
using System.IO;


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
            {
                OutputpathBox.Text = Settings.Default.CustomOutputPath;
            }

            NormalizationMethodComboBox.ItemsSource = Globals.NormalizationMethods.Keys;
            NormalizationMethodComboBox.SelectedItem = Settings.Default.DefaultNormalization;
         
        }

        private void NormalizationMethodChanged(object sender, SelectionChangedEventArgs e)
        {

            Console.WriteLine((sender as ComboBox).SelectedItem as string);
            Settings.Default.DefaultNormalization = (sender as ComboBox).SelectedItem as string;
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
