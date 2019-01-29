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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HIPA;
using HIPA.Statics;

namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für General.xaml
    /// </summary>
    public partial class Calculations : Page
    {
        public Calculations()
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

    

        private void SaveSettings(object sender, RoutedEventArgs e)
        {

        }
    }
}
