using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LazerMark
{
    internal class SentInfo
    {
        public String srcFile { get; set; }
        private Queue<String> sentInfo;
        private int _initialLineCount;
        public int initialLineCount { get { return _initialLineCount; } }
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
                sentInfo.Clear();
                _initialLineCount = 0;
                while (true)
                {
                    string str = sr.ReadLine();                    
                    if (str == null) break;
                    else
                    {
                        sentInfo.Enqueue(str);
                        _initialLineCount++;
                    }
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
