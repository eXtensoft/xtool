using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
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

namespace XTool
{
    /// <summary>
    /// Interaction logic for DataEncryptionView.xaml
    /// </summary>
    public partial class DataEncryptionView : UserControl, INotifyPropertyChanged
    {


        #region Data (DataTable)


        /// <summary>
        /// Gets or sets the DataTable value for Data
        /// </summary>
        /// <value> The DataTable value.</value>
        private DataTable _Data;
        public DataTable Data
        {
            get { return _Data; }
            set
            {
                if (_Data != value)
                {
                    _Data = value;
                    OnPropertyChanged("Data");
                }
            }
        }

        #endregion



        #region Encryption (EncryptionOption)


        /// <summary>
        /// Gets or sets the EncryptionOption value for Encryption
        /// </summary>
        /// <value> The EncryptionOption value.</value>
        private EncryptionOption _Encryption;
        public EncryptionOption Encryption
        {
            get { return _Encryption; }
            set
            {
                if (_Encryption != value)
                {
                    _Encryption = value;
                    OnPropertyChanged("Encryption");
                }
            }
        }

        #endregion

        #region IsShow (bool)

        /// <summary>
        /// Gets or sets the bool value for IsShow
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsShow = true;
        public bool IsShow
        {
            get { return _IsShow; }
            set
            {
                if (_IsShow != value)
                {
                    if (value)
                    {
                        Encryption = EncryptionOption.None;
                    }
                    _IsShow = value;
                    OnPropertyChanged("IsShow");
                }
            }
        }

        #endregion


        #region IsEncrypt (bool)

        /// <summary>
        /// Gets or sets the bool value for IsEncrypt
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsEncrypt;
        public bool IsEncrypt
        {
            get { return _IsEncrypt; }
            set
            {
                if (_IsEncrypt != value)
                {
                    if (value)
                    {
                        Encryption = EncryptionOption.Encrypt;
                    }
                    _IsEncrypt = value;
                    OnPropertyChanged("IsEncrypt");
                }
            }
        }

        #endregion


        #region IsDecrypt (bool)

        /// <summary>
        /// Gets or sets the bool value for IsDecrypt
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsDecrypt;
        public bool IsDecrypt
        {
            get { return _IsDecrypt; }
            set
            {
                if (_IsDecrypt != value)
                { 
                    if (value)
                    {
                        Encryption = EncryptionOption.Decrypt;
                    }
                    _IsDecrypt = value;
                    OnPropertyChanged("IsDecrypt");
                }
            }
        }

        #endregion


        #region IsUpdate (bool)

        /// <summary>
        /// Gets or sets the bool value for IsUpdate
        /// </summary>
        /// <value> The bool value.</value>
        private bool _IsUpdate;
        public bool IsUpdate
        {
            get { return _IsUpdate; }
            set
            {
                if (_IsUpdate != value)
                {
                    _IsUpdate = value;
                    OnPropertyChanged("IsUpdate");
                }
            }
        }

        #endregion

        #region EncryptionKey (string)

        /// <summary>
        /// Gets or sets the string value for EncryptionKey
        /// </summary>
        /// <value> The string value.</value>
        private string _EncryptionKey = "abcdefghijklmnopqrstuvwxyz012345";
        public string EncryptionKey
        {
            get { return _EncryptionKey; }
            set
            {
                if (_EncryptionKey != value)
                {
                    _EncryptionKey = value;
                    OnPropertyChanged("EncryptionKey");
                }
            }
        }

        #endregion

        private ICommand _ExecuteCommand;
        public ICommand ExecuteCommand
        {
            get
            {
                if (_ExecuteCommand == null)
                {
                    _ExecuteCommand = new RelayCommand(param => Execute(), param => CanExecute());
                }
                return _ExecuteCommand;
            }
        }

        private bool CanExecute()
        {
            return true;
        }

        private void Execute()
        {
            Data = null;
            DataTable dt = new DataTable() { TableName = "Data" };
            SqlTableViewModel vm = DataContext as SqlTableViewModel;
            if (vm != null)
            {
                ScriptGenerator generator = new ScriptGenerator(vm);
                Discovery.SqlColumn keyColumn;
                if (vm.TryFindPKColumn(out keyColumn))
                {
                    
                    var column = vm.Columns.ToList().Find(x => !x.IsPrimaryKey && x.IsEncrypt);
                    if (column != null)
                    {
                        dt = TableDataEncryptor.Execute(Encryption,IsUpdate, EncryptionKey, vm.GetConnectionString(), column.ColumnName,column.MaxLength, keyColumn.ColumnName, vm.TableName, vm.TableSchema);
                    }
                }
                Data = dt; 
            }
        }

        public DataEncryptionView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var data = DataContext;
            string s = data.GetType().Name;
            //MessageBox.Show(s);
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
