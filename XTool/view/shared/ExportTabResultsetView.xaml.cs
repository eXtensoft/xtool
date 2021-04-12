using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Data;

namespace XTool
{
    /// <summary>
    /// Interaction logic for ExportTabResultsetView.xaml
    /// </summary>
    public partial class ExportTabResultsetView : UserControl, INotifyPropertyChanged
    {
        FontFamily fixedwidth = new FontFamily("Courier New");
        FontFamily allelse = new FontFamily("Consolas");

        private DataTable dt = null;

        private OutputTypeOption _OutputType = OutputTypeOption.Delimited;

        #region XmlRootTag (string)

        private string _XmlRootTag = "Dataset";

        /// <summary>
        /// Gets or sets the string value for XmlRootTag
        /// </summary>
        /// <value> The string value.</value>

        public string XmlRootTag
        {
            get { return (String.IsNullOrEmpty(_XmlRootTag)) ? String.Empty : _XmlRootTag; }
            set
            {
                if (_XmlRootTag != value)
                {
                    _XmlRootTag = value;
                    OnPropertyChanged("XmlRootTag");
                }
            }
        }

        #endregion

        #region XmlItemTag (string)

        private string _XmlItemTag = "Row";

        /// <summary>
        /// Gets or sets the string value for XmlItemTag
        /// </summary>
        /// <value> The string value.</value>

        public string XmlItemTag
        {
            get { return _XmlItemTag; }
            set
            {
                if (_XmlItemTag != value)
                {
                    _XmlItemTag = value;
                    OnPropertyChanged("XmlItemTag");
                }
            }
        }

        #endregion

        #region IsDelimitedChecked (bool)


        /// <summary>
        /// Gets or sets the bool value for IsDelimitedChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsDelimitedChecked
        {
            get { return GetOutputType(OutputTypeOption.Delimited); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Delimited);
                }
            }
        }

        #endregion

        #region IsXmlChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsXmlChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsXmlChecked
        {
            get { return GetOutputType(OutputTypeOption.Xml); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Xml);
                }
            }
        }

        #endregion

        #region IsFixedChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsFixedChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsFixedChecked
        {
            get { return GetOutputType(OutputTypeOption.Fixed); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Fixed);
                }

            }
        }

        #endregion

        #region IsDynamicChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsExplicitChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsDynamicChecked
        {
            get { return GetOutputType(OutputTypeOption.Dynamic); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Dynamic);
                }

            }
        }

        #endregion

        #region IsTemplateChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsTemplateChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsTemplateChecked
        {
            get { return GetOutputType(OutputTypeOption.Template); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Template);
                }

            }
        }

        #endregion

        #region IsJsonChecked (bool)

        /// <summary>
        /// Gets or sets the bool value for IsJsonChecked
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsJsonChecked
        {
            get { return GetOutputType(OutputTypeOption.Json); }
            set
            {
                if (value)
                {
                    SetOutputType(OutputTypeOption.Json);
                }

            }
        }

        #endregion

        private DelimiterOption _Delimiter;
        public DelimiterOption Delimiter
        {
            get { return _Delimiter; }
            set
            {
                if (_Delimiter != value)
                {
                    _Delimiter = value;
                    OnPropertyChanged("Delimiter");
                }
            }
        }



        private PadOption _Pad = PadOption.Right;

        #region IsPadLeft (bool)


        /// <summary>
        /// Gets or sets the bool value for IsPadLeft
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsPadLeft
        {
            get { return (_Pad == PadOption.Left) ? true : false; }
            set
            {
                _Pad = (value) ? PadOption.Left : PadOption.Right;
                OnPropertyChanged("IsPadRight");
                OnPropertyChanged("IsPadLeft");
            }
        }

        #endregion

        #region IsPadRight (bool)

        /// <summary>
        /// Gets or sets the bool value for IsPadRight
        /// </summary>
        /// <value> The bool value.</value>

        public bool IsPadRight
        {
            get { return (_Pad == PadOption.Right) ? true : false; }
            set
            {
                _Pad = (value) ? PadOption.Right : PadOption.Left;
                OnPropertyChanged("IsPadRight");
                OnPropertyChanged("IsPadLeft");
            }
        }

        #endregion

        #region Padding (int)

        private int _Padding;

        /// <summary>
        /// Gets or sets the int value for Padding
        /// </summary>
        /// <value> The int value.</value>

        public int PadAmount
        {
            get { return _Padding; }
            set
            {
                if (_Padding != value)
                {
                    _Padding = value;
                    OnPropertyChanged("PadAmount");
                }
            }
        }

        #endregion


        #region PrePend (string)

        private string _PrePend;

        /// <summary>
        /// Gets or sets the string value for PrePend
        /// </summary>
        /// <value> The string value.</value>

        public string PrePend
        {
            get { return (String.IsNullOrEmpty(_PrePend)) ? String.Empty : _PrePend; }
            set
            {
                if (_PrePend != value)
                {
                    _PrePend = value;
                    OnPropertyChanged("PrePend");

                }
            }
        }

        #endregion

        #region PostPend (string)

        private string _PostPend;

        /// <summary>
        /// Gets or sets the string value for PostPend
        /// </summary>
        /// <value> The string value.</value>

        public string PostPend
        {
            get { return (String.IsNullOrEmpty(_PostPend)) ? String.Empty : _PostPend; }
            set
            {
                if (_PostPend != value)
                {
                    _PostPend = value;
                    OnPropertyChanged("PostPend");
                }
            }
        }

        #endregion

        #region FieldDelimiter (string)

        private string _FieldDelimiter;

        /// <summary>
        /// Gets or sets the string value for FieldDelimiter
        /// </summary>
        /// <value> The string value.</value>

        public string FieldDelimiter
        {
            get { return (String.IsNullOrEmpty(_FieldDelimiter)) ? String.Empty : _FieldDelimiter; }
            set
            {
                if (_FieldDelimiter != value)
                {
                    _FieldDelimiter = value;
                    OnPropertyChanged("FieldDelimiter");
                }
            }
        }

        #endregion

        #region RecordDelimiter (string)

        private string _RecordDelimiter;

        /// <summary>
        /// Gets or sets the string value for RecordDelimiter
        /// </summary>
        /// <value> The string value.</value>

        public string RecordDelimiter
        {
            get { return (String.IsNullOrEmpty(_RecordDelimiter)) ? String.Empty : _RecordDelimiter; }
            set
            {
                if (_RecordDelimiter != value)
                {
                    _RecordDelimiter = value;
                    OnPropertyChanged("RecordDelimiter");
                }
            }
        }

        #endregion


        public string OutputToFileLabel
        {
            get
            {
                return String.Format("Write to {0} file...", _OutputType.ToString());
            }
        }

        private ICommand _OutputToFileCommand;
        public ICommand OutputToFileCommand
        {
            get
            {
                if (_OutputToFileCommand == null)
                {
                    _OutputToFileCommand = new RelayCommand(
                        param => OutputToFile(),
                        param => CanOutputToFile());
                }
                return _OutputToFileCommand;
            }
        }

        private ICommand _PreviewCommand;
        public ICommand PreviewCommand
        {
            get
            {
                if (_PreviewCommand == null)
                {
                    _PreviewCommand = new RelayCommand(
                        param => Preview(),
                        param => CanPreview());
                }
                return _PreviewCommand;
            }
        }



        public ExportTabResultsetView()
        {
            InitializeComponent();
        }
        private bool CanOutputToFile()
        {
            return true;
        }

        private void OutputToFile()
        {

            string fullFilepath = String.Empty;
            switch (_OutputType)
            {
                case OutputTypeOption.Xml:
                    if (FileSystem.SolicitFilepath(String.Empty, ".xml", String.Empty, out fullFilepath))
                    {

                        dt.ToXmlFilepath(fullFilepath, _XmlRootTag, _XmlItemTag);
                    }
                    break;
                case OutputTypeOption.Delimited:
                    int i = (int)Delimiter;
                    string extension = (i == 44) ? ".csv" : (i == 9) ? ".tab" : ".txt";
                    char delimiter = (char)i;
                    if (FileSystem.SolicitFilepath(String.Empty, extension, String.Empty, out fullFilepath))
                    {
                        //vm.ToCsvFilepath(delimiter, fullFilepath);
                        dt.ToDelimitedFilepath(fullFilepath, delimiter);
                    }
                    break;
                case OutputTypeOption.Fixed:
                    if (FileSystem.SolicitFilepath(String.Empty, ".txt", String.Empty, out fullFilepath))
                    {
                        dt.ToFixedFilepath(fullFilepath, _Padding, _Pad);
                    }
                    break;
                case OutputTypeOption.Json:
                    if (FileSystem.SolicitFilepath(String.Empty, "", String.Empty, out fullFilepath))
                    {
                        dt.ToJsonFilepath(fullFilepath);
                    }
                    break;
                default:
                    break;
            }
        }

        private void Preview()
        {
            txbOutput.Text = String.Empty;
            txbOutput.FontFamily = allelse;

            if (dt != null)
            {
                switch (_OutputType)
                {
                    case OutputTypeOption.Xml:
                        txbOutput.Text = dt.ToXDocument(_XmlRootTag, _XmlItemTag).ToString();
                        break;
                    case OutputTypeOption.Delimited:
                        int i = (int)Delimiter;
                        char delimiter = (char)i;
                        txbOutput.Text = dt.ToDelimited(delimiter);
                        break;
                    case OutputTypeOption.Fixed:
                        txbOutput.FontFamily = fixedwidth;
                        txbOutput.Text = dt.ToFixed(_Padding, _Pad);
                        break;
                    case OutputTypeOption.Json:
                        txbOutput.Text = dt.ToJson();
                        break;
                    case OutputTypeOption.Dynamic:
                        txbOutput.Text = dt.ToDynamic(_RecordDelimiter, _FieldDelimiter, _PrePend, _PostPend);
                        break;
                    case OutputTypeOption.Template:
                        if (!IsTemplateChecked)
                        {
                            txbOutput.Text = dt.ToJson();
                        }
                        else
                        {
                            txbTemplateData.Text = dt.ToJson();
                        }
                        
                        break;
                    default:
                        break;
                }

            }
        }

        private bool CanPreview()
        {
            return true;
        }

        private void SetOutputType(OutputTypeOption option)
        {
            _OutputType = option;
            foreach (OutputTypeOption item in Enum.GetValues(typeof(OutputTypeOption)))
            {
                string s = String.Format("Is{0}Checked", item.ToString());
                OnPropertyChanged(s);
                OnPropertyChanged("OutputToFileLabel");
            }
        }

        private bool GetOutputType(OutputTypeOption caller)
        {
            return (caller == _OutputType);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dt = DataContext as DataTable;
        }

    }
}
