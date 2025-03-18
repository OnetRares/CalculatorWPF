using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Globalization;
using System.Windows;


namespace CalculatorWPF
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private bool isGrouped { get; set; }
        private string _display;
        private CalculatorEngine _engine;
        private bool _isNewInput;
        private string customClipboard = "";
        public string currentTheme { get; set; }

        public ObservableCollection<double> MemoryStack { get; set; }
        public ICommand DigitCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand FunctionCommand { get; }
        public ICommand MemoryStoreCommand { get; }
        public ICommand MemoryRecallCommand { get; }
        public ICommand MemoryClearCommand { get; }
        public ICommand MemoryStackCommand { get; }
        public ICommand MemoryAddCommand { get; }
        public ICommand MemorySubtractCommand { get; }

        public ICommand DigitalGroupingCommand { get; }

        public ICommand StandardModeCommand { get; }

        public ICommand ProgrammerModeCommand { get; }

        public ICommand AboutCommnad { get; }
        public ICommand CutCommand { get; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }




        public string Display
        {
            get => _display;
            set { _display = value; OnPropertyChanged(nameof(Display)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CalculatorViewModel()
        {
            _display = "0";
            _engine = new CalculatorEngine();
            _isNewInput = true;
            MemoryStack = new ObservableCollection<double>();
            isGrouped = Properties.Settings.Default.DigitGroupingEnabled;
            currentTheme = "BlueTheme";

            DigitCommand = new RelayCommand<string>(AppendDigit);
            OperatorCommand = new RelayCommand<string>(ProcessOperator);
            EqualCommand = new RelayCommand(ExecuteEqual);
            ClearCommand = new RelayCommand(Clear);
            BackspaceCommand = new RelayCommand(Backspace);
            FunctionCommand = new RelayCommand<string>(ProcessFunction);
            MemoryStoreCommand = new RelayCommand(StoreMemory);
            MemoryRecallCommand = new RelayCommand(RecallMemory);
            MemoryClearCommand = new RelayCommand(ClearMemory);
            MemoryStackCommand = new RelayCommand(OpenMemoryStack);
            MemoryAddCommand = new RelayCommand(AddToMemory);
            MemorySubtractCommand = new RelayCommand(SubtractFromMemory);
            DigitalGroupingCommand = new RelayCommand(DigitGroupingFunction);
            AboutCommnad = new RelayCommand(About);
            CutCommand = new RelayCommand(Cut);
            CopyCommand = new RelayCommand(Copy);
            PasteCommand = new RelayCommand(Paste);

            StandardModeCommand = new RelayCommand(StandardMode);
            ProgrammerModeCommand = new RelayCommand(ProgrammerMode);

        }

        private void StandardMode()
        {

            MainWindow standardWindow = new MainWindow();
            standardWindow.Show();
            Properties.Settings.Default.Save();
            CloseCurrentWindow();

        }

        private void ProgrammerMode()
        {

            ProgrammerMainWindow programmerWindow = new ProgrammerMainWindow();
            programmerWindow.Show();
            CalculatorWPF.Properties.Settings.Default.Save();
            CloseCurrentWindow();
        }
        private void CloseCurrentWindow()
        {
            foreach (Window win in Application.Current.Windows)
            {
                if (win.DataContext == this)
                {
                    win.Close();
                    break;
                }
            }
        }
        private void About()
        {
            MessageBox.Show("Aplication implement by: Onet Rares-Nicolae, Group 10LF233");
        }


        private void DigitGroupingFunction()
        {

            if (isGrouped)
            {

                Display = Display.Replace(",", "");
                isGrouped = false;
            }
            else
            {

                if (double.TryParse(Display, out double value))
                {

                    CultureInfo currentCulture = CultureInfo.CurrentCulture;


                    Display = value.ToString("N0", currentCulture);
                    isGrouped = true;
                }
            }
            CalculatorWPF.Properties.Settings.Default.DigitGroupingEnabled = isGrouped;
            Properties.Settings.Default.Save();
        }


        private void AppendDigit(string digit)
        {
            if (_isNewInput)
            {
                Display = (digit == ".") ? "0." : digit;
                _isNewInput = false;
            }
            else
            {
                if (digit == "." && Display.Contains(".")) return;
                Display += digit;
            }

            if (isGrouped && double.TryParse(Display, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out double value))
            {
                string decimalPart = Display.Contains(".") ? Display.Substring(Display.IndexOf(".")) : "";
                Display = value.ToString("N0", CultureInfo.CurrentCulture) + decimalPart;
            }
        }




        private void AddToMemory()
        {
            if (double.TryParse(Display, out double value))
            {
                if (MemoryStack.Count > 0)
                    MemoryStack[0] += value;
                else
                    MemoryStack.Add(value);
            }
        }

        private void SubtractFromMemory()
        {
            if (double.TryParse(Display, out double value))
            {
                if (MemoryStack.Count > 0)
                    MemoryStack[0] -= value;
                else
                    MemoryStack.Add(-value);
            }

        }


     
        private void ProcessOperator(string op)
        {
            if (double.TryParse(Display, out double operand))
            {
                double result = _engine.ProcessOperand(operand);
                Display = result.ToString();
            }
            _engine.SetOperator(op);
            _isNewInput = true;
        }

        private void ExecuteEqual()
        {
            if (double.TryParse(Display, out double operand))
            {
                double result = _engine.ProcessOperand(operand);

                if (isGrouped)
                {
                    CultureInfo currentCulture = CultureInfo.CurrentCulture;
                    Display = result.ToString("N", currentCulture); 
                }
                else
                {
                    Display = result.ToString();
                }
            }
            _engine.ClearOperator();
            _isNewInput = true;
        }

        private void Clear() => Display = "0";

        private void Backspace()
        {
            if (Display.Length > 1)
                Display = Display.Substring(0, Display.Length - 1);
            else
                Display = "0";
        }

        private void ProcessFunction(string func)
        {
            if (double.TryParse(Display, out double value))
            {
                double result = value;
                switch (func)
                {
                    case "%": result = value / 100; break;
                    case "√": result = Math.Sqrt(value); break;
                    case "x²": result = value * value; break;
                    case "1/x": result = 1 / value; break;
                    case "+/-": result = -value; break;
                }
                Display = result.ToString();
                _isNewInput = true;
            }
        }

        private void StoreMemory()
        {
            if (double.TryParse(Display, out double value))
                MemoryStack.Add(value);
        }

        private void RecallMemory()
        {
            if (MemoryStack.Count > 0)
                Display = MemoryStack[0].ToString();
        }

        private void ClearMemory() => MemoryStack.Clear();

        private void OpenMemoryStack()
        {
            MemoryStackWindow memoryWindow = new MemoryStackWindow(MemoryStack, currentTheme);
            memoryWindow.Show();
        }
        private void Cut()
        {
            
            Clipboard.SetText(Display);
          
            Display = "";
        }

        private void Copy()
        {
            Clipboard.SetText(Display); 
        }

        private void Paste()
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();
                if (double.TryParse(clipboardText, out double clipboardValue))
                {
                    Display = clipboardValue.ToString();
                }
                else
                {
                    MessageBox.Show("The cliboard don't contains a valid number.");
                }
            }
            else
            {
                MessageBox.Show("The clipboard don't contains text.");
            }

        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
