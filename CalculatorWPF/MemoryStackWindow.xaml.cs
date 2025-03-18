using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorWPF
{
    public partial class MemoryStackWindow : Window
    {
        public ObservableCollection<double> MemoryStack { get; set; }

        public MemoryStackWindow(ObservableCollection<double> memoryStack, string Theme)
        {
         
            InitializeComponent();
            MemoryStack = memoryStack;
            ChangeTheme(Theme);
            DataContext = this; 
           
        }

        public void ChangeTheme(string themeName)
        {
            var theme = new ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };

            this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(theme);

            this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/Style.xaml", UriKind.Relative) });
        }

        private void RemoveFromMemory_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryListBox.SelectedItem is double selectedValue)
            {
                MemoryStack.Remove(selectedValue);
            }
        }

        private void AddToMemory_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryListBox.SelectedItem is double selectedValue)
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    if (double.TryParse(mainWindow.txtDisplay.Text, out double displayValue))
                    {
                        int index = MemoryStack.IndexOf(selectedValue);
                        if (index >= 0)
                        {
                            MemoryStack[index] = selectedValue + displayValue;
                        }
                    }
                }
                else if(Application.Current.MainWindow is ProgrammerMainWindow PmainWindow)
                {
                    if (double.TryParse(PmainWindow.txtDisplay.Text, out double displayValue))
                    {
                        int index = MemoryStack.IndexOf(selectedValue);
                        if (index >= 0)
                        {
                            MemoryStack[index] = selectedValue + displayValue;
                        }
                    }

                }
            }
        }

        private void SubtractFromMemory_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryListBox.SelectedItem is double selectedValue)
            {
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    if (double.TryParse(mainWindow.txtDisplay.Text, out double displayValue))
                    {
                        int index = MemoryStack.IndexOf(selectedValue);
                        if (index >= 0)
                        {
                            MemoryStack[index] = selectedValue - displayValue;
                        }
                    }
                }
                else if(Application.Current.MainWindow is ProgrammerMainWindow PmainWindow)
                {
                    if (double.TryParse(PmainWindow.txtDisplay.Text, out double displayValue))
                    {
                        int index = MemoryStack.IndexOf(selectedValue);
                        if (index >= 0)
                        {
                            MemoryStack[index] = selectedValue - displayValue;
                        }
                    }

                }
            }
        }

        private void UseMemoryValue_Click(object sender, RoutedEventArgs e)
        {
            if (MemoryListBox.SelectedItem is double selectedValue)
            {
                if(Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.txtDisplay.Text = selectedValue.ToString();
                }
                else if(Application.Current.MainWindow is ProgrammerMainWindow PmainWindow)
                {
                    PmainWindow.txtDisplay.Text = selectedValue.ToString();
                }
                Close();
            }
        }
    }
}
