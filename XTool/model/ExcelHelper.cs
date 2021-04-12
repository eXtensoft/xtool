using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Windows;

namespace XTool
{
    public class ExcelHelper
    {
        private enum ExcelVersion
        {
            Xls,
            Xlsx
        }

        #region private fields

        private ExcelVersion _version;
        private bool _filevalidated;
        private Dictionary<string, string> _tablemaps = null;

        #endregion

        #region propertys

        #region Workbook (DataSet)

        private DataSet _Workbook = new DataSet();

        /// <summary>
        /// Gets or sets the DataSet value for Workbook
        /// </summary>
        /// <value> The DataSet value.</value>

        public DataSet Workbook
        {
            get { return _Workbook; }
            set
            {
                if (_Workbook != value)
                {
                    _Workbook = value;
                }
            }
        }

        #endregion

        #region FirstRowAsHeading (bool)
        private bool _firstrowasheading;
        public bool FirstRowAsHeading
        {
            get { return _firstrowasheading; }
            set { _firstrowasheading = value; }
        }

        #endregion

        #region Sheets (string)
        private List<string> _sheets = new List<string>();
        public List<string> Sheets
        {
            get { return _sheets; }
            set { _sheets = value; }
        }
        #endregion


        #region Sampling (int)
        private int _sampling;
        public int Sampling
        {
            get { return (_sampling > 0) ? _sampling : 0; }
            set { _sampling = value; }
        }
        #endregion

        #region DocumentPassword (string)
        private string _documentpassword;
        public string DocumentPassword
        {
            get { return (String.IsNullOrEmpty(_documentpassword)) ? String.Empty : _documentpassword; }
            set { _documentpassword = value; }
        }
        #endregion

        #region FullFilePath (string)
        private string _fullfilepath;
        public string FullFilePath
        {
            get { return (String.IsNullOrEmpty(_fullfilepath)) ? String.Empty : _fullfilepath; }
            set { _fullfilepath = value; }
        }
        #endregion

        #endregion

        #region constructors

        public ExcelHelper(string fullFilePath)
        {
            _fullfilepath = fullFilePath;
        }

        #endregion



        #region instance methods

        public DataSet FetchSample()
        {
            _Workbook = new DataSet();
            if (Interrogate())
            {
                ImportWorksheets();
            }
            DataSet ds = new DataSet();
            ds = _Workbook.Copy();
            return ds;
        }

        public bool Interrogate()
        {
            if (File.Exists(_fullfilepath) && ValidateVersion())
            {
                if (LoadSchemaFromFile(_fullfilepath))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }

        }


        public DataTable GetWorksheet(string s)
        {
            DataTable dt = _Workbook.Tables[s];
            if (dt != null)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        public void ImportWorksheets()
        {
            foreach (string s in _sheets)
            {
                DataTable dt = null;
                StringBuilder sb = new StringBuilder(s);
                sb.Append("$");
                dt = LoadSpecifiedSheet(FullFilePath, sb.ToString());
                if (dt != null)
                {
                    dt.TableName = s;
                    _Workbook.Tables.Add(dt);
                }

            }
        }


        public IList<string> GetWorkSheets()
        {
            IList<string> l = _sheets;
            return l;
        }

        public DataTable GetData(string sheetName)
        {
            if (_Workbook.Tables[sheetName] != null)
            {
                return _Workbook.Tables[sheetName];
            }
            else
            {
                return null;
            }
        }
        #endregion




        #region helper methods

        private DataTable LoadSpecifiedSheet(string fileName, string sheetName)
        {
            string s = sheetName.TrimEnd('$');
            OleDbConnection conn = this.ReturnConnection(fileName);
            DataTable SheetData = null;
            try
            {
                conn.Open();
                string sql = GenerateSelectStatement(sheetName);
                //retrieve datareader with data for that sheet			
                OleDbDataAdapter SheetAdapter = new OleDbDataAdapter(sql, conn);
                SheetData = new DataTable(_tablemaps[s]);
                SheetAdapter.Fill(SheetData);

                ///
                //OleDbCommand cmd = new OleDbCommand(sql, conn);
                //OleDbDataReader reader = cmd.ExecuteReader();
                //SheetData = new DataTable();
                //int max = reader.FieldCount;
                //for (int i = 1; i <= max; i++)
                //{
                //    SheetData.Columns.Add(String.Format("F{0}", i.ToString()), typeof(System.String));
                //}
                //if (reader.HasRows)
                //{
                //    while (reader.Read())
                //    {
                //        object o = reader[8];
                //        DataRow r = SheetData.NewRow();
                //        for (int i = 0; i < max; i++)
                //        {
                //            r[i] = reader[i].ToString();
                //        }
                //        SheetData.Rows.Add(r);
                //    }
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return SheetData;
        }

        private string GenerateSelectStatement(string sheetName)
        {
            string s = _tablemaps[sheetName.TrimEnd('$')] + "$";
            StringBuilder sb = new StringBuilder("Select ");
            if (Sampling > 0)
            {
                sb.Append("Top " + _sampling.ToString() + " ");
            }
            sb.Append("* From [");
            sb.Append(s);
            sb.Append("]");
            return sb.ToString();
            //return "select * from [" + sheetName + "]";
        }

        private bool LoadSchemaFromFile(string fileName)
        {
            _tablemaps = new Dictionary<string, string>();
            OleDbConnection conn = this.ReturnConnection(fileName);
            try
            {
                //http://www.microsoft.com/en-us/download/confirmation.aspx?id=23734

                conn.Open();

                DataTable SchemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null });
                if (SchemaTable.Rows.Count > 0)
                {
                    //SheetNames = new string[SchemaTable.Rows.Count];
                    int i = 0;
                    foreach (DataRow TmpRow in SchemaTable.Rows)
                    {
                        if (TmpRow["TABLE_TYPE"].ToString() == "TABLE")
                        {
                            _sheets.Add(StripNonAlphNumeric(TmpRow["TABLE_NAME"].ToString()));
                            i++;
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Opening the .xslx file failed because your system needs the following download.");
                sb.AppendLine("http://www.microsoft.com/en-us/download/confirmation.aspx?id=23734");
                System.Windows.MessageBox.Show(sb.ToString());
                return false;
            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }

        private string StripNonAlphNumeric(string p)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in p.ToCharArray())
            {
                if (Char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }
            string s = sb.ToString();
            _tablemaps.Add(s, p.Replace("$", ""));
            return s;
        }

        private OleDbConnection ReturnConnection(string fileName)
        {
            StringBuilder sb = new StringBuilder();
            switch (_version)
            {
                case ExcelVersion.Xls:
                    sb.Append("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + "; Jet OLEDB:Engine Type=5;" + "Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1\"");
                    break;
                case ExcelVersion.Xlsx:
                    sb.Append(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties=Excel 12.0");
                    break;
                default:
                    break;
            }
            return new OleDbConnection(sb.ToString());

        }

        private bool ValidateVersion()
        {
            bool b = false;
            FileInfo fi = new FileInfo(_fullfilepath);
            if (fi.Extension.ToLower() == ".xls")
            {
                _version = ExcelVersion.Xls;
                b = true;
            }
            else if (fi.Extension.ToLower() == ".xlsx")
            {
                _version = ExcelVersion.Xlsx;
                b = true;
            }
            _filevalidated = b;
            return b;
        }

        #endregion


    }
}

/*
 To use this download: 1.If you are the user of an application, consult your application 
 * documentation for details on how to use the appropriate driver.
 2.If you are an application developer using OLEDB, set the Provider argument 
 * of the ConnectionString property to “Microsoft.ACE.OLEDB.12.0”. 

If you are connecting to Microsoft Office Excel data, add the appropriate 
 * Extended Properties of the OLEDB connection string based on the Excel file type: 

File Type (extension)                                       Extended Properties
 ---------------------------------------------------------------------------------------------
 Excel 97-2003 Workbook (.xls)                              "Excel 8.0"
 Excel 2007 Workbook (.xlsx)                                 "Excel 12.0 Xml"
 Excel 2007 Macro-enabled workbook (.xlsm)          "Excel 12.0 Macro"
 Excel 2007 Non-XML binary workbook (.xlsb)          "Excel 12.0"
 

3.If you are an application developer using ODBC to connect to Microsoft Office Access data, 
 * set the Connection String to “Driver={Microsoft Access Driver (*.mdb, *.accdb)};DBQ=path to mdb/accdb file”
 4.If you are an application developer using ODBC to connect to Microsoft Office Excel data, 
 * set the Connection String to “Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};DBQ=path to xls/xlsx/xlsm/xlsb 

 */

