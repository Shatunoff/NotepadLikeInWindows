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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notepad
{
    //TODO:Открытие нового экземпляра приложения
    //TODO:Печать и параметры печати
    //TODO:Отменить действие
    //TODO:Вырезать-Копировать-Вставить-Удалить
    //TODO:Поиск по документу (Найти, Найти далее, Найти ранее)
    //TODO:Замена внутри документа
    //TODO:Переход к указанной строке (при отсутствии переноса по словам)
    //TODO:Выделение всего текста
    //TODO:Добавление времени и даты в документ
    //TODO:Перенос текста по словам
    //TODO:Настройка шрифта
    //TODO:Минимальный размер главного окна
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
    }
}
