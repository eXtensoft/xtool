// <copyright file="FileConnectionInfoViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Windows.Input;


    public class FileConnectionInfoViewModel: ConnectionInfoViewModel
    {

        private bool _HasHeader;
        public bool HasHeader
        {
            get { return _HasHeader; }
            set
            {
                _HasHeader = value;
                OnPropertyChanged("HasHeader");
                TryParseDelimited();
            }
        }


        private DelimiterOption _Delimiter;
        public DelimiterOption Delimiter
        {
            get { return _Delimiter; }
            set
            {
                _Delimiter = value;
                OnPropertyChanged("Delimiter");
                TryParseDelimited();
            }
        }


        private TabDataProfiler _Profiler = null;
        public TabDataProfiler DataProfiler
        {
            get { return _Profiler; }
            set
            {
                _Profiler = value;
                OnPropertyChanged("DataProfiler");
            }
        }

        private DataSetViewModel _ViewModel;
        public DataSetViewModel ViewModel
        {
            get { return _ViewModel; }
            set
            {
                _ViewModel = value;
                OnPropertyChanged("ViewModel");
            }
        }

        private string _Data = String.Empty;
        public string Data
        {
            get { return _Data; }
            set
            {
                _Data = value;
                OnPropertyChanged("Data");
            }
        }

        private DataTable _TabularData = new DataTable();
        public DataTable TabularData
        {
            get { return _TabularData; }
            set
            {
                _TabularData = value;
                OnPropertyChanged("TabularData");
            }
        }

        private int _LineCount;
        public int LineCount
        {
            get { return _LineCount; }
            set
            {
                _LineCount = value;
                OnPropertyChanged("LineCount");
            }
        }

        private bool _HasEnclosingQuotes;
        public bool HasEnclosingQuotes
        {
            get { return _HasEnclosingQuotes;  }
            set
            {
                _HasEnclosingQuotes = value;
                OnPropertyChanged("HasEnclosingQuotes");
            }
        }

        private Display _SelectedCount = null;
        public Display SelectedCount
        {
            get { return _SelectedCount; }
            set
            {
                _SelectedCount = value;
                OnPropertyChanged("SelectedDisplay");
            }
        }

        public int Max
        {
            get
            {
                if (SelectedCount != null)
                {
                    if (SelectedCount.MaxDistinct < LineCount)
                    {
                        return SelectedCount.MaxDistinct;
                    }
                    else
                    {
                        return LineCount;
                    }
                }
                else
                {
                    return 10;
                }
            }
        }

        private FileInfo _Info = null;

        public FileInfo Info
        {
            get
            {
                return _Info;
            }
            set
            {
                _Info = value;
                OnPropertyChanged("Info");
            }
        }
        private string _Candidate = String.Empty;

        private ICommand _OpenFileCommand;
        public ICommand OpenFileCommand
        {
            get
            {
                if (_OpenFileCommand == null)
                {
                    _OpenFileCommand = new RelayCommand(
                        param => OpenFile(),
                        param => CanOpenFile());
                }
                return _OpenFileCommand;
            }
        }

        private ICommand _ExamineFileContentsCommand;
        public ICommand ExamineFileContentsCommand
        {
            get
            {
                if (_ExamineFileContentsCommand == null)
                {
                    _ExamineFileContentsCommand = new RelayCommand(
                        param => ExamineFileContents(),
                        param => CanExamineFileContents());
                }
                return _ExamineFileContentsCommand;
            }
        }


        public FileConnectionInfoViewModel(FileConnectionInfo model)
        {
            Model = model;
            base.Model = model;
        }

        private bool CanOpenFile()
        {
            bool b = true;
            return b;
        }

        private void OpenFile()
        {

            FileInfo info = null;
            string candidate = String.Empty;
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = String.Format("Flat Files|*.csv;*.txt");

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                candidate = dialog.FileName;
                if (TryValidateCandidate(candidate, out info))
                {
                    Info = info;
                    Name = Info.FullName;

                }
            }
        }

        private bool TryValidateCandidate(string candidate, out FileInfo info)
        {
            info = null;
            bool b = false;
            if (!String.IsNullOrEmpty(candidate) && File.Exists(candidate))
            {
                info = new FileInfo(candidate);
                if (PerformBasicValidations(info))
                {
                    b = true;
                }
            }
            return b;
        }

        private bool PerformBasicValidations(FileInfo info)
        {            
            string[] t = File.ReadAllLines(info.FullName);
            LineCount = t.Length;
            return true;
        }

        private bool CanExamineFileContents()
        {
            return (Info != null);
        }

        private void ExamineFileContents()
        {
            if (Info != null && Info.Exists)
            {
                string[] t = File.ReadAllLines(Info.FullName);
                StringBuilder sb = new StringBuilder();
                int max = Max;
                if (t.Length >= max)
                {
                    for (int i = 0; i < Max; i++)
                    {
                        sb.AppendLine(t[i]);
                    }
                }
                Data = sb.ToString();
                TryParseDelimited();
            }
        }

        private bool CanExecuteProfile()
        {
            return true;
        }

        private void ExecuteProfile()
        {
            DataTable dt = new DataTable();
            DataTableReader r = new DataTableReader(dt);

            ProfilerSettings settings = new ProfilerSettings() { MaxDistinct = 100 };
            TabDataProfiler profiler = new TabDataProfiler(settings);
            profiler.Execute(r);
            profiler.Calculate();
            DataProfiler = profiler;
        }

        private void TryParseDelimited()
        {
            int i = 9;
            switch (Delimiter)
            {
                case DelimiterOption.Comma:
                    i = 44;
                    break;
                case DelimiterOption.Tab:
                    i = 9;
                    break;
                case DelimiterOption.Pipe:
                    i = 124;
                    break;
                case DelimiterOption.Tilde:
                    i = 126;
                    break;
                case DelimiterOption.Semicolon:
                    i = 59;
                    break;
                default:
                    break;
            }
            char c = (char)i;
            TryParseDelimited(c);
        }

        private void TryParseDelimited(char c)
        {
            DataTable dt = new DataTable();
            char[] delimiters = new char[1];
            delimiters[0] = c;
            if (Info != null && Info.Exists)
            {
                SortedDictionary<int, int> fieldsprofile = new SortedDictionary<int, int>();
                string[] t = File.ReadAllLines(Info.FullName);
                int start = HasHeader ? 1 : 0;
                string[] h = null;
                if (HasHeader)
                {
                    h = t[0].Split(delimiters);
                }
                StringBuilder sb = new StringBuilder();
                int max = Max;
                if (t.Length >= max)
                {
                    for (int i = start; i < max; i++)
                    {
                        string[] f = t[i].Split(delimiters);
 
                        if (!fieldsprofile.ContainsKey(f.Length))
                        {
                            fieldsprofile.Add(f.Length, 0);
                        }
                        fieldsprofile[f.Length]++;
                        sb.AppendLine(t[i]);
                    }

                    var kvp = fieldsprofile.Max();
                    for (int i = 0; i < kvp.Key; i++)
                    {
                        string name = HasHeader ? h[i] : String.Format("field-{0}", i + 1);
                        dt.Columns.Add(new DataColumn(name));
                    }
                    for (int i = start; i < max; i++)
                    {
                        DataRow row = dt.NewRow();
                        var arr = t[i].Split(delimiters);

                        row.ItemArray = CleanArray(arr);
                        dt.Rows.Add(row);
                    }
                }
                Data = sb.ToString();
                TabularData = dt;
            }
        }

        private object[] CleanArray(string[] arr)
        {

            if (!HasEnclosingQuotes)
            {
                return arr;
            }
            else
            {
                var trim = new char[] { '\'', '\"' };
                List<string> list = new List<string>();
                foreach (var item in arr)
                {
                    string s = item.Trim(trim);
                    list.Add(s);
                }
                return list.ToArray();
            }
        }

        protected override bool CanExecuteDiscovery()
        {
            return true;
        }

        protected override void ExecuteDiscovery()
        {
            int k = 9;
            switch (Delimiter)
            {
                case DelimiterOption.Comma:
                    k = 44;
                    break;
                case DelimiterOption.Tab:
                    k = 9;
                    break;
                case DelimiterOption.Pipe:
                    k = 124;
                    break;
                case DelimiterOption.Tilde:
                    k = 126;
                    break;
                default:
                    break;
            }
            char c = (char)k;

            DataTable dt = new DataTable();
            char[] delimiters = new char[1];
            delimiters[0] = c;
            if (Info.Exists)
            {
                SortedDictionary<int, int> fieldsprofile = new SortedDictionary<int, int>();
                string[] t = File.ReadAllLines(Info.FullName);
                int start = HasHeader ? 1 : 0;
                string[] h = null;
                if (HasHeader)
                {
                    h = t[0].Split(delimiters);
                }

                StringBuilder sb = new StringBuilder();
                for (int i = start; i < t.Length; i++)
                {
                    string[] f = t[i].Split(delimiters);

                    if (!fieldsprofile.ContainsKey(f.Length))
                    {
                        fieldsprofile.Add(f.Length, 0);
                    }
                    fieldsprofile[f.Length]++;
                    sb.AppendLine(t[i]);
                }

                var kvp = fieldsprofile.Max();
                for (int i = 0; i < kvp.Key; i++)
                {
                    string name = HasHeader ? h[i] : String.Format("field-{0}", i + 1);
                    dt.Columns.Add(new DataColumn(name));
                }
                for (int i = start; i < t.Length; i++)
                {
                    DataRow row = dt.NewRow();
                    row.ItemArray = t[i].Split(delimiters);
                    dt.Rows.Add(row);
                }

                Data = sb.ToString();
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ViewModel = new DataSetViewModel(ds);
            }
        }



    }
}
