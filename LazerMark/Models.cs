using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LazerMark
{
    /// <summary>
    /// Строка для отправки и статус, была ли строка отослана
    /// </summary>
    internal class InfoToSend
    {
        public String destString { get; set; } = "";
        public bool sent { get; set; } = false;
    }
    internal class SentInfo
    {
        public String srcFile { get; set; }
        private Queue<String> sentInfo;
        public SentInfo(String srcName = "")
        {
            srcFile = srcName;
            sentInfo = new Queue<String>();
        }
        public bool FillData()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(srcFile, Encoding.GetEncoding("utf-8"));
            try
            {
                while (true)
                {
                    string str = sr.ReadLine();
                    sentInfo.Enqueue(str);
                    if (str == null) break;
                }
                sr.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }
        public String GetString()
        {
            return sentInfo.Peek();
        }
        public void Remove()
        {
            sentInfo.Dequeue();
        }
        public bool IsEmpty()
        {
            return sentInfo.Count < 1;
        }
    }
}
