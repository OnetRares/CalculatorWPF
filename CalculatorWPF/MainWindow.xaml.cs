using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CalculatorWPF
{
    public partial class MainWindow : Window
    {
       
        private string customClipboard = "";


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CalculatorViewModel();
            this.Icon = new BitmapImage(new Uri("../../../calculator.ico", UriKind.Relative));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.LastMode = "Standard";
            Properties.Settings.Default.Save();
        }
        [STAThread]
        public static void Main()
        {
            var app = new Application();

            string lastMode = Properties.Settings.Default.LastMode; 

            if (lastMode == "Programmer")
            {
                var programmerWindow = new ProgrammerMainWindow();
                app.Run(programmerWindow);
            }
            else
            {
                var mainWindow = new MainWindow();
                app.Run(mainWindow); 
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                var viewModel = (CalculatorViewModel)DataContext;
                if (e.Key == Key.X)
                {
                    Clipboard.SetText(viewModel.Display);
                    viewModel.Display = "";
                    return;
                }
                else if (e.Key == Key.C)
                {

                    Clipboard.SetText(viewModel.Display);
                    return;
                }
                else if (e.Key == Key.V)
                {
                    if (Clipboard.ContainsText())
                    {
                        string clipboardText = Clipboard.GetText();
                        if (double.TryParse(clipboardText, out double clipboardValue))
                        {
                            viewModel.Display = clipboardValue.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Clipboard-ul nu conține un număr valid.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Clipboard-ul nu conține text.");
                    }
                    return;
                }
            }

            var vm = (CalculatorViewModel)DataContext;

            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = (e.Key - Key.D0).ToString();
                vm.DigitCommand.Execute(digit);
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string digit = (e.Key - Key.NumPad0).ToString();
                vm.DigitCommand.Execute(digit);
            }
            else
            {
                if (e.Key == Key.Add || (e.Key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.None))
                    vm.OperatorCommand.Execute("+");
                else if (e.Key == Key.Subtract || (e.Key == Key.OemMinus && Keyboard.Modifiers == ModifierKeys.None))
                    vm.OperatorCommand.Execute("-");
                else if (e.Key == Key.Multiply)
                    vm.OperatorCommand.Execute("×");
                else if (e.Key == Key.Divide)
                    vm.OperatorCommand.Execute("÷");
                else if (e.Key == Key.Return || e.Key == Key.Enter)
                    vm.EqualCommand.Execute(null);
                else if (e.Key == Key.Back)
                    vm.BackspaceCommand.Execute(null);
                else if (e.Key == Key.Escape)
                    vm.ClearCommand.Execute(null);
            }
        }

        public void ChangeThemeButton_Click(object sender, RoutedEventArgs e)
        {
            string themeName = (sender as MenuItem).Header.ToString();

            var theme = new ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };
            (DataContext as CalculatorViewModel).currentTheme = themeName;
            this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(theme);

            this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/Style.xaml", UriKind.Relative) });

            this.InvalidateVisual();
        }


        private void OpenExpressionInputWindow(object sender, RoutedEventArgs e)
        {
            var window = new ExpressionInputWindow();
            window.ShowDialog();
        }
    }
}
