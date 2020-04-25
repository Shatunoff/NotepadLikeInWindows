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
        TextEditor textEditor = new TextEditor();

        public MainWindow()
        {
            InitializeComponent();
            tbNotepad.DataContext = textEditor;
        }

        private void mmFileOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Открыть текстовый файл";
            open.Filter = "Текстовые файлы (*.txt) |*.txt| Все файлы (*.*)|*.*";
            if (open.ShowDialog() == DialogResult.Value)
                textEditor.LoadFile(open.FileName);
        }

        private void mmFileSaveAs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
