// <copyright file="XmlConnectionInfoViewModel.cs" company="eXtensible Solutions LLC">
// Copyright © 2015 All Right Reserved
// </copyright>

namespace XTool
{
    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
    using System.Windows.Input;



    public sealed class XmlConnectionInfoViewModel : ConnectionInfoViewModel
    {
        public new XmlConnectionInfo Model { get; set; }

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


        public XmlConnectionInfoViewModel(XmlConnectionInfo model)
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
            dialog.Filter = String.Format("Excel Worksheets|*.xls;*.xlsx");

            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                candidate = dialog.FileName;
                if (TryValidateCandidate(candidate, out info))
                {
                    Info = info;

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
            return true;
        }

        private bool CanExamineFileContents()
        {
            return (Info != null);
        }

        private void ExamineFileContents()
        {
            //ExcelHelper helper = new ExcelHelper(info.FullName);
            //if (helper.Interrogate())
            //{
            //    helper.ImportWorksheets();
            //    ViewModel = new DataSetViewModel(helper.Workbook) { Helper = helper };
            //}
        }
    }
}
