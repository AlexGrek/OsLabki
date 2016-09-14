using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace Terminals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Thread A, B, C;
        private int[] _registry = new int[4];   //terminal A, terminal B, terminal C in, terminal C out
        private string[] _term = { "off", "off" };
        public Semaphore Sem = new Semaphore(0, 1), Sem2 = new Semaphore(1, 1);

        private string _check = "Initialized" + Environment.NewLine;
        public string Check
        {
            get
            {
                return _check;
            }
            set
            {
                _check = value + Environment.NewLine + _check;
                OnPropertyChanged("Check");
            }
        }

        public string TermA
        {
            get
            {
                return _term[0];
            }
            set
            {
                _term[0] = value;
                OnPropertyChanged("TermA");
            }
        }

        public string TermB
        {
            get
            {
                return _term[1];
            }
            set
            {
                _term[1] = value;
                OnPropertyChanged("TermB");
            }
        }

        public void Write(int i, string content)
        {
            _term[i] = content;
            OnPropertyChanged("TermA");
            OnPropertyChanged("TermB");
        }

        private int _coins5 = 20;
        public int Coins5
        {
            get
            {
                return _coins5;
            }
            set
            {
                _coins5 = value;
                OnPropertyChanged("Coins5");
            }
        }

        private int _coins1 = 50;
        public int Coins1
        {
            get
            {
                return _coins1;
            }
            set
            {
                _coins1 = value;
                OnPropertyChanged("Coins1");
            }
        }

        private int _coins2 = 25;
        public int Coins2
        {
            get
            {
                return _coins2;
            }
            set
            {
                _coins2 = value;
                OnPropertyChanged("Coins2");
            }
        }

        private int _coins10 = 15;
        public int Coins10
        {
            get
            {
                return _coins10;
            }
            set
            {
                _coins10 = value;
                OnPropertyChanged("Coins10");
            }
        }

        private int _coins25 = 10;
        public int Coins25
        {
            get
            {
                return _coins25;
            }
            set
            {
                _coins25 = value;
                OnPropertyChanged("Coins25");
            }
        }

        private int _coins50 = 5;
        public int Coins50
        {
            get
            {
                return _coins50;
            }
            set
            {
                _coins50 = value;
                OnPropertyChanged("Coins50");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            RunProcesses();
        }

        private void RunProcesses()
        {
            A = new Thread(() => UIprocess(0));
            B = new Thread(() => UIprocess(1));
            C = new Thread(() => CalculatorProcess());

            A.Start();
            B.Start();
            C.Start();
        }

        public void UIprocess(int ind)
        {
            var state = TerminalState.Off;
            var timer = 5;
            for (;;)
            {
                Thread.Sleep(200);
                switch(state)
                {
                    case TerminalState.Off:
                        if (_registry[ind] == 10)
                        {
                            _registry[ind] = 0;
                            state = TerminalState.WaitingForInput;
                            Write(ind, "Select city, please");
                        }
                        else
                            Write(ind, "off");
                        break;

                    case TerminalState.WaitingForInput:
                        if (_registry[ind] != 0)
                        {
                            var got = _registry[ind];
                            _registry[ind] = 0;
                            state = TerminalState.WaitingForResponce;
                            Write(ind, "Processing...");
                            var result = DealWithMath(got);
                            state = TerminalState.ShowingMessage;
                            if (result)
                                Write(ind, "Take ur coins");
                            else
                                Write(ind, "Sorry, no coins 4 u");
                        }
                        break;

                    case TerminalState.ShowingMessage:
                        if (timer <= 0)
                        {
                            state = TerminalState.Off;
                            timer = 5;
                        }
                        else
                            timer -= 1;
                        break;

                }
            }
        }

        private bool DealWithMath(int k)
        {
            Thread.Sleep(100);
            int[] bilets = { 0, 28, 37, 50, 77, 91 };
            var rest = 100 - bilets[k];
            _registry[2] = rest;
            Sem2.Release();
            Sem.WaitOne();
            return _registry[3] == 1;
        }

        public void CalculatorProcess()
        {
            for (;;)
            {
                Sem2.WaitOne();
                if (_registry[2] != 0)
                {
                    var rest = _registry[2];
                    _registry[2] = 0;
                    Thread.Sleep(2000);
                    if (Count(rest))
                        _registry[3] = 1; //means ok
                    else
                        _registry[3] = 0; //not ok
                    Thread.Sleep(300);
                    Sem.Release();
                }
            }
        }

        bool Count(int rest)
        {
            var wallet = new Dictionary<int, int>
                { [1] = Coins1, [2] = Coins2, [5] = Coins5, [10] = Coins10, [25] = Coins25, [50] = Coins50 };
            int[] variants = { 50, 25, 10, 5, 2, 1 };

            int sum = 0;

            foreach (var v in variants)
            {
                while (wallet[v] > 0 && sum + v <= rest)
                {
                    wallet[v]--;
                    sum += v;
                    Thread.Sleep(100);
                }
                if (rest == sum)
                    break;
            }

            if (rest == sum)
            {
                var check = $@" Needed: {rest} (cost: {100 - rest})
                    1: {Coins1 - wallet[1]}
                    2: {Coins2 - wallet[2]}
                    5: {Coins5 - wallet[5]}
                    10: {Coins10 - wallet[10]}
                    25: {Coins25 - wallet[25]}
                    50: {Coins50 - wallet[50]}";
                Check = check;

                // update new coins values
                Coins1 = wallet[1];
                Coins10 = wallet[10];
                Coins2 = wallet[2];
                Coins25 = wallet[25];
                Coins50 = wallet[50];
                Coins5 = wallet[5];
            }
            else
                Check = $"Needed: {rest} (cost: {100 - rest})\nNo coins to give";

            return rest == sum;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {         
            if (sender == a0)
            {
                _registry[0] = 10;
                return;
            }
            if (sender == b0)
            {
                _registry[1] = 10;
                return;
            }

            if (sender == a1)
                _registry[0] = 1;
            if (sender == a2)
                _registry[0] = 2;
            if (sender == a3)
                _registry[0] = 3;
            if (sender == a4)
                _registry[0] = 4;
            if (sender == a5)
                _registry[0] = 5;

            if (sender == b1)
                _registry[1] = 1;
            if (sender == b2)
                _registry[1] = 2;
            if (sender == b3)
                _registry[1] = 3;
            if (sender == b4)
                _registry[1] = 4;
            if (sender == b5)
                _registry[1] = 5;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(propertyName);

            handler?.Invoke(this, eventArguments);
        }

        public enum TerminalState { Off, WaitingForInput, WaitingForResponce, ShowingMessage}
    }
}
