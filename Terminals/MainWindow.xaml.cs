using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Terminals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            PropertyChangedEventArgs eventArguments = new PropertyChangedEventArgs(propertyName);

            handler?.Invoke(this, eventArguments);
        }
    }
}
