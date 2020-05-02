using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Reflection;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Printing;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Notepad
{
    public class TextEditor : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _fileName;
        private string _filePath;
        private string _content;
        private bool _contentChanged;
        private Encoding _contentEncoding;

        /// <summary>
        /// Индикатор изменения текста. Содержит False, если текст не подвергался изменениям с момента последнего сохранения и наоборот.
        /// </summary>
        public bool ContentChanged 
        { 
            get
            {
                return _contentChanged;
            }
            set
            {
                _contentChanged = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }

        /// <summary>
        /// Наименование открытого/сохраненного текстового файла.
        /// </summary>
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

        /// <summary>
        /// Title для главного окна, содержащее наименование файла и название программы. Также содержит индикатор изменения текста.
        /// </summary>
        public string WindowTitle
        {
            get
            {
                string result = _fileName + " - " + Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
                if (ContentChanged)
                    return "*" + result;
                else return result;
            }
        }

        /// <summary>
        /// Путь к файлу, хранящему текст. Путь содержит null, если текст не был загружен или сохранен.
        /// </summary>
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
                NotifyPropertyChanged("WindowTitle");
            }
        }

        /// <summary>
        /// Количество строк в тексте.
        /// </summary>
        public int ContentLinesCount
        {
            get
            {
                return _content.Split('\n').Length;
            }
        }

        /// <summary>
        /// Текст.
        /// </summary>
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
                NotifyPropertyChanged("ContentLinesCount");
            }
        }

        /// <summary>
        /// Название кодировки текста (техническое).
        /// </summary>
        public string ContentEncodingHeaderName
        {
            get
            {
                return _contentEncoding.HeaderName;
            }
        }

        /// <summary>
        /// Название кодировки текста (понятное пользователю).
        /// </summary>
        public string ContentEncodingName
        {
            get
            {
                return _contentEncoding.EncodingName;
            }
        }

        /// <summary>
        /// Количество символов в тексте.
        /// </summary>
        public int ContentLength
        {
            get
            {
                return ((_content.Replace("\n", "")).Replace("\r", "")).Length;
            }
        }

        /// <summary>
        /// Количество символов в тексте без учета пробелов.
        /// </summary>
        public int ContentLengthWithoutSpaces
        {
            get
            {
                return (((_content.Replace('\n', ' ')).Replace('\r', ' ')).Replace(" ", "")).Length;
            }
        }

        /// <summary>
        /// Кодировка текста
        /// </summary>
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

        #region Свойства для поиска и замены текста
        /// <summary>
        /// Текущее положение курсора в тексте
        /// </summary>
        public int ContentSelectionStart { get; set; }
        //public int ContentSearchSelectionStart { get; set; }
        /// <summary>
        /// Результаты поиска методом FindText.
        /// </summary>
        public MatchCollection SearchResults { get; private set; }
        /// <summary>
        /// Последний поисковый запрос.
        /// </summary>
        public string SearchQuery { get; private set; }
        /// <summary>
        /// Флаг показывает, учитывался ли регистр символов во время последнего запроса.
        /// </summary>
        public bool SearchSensetive { get; private set; }
        /// <summary>
        /// Флаг показывает, было ли включено зацикливание во время последнего поиска
        /// </summary>
        public bool SearchInCycle { get; set; }
        /// <summary>
        /// Индекс текущего результата поиска
        /// </summary>
        private int currentPosition;
        /// <summary>
        /// Позиция текущего результата поиска
        /// </summary>
        public int CurrentSearchResultPosition
        {
            get
            {
                return SearchResults[currentPosition].Index;
            }
        }
        #endregion

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
            FilePath = null;
            Content = String.Empty;
            ContentEncoding = Encoding.UTF8;
            ContentChanged = false;
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
                ContentChanged = false;
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
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
                ContentChanged = false;
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            finally
            {
                sw?.Close();
                fs?.Close();
            }
        }

        public void FindResultMoveNext(bool findInCycle)
        {
            currentPosition++;
            if (findInCycle)
            {
                if (currentPosition >= SearchResults.Count)
                    currentPosition = 0;
            }
        }

        public void FindResultMoveBack(bool findInCycle)
        {
            currentPosition--;
            if (findInCycle)
            {
                if (currentPosition < 0)
                    currentPosition = SearchResults.Count - 1;
            }
        }        

        /// <summary>
        /// Производит поиск указанной строки в тексте. Результат присваивается свойству SearchResults класса TextEditor.
        /// </summary>
        /// <param name="searchQuery">Поисковый запрос</param>
        /// <param name="sensetive">Учет регистра букв при поиске</param>
        public void SearchText(string searchQuery, bool sensetive)
        {
            SearchQuery = searchQuery;
            SearchSensetive = sensetive;
            
            if (sensetive)
            {
                Regex regex = new Regex(SearchQuery);
                SearchResults = regex.Matches(Content);
            }
            else
            {
                Regex regex = new Regex(SearchQuery, RegexOptions.IgnoreCase);
                SearchResults = regex.Matches(Content);
            }

            ActualFindNext();
        }

        #region Мне стыдно за этот код :-(
        // Актуализация текущей позиции поиска в зависимости от положения курсора в TextBox
        public void ActualFindNext(bool state = true)
        {
            if (state)
            {
                if (SearchResults != null)
                {
                    if (SearchResults.Count > 0)
                    {
                        for (int i = 0; i < SearchResults.Count; i++)
                        {
                            if (SearchResults[i].Index <= ContentSelectionStart - 1)
                            {
                                currentPosition = i;
                            }
                        }
                    }
                }
            }
            else
            {
                if (SearchResults != null)
                {
                    if (SearchResults.Count > 0)
                    {
                        for (int i = 0; i < SearchResults.Count; i++)
                        {
                            if (SearchResults[i].Index <= ContentSelectionStart + SearchQuery.Length - 1)
                            {
                                currentPosition = i;
                            }
                        }
                    }
                }
            }
        }
        #endregion

    }

    public class SearchQueryNotFoundException : Exception
    {
        public SearchQueryNotFoundException(string message) : base(message)
        {

        }
    }
}
