using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Notepad
{
    public class TextEditor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _fileName;
        private string _filePath;
        private string _content;
        private Encoding _contentEncoding;

        public string FileName
        {
            get
            {
                return _fileName;
            }
            private set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        public string FilePath 
        { 
            get
            {
                return _filePath;
            }
            private set
            {
                _filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }

        public string Content 
        { 
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                NotifyPropertyChanged("Content");
                NotifyPropertyChanged("ContentLength");
                NotifyPropertyChanged("ContentLengthWithoutSpaces");
            }
        }

        public string ContentEncodingHeaderName
        {
            get
            {
                return _contentEncoding.HeaderName;
            }
        }

        public string ContentEncodingName
        {
            get
            {
                return _contentEncoding.EncodingName;
            }
        }

        public int ContentLength
        {
            get
            {
                return _content.Length;
            }
        }

        public int ContentLengthWithoutSpaces
        {
            get
            {
                return (((_content.Replace('\n', ' ')).Replace('\r', ' ')).Replace(" ", "")).Length;
            }
        }

        public Encoding ContentEncoding
        {
            get
            {
                return _contentEncoding;
            }
            set
            {
                _contentEncoding = value;
                NotifyPropertyChanged("ContentEncoding");
                NotifyPropertyChanged("ContentEncodingName");
                NotifyPropertyChanged("ContentEncodingHeaderName");
            }
        }

        public TextEditor()
        {
            CreateNew();
        }

        public TextEditor(string fileName)
        {
            LoadFile(fileName);
        }

        // Уведомление об изменении свойства
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public void CreateNew()
        {
            FileName = "Безымянный";
            Content = String.Empty;
            ContentEncoding = Encoding.UTF8;
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
                Content = sr.ReadToEnd();
                ContentEncoding = sr.CurrentEncoding;
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
                sw = new StreamWriter(fs, ContentEncoding);
                sw.Write(Content);
                FilePath = fileName;
                FileName = Path.GetFileName(fileName);
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

        public int GetContentLength()
        {
            return Content.Length;
        }

        public int GetContentLengthWithoutSpaces()
        {
            return (Content.Replace(" ", "")).Length;
        }
    }
}
