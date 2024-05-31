using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SWE02_Projekt_Tictactoe_Netzwerkfaehig
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //((MainWindow)Application.Current.MainWindow) -> Zugriff auf Mainwindow
        //private MainWindow m1;


        private int turn;   //Gíbt an wer als nächstes am Zug ist 0->player, 1->player2
        private int winrot;     //Zählt die Anzahl für gewonnene Runden von Rot
        private int winblau;    //Zählt die Anzahl für gewonnene Runden von Blau
        private string p2turn;

        SolidColorBrush backgroundcolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");   //Standardfarbe der Buttons in XAML, wichtig für richtige Farbe nach Reset

        Button pressedbutton;   //der gedrückte Button

        //MainWindow m1 = new MainWindow(); //erstellt Mainwindowobjekt -> Zugriff auf ip port usw.

        private MainWindow m1;
        private Window2 win2;

        //Spieler
        public Player player;
        public Player player2;

        //Button-Reihen
        private List<Button> row1;
        private List<Button> row2;
        private List<Button> row3;

        //Button-Spalten
        private List<Button> column1;
        private List<Button> column2;
        private List<Button> column3;

        //Button-Diagonalen
        private List<Button> diagonal1;
        private List<Button> diagonal2;

        public Window1(MainWindow m1, Window2 win2)
        {
            InitializeComponent();
            this.m1 = m1;
            this.win2 = win2;

            this.m1.gamestart += gamestart;

            turn = 0;
            winrot = 0;
            winblau = 0;

            //Reihe 1 obere Reihe
            row1 = new List<Button>()
            {
                btntopleft, btntopmid, btntopright
            };
            //Reihe 2 mittlere Reihe
            row2 = new List<Button>()
            {
                btnmidleft, btnmidmid, btnmidright
            };
            //Reihe 3 untere Reihe
            row3 = new List<Button>()
            {
                btnbotleft, btnbotmid, btnbotright
            };

            //Spalte 1 linke Spalte
            column1 = new List<Button>()
            {
                btntopleft, btnmidleft, btnbotleft
            };
            //Spalte 2 mittlere Spalte
            column2 = new List<Button>()
            {
                btntopmid, btnmidmid, btnbotmid
            };
            //Spalte 3 rechte Spalte
            column3 = new List<Button>()
            {
                btntopright, btnmidright, btnbotright
            };

            //Diagonale 1 von links oben nach rechts unten
            diagonal1 = new List<Button>()
            {
                btntopleft, btnmidmid, btnbotright
            };
            //Diagonale 2 von rechts oben nach links unten
            diagonal2 = new List<Button>()
            {
                btntopright, btnmidmid, btnbotleft
            };
        }


        private async void gamestart(object sender, EventArgs e)
        {
            player = new Player(m1.Pteam, 0, m1.Pname);
            player2 = new Player("", 0, "");

            Byte[] serverBuffer = new Byte[1024];
            int bytes = await m1.ClientSocket.ReceiveAsync(new ArraySegment<byte>(serverBuffer), SocketFlags.None);
            player2.Name = Encoding.UTF8.GetString(serverBuffer, 0, bytes);

            if (player.Team == "O")
            {
                InitializeComponent();
                player2.Team = "X";
                tbkwinx.Text = $"{player2.Name}  {player2.Wins}";
                tbkwino.Text = $"{player.Wins}  {player.Name}";
                await player2turn();
            }
            else if (player.Team == "X")
            {
                player2.Team = "O";
                tbkwino.Text = $"{player2.Wins}  {player2.Name}";
                tbkwinx.Text = $"{player.Name}  {player.Wins}";
            }
        }

        private void gameloop()
        {
            lockbuttons();
            int win = 0;    //erhält die return Werte der Gewinn-funktionen
                            //return 0 -> keiner hat in diesem Zug gewonnen
                            //return 1 -> Rot, also O hat gewonnen
                            //return 2 -> Blau, also X hat gewonnen

            //Prüfen ob ein Spieler in diesem Zug gewonnen hat
            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            //Rot hat gewonnen
            if (win == 1)
            {
                lockbuttons();
                btnnewgame.IsEnabled = true; //entsperrt Reset-button

                //aktualisiert Spiel-UI
                if (player.Team == "O")
                {
                    player.Wins++;
                    tbkwino.Text = $"{player.Wins}  {player.Name}";

                }
                else if (player2.Team == "O")
                {
                    player2.Wins++;
                    tbkwino.Text = $"{player2.Wins}  {player2.Name}";
                }

            }
            //Blau hat gewonnen
            else if (win == 2)
            {
                lockbuttons();
                btnnewgame.IsEnabled = true; //entsperrt Reset-button

                //aktualisiert Spiel-UI
                if (player.Team == "X")
                {
                    player.Wins++;
                    tbkwinx.Text = $"{player.Name}  {player.Wins}";

                }
                else if (player2.Team == "X")
                {
                    player2.Wins++;
                    tbkwinx.Text = $"{player2.Name}  {player2.Wins}";
                }
            }

            if (turn == 0)
            {
                unlockbuttons();
            }
            else if (turn == 1)
            {
                player2turn();
            }

        }

        private async Task player2turn()
        {
            lockbuttons();
            Byte[] serverBuffer = new Byte[1024];
            p2turn = "";

            int bytes = await m1.ClientSocket.ReceiveAsync(new ArraySegment<byte>(serverBuffer), SocketFlags.None);
            p2turn += Encoding.UTF8.GetString(serverBuffer, 0, bytes);

            if (player2.Team == "X")
            {
                if (p2turn[0] == 'a')
                {
                    blueturn(row1[p2turn[1] - '0']);
                }
                else if (p2turn[0] == 'b')
                {
                    blueturn(row2[p2turn[1] - '0']);
                }
                else if (p2turn[0] == 'c')
                {
                    blueturn(row3[p2turn[1] - '0']);
                }
            }
            else if (player2.Team == "O")
            {
                if (p2turn[0] == 'a')
                {
                    redturn(row1[p2turn[1] - '0']);
                }
                else if (p2turn[0] == 'b')
                {
                    redturn(row2[p2turn[1] - '0']);
                }
                else if (p2turn[0] == 'c')
                {
                    redturn(row3[p2turn[1] - '0']);
                }
            }
            turn = 0;
            gameloop();
        }


        private int checkforrowwin()
        {
            //return 0 -> keiner hat in diesem Zug gewonnen
            //return 1 -> Rot, also O hat gewonnen
            //return 2 -> Blau, also X hat gewonnen
            int checksum = 0;

            //Test Reihe 1
            foreach (Button i in row1)
            {

                if (i.Content == "O")   //Button ist Rot
                {
                    checksum++;

                }

                else if (i.Content == "X")  //Button ist Blau
                {
                    checksum--;
                }
            }

            //Alle 3 Buttons von Reihe 1 Rot -> checksum = 3
            //Alle 3 Buttons von Reihe 1 Blau -> checksum = -3
            //Bei 2 verschiedenen Farben in einer Reihe kommt ein anderer Wert raus -> keiner hat gewonnen

            if (checksum == 3)
            {
                return 1;   //Rot hat gewonnen

            }
            else if (checksum == -3)
            {
                return 2;   //Blau hat gewonnen
            }
            else
            {
                checksum = 0; //checksum wird für die Nächste Reihe zurück auf 0 gestellt
            }

            foreach (Button i in row2)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            foreach (Button i in row3)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            return 0;   //niemand hat in der Reihe gewonnen
        }

        //selbes System wie bei checkforrowwin() für Erklärung siehe oben
        public int checkforcolumnwin()
        {
            //return 0 -> keiner hat in diesem Zug gewonnen
            //return 1 -> Rot, also der Spieler mit dem O hat gewonnen
            //return 2 -> Blaut, also X hat gewonnen
            int checksum = 0;

            foreach (Button i in column1)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            foreach (Button i in column2)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            foreach (Button i in column3)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            return 0;
        }
        //selbes System wie bei checkforrowwin() für Erklärung siehe oben
        public int checkfordiagonalwin()
        {

            //return 0 -> keiner hat in diesem Zug gewonnen
            //return 1 -> Rot, also der Spieler mit dem O hat gewonnen
            //return 2 -> Blaut, also X hat gewonnen
            int checksum = 0;

            foreach (Button i in diagonal1)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            foreach (Button i in diagonal2)
            {


                if (i.Content == "O")
                {
                    checksum++;
                }
                else if (i.Content == "X")
                {
                    checksum--;
                }
            }

            if (checksum == 3)
            {
                return 1;
            }
            else if (checksum == -3)
            {
                return 2;
            }
            else
            {
                checksum = 0;
            }

            return 0;

        }

        public void lockbuttons()   //Sperrt alle Buttons
        {
            foreach (Button btn in row1)
            {
                btn.IsEnabled = false;
            }

            foreach (Button btn in row2)
            {
                btn.IsEnabled = false;
            }

            foreach (Button btn in row3)
            {
                btn.IsEnabled = false;
            }
        }
        public void unlockbuttons()    //entsperrt alle Buttons
        {
            foreach (Button btn in row1)
            {
                btn.IsEnabled = true;
            }

            foreach (Button btn in row2)
            {
                btn.IsEnabled = true;
            }

            foreach (Button btn in row3)
            {
                btn.IsEnabled = true;
            }
        }



        private void redturn(Button pressedbutton)  // Rot ist am Zug
        {
            pressedbutton.Content = "O";    //Content bei Rot immer O
            pressedbutton.Background = new SolidColorBrush(Colors.Red);     //Farbe Rot
        }

        private void blueturn(Button pressedbutton) //Blau ist am Zug
        {
            pressedbutton.Content = "X";    //Content bei Blau immer X
            pressedbutton.Background = new SolidColorBrush(Colors.Blue);    //Farbe Blau
        }

        //erstellt string in dem Format wie er ihn dem Server weiterschickt
        private string genbtnstring(string name)
        {
            string btnstring = "";

            //a, b oder c stehen für Reihe 1, 2 oder 3
            //0, 1 oder 2 stehen für den Index innerhalb der jeweiligen Liste und somit für die Buttons
            
            
            if (name[3] == 't')
            {
                if (name[6] == 'l')
                {
                    btnstring = "a0";
                }
                else if (name[6] == 'm')
                {
                    btnstring = "a1";
                }
                else if (name[6] == 'r')
                {
                    btnstring = "a2";
                }

            }
            else if (name[3] == 'm')
            {
                if (name[6] == 'l')
                {
                    btnstring = "b0";
                }
                else if (name[6] == 'm')
                {
                    btnstring = "b1";
                }
                else if (name[6] == 'r')
                {
                    btnstring = "b2";
                }
            }
            else if (name[3] == 'b')
            {
                if (name[6] == 'l')
                {
                    btnstring = "c0";
                }
                else if (name[6] == 'm')
                {
                    btnstring = "c1";
                }
                else if (name[6] == 'r')
                {
                    btnstring = "c2";
                }
            }
            return btnstring;
        }

        private async void btnclick(object sender, RoutedEventArgs e)
        {
            pressedbutton = (Button)sender;
            if (pressedbutton.Content == null)
            {
                if (player.Team == "X")
                {
                    blueturn(pressedbutton);
                    turn++;
                    tbkturn.Background = new SolidColorBrush(Colors.Red);
                }
                else if (player.Team == "O")
                {
                    redturn(pressedbutton);
                    turn++;
                    tbkturn.Background = new SolidColorBrush(Colors.Blue);
                }
            }

            string pturn = genbtnstring(pressedbutton.Name);
            await m1.ClientSocket.SendAsync(Encoding.UTF8.GetBytes(pturn), SocketFlags.None);
            turn = 1;
            gameloop();
        }


        private void btnnewgame_Click(object sender, RoutedEventArgs e) //Funktion des Reset Buttons, setzt das spielfeld wieder auf standard zurück
        {
            tbkturn.Background = new SolidColorBrush(Colors.Blue);  //Farbe der "Wer ist am Zug" Anzeige auf Blau weil Blau beginnt immer


            //Alle Buttons werden Reihe für Reihe einzeln wieder auf Standard zurückgesetzt -> Content = null, Background = #FFDDDDDD
            foreach (Button btn in row1)
            {
                btn.Background = backgroundcolor;
                btn.Content = null;
            }
            foreach (Button btn in row2)
            {
                btn.Background = backgroundcolor;
                btn.Content = null;
            }
            foreach (Button btn in row3)
            {
                btn.Background = backgroundcolor;
                btn.Content = null;
            }

            unlockbuttons();
            btnnewgame.IsEnabled = false;
            turn = 0;   //turn zurück auf null gestellt 
        }

        private void btnclose_Click(object sender, RoutedEventArgs e)
        {
            m1.ClientSocket.Close();
            Environment.Exit(0);
        }


    }

    public class Player
    {

        private string team;
        private int wins;
        private string name;

        public Player(string team, int wins, string name)
        {
            this.team = team;
            this.wins = wins;
            this.name = name;
        }

        public string Team { get => team; set => team = value; }
        public int Wins { get => wins; set => wins = value; }
        public string Name { get => name; set => name = value; }
    }

}
