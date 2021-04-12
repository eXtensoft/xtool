using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
using JunkHarness;
using Microsoft.Win32;

namespace XTool
{
    /// <summary>
    /// Interaction logic for SolutionView.xaml
    /// </summary>
    public partial class SolutionView : UserControl, INotifyPropertyChanged
    {

        private DataTable _FieldComparison;
        public DataTable FieldComparison
        {
            get { return _FieldComparison; }
            set
            {
                if (_FieldComparison != value)
                {
                    _FieldComparison = value;
                    OnPropertyChanged("FieldComparison");
                }
            }
        }

        private List<FileInfo> _Candidates;
        public string Filepath { get; set; }

        public List<SolutionFile> Solutions { get; set; }

        public DateTime Tds { get; set; }

        public string Branch { get; set; }


        public SolutionView()
        {
            InitializeComponent();
        }
        private void File_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
            {
                Tds = DateTime.Now;
                string[] candidates = ofd.FileNames;
                _Candidates = new List<FileInfo>();
                foreach (var item in candidates)
                {
                    _Candidates.Add(new FileInfo(item));
                }

            }
            Branch = "1.5";
        }


        private void Execute_Click(object sender, RoutedEventArgs e)
        {

            int j = Solutions.Count;
            //dgrItems.ItemsSource = Solutions;

            var c = new ReferenceFieldComparisonCollection(Solutions);
            DataTable dt = c.ToDataTable(JunkHarness.CompareOption.Project);
            dgrItems.ItemsSource = dt.DefaultView;
            dt.WriteXml(@"c:\temp\execute.xml", XmlWriteMode.WriteSchema);
            GenericSerializer.WriteGenericList<SolutionFile>(Solutions, "solutions.xml");
        }





        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_Candidates != null && _Candidates.Count > 0)
            {
                Solutions = new List<SolutionFile>();
                foreach (FileInfo info in _Candidates)
                {
                    SolutionFile sf = SolutionFile.Create(info, Branch, Tds);
                    sf.Moniker = String.Format("{0}-{1}-{2}.xml", info.Name, Branch, Tds.ToShortDateString().Replace('/', '.'));

                    Solutions.Add(sf);
                }
            }
            foreach (var item in Solutions)
            {
                GenericSerializer.WriteGenericItem<SolutionFile>(item, item.Moniker);

            }

            var c = new ReferenceFieldComparisonCollection(Solutions);
            DataTable dt = c.ToDataTable(JunkHarness.CompareOption.Project);
            dt.WriteXml(@"c:\temp\save.xml", XmlWriteMode.WriteSchema);
            dgrItems.ItemsSource = dt.DefaultView;
        }





        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion






    }

}
