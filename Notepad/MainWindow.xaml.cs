using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notepad
{
    //TODO:Открытие нового экземпляра приложения
    //TODO:Печать и параметры печати
    //TODO:Поиск по документу (Найти, Найти далее, Найти ранее)
    //TODO:Замена внутри документа
    //TODO:Переход к указанной строке (при отсутствии переноса по словам)
    //TODO:Настройка шрифта
    //TODO:Масштаб текста (если возможно)
    //TODO:Окно "О программе"
    //TODO:Запоминание пользовательских установок (размер окна, шрифт, отображение строки состояния, перенос по словам) (отдельным статическим классом, который берет информацию из Property.Settings)
    //TODO:Отображение координат курсора в строке состояния (строка, столбец)
    //TODO:Индикатор изменения текста для вывода подтверждения о закрытии или создании нового документов до сохранения старого.
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextEditor textEditor = new TextEditor();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = textEditor;
            tbNotepad.DataContext = textEditor;
            statusStrip.DataContext = textEditor;
            tbNotepad.Language = XmlLanguage.GetLanguage("ru-RU");
            mmViewStatusStrip.IsChecked = statusStrip.Visibility == Visibility.Visible ? true : false;
        }

        private void mmFileCreate_Click(object sender, RoutedEventArgs e)
        {
            textEditor.CreateNew();
        }

        private void mmFileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog() 
            { 
                Title = "Открыть текстовый файл", 
                Filter = "Текстовые файлы (*.txt) |*.txt| Все файлы (*.*)|*.*" 
            };

            if (open.ShowDialog() == true)
                textEditor.LoadFile(open.FileName);

        }

        private void mmFileSave_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.FilePath != null)
                textEditor.SaveFile(textEditor.FilePath);
            else mmFileSaveAs_Click(sender, e);
        }

        private void mmFileSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog()
            {
                Title = "Сохранить текстовый файл",
                FileName = textEditor.FileName,
                Filter = "Текстовые файлы (*.txt) |*.txt| Все файлы (*.*)|*.*"
            };

            if(save.ShowDialog() == true)
                textEditor.SaveFile(save.FileName);
        }

        private void mmEditUndo_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.Undo();
        }

        private void mmEditCut_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.Cut();
        }

        private void mmEditCopy_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.Copy();
        }

        private void mmEditPaste_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.Paste();
        }

        private void mmEditRemove_Click(object sender, RoutedEventArgs e)
        {
            if (tbNotepad.SelectionLength < 1)
                tbNotepad.Select(tbNotepad.SelectionStart, 1);
            tbNotepad.SelectedText = String.Empty;
        }

        private void mmEditRedo_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.Redo();
        }

        private void mmEditSelectAll_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.SelectAll();
        }

        private void mmEditTimeAndData_Click(object sender, RoutedEventArgs e)
        {
            tbNotepad.SelectedText = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
            tbNotepad.SelectionLength = 0;
        }

        private void mmViewSpellCheck_Checked(object sender, RoutedEventArgs e)
        {
            tbNotepad.SpellCheck.IsEnabled = true;
        }

        private void mmViewSpellCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            tbNotepad.SpellCheck.IsEnabled = false;
        }

        private void mmFormatWrap_Checked(object sender, RoutedEventArgs e)
        {
            tbNotepad.TextWrapping = TextWrapping.Wrap;
            sbiTextLinesCountLabel.Visibility = Visibility.Collapsed;
            sbiTextLinesCount.Visibility = Visibility.Collapsed;
        }

        private void mmFormatWrap_Unchecked(object sender, RoutedEventArgs e)
        {
            tbNotepad.TextWrapping = TextWrapping.NoWrap;
            sbiTextLinesCountLabel.Visibility = Visibility.Visible;
            sbiTextLinesCount.Visibility = Visibility.Visible;
        }

        private void mmViewStatusStrip_Checked(object sender, RoutedEventArgs e)
        {
            statusStrip.Visibility = Visibility.Visible;
        }

        private void mmViewStatusStrip_Unchecked(object sender, RoutedEventArgs e)
        {
            statusStrip.Visibility = Visibility.Collapsed;
        }
    }
}
