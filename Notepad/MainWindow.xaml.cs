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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TextEditor textEditor = new TextEditor();

        public MainWindow()
        {
            InitializeComponent();
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
    }
}
