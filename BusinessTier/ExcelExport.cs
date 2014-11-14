using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MSExcel = Microsoft.Office.Interop.Excel;

using Infrastructure;
using System.IO;
using System.Data.OleDb;

namespace BusinessTier
{
    public class ExcelExport
    {
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="path"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string CreateExcelTable(DataSet ds, string path, string fileName, string excelType)
        {
            var dt = ds.Tables[0];
            object objMissing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Excel.Range head = null;
            Microsoft.Office.Interop.Excel.Range ranges = null;
            Microsoft.Office.Interop.Excel.Range range = null;
            try
            {
                var app = new Microsoft.Office.Interop.Excel.Application();
                app.Visible = false;
                Microsoft.Office.Interop.Excel.Workbook workBook = app.Workbooks.Add(objMissing);
                Microsoft.Office.Interop.Excel.Worksheet workSheet = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets[1];

                int colIndex = 0;

                string newFileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                Log.SaveNote(path + newFileName);

                foreach (DataColumn col in dt.Columns)
                {
                    //添加列名
                    colIndex++;
                    head = (Microsoft.Office.Interop.Excel.Range)workSheet.Cells[1, colIndex];
                    //head.Cells[1, colIndex] = col.ColumnName;
                    head.Value2 = col.ColumnName;
                    head = null;
                }

                int cols = dt.Columns.Count, rows = dt.Rows.Count;
                range = workSheet.get_Range("A2", objMissing);
                ranges = range.get_Resize(rows, cols);
                object[,] datas = new object[rows, cols];
                for (int r = 0; r < rows; r++)
                {
                    var row = dt.Rows[r];
                    for (int j = 0; j < cols; j++)
                    {
                        datas[r, j] = row[j];
                    }

                }
                ranges.Value2 = datas;

                workBook.SaveAs(path + "\\" + newFileName, objMissing, objMissing, objMissing, objMissing, objMissing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, objMissing,
                    objMissing, objMissing, objMissing);

                workBook.Close(false, objMissing, objMissing);


                app.Quit();
                workBook = null;
                workSheet = null;
                app = null;

                return excelType + "\\" + newFileName;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                ranges = null;
            }

        }


        /// <summary>
        /// 把数据从Excel装载到DataTable
        /// </summary>
        /// <param name="pathName">带路径的Excel文件名</param>
        /// <param name="sheetName">工作表名</param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string pathName, string sheetName)
        {
            DataTable tbContainer = new DataTable();
            string strConn = string.Empty;
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Sheet1";
            }
            FileInfo file = new FileInfo(pathName);
            if (!file.Exists)
            {
                throw new Exception("文件不存在");
            }
            string extension = file.Extension;
            try
            {
                switch (extension)
                {
                    case ".xls":
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                        break;
                    case ".xlsx":
                        strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathName + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
                        break;
                    default:
                        strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                        break;
                }

                //链接Excel
                OleDbConnection cnnxls = new OleDbConnection(strConn);
                //读取Excel里面有 表Sheet1
                OleDbDataAdapter oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
                DataSet ds = new DataSet();
                //将Excel里面有表内容装载到内存表中！
                oda.Fill(tbContainer);
            }
            catch (Exception)
            {
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathName + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                //链接Excel
                OleDbConnection cnnxls = new OleDbConnection(strConn);
                //读取Excel里面有 表Sheet1
                OleDbDataAdapter oda = new OleDbDataAdapter(string.Format("select * from [{0}$]", sheetName), cnnxls);
                DataSet ds = new DataSet();
                //将Excel里面有表内容装载到内存表中！
                oda.Fill(tbContainer);
            }

            return tbContainer;
        }

    }
}
