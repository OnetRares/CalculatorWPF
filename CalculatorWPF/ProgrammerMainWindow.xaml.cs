using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CalculatorWPF
{
    public partial class ProgrammerMainWindow : Window
    {
        private int currentBaseNumeration { get; set; }
        public ProgrammerMainWindow()
        {
            InitializeComponent();
            currentBaseNumeration = CalculatorWPF.Properties.Settings.Default.BaseNumeration;
            UpdateButtons();
            DataContext = new ProgrammerCalculatorViewModel();
            this.Icon = new BitmapImage(new Uri("../../../calculator.ico", UriKind.Relative));



        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Properties.Settings.Default.LastMode = "Programmer";
            Properties.Settings.Default.Save();
        }

        public void UpdateButtons()
        {
            string comparer = "";
            switch (currentBaseNumeration)
            {
                case 10:
                    comparer = "ABCDEF";
                    break;
                case 2:
                    comparer = "23456789ABCDEF";
                    break;
                case 8:
                    comparer = "89ABCDEF";
                    break;
                case 16:
                    comparer = "";
                    break;
            }
            foreach (var child in ProgrammerMain.Children)
            {
                Button button = child as Button;
                if (comparer.Contains(button.Content.ToString()))
                {
                    button.IsEnabled = false;
                }
                else
                {
                    button.IsEnabled = true;
                }
            }
        }

        private void ChangeBaseCommand(object sender,RoutedEventArgs e)
        {
            switch ((sender as Button).Content)
            {
                case "Binary":
                    currentBaseNumeration = 2;
                    CalculatorWPF.Properties.Settings.Default.BaseNumeration = 2;
                    break;
                case "Octal":
                    currentBaseNumeration = 8;
                    CalculatorWPF.Properties.Settings.Default.BaseNumeration = 8;
                    break;
                case "Decimal":
                    currentBaseNumeration = 10;
                    CalculatorWPF.Properties.Settings.Default.BaseNumeration = 10;
                    break;
                case "Hexadecimal":
                    currentBaseNumeration = 16;
                    CalculatorWPF.Properties.Settings.Default.BaseNumeration = 16;
                    break;
            }
            CalculatorWPF.Properties.Settings.Default.Save();
            UpdateButtons();
        }


       
    private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (ProgrammerCalculatorViewModel)DataContext;

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {

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

            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = (e.Key - Key.D0).ToString();
                viewModel.DigitCommand.Execute(digit);
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                string digit = (e.Key - Key.NumPad0).ToString();
                viewModel.DigitCommand.Execute(digit);
            }
            else if (viewModel.SelectedBase == NumberBase.Hexadecimal)
            {
                if (e.Key == Key.A)
                    viewModel.DigitCommand.Execute("A");
                else if (e.Key == Key.B)
                    viewModel.DigitCommand.Execute("B");
                else if (e.Key == Key.C)
                    viewModel.DigitCommand.Execute("C");
                else if (e.Key == Key.D)
                    viewModel.DigitCommand.Execute("D");
                else if (e.Key == Key.E)
                    viewModel.DigitCommand.Execute("E");
                else if (e.Key == Key.F)
                    viewModel.DigitCommand.Execute("F");


                if (e.Key == Key.Add || (e.Key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.None))
                    viewModel.OperatorCommand.Execute("+");
                else if (e.Key == Key.Subtract || (e.Key == Key.OemMinus && Keyboard.Modifiers == ModifierKeys.None))
                    viewModel.OperatorCommand.Execute("-");
                else if (e.Key == Key.Multiply)
                    viewModel.OperatorCommand.Execute("×");
                else if (e.Key == Key.Divide)
                    viewModel.OperatorCommand.Execute("÷");
                else if (e.Key == Key.Return || e.Key == Key.Enter)
                    viewModel.EqualCommand.Execute(null);
                else if (e.Key == Key.Back)
                    viewModel.BackspaceCommand.Execute(null);
                else if (e.Key == Key.Escape)
                    viewModel.ClearCommand.Execute(null);
            }
            else
            {
                if (e.Key == Key.Add || (e.Key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.None))
                    viewModel.OperatorCommand.Execute("+");
                else if (e.Key == Key.Subtract || (e.Key == Key.OemMinus && Keyboard.Modifiers == ModifierKeys.None))
                    viewModel.OperatorCommand.Execute("-");
                else if (e.Key == Key.Multiply)
                    viewModel.OperatorCommand.Execute("×");
                else if (e.Key == Key.Divide)
                    viewModel.OperatorCommand.Execute("÷");
                else if (e.Key == Key.Return || e.Key == Key.Enter)
                    viewModel.EqualCommand.Execute(null);
                else if (e.Key == Key.Back)
                    viewModel.BackspaceCommand.Execute(null);
                else if (e.Key == Key.Escape)
                    viewModel.ClearCommand.Execute(null);
            }
        }
        private void OpenExpressionInputWindow(object sender, RoutedEventArgs e)
        {
            ExpressionInputWindow window = new ExpressionInputWindow();
            window.ShowDialog();
        }

        public void ChangeThemeButton_Click(object sender, RoutedEventArgs e)
        {
            string themeName = (sender as MenuItem).Header.ToString();

            var theme = new ResourceDictionary { Source = new Uri($"Themes/{themeName}.xaml", UriKind.Relative) };
            (DataContext as ProgrammerCalculatorViewModel).currentTheme = themeName;
            this.Resources.MergedDictionaries.Clear();
            this.Resources.MergedDictionaries.Add(theme);

            this.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/Style.xaml", UriKind.Relative) });

            this.InvalidateVisual();
        }




    }
}
