﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace SWE02_Projekt_Tictactoe_Netzwerkfaehig
{
    public partial class MainWindow : Window
    {
        private Window1 win1;
        private Window2 win2;


        public MainWindow()
        {
            InitializeComponent();
            win2 = new Window2(this);
            win1 = new Window1(this, win2);


        }

        private string pteam;
        private string ip;
        private string pname;
        private int port;
        //port bei server 11111

        private Socket clientSocket;
        private IPEndPoint serverendpoint;

        public event EventHandler gamestart;


        public string Ip { get => ip; set => ip = value; }
        public int Port { get => port; set => port = value; }
        public string Pname { get => pname; set => pname = value; }
        public Socket ClientSocket { get => clientSocket; set => clientSocket = value; }
        public IPEndPoint Serverendpoint { get => serverendpoint; set => serverendpoint = value; }
        public string Pteam { get => pteam; set => pteam = value; }

        protected void onGamestart()
        {
            //ruft den Start des Spiels im anderen Fenster auf
            gamestart?.Invoke(this, EventArgs.Empty);
        }

        private void btn_start_Click(object sender, RoutedEventArgs e) //zeigt das Fenster mit Tictactoe und versteckt das erste
        {


            //Ist die Checkbox zum Lokalen spiel angehackt? -> Ja Window2
                                                         // -> Nein Window1
            if (cbxplaylocal.IsChecked == false)
            {
                win1.Show();
                this.Hide();
                onGamestart();
            }
            else if (cbxplaylocal.IsChecked == true)
            {
                win2.Show();
                this.Hide();
                
            }
        }

        private async void btn_connect_Click(object sender, RoutedEventArgs e)
        {
            btn_connect.IsEnabled = false;
            this.Ip = tbxip.Text;
            this.Port = Convert.ToInt32(tbxport.Text);
            this.Pname = tbxname.Text;

            //erstellt den Enpoint des Servers und den Socket
            serverendpoint = new IPEndPoint(IPAddress.Parse(ip), port);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //verbindet sich mit dem Server
            await clientSocket.ConnectAsync(serverendpoint);

            Byte[] serverBuffer = new Byte[1024];
            String message = String.Empty;

            //erhält die Teamzuweisung vom Server
            int bytes = await clientSocket.ReceiveAsync(serverBuffer, SocketFlags.None);
            message += Encoding.UTF8.GetString(serverBuffer, 0, bytes);
            Pteam = message;

            if (message == "X" || message == "O")
            {
                tbxsuccess.Text = "Connection successful!";
            }
            else if (message != "X" && message != "O")
            {
                tbxsuccess.Text = "Something went wrong with the teamselection!";
            }

            //erwartet den string "ready" vom Server 
            int bytes2 =  clientSocket.Receive(serverBuffer, SocketFlags.None);
            string readysign = Encoding.UTF8.GetString(serverBuffer, 0, bytes2);

            //ist der Server Bereit schickt der Client ihm seinen Spielernamen
            if (readysign == "ready")
            {
                await clientSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(pname)), SocketFlags.None);
            }

            

            
            cbxplaylocal.IsEnabled = false;
            btn_start.IsEnabled = true;
        }

        private void playlocal_Checked(object sender, RoutedEventArgs e)
        {
            btn_connect.IsEnabled = false;
            tbxip.IsEnabled = false;
            tbxname.IsEnabled = false;
            tbxport.IsEnabled = false;

            btn_start.IsEnabled = true;

        }

        private void playlocal_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_connect.IsEnabled = true;
            tbxip.IsEnabled = true;
            tbxname.IsEnabled = true;
            tbxport.IsEnabled = true;

            btn_start.IsEnabled = false;
        }

        private void Test_Checked(object sender, RoutedEventArgs e)
        {
            tbxname.Text = "Test";
            tbxip.Text = "127.0.0.1";
            tbxport.Text = "11111";


        }
    }
}