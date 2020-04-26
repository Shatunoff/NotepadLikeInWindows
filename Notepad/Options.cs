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
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Notepad
{
    public class Options : INotifyPropertyChanged
    {
        #region Главное окно
        // Ширина
        public int WindowWidth
        {
            get
            {
                return Properties.Settings.Default.WindowWidth;
            }
            set
            {
                Properties.Settings.Default.WindowWidth = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("WindowWidth");
            }
        }

        // Высота
        public int WindowHeight
        {
            get
            {
                return Properties.Settings.Default.WindowHeight;
            }
            set
            {
                Properties.Settings.Default.WindowHeight = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("WindowHeight");
            }
        }
        #endregion

        #region Шрифт
        // Шрифт
        public System.Drawing.Font TextFont
        {
            get
            {
                return Properties.Settings.Default.TextFont;
            }
            set
            {
                Properties.Settings.Default.TextFont = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("TextFont");
                NotifyPropertyChanged("TextFontFamily");
                NotifyPropertyChanged("TextFontSize");
                NotifyPropertyChanged("TextFontWeight");
                NotifyPropertyChanged("TextFontStyle");
            }
        }

        // Название шрифта
        public string TextFontFamily
        {
            get
            {
                return TextFont.Name;
            }
        }

        // Размер
        public float TextFontSize
        {
            get
            {
                return TextFont.Size * 96 / 72;
            }
        }

        // Толщина
        public FontWeight TextFontWeight
        {
            get
            {
                return TextFont.Bold ? FontWeights.Bold : FontWeights.Regular;
            }
        }

        // Стиль
        public System.Windows.FontStyle TextFontStyle
        {
            get
            {
                return TextFont.Italic ? FontStyles.Italic : FontStyles.Normal;
            }
        }
        #endregion

        #region Чекбоксы на форме
        // Чекбокс Вид-Строка состояния
        public bool StatusStripVisiblityIsChecked
        {
            get
            {
                return Properties.Settings.Default.StatusStripVisiblity;
            }
            set
            {
                Properties.Settings.Default.StatusStripVisiblity = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("StatusStripVisiblityIsChecked");
                NotifyPropertyChanged("StatusStripVisiblity");
            }
        }

        // Отображение статусбара
        public Visibility StatusStripVisiblity
        {
            get
            {
                if (Properties.Settings.Default.StatusStripVisiblity)
                    return Visibility.Visible;
                else return Visibility.Collapsed;
            }
        }

        // Чекбокс Формат-Перенос по словам
        public bool TextWrappingIsChecked
        {
            get
            {
                return Properties.Settings.Default.TextWrapping;
            }
            set
            {
                Properties.Settings.Default.TextWrapping = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("TextWrappingIsChecked");
                NotifyPropertyChanged("TextWrappingWrapping");
            }
        }

        // Перенос по словам
        public TextWrapping TextWrappingWrapping
        {
            get
            {
                if (Properties.Settings.Default.TextWrapping)
                    return TextWrapping.Wrap;
                else return TextWrapping.NoWrap;
            }
        }

        // Проверка орфографии
        public bool SpellCheckingIsChecked
        {
            get
            {
                return Properties.Settings.Default.SpellChecking;
            }
            set
            {
                Properties.Settings.Default.SpellChecking = value;
                Properties.Settings.Default.Save();
                NotifyPropertyChanged("SpellCheckingIsChecked");
                NotifyPropertyChanged("SpellCheckingDictonary");
            }
        }

        // Имя локализации словаря для проверки орфографии
        public XmlLanguage SpellCheckingDictonary
        {
            get
            {
                return XmlLanguage.GetLanguage(Properties.Settings.Default.SpellCheckDictonary);
            }
        }
        #endregion

        #region Уведомление об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;

        // Уведомление об изменении свойства
        public void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion
    }
}
