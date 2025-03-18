using CalculatorWPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using System.Windows;
using System.Linq;

public class ProgrammerCalculatorViewModel : INotifyPropertyChanged
{
    private string _display;
    private CalculatorEngine _engine;
    private bool _isNewInput;
    private NumberBase _selectedBase { get; set; }

   public string currentTheme { get; set; }
    private bool isGrouped { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public ObservableCollection<double> MemoryStack { get; set; }

    public ProgrammerCalculatorViewModel()
    {
        _display = "0";
        _engine = new CalculatorEngine();
        _isNewInput = true;
        SelectedBase = NumberBase.Decimal;
        MemoryStack = new ObservableCollection<double>();
        isGrouped = CalculatorWPF.Properties.Settings.Default.DigitGroupingEnabled;
        currentTheme = "BlueTheme";
       

        switch (CalculatorWPF.Properties.Settings.Default.BaseNumeration)
        {
            case 2:
                _selectedBase = NumberBase.Binary;
                break;
            case 8:
                _selectedBase = NumberBase.Octal;
                break;
            case 10:
                _selectedBase = NumberBase.Decimal;
                break;
            case 16:
                _selectedBase = NumberBase.Hexadecimal;
                break;
        }

        // Comenzi pentru operații standard
        DigitCommand = new RelayCommand<string>(AppendDigit);
        OperatorCommand = new RelayCommand<string>(ProcessOperator);
        EqualCommand = new RelayCommand(ExecuteEqual);
        ClearCommand = new RelayCommand(Clear);
        BackspaceCommand = new RelayCommand(Backspace);
        FunctionCommand = new RelayCommand<string>(ProcessFunction);
        DigitalGroupingCommand = new RelayCommand(DigitGroupingFunction);
        AboutCommnad = new RelayCommand(About);
        CutCommand = new RelayCommand(Cut);
        CopyCommand = new RelayCommand(Copy);
        PasteCommand = new RelayCommand(Paste);
        MemoryStoreCommand = new RelayCommand(StoreMemory);
        MemoryClearCommand = new RelayCommand(ClearMemory);
        MemoryStackCommand = new RelayCommand(OpenMemoryStack);
        MemoryAddCommand = new RelayCommand(AddToMemory);
        MemorySubtractCommand = new RelayCommand(SubtractFromMemory);

        StandardModeCommand = new RelayCommand(StandardMode);
        ProgrammerModeCommand = new RelayCommand(ProgrammerMode);
    }

    // Comenzi
    public ICommand DigitCommand { get; }
    public ICommand OperatorCommand { get; }
    public ICommand EqualCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand FunctionCommand { get; }
    public ICommand DigitalGroupingCommand { get; }
    public ICommand StandardModeCommand { get; }
    public ICommand ProgrammerModeCommand { get; }
    public ICommand AboutCommnad { get; }
    public ICommand CutCommand { get; }
    public ICommand CopyCommand { get; }
    public ICommand PasteCommand { get; }
    public ICommand MemoryStoreCommand { get; }
    public ICommand MemoryClearCommand { get; }
    public ICommand MemoryStackCommand { get; }
    public ICommand MemoryAddCommand { get; }
    public ICommand MemorySubtractCommand { get; }

    public NumberBase SelectedBase
    {
        get => _selectedBase;
        set
        {
            if (_selectedBase != value)
            {
                _selectedBase = value;
                OnPropertyChanged(nameof(SelectedBase));
                OnPropertyChanged(nameof(AvailableDigitButtons));
            }
        }
    }

    public ObservableCollection<string> AvailableDigitButtons
    {
        get
        {
            switch (SelectedBase)
            {
                case NumberBase.Binary:
                    return new ObservableCollection<string> { "0", "1" };
                case NumberBase.Octal:
                    return new ObservableCollection<string>(Enumerable.Range(0, 8).Select(n => n.ToString()));
                case NumberBase.Decimal:
                    return new ObservableCollection<string>(Enumerable.Range(0, 10).Select(n => n.ToString()));
                case NumberBase.Hexadecimal:
                    var digits = Enumerable.Range(0, 10).Select(n => n.ToString()).ToList();
                    digits.AddRange(new[] { "A", "B", "C", "D", "E", "F" });
                    return new ObservableCollection<string>(digits);
                default:
                    return new ObservableCollection<string>();
            }
        }
    }

    public string Display
    {
        get => _display;
        set
        {
            if (_display != value)
            {
                _display = value;
                OnPropertyChanged(nameof(Display));
                OnPropertyChanged(nameof(BinaryDisplay));
                OnPropertyChanged(nameof(OctalDisplay));
                OnPropertyChanged(nameof(DecimalDisplay));
                OnPropertyChanged(nameof(HexDisplay));
            }
        }
    }

    private void AppendDigit(string digit)
    {
        if (_isNewInput)
        {
            Display = digit;
            _isNewInput = false;
        }
        else
        {
            Display += digit;
        }

        if (SelectedBase == NumberBase.Hexadecimal && "ABCDEF".Contains(digit.ToUpper()))
        {
            string convertedDigit = digit.ToUpper() switch
            {
                "A" => "10",
                "B" => "11",
                "C" => "12",
                "D" => "13",
                "E" => "14",
                "F" => "15",
                _ => digit
            };
            Display = convertedDigit;
        }

        if (double.TryParse(Display, out double decimalValue))
        {
            if (isGrouped)
            {
                CultureInfo currentCulture = CultureInfo.CurrentCulture;
                Display = decimalValue.ToString("N0", currentCulture);
            }
            else
            {
                Display = decimalValue.ToString();
            }
        }
    }

    private void ProcessOperator(string op)
    {
        string inputValue = Display;
        if ("ABCDEF".Contains(inputValue.ToUpper()))
        {
            inputValue = inputValue.ToUpper() switch
            {
                "A" => "10",
                "B" => "11",
                "C" => "12",
                "D" => "13",
                "E" => "14",
                "F" => "15",
                _ => "0"
            };
        }


        if (double.TryParse(inputValue, out double operand))
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
                Display = result.ToString("N0", currentCulture); 
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
        CalculatorWPF.Properties.Settings.Default.Save();
    }






    private void StandardMode()
    {
        MainWindow standardWindow = new MainWindow();
        standardWindow.Show();
        CalculatorWPF.Properties.Settings.Default.Save();
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
                MessageBox.Show("The clickboard don't contains valid number.");
            }
        }
        else
        {
            MessageBox.Show("The clickboard don't contain a text.");
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public ObservableCollection<NumberBase> AvailableBaseList
    {
        get
        {
            return new ObservableCollection<NumberBase>
            {
                NumberBase.Binary,
                NumberBase.Octal,
                NumberBase.Decimal,
                NumberBase.Hexadecimal
            };
        }
    }


    public string BinaryDisplay
    {
        get
        {
            string displayWithoutFormatting = Display.Replace(",", "");

            if (long.TryParse(displayWithoutFormatting, out long value))
            {
                return Convert.ToString(value, 2);  
            }
            return "Invalid"; 
        }
    }

    public string OctalDisplay
    {
        get
        {
            string displayWithoutFormatting = Display.Replace(",", "");

            if (long.TryParse(displayWithoutFormatting, out long value))
            {
                return Convert.ToString(value, 8); 
            }
            return "Invalid"; 
        }
    }

    public string DecimalDisplay
    {
        get => Display; 
    }

    public string HexDisplay
    {
        get
        {
            string displayWithoutFormatting = Display.Replace(",", "");

            if (long.TryParse(displayWithoutFormatting, out long value))
            {
                return Convert.ToString(value, 16).ToUpper();  
            }
            return "Invalid"; 
        }
    }

    private void StoreMemory()
    {
        if (double.TryParse(Display, out double value))
            MemoryStack.Add(value);
    }    
    private void ClearMemory() => MemoryStack.Clear();

    private void OpenMemoryStack()
    {
        MemoryStackWindow memoryWindow = new MemoryStackWindow(MemoryStack,currentTheme);
        memoryWindow.Show();
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

}
