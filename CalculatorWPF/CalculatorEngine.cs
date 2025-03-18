using System;
using System.Windows;

namespace CalculatorWPF
{
    public class CalculatorEngine
    {
        public double CurrentResult { get; private set; }
        public string PendingOperator { get; private set; } = null;

        public CalculatorEngine()
        {
            Reset();
        }

        public void Reset()
        {
            CurrentResult = 0;
            PendingOperator = null;
        }

        
        public double ProcessOperand(double operand)
        {
            if (PendingOperator == null)
            {
                CurrentResult = operand;
            }
            else
            {
                switch (PendingOperator)
                {
                    case "+":
                        CurrentResult += operand;
                        break;
                    case "-":
                        CurrentResult -= operand;
                        break;
                    case "×":
                    case "*":
                        CurrentResult *= operand;
                        break;
                    case "÷":
                    case "/":
                        if (operand == 0)
                        {
                            MessageBox.Show("Can't divide at 0");
                            CurrentResult = 0;
                            break;
                          
                        }
                        CurrentResult /= operand;
                        break;
                    default:
                        throw new InvalidOperationException("Invalid operator.");
                }
            }
            return CurrentResult;
        }

        public void SetOperator(string op)
        {
            PendingOperator = op;
        }

        public void ClearOperator()
        {
            PendingOperator = null;
        }
    }
}
