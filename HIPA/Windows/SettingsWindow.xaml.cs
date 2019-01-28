using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace HIPA.Windows {
    /// <summary>
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window {
        public SettingsWindow()
        {
            InitializeComponent();
            NormalizationMethodComboBox.ItemsSource = Globals.NormalizationMethods.Keys;
            NormalizationMethodComboBox.SelectedItem = Settings.Default.DefaultNormalization;
        }

        private void NormalizationMethodChanged(object sender, SelectionChangedEventArgs e)
        {

            Console.WriteLine((sender as ComboBox).SelectedItem as string);
            Settings.Default.DefaultNormalization = (sender as ComboBox).SelectedItem as string;
            Settings.Default.Save();
        }
    }
}
