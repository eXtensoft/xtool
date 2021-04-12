using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XTool
{
    public class TableDataEncryptor
    {

        public static DataTable Execute(EncryptionOption encryptMode, bool isUpdate, string encryptionKey, string connectionString, string targetColumnName, int targetColumnSize, string keyColumnName, string tableName, string schema = "dbo")
        {
            int i = 0;
            int maxLength = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Sequence", typeof(Int32));
            dt.Columns.Add(keyColumnName, typeof(System.String));
            dt.Columns.Add(targetColumnName, typeof(System.String));

            if (encryptMode == EncryptionOption.Decrypt)
            {
                dt.Columns.Add("decrypted", typeof(System.String));
            }

            if (encryptMode == EncryptionOption.Encrypt)
            {
                dt.Columns.Add("encrypted", typeof(System.String));

            }
            if (isUpdate)
            {
                dt.Columns.Add("Update Status", typeof(System.Boolean));
            }
            else
            {
                dt.Columns.Add("Length", typeof(System.Int32));
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
                    const string encryptedParamName = "@encrypted";
                    const string keyParamName = "@key";
                    using (SqlCommand cmdSelect = cn.CreateCommand())
                    {


                        #region initialize select command

                        cmdSelect.CommandType = System.Data.CommandType.Text;
                        cmdSelect.CommandText = "select [" + keyColumnName + "],[" + targetColumnName + "] from [" + schema + "].[" + tableName + "] ";

                        #endregion

                        using (SqlDataReader reader = cmdSelect.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = dt.NewRow();
                                int key = reader.GetInt32(reader.GetOrdinal(keyColumnName));
                                row[0] = ++i;
                                row[1] = key;

                                if (!reader.IsDBNull(reader.GetOrdinal(targetColumnName)))
                                {
                                    string data = reader.GetString(reader.GetOrdinal(targetColumnName));
                                    if (!String.IsNullOrWhiteSpace(data))
                                    {
                                        bool b = false;
                                        int fieldLength = 0;
                                        string fieldData = String.Empty;
                                        row[2] = data;
                                        if (encryptMode == EncryptionOption.Encrypt)
                                        {
                                            b = AesEncryptor.TryEncryptString(data, encryptionKey, out fieldData);
                                        }
                                        else if (encryptMode == EncryptionOption.Decrypt)
                                        {
                                            b = AesEncryptor.TryDecryptString(data, encryptionKey, out fieldData);
                                        }
                                        else
                                        {
                                            fieldLength = data.Length;
                                            if (fieldLength > maxLength)
                                            {
                                                maxLength = fieldLength;
                                            }
                                            row[3] = fieldLength;
                                        }
                                        if (b)
                                        {
                                            fieldLength = fieldData.Length;
                                            if (fieldLength > maxLength)
                                            {
                                                maxLength = fieldLength;
                                            }
                                            row[3] = fieldData;

                                            if (!isUpdate)
                                            {
                                                row[4] = fieldLength;
                                            }
                                        }
                                    }
                                }
                                dt.Rows.Add(row);
                            }
                        }


                    }

                    if (isUpdate)
                    {
                        using (SqlCommand cmdUpdate = cn.CreateCommand())
                        {
                            #region initialize update command

                            SqlParameter encryptedParam = new SqlParameter(encryptedParamName, System.Data.SqlDbType.Text);
                            SqlParameter keyParam = new SqlParameter(keyParamName, System.Data.SqlDbType.Int);
                            cmdUpdate.CommandType = System.Data.CommandType.Text;
                            cmdUpdate.CommandText = "update [" + schema + "].[" + tableName + "] set [" + targetColumnName + "] = @encrypted where [" + keyColumnName + "] = @key";
                            cmdUpdate.Parameters.Add(encryptedParam);
                            cmdUpdate.Parameters.Add(keyParam);
                            #endregion

                            foreach (DataRow row in dt.Rows)
                            {
                                int id;
                                if (Int32.TryParse(row[1].ToString(), out id))
                                {
                                    string data = (string)row[3];
                                    keyParam.Value = id;
                                    encryptedParam.Value = data;
                                    try
                                    {
                                        int rowsAffected = cmdUpdate.ExecuteNonQuery();
                                        row[4] = rowsAffected == 1;
                                    }
                                    catch (Exception ex)
                                    {
                                        string message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                                        System.Windows.MessageBox.Show(message);
                                        row[4] = false;
                                    }
                                }
                            }

                        }

                        //keyParam.Value = key;
                        //encryptedParam.Value = fieldData;
                        //try
                        //{
                        //    int rowsAffected = cmdUpdate.ExecuteNonQuery();
                        //    row[4] = true;
                        //}
                        //catch (Exception ex)
                        //{
                        //    row[4] = false;
                        //}
                    }
                }

            }
            catch (Exception ex)
            {

            }


            return dt;


        }



    }
}
