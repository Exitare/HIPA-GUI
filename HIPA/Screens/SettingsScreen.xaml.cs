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
using HIPA.Statics;


namespace HIPA.Screens
{
    /// <summary>
    /// Interaktionslogik für SettingsScreen.xaml
    /// </summary>
    public partial class SettingsScreen : Window {
        public SettingsScreen()
        {
            InitializeComponent();
            CreateTabBar();

        }

        private void CreateTabBar()
        {
            CreateTab("Files", new FilePage());
            CreateTab("Network", new Network());
        }


        private void CreateTab(string name, Page page)
        {
            TabItem Tab = new TabItem
            {
                Header = name

            };

            Tab.Height = 50;
            Frame tabFrame = new Frame();
            tabFrame.Content = page;
            Tab.Content = tabFrame;
            TabControl.Items.Add(Tab);
        }


    }
}
