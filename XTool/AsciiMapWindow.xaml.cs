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

namespace XTool
{
    /// <summary>
    /// Interaction logic for AsciiMapWindow.xaml
    /// </summary>
    public partial class AsciiMapWindow : Window
    {
        public List<AsciiMap> Maps { get; set; }
        public AsciiMapWindow()
        {
            InitializeComponent();
        }

        private void Button_Read(object sender, RoutedEventArgs e)
        {

            Maps = Bootstrapper.GetAsciiMaps();
            dgrItems.ItemsSource = Maps;
        }

        private void Button_Write(object sender, RoutedEventArgs e)
        {
            string fullFilepath = String.Empty;
            if (FileSystem.SolicitFilepath(String.Empty, ".xml", String.Empty, out fullFilepath))
            {
                GenericSerializer.WriteGenericList<AsciiMap>(Maps, fullFilepath);
            }
        }
    }
}
