using Calculator.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.TextFormatting;

namespace Calculator.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _KeyPressedString;

        public string KeyPressedString
        {
            get { return _KeyPressedString; }
            set { _KeyPressedString = value; OnPropertyChanged("KeyPressedString"); }
        }
        private string _Entered_Number;

        public string Entered_Number
        {
            get { return _Entered_Number; }
            set { _Entered_Number = value; OnPropertyChanged("Entered_Number"); }
        }

        private ButtonPressedCommand _buttonPressedCommand;

        public ButtonPressedCommand buttonPressedCommand
        {
            get { return _buttonPressedCommand; }
            set { _buttonPressedCommand = value; }
        }

        List<string> EnteredKeys;
        double Number = 0;
        bool FirstNumberEntered = true;
        bool EqualToFlag = true;
        bool FunctionPressed = false;
        string SelectedFunction = "";
        public string PreviousEnteredKey = "";

        public MainViewModel()
        {
            Entered_Number = "0";
            KeyPressedString = "";
            EnteredKeys = new List<string>();
            buttonPressedCommand=new ButtonPressedCommand(this);
        }

        void UpdateEnteredKeysOnGui()
        {
            string temp = "";
            for (int i = 0; i < EnteredKeys.Count; i++)
            {
                temp+= EnteredKeys[i];
            }
            KeyPressedString=temp;
        }

        void Addition()
        {
            Number += Convert.ToDouble(Entered_Number);
            Entered_Number=Number.ToString();
        }
        void Substraction()
        {
            Number -= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }
        void Multiplication()
        {
            Number *= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }
        void Division()
        {
            Number /= Convert.ToDouble(Entered_Number);
            Entered_Number = Number.ToString();
        }

        void CloseApp()
        {
            App.Current.Shutdown();
        }
        void EqualTo()
        {
            EnteredKeys.Clear();
            EnteredKeys.Add (Entered_Number);
            EqualToFlag = false;
        }
        void Clear()
        {
            EnteredKeys.Clear();
            KeyPressedString = "";
            Entered_Number = "0";
            Number = 0;
            FirstNumberEntered = true;
            EqualToFlag = false;
        }
        bool PressedButtonOperator(string pressedButton)
        {
            if (pressedButton=="0" || pressedButton == "1" || pressedButton == "2" || pressedButton == "3" || pressedButton == "4" || pressedButton == "5" || pressedButton == "6" || pressedButton == "7" || pressedButton == "8" || pressedButton == "9" || pressedButton==",")
            {
                if (EqualToFlag)
                {
                    Clear();
                }

                EnteredKeys.Add(pressedButton);
                UpdateEnteredKeysOnGui();

                PreviousEnteredKey = pressedButton;

                if (FunctionPressed)
                {
                    Entered_Number = "0";
                    FunctionPressed = false;
                }
                if (Entered_Number=="0")
                {
                    Entered_Number = pressedButton;
                }
                else
                {
                    Entered_Number += pressedButton;
                }

                return false;
            }
            else
                return true;
        }
        public void GetPressedButton(string pressedButton)
        {
            if (PressedButtonOperator(pressedButton))
            {
                if(PreviousEnteredKey!=pressedButton &&PreviousEnteredKey!="+" && PreviousEnteredKey != "-" && PreviousEnteredKey != "*" && PreviousEnteredKey != "/" && PreviousEnteredKey != "Esc") 
                {
                    if (EnteredKeys.Count == 0)
                    {
                        EnteredKeys.Add(Entered_Number);
                        UpdateEnteredKeysOnGui();
                    }

                    if (FirstNumberEntered)
                    {
                        Number=Convert.ToDouble(Entered_Number);
                        Entered_Number=Number.ToString();
                        FirstNumberEntered = false;
                    }
                    else
                    {
                        switch(SelectedFunction)
                        {
                            case "Addition": Addition(); break;
                            case "Subtraction": Substraction(); break;
                            case "Multiplication": Multiplication(); break;
                            case "Division": Division(); break;
                            case "EqualTo": EqualTo(); break;
                            case "Esc": CloseApp(); break;
                        }
                    }

                    switch(pressedButton)
                    {
                        case "+":
                            SelectedFunction = "Addition";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "-":
                            SelectedFunction = "Substraction";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "*":
                            SelectedFunction = "Multiplication";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "/":
                            SelectedFunction = "Division";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "=":
                            SelectedFunction = "EqualTo";
                            EnteredKeys.Add(pressedButton);
                            UpdateEnteredKeysOnGui();
                            PreviousEnteredKey = pressedButton;
                            FunctionPressed = true;
                            break;
                        case "Clr":
                            Clear();
                            FunctionPressed = true;
                            PreviousEnteredKey = pressedButton;
                            break;
                        case "Esc":                         
                            FunctionPressed = true;
                            PreviousEnteredKey = pressedButton;
                            CloseApp();
                            break;
                    }
                }else if(PreviousEnteredKey=="+" || PreviousEnteredKey == "-" || PreviousEnteredKey == "*" || PreviousEnteredKey == "/" || PreviousEnteredKey == "clr")
                {
                    EnteredKeys.RemoveAt(EnteredKeys.Count - 1);
                    EnteredKeys.Add(pressedButton);
                    UpdateEnteredKeysOnGui();

                    PreviousEnteredKey= pressedButton;
                    FunctionPressed= true;

                    switch(pressedButton)
                    {
                        case "+": SelectedFunction = "Addition";break;
                        case "-": SelectedFunction = "Substraction";break;
                        case "*": SelectedFunction = "Multiplication"; break;
                        case "/": SelectedFunction = "Division";break;
                        case "=": SelectedFunction = "EqualTo"; break;
                        case "Clr": Clear(); break;
                    }
                }
            }
        }
    }
}
