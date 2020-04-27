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
using System.Windows.Shapes;

namespace Notepad
{
    /// <summary>
    /// Логика взаимодействия для GoToWindow.xaml
    /// </summary>
    public partial class GoToWindow : Window
    {
        private int linesCount;
        public int GoToLine { get; set; }

        public GoToWindow(int linesCount)
        {
            InitializeComponent();
            this.linesCount = linesCount;
            tbStringNumber.Focus();
        }

        private void btnGoTo_Click(object sender, RoutedEventArgs e)
        {
            if (tbStringNumber.Text.Length > 0)
            {
                GoToLine = int.Parse(tbStringNumber.Text);
                if (GoToLine <= linesCount && GoToLine > 0)
                    DialogResult = true;
                else
                    MessageBox.Show("Номер строки не соответствует общему число строк.", this.Title);
            }
            else
                MessageBox.Show("Укажите номер строки для перехода к ней.", this.Title);
        }

        private void tbStringNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}
