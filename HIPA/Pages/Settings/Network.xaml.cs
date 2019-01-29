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
using HIPA.Statics;
namespace HIPA
{
    /// <summary>
    /// Interaktionslogik für Network.xaml
    /// </summary>
    public partial class Network : Page
    {
        public Network()
        {
            InitializeComponent();

            ProxyPortBox.Text = Settings.Default.Proxy_Port.ToString();
            ProxyURLBox.Text = Settings.Default.Proxy_URL;
            ProxyUsernameBox.Text = Settings.Default.Proxy_Username;
            ProxyPasswordBox.Password = Settings.Default.Proxy_Password;
            if (Settings.Default.Proxy_Active)
            {
                ProxyURLBox.IsEnabled = true;
                ProxyPortBox.IsEnabled = true;
                ProxyPasswordBox.IsEnabled = true;
                ProxyUsernameBox.IsEnabled = true;
                ProxyCheckBox.IsChecked = true;
            }
           
        }

        private void CheckedProxy(object sender, RoutedEventArgs e)
        {
            ProxyURLBox.IsEnabled = true;
            ProxyPortBox.IsEnabled = true;
            ProxyPasswordBox.IsEnabled = true;
            ProxyUsernameBox.IsEnabled = true;
        }

        private void UncheckedProxy(object sender, RoutedEventArgs e)
        {
            ProxyPortBox.IsEnabled = false;
            ProxyURLBox.IsEnabled = false;
            ProxyPasswordBox.IsEnabled = false;
            ProxyUsernameBox.IsEnabled = false;
        }


        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            if (ProxyPortBox.Text.Length == 0 || ProxyURLBox.Text.Length == 0)
            {
                MessageBox.Show("Please fill the needed Fields URL and Port");
                return;
            }



            Console.WriteLine(ProxyCheckBox.IsChecked.Value);
            if (ProxyCheckBox.IsChecked.Value)
            {
                Settings.Default.Proxy_Active = ProxyCheckBox.IsChecked.Value;
                Settings.Default.Proxy_URL = ProxyURLBox.Text;
                Settings.Default.Proxy_Password = ProxyPasswordBox.Password;
                Settings.Default.Proxy_Username = ProxyUsernameBox.Text;
                bool successfullyParsed = int.TryParse(ProxyPortBox.Text, out int port);
                if (port == 0)
                {
                    MessageBox.Show("Please insert a valid Portnumber");
                    return;
                }
                Settings.Default.Proxy_Port = port;
                Console.WriteLine("Using Proxy: {0}", Settings.Default.Proxy_URL + ":" + Settings.Default.Proxy_Port);
            }
            else
            {
                Settings.Default.Proxy_Active = ProxyCheckBox.IsChecked.Value;
            }
            Settings.Default.Save();
            

        }
    }
}
