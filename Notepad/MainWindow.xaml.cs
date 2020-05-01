using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
    //TODO:Прописать горячие клавиши для элементов меню

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool searchWindowShowed { get; set; } // Индикатор открытия окна поиска и замены
        public Options options { get; private set; } = new Options(); // Класс для работы с настройками
        public TextEditor textEditor { get; set; } // Класс для работы с текстом
        
        public MainWindow()
        {
            InitializeComponent();
            textEditor = new TextEditor(); // Создание нового документа
            SetBindingOptions();
            SetBindingTextEditor();
            tbNotepad.Focus();
        }

        public MainWindow(string fileName)
        {
            InitializeComponent();
            textEditor = new TextEditor(fileName); // Загрузка выбранного документа
            SetBindingOptions();
            SetBindingTextEditor();
            tbNotepad.Focus();
            textEditor.ContentChanged = false;
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
            // Text в tbNotepad
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
            if (textEditor.ContentChanged)
            {
                confirmationWindow confirm = new confirmationWindow(textEditor.FileName);
                confirm.Owner = this;
                confirm.ShowDialog();
                if (confirm.ConfirmationResult == confirmationResult.YES)
                {
                    mmFileSave_Click(sender, e);
                    mmFileCreate_Click(sender, e);
                }
                if (confirm.ConfirmationResult == confirmationResult.NO)
                    textEditor.CreateNew();
            }
            else
                textEditor.CreateNew();

        }

        private void mmFileOpen_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.ContentChanged)
            {
                confirmationWindow confirm = new confirmationWindow(textEditor.FileName);
                confirm.Owner = this;
                confirm.ShowDialog();
                if (confirm.ConfirmationResult == confirmationResult.YES)
                {
                    mmFileSave_Click(sender, e);
                    mmFileOpen_Click(sender, e);
                }
                if (confirm.ConfirmationResult == confirmationResult.NO)
                    FileOpen();
            }
            else
                FileOpen();
        }

        private void FileOpen()
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
            sbiTextLinesCountLabel.Visibility = Visibility.Collapsed;
            sbiTextLinesCount.Visibility = Visibility.Collapsed;
        }

        private void mmFormatWrap_Unchecked(object sender, RoutedEventArgs e)
        {
            mmEditGoTo.IsEnabled = true;
            sbiTextLinesCountLabel.Visibility = Visibility.Visible;
            sbiTextLinesCount.Visibility = Visibility.Visible;
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

        private void tbNotepad_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Сообщаем об изменении текста в класс textEditor
            textEditor.ContentChanged = true;
            // Возможность поиска и замены доступна только если есть, в чем искать
            if (tbNotepad.Text.Length > 0)
                mmEditFind.IsEnabled = true;  
            else
                mmEditFind.IsEnabled = false;
            // Обновление результатов поиска при изменении текста
            if (textEditor.SearchResults != null)
            {
                if (textEditor.SearchResults.Count > 0)
                    textEditor.SearchText(textEditor.SearchQuery, textEditor.SearchSensetive);
            }
        }

        private void mmEditGoTo_Click(object sender, RoutedEventArgs e)
        {
            GoToWindow go = new GoToWindow(textEditor.ContentLinesCount);
            go.Owner = this;
            go.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            if (go.ShowDialog().Value)
            {
                tbNotepad.SelectionStart = tbNotepad.GetCharacterIndexFromLineIndex(go.GoToLine - 1);
                tbNotepad.SelectionLength = tbNotepad.GetLineLength(go.GoToLine - 1);
                tbNotepad.CaretIndex = tbNotepad.SelectionStart;
                tbNotepad.ScrollToLine(go.GoToLine - 1);
                tbNotepad.Focus();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (textEditor.ContentChanged)
            {
                confirmationWindow confirm = new confirmationWindow(textEditor.FileName);
                confirm.Owner = this;
                confirm.ShowDialog();
                if (confirm.ConfirmationResult == confirmationResult.YES)
                {
                    mmFileSave_Click(sender, new RoutedEventArgs());
                    e.Cancel = false;
                }
                if (confirm.ConfirmationResult == confirmationResult.NO)
                    e.Cancel = false;
                if (confirm.ConfirmationResult == confirmationResult.CANCEL)
                    e.Cancel = true;
            }
        }

        private void tbNotepad_SelectionChanged(object sender, RoutedEventArgs e)
        {
            textEditor.ContentSelectionStart = tbNotepad.SelectionStart;
            if (textEditor.SearchResults != null)
            {
                textEditor.ActualFindNext(false);
            }
            int lineIndex = tbNotepad.GetLineIndexFromCharacterIndex(tbNotepad.SelectionStart);
            int colIndex = tbNotepad.SelectionStart - tbNotepad.GetCharacterIndexFromLineIndex(lineIndex);
            sbiCursorPosition.Content = $"Стр {lineIndex + 1}, стлб {colIndex + 1}";
            sbiTextSelectLenGet.Content = $"({tbNotepad.SelectionLength})";
            sbiTextSelectLenWithoutSpacesGet.Content = $"({(((tbNotepad.SelectedText.Replace('\n', ' ')).Replace('\r', ' ')).Replace(" ", "")).Length})";
        }

        private void mmHelpAboutProgram_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }

        private void mmEditFind_Click(object sender, RoutedEventArgs e)
        {
            if (tbNotepad.Text.Length > 0 && !searchWindowShowed)
            {
                searchWindowShowed = true;
                SearchWindow search = textEditor.SearchQuery == null ? new SearchWindow() : new SearchWindow(textEditor.SearchQuery);
                search.Owner = this;
                search.Show();
            }
        }

        // Выделение результата поиска в textbox'e
        public void SelectFindResult(bool findInCycle, bool directionNext = true)
        {
            textEditor.SearchInCycle = findInCycle;
            if (directionNext) textEditor.FindResultMoveNext(findInCycle);
            else textEditor.FindResultMoveBack(findInCycle);
            try
            {
                tbNotepad.Focus();
                tbNotepad.Select(textEditor.CurrentSearchResultPosition, textEditor.SearchQuery.Length);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show($"Не удается найти \"{textEditor.SearchQuery}\"", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($"Не удается найти \"{textEditor.SearchQuery}\"", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
            }
        }

        private void mmEditFindNext_Click(object sender, RoutedEventArgs e)
        {
            SelectFindResult(textEditor.SearchInCycle);
        }

        private void mmEditFindBack_Click(object sender, RoutedEventArgs e)
        {
            SelectFindResult(textEditor.SearchInCycle, false);
        }

        private double scaleXY = 1;
        private double ScaleXY 
        { 
            get
            {
                return scaleXY;
            }
            set
            {
                scaleXY = value;
            }
        }

        public string TextBoxScale
        {
            get
            {
                return (ScaleXY * 100.0).ToString("N0") + "%";
            }
        }

        private void mmViewScalePlus_Click(object sender, RoutedEventArgs e)
        {
            ScalePlus();
        }

        private void mmViewScaleMinus_Click(object sender, RoutedEventArgs e)
        {
            ScaleMinus();
        }

        private void mmViewScaleDefault_Click(object sender, RoutedEventArgs e)
        {
            scaleXY = 1;
            tbNotepad.LayoutTransform = new ScaleTransform(ScaleXY, ScaleXY);
            sbiTextScale.Content = TextBoxScale;
        }

        private void ScalePlus()
        {
            ScaleXY += 0.1;
            tbNotepad.LayoutTransform = new ScaleTransform(ScaleXY, ScaleXY);
            sbiTextScale.Content = TextBoxScale;
        }

        private void ScaleMinus()
        {
            ScaleXY -= 0.1;
            tbNotepad.LayoutTransform = new ScaleTransform(ScaleXY, ScaleXY);
            sbiTextScale.Content = TextBoxScale;
        }

        private void tbNotepad_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (e.Delta > 0) ScalePlus();
                if (e.Delta < 0) ScaleMinus();
            }
        }
    }
}
