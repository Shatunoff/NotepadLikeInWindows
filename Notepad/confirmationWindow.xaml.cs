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
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Forms;

namespace Notepad
{
    public enum confirmationResult { YES, NO, CANCEL}
    /// <summary>
    /// Логика взаимодействия для confirmationWindow.xaml
    /// </summary>
    public partial class confirmationWindow : Window
    {
        public confirmationResult ConfirmationResult { get; set; }

        public confirmationWindow(string fileName)
        {
            InitializeComponent();
            this.Title = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
            tbQuestion.Text = $"Вы хотите сохранить изменения в файле \"{fileName}\"?";
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationResult = confirmationResult.YES;
            DialogResult = true;
        }

        private void btnNotSave_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationResult = confirmationResult.NO;
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationResult = confirmationResult.CANCEL;
            DialogResult = null;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            
        }
    }
}
