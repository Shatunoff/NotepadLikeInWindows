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
    /// Логика взаимодействия для SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private MainWindow main; // Ссылка на форму-родитель

        public SearchWindow()
        {
            InitializeComponent();
            tbSearchQuery.Focus();
        }

        public SearchWindow(string lastSearchQuery)
        {
            InitializeComponent();
            tbSearchQuery.Text = lastSearchQuery;
            tbSearchQuery.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = this.Owner as MainWindow;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            main.searchWindowShowed = false;
        }

        private void btnFindNext_Click(object sender, RoutedEventArgs e)
        {
            RefreshSearchResults();
            btnFindClick();
        }

        private void btnFindBack_Click(object sender, RoutedEventArgs e)
        {
            RefreshSearchResults();
            btnFindClick(false);
        }

        private void RefreshSearchResults()
        {
            if (tbSearchQuery.Text.Length > 0)
            {
                if (main.textEditor.SearchQuery != tbSearchQuery.Text || main.textEditor.SearchSensetive != checkRegister.IsChecked.Value)
                    main.textEditor.SearchText(tbSearchQuery.Text, checkRegister.IsChecked.Value);
            }
        }

        private void btnFindClick(bool directionNext = true)
        {
            if (tbSearchQuery.Text.Length > 0)
            {
                main.SelectFindResult(checkCycle.IsChecked.Value, directionNext);
                main.mmEditFindNext.IsEnabled = main.mmEditFindBack.IsEnabled = true;
            }
        }

        private void btnRaplceAll_Click(object sender, RoutedEventArgs e)
        {
            RefreshSearchResults();
            while (main.textEditor.SearchResults.Count > 0)
                btnReplace_Click(sender, e);
        }

        private void btnReplace_Click(object sender, RoutedEventArgs e)
        {
            if (main.tbNotepad.SelectionLength > 0)
            {
                if (checkRegister.IsChecked.Value)
                {
                    if (main.tbNotepad.SelectedText == tbSearchQuery.Text)
                    {
                        main.tbNotepad.SelectedText = tbReplaceQuery.Text;
                        main.tbNotepad.SelectionLength = tbReplaceQuery.Text.Length;
                        btnFindClick();
                    }
                }
                else
                {
                    if (main.tbNotepad.SelectedText.ToLower() == tbSearchQuery.Text.ToLower())
                    {
                        main.tbNotepad.SelectedText = tbReplaceQuery.Text;
                        main.tbNotepad.SelectionLength = tbReplaceQuery.Text.Length;
                        btnFindClick();
                    }
                }
            }
            else
            {
                RefreshSearchResults();
                btnFindClick();
            }
        }
    }
}
