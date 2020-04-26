using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    //TODO:Масштаб текста (если возможно)
    //TODO:Окно "О программе"
    //TODO:Отображение координат курсора в строке состояния (строка, столбец)
    //TODO:Индикатор изменения текста для вывода подтверждения о закрытии или создании нового документов до сохранения старого.
    //TODO:Чтение аргументов командной строки для запуска текстовых файлов через мой блокнот

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Options options = new Options(); // Класс для работы с настройками
        public TextEditor textEditor = new TextEditor(); // Класс для работы с текстом

        public MainWindow()
        {
            InitializeComponent();
            SetBindingOptions();
            SetBindingTextEditor();
        }

        // Связывание сохраненных настроек с настройками приложения
        private void SetBindingOptions()
        {
            // Ширина окна
            Binding bindOptionsWidth = new Binding();
            bindOptionsWidth.Source = options; // элемент-источник
            bindOptionsWidth.Path = new PropertyPath("WindowWidth"); // свойство элемента-источника
            bindOptionsWidth.Mode = BindingMode.TwoWay;
            bindOptionsWidth.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.SetBinding(Window.WidthProperty, bindOptionsWidth); // установка привязки для элемента-приемника
            // Высота окна
            Binding bindOptionsHeight = new Binding();
            bindOptionsHeight.Source = options; // элемент-источник
            bindOptionsHeight.Path = new PropertyPath("WindowHeight"); // свойство элемента-источника
            bindOptionsHeight.Mode = BindingMode.TwoWay;
            bindOptionsHeight.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.SetBinding(Window.HeightProperty, bindOptionsHeight); // установка привязки для элемента-приемника
            // Шрифт-Имя
            Binding bindOptionsFontFamily = new Binding();
            bindOptionsFontFamily.Source = options; // элемент-источник
            bindOptionsFontFamily.Path = new PropertyPath("TextFontFamily"); // свойство элемента-источника
            bindOptionsFontFamily.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.FontFamilyProperty, bindOptionsFontFamily); // установка привязки для элемента-приемника
            // Шрифт-Размер
            Binding bindOptionsFontSize = new Binding();
            bindOptionsFontSize.Source = options; // элемент-источник
            bindOptionsFontSize.Path = new PropertyPath("TextFontSize"); // свойство элемента-источника
            bindOptionsFontSize.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.FontSizeProperty, bindOptionsFontSize); // установка привязки для элемента-приемника
            // Шрифт-Толщина
            Binding bindOpFontWeight = new Binding();
            bindOpFontWeight.Source = options; // элемент-источник
            bindOpFontWeight.Path = new PropertyPath("TextFontWeight"); // свойство элемента-источника
            bindOpFontWeight.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.FontWeightProperty, bindOpFontWeight); // установка привязки для элемента-приемника
            // Шрифт-Стиль
            Binding bindOptionsFontStyle = new Binding();
            bindOptionsFontStyle.Source = options; // элемент-источник
            bindOptionsFontStyle.Path = new PropertyPath("TextFontStyle"); // свойство элемента-источника
            bindOptionsFontStyle.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.FontStyleProperty, bindOptionsFontStyle); // установка привязки для элемента-приемника
            // Строка состояния - Чекбокс в меню
            Binding bindOptionsStatusStripMenuCheck = new Binding();
            bindOptionsStatusStripMenuCheck.Source = options; // элемент-источник
            bindOptionsStatusStripMenuCheck.Path = new PropertyPath("StatusStripVisiblityIsChecked"); // свойство элемента-источника
            bindOptionsStatusStripMenuCheck.Mode = BindingMode.TwoWay;
            bindOptionsStatusStripMenuCheck.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            mmViewStatusStrip.SetBinding(MenuItem.IsCheckedProperty, bindOptionsStatusStripMenuCheck); // установка привязки для элемента-приемника
            // Строка состояния - Видимость
            Binding bindOptionsStatusStripVisiblity = new Binding();
            bindOptionsStatusStripVisiblity.Source = options; // элемент-источник
            bindOptionsStatusStripVisiblity.Path = new PropertyPath("StatusStripVisiblity"); // свойство элемента-источника
            bindOptionsStatusStripVisiblity.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            statusStrip.SetBinding(StatusBar.VisibilityProperty, bindOptionsStatusStripVisiblity); // установка привязки для элемента-приемника
            // Перенос по словам - Чекбокс в меню
            Binding bindOptionsTextWrappingMenuCheck = new Binding();
            bindOptionsTextWrappingMenuCheck.Source = options; // элемент-источник
            bindOptionsTextWrappingMenuCheck.Path = new PropertyPath("TextWrappingIsChecked"); // свойство элемента-источника
            bindOptionsTextWrappingMenuCheck.Mode = BindingMode.TwoWay;
            bindOptionsTextWrappingMenuCheck.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            mmFormatWrap.SetBinding(MenuItem.IsCheckedProperty, bindOptionsTextWrappingMenuCheck); // установка привязки для элемента-приемника
            // Перенос по словам - активация-деактивация
            Binding bindOptionsTextWrappingWrappingy = new Binding();
            bindOptionsTextWrappingWrappingy.Source = options; // элемент-источник
            bindOptionsTextWrappingWrappingy.Path = new PropertyPath("TextWrappingWrapping"); // свойство элемента-источника
            bindOptionsTextWrappingWrappingy.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.TextWrappingProperty, bindOptionsTextWrappingWrappingy); // установка привязки для элемента-приемника
            // Проверка орфографии - Чекбокс в меню
            Binding bindOptionsSpellCheckMenuCheck = new Binding();
            bindOptionsSpellCheckMenuCheck.Source = options; // элемент-источник
            bindOptionsSpellCheckMenuCheck.Path = new PropertyPath("SpellCheckingIsChecked"); // свойство элемента-источника
            bindOptionsSpellCheckMenuCheck.Mode = BindingMode.TwoWay;
            bindOptionsSpellCheckMenuCheck.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            mmFormatSpellCheck.SetBinding(MenuItem.IsCheckedProperty, bindOptionsSpellCheckMenuCheck); // установка привязки для элемента-приемника
            // Проверка орфографии - Словарь
            Binding bindOptionsSpellCheck = new Binding();
            bindOptionsSpellCheck.Source = options; // элемент-источник
            bindOptionsSpellCheck.Path = new PropertyPath("SpellCheckingDictonary"); // свойство элемента-источника
            bindOptionsSpellCheck.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.LanguageProperty, bindOptionsSpellCheck); // установка привязки для элемента-приемника
        }

       // Связывание элементов приложения с классом для работы с текстовыми файлами
        private void SetBindingTextEditor()
        {
            // Title Главного окна
            Binding bindTextTitle = new Binding();
            bindTextTitle.Source = textEditor; // элемент-источник
            bindTextTitle.Path = new PropertyPath("WindowTitle"); // свойство элемента-источника
            bindTextTitle.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            this.SetBinding(Window.TitleProperty, bindTextTitle); // установка привязки для элемента-приемника
            //Text в tbNotepad
            Binding bindTextContent = new Binding();
            bindTextContent.Source = textEditor; // элемент-источник
            bindTextContent.Path = new PropertyPath("Content"); // свойство элемента-источника
            bindTextContent.Mode = BindingMode.TwoWay;
            bindTextContent.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            tbNotepad.SetBinding(TextBox.TextProperty, bindTextContent);
            // Количество строк в документе
            Binding bindTextLinesCount = new Binding();
            bindTextLinesCount.Source = textEditor; // элемент-источник
            bindTextLinesCount.Path = new PropertyPath("ContentLinesCount"); // свойство элемента-источника
            bindTextLinesCount.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sbiTextLinesCount.SetBinding(StatusBarItem.ContentProperty, bindTextLinesCount); // установка привязки для элемента-приемника
            // Количество символов
            Binding bindTextLength = new Binding();
            bindTextLength.Source = textEditor; // элемент-источник
            bindTextLength.Path = new PropertyPath("ContentLength"); // свойство элемента-источника
            bindTextLength.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sbiTextLenGet.SetBinding(StatusBarItem.ContentProperty, bindTextLength); // установка привязки для элемента-приемника
            // Количество символов без пробелов
            Binding bindTextLengthWithoutSpaces = new Binding();
            bindTextLengthWithoutSpaces.Source = textEditor; // элемент-источник
            bindTextLengthWithoutSpaces.Path = new PropertyPath("ContentLengthWithoutSpaces"); // свойство элемента-источника
            bindTextLengthWithoutSpaces.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sbiTextLenWithoutSpacesGet.SetBinding(StatusBarItem.ContentProperty, bindTextLengthWithoutSpaces); // установка привязки для элемента-приемника
            // Название кодировки
            Binding bindTextEncodingName = new Binding();
            bindTextEncodingName.Source = textEditor; // элемент-источник
            bindTextEncodingName.Path = new PropertyPath("ContentEncodingName"); // свойство элемента-источника
            bindTextEncodingName.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            sbiEncodingName.SetBinding(StatusBarItem.ContentProperty, bindTextEncodingName); // установка привязки для элемента-приемника
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
            mmEditGoTo.IsEnabled = false;
        }

        private void mmFormatWrap_Unchecked(object sender, RoutedEventArgs e)
        {
            mmEditGoTo.IsEnabled = true;
        }

        private void mmFormatFont_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FontDialog font = new System.Windows.Forms.FontDialog();
            font.ShowEffects = false;
            font.Font = options.TextFont;
            if (font.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                options.TextFont = font.Font;
        }

        private void mmFileNewWindow_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Assembly.GetExecutingAssembly().Location);
        }

        private void mmFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
