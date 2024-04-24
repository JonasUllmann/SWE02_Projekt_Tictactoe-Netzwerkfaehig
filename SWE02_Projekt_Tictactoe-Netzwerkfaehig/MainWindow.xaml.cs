using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SWE02_Projekt_Tictactoe_Netzwerkfaehig
{
    public partial class MainWindow : Window
    {
        Window1 win1 = new Window1();

        public MainWindow()
        {
            InitializeComponent();
        }

        private string ip;
        private string pname;
        private int port;


        public string Ip { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public string Pname { get => pname; set => pname = value; }

        private void btn_start_Click(object sender, RoutedEventArgs e) //zeigt das Fenster mit Tictactoe und schließt das erste
        {

            win1.Show();

            this.Close();
            
            
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            this.Ip = tbxip.Text;
            this.Port = Convert.ToInt32(tbxport.Text);
            this.Pname = tbxname.Text;

            IPEndPoint serverendpoint = new IPEndPoint(IPAddress.Parse(ip), port);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            clientSocket.Connect(serverendpoint);


            
        }

    }
}