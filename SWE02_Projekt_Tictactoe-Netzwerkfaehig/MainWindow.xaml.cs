using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

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
        //port bei server 11111

        private Socket clientSocket;
        private IPEndPoint serverendpoint;


        public string Ip { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public string Pname { get => pname; set => pname = value; }
        public Socket ClientSocket { get => clientSocket; set => clientSocket = value; }
        public IPEndPoint Serverendpoint { get => serverendpoint; set => serverendpoint = value; }

        private void btn_start_Click(object sender, RoutedEventArgs e) //zeigt das Fenster mit Tictactoe und schließt das erste
        {
            win1.Show();
            this.Hide();
        }

        private void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            this.Ip = tbxip.Text;
            this.Port = Convert.ToInt32(tbxport.Text);
            this.Pname = tbxname.Text;

            serverendpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(serverendpoint);
            clientSocket.Send(Encoding.UTF8.GetBytes(pname));



            btn_start.IsEnabled = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            btn_connect.IsEnabled = false;
            tbxip.IsEnabled = false;
            tbxname.IsEnabled = false;
            tbxport.IsEnabled = false;

            btn_start.IsEnabled = true;

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_connect.IsEnabled = true;
            tbxip.IsEnabled = true;
            tbxname.IsEnabled = true;
            tbxport.IsEnabled = true;

            btn_start.IsEnabled = false;
        }
    }
}