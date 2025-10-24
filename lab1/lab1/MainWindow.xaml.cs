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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab1
{
    public partial class MainWindow : Window
    {
        private string currentInput = "0";
        private string previousInput = "";
        private string operation = "";
        private bool newInput = true;
        private bool operationPressed = false;

        public MainWindow()
        {
            InitializeComponent();
            UpdateDisplay();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string number = button.Content.ToString();

            if (currentInput == "0" || newInput || operationPressed)
            {
                currentInput = number;
                newInput = false;
                operationPressed = false;
            }
            else
            {
                currentInput += number;
            }

            UpdateDisplay();
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            if (operationPressed) return;

            Button button = (Button)sender;
            string newOperation = button.Content.ToString();

            if (!string.IsNullOrEmpty(operation) && !newInput)
            {
                Calculate();
            }
            else if (newInput)
            {
                previousInput = currentInput;
            }

            operation = newOperation;
            previousInput = currentInput;
            newInput = true;
            operationPressed = true;
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(previousInput) && !newInput)
            {
                Calculate();
                operation = "";
                previousInput = "";
            }
            newInput = true;
            operationPressed = false;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            currentInput = "0";
            previousInput = "";
            operation = "";
            newInput = true;
            operationPressed = false;
            UpdateDisplay();
        }

        private void Calculate()
        {
            try
            {
                double prev = double.Parse(previousInput);
                double current = double.Parse(currentInput);
                double result = 0;

                switch (operation)
                {
                    case "+":
                        result = prev + current;
                        break;
                    case "-":
                        result = prev - current;
                        break;
                    case "*":
                        result = prev * current;
                        break;
                    case "/":
                        if (current == 0)
                        {
                            throw new DivideByZeroException();
                        }
                        result = prev / current;
                        break;
                }

                result = Math.Round(result, 10);

                currentInput = result.ToString();
                if (currentInput.Contains(",") && currentInput.EndsWith(",0"))
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 2);
                }

                UpdateDisplay();
            }
            catch (DivideByZeroException)
            {
                currentInput = "Error";
                UpdateDisplay();
                currentInput = "0";
                newInput = true;
            }
            catch (Exception)
            {
                currentInput = "Error";
                UpdateDisplay();
                currentInput = "0";
                newInput = true;
            }
        }

        private void UpdateDisplay()
        {
            DisplayTextBox.Text = currentInput;
        }
    }
}
