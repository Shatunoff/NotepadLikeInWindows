using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notepad
{
    public class TextEditor
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Content { get; set; }

        public TextEditor()
        {
            CreateNew();
        }

        public TextEditor(string fileName)
        {
            LoadFile(fileName);
        }

        public void CreateNew()
        {
            FileName = "Безымянный";
            Content = String.Empty;
        }

        public void LoadFile(string fileName)
        {
            FileStream fs = null;
            StreamReader sr = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs, true);
                FileName = Path.GetFileName(fileName);
                FilePath = fileName;
                Content = String.Empty;
                while (!sr.EndOfStream)
                    Content += sr.Read();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                sr?.Close();
                fs?.Close();
            }
        }

        public void SaveFile(string fileName)
        {
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs, Encoding.Unicode);
                sw.Write(Content);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                sw?.Close();
                fs?.Close();
            }
        }
    }
}
