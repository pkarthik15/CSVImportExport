using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVImportExport
{
    public class CSVLibrary
    {
        public static DataTable Import(string srcFilePath, bool hasHeader)
        {           
            DataTable datatable = new DataTable();
            StreamReader sr = null;
            try
            {               
                using (sr = new StreamReader(new FileStream(srcFilePath, FileMode.Open, FileAccess.Read)))
                {
                    string line = string.Empty;
                    string[] headers = sr.ReadLine().Split(',');
                    DataRow dr = datatable.NewRow();                   
                    for (int i = 0; i < headers.Length; i++)
                    {                      
                        if (hasHeader)
                        {                            
                            datatable.Columns.Add(headers[i]);
                        }
                        else
                        {                          
                            datatable.Columns.Add("HEADER_" + i + 1);
                            dr[i] = headers[i];
                        }
                    }                    
                    if (!hasHeader)
                    {                       
                        datatable.Rows.Add(dr);
                    }                   
                    while ((line = sr.ReadLine()) != null)
                    {                      
                        string[] rows = line.Split(',');
                        dr = datatable.NewRow();
                        if (string.IsNullOrEmpty(line))
                        {                          
                            continue;
                        }                       
                        for (int i = 0; i < headers.Length; i++)
                        {                           
                            dr[i] = rows[i];
                        }                       
                        datatable.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {              
                throw ex;
            }
            finally
            {               
                sr.Dispose();
                sr.Close();
            }          
            return datatable;
        }

        public static bool Export(string destFilePath, DataTable dataTable)
        {           
            bool isSuccess = false;
            StreamWriter sw = null;
            try
            {               
                StringBuilder stringBuilder = new StringBuilder();             
                stringBuilder.Append(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList()) + "\n"); 
                dataTable.AsEnumerable().ToList<DataRow>().ForEach(row => stringBuilder.Append(string.Join(",", row.ItemArray) + "\n"));
                string fileContent = stringBuilder.ToString();
                sw = new StreamWriter(new FileStream(destFilePath, FileMode.Create, FileAccess.Write));
                sw.Write(fileContent);
                isSuccess = true;
            }
            catch (Exception ex)
            {              
                throw ex;
            }
            finally
            {               
                sw.Flush();
                sw.Dispose();
                sw.Close();
            }           
            return isSuccess;
        }
    }
}
