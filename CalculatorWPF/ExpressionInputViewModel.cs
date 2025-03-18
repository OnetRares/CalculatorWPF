using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CalculatorWPF
{
    public class ExpressionInputViewModel : INotifyPropertyChanged
    {
        private string _expression;
        public string Expression
        {
            get => _expression;
            set
            {
                _expression = value;
                OnPropertyChanged(nameof(Expression));
            }
        }

        public ICommand CalculateCommand { get; }
        public ICommand CloseCommand { get; }

        public event Action<string> CalculationCompleted;

        public ExpressionInputViewModel()
        {
            CalculateCommand = new RelayCommand(CalculateExpression);
            CloseCommand = new RelayCommand(CloseWindow);
        }

        private void CalculateExpression()
        {
            if (string.IsNullOrWhiteSpace(Expression) || !Expression.EndsWith("="))
            {
                MessageBox.Show("Expresion must contain '=' at the end", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string infix = Expression.TrimEnd('=');
                string rpn = ConvertToRPN(infix);
                double result = EvaluateRPN(rpn);
                CalculationCompleted?.Invoke(result.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Can't evaluat result: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            CalculationCompleted?.Invoke(null);
        }

        private string ConvertToRPN(string infix)
        {
            Dictionary<char, int> precedence = new()
            {
                { '+', 1 }, { '-', 1 },
                { '*', 2 }, { '/', 2 }
            };

            Stack<char> operators = new();
            List<string> output = new();
            string number = "";

            foreach (char c in infix)
            {
                if (char.IsDigit(c) || c == '.')
                {
                    number += c;
                }
                else
                {
                    if (!string.IsNullOrEmpty(number))
                    {
                        output.Add(number);
                        number = "";
                    }

                    while (operators.Count > 0 && precedence.ContainsKey(c) && precedence[operators.Peek()] >= precedence[c])
                    {
                        output.Add(operators.Pop().ToString());
                    }

                    operators.Push(c);
                }
            }

            if (!string.IsNullOrEmpty(number))
                output.Add(number);

            while (operators.Count > 0)
                output.Add(operators.Pop().ToString());

            return string.Join(" ", output);
        }

        private double EvaluateRPN(string rpn)
        {
            Stack<double> stack = new();
            string[] tokens = rpn.Split(' ');

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    if (stack.Count < 2)
                        throw new InvalidOperationException("Invalid expresion");

                    double b = stack.Pop();
                    double a = stack.Pop();
                    double result = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b,
                        _ => throw new InvalidOperationException("Unknown operator")
                    };
                    stack.Push(result);
                }
            }

            return stack.Pop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
