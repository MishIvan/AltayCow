using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SocketSample
{
    internal class MyPublicCS
    {
        public static string destFile;

        public static string finishedFolder;

        static MyPublicCS()
        {
            MyPublicCS.destFile = "./now.txt";
            MyPublicCS.finishedFolder = "./FinishedData/";
        }

        public MyPublicCS()
        {
        }

        public static void CreateTxtFile(string content)
        {
            string srcFile = MyXmlSet.GetMyConfig("/configuration/srcFile");
            if (srcFile.Trim() != "")
            {
                if (File.Exists(srcFile))
                {
                    DirectoryInfo di = Directory.GetParent(srcFile);
                    string txtFilePath = string.Concat(di.ToString(), "/result.txt");
                    if (File.Exists(txtFilePath))
                    {
                        File.Delete(txtFilePath);
                    }
                    FileStream fsTxtFile = new FileStream(txtFilePath, FileMode.CreateNew, FileAccess.Write);
                    StreamWriter swTxtFile = new StreamWriter(fsTxtFile, Encoding.GetEncoding("UTF-8"));
                    swTxtFile.Write(content);
                    swTxtFile.Flush();
                    swTxtFile.Close();
                    fsTxtFile.Close();
                }
            }
        }

        public static DataTable GetTxtData(string strFile)
        {
            DataTable dataTable;
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            try
            {
                StreamReader sr = new StreamReader(strFile, Encoding.GetEncoding("utf-8"));
                try
                {
                    int i = 0;
                    while (true)
                    {
                        string str = sr.ReadLine();
                        string line = str;
                        if (str == null)
                        {
                            break;
                        }
                        DataRow dr = dt.NewRow();
                        string c0 = "";
                        string c1 = "";
                        if (line.Trim() != "")
                        {
                            i++;
                            c0 = i.ToString();
                            c1 = line.Trim();
                            dr[0] = c0;
                            dr[1] = c1;
                            dt.Rows.Add(dr);
                        }
                    }
                    sr.Close();
                }
                finally
                {
                    if (sr != null)
                    {
                        ((IDisposable)sr).Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                dataTable = null;
                return dataTable;
            }
            dataTable = dt;
            return dataTable;
        }

        public static int FillQueue(string strFile, Queue<string> qstring)
        {
            qstring.Clear();
            try
            {
                StreamReader sr = new StreamReader(strFile, Encoding.GetEncoding("utf-8"));
                while (true)
                {
                    string str = sr.ReadLine();
                    if (str == null)
                    {
                        break;
                    }
                    else
                    {
                        if(!string.IsNullOrEmpty(str)) qstring.Enqueue(str);
                    }
                }
                sr.Close();
            }
            catch (Exception exception)
            {                
                return qstring.Count;
            }
            return qstring.Count;
        }

    }
}