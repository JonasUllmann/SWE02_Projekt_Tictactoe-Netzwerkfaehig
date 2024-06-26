﻿using System.Net.Sockets;
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
        private string p2turn;  //der string mit den Zugdaten der später an den Server gesendet wird

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
            //erstellen der Spielerobjekte
            player = new Player(m1.Pteam, 0, m1.Pname);
            player2 = new Player("", 0, "");

            Byte[] serverBuffer = new Byte[1024];
            
            //erhalten des Namens vom anderen Spieler
            int bytes = await m1.ClientSocket.ReceiveAsync(new ArraySegment<byte>(serverBuffer), SocketFlags.None);
            player2.Name = Encoding.UTF8.GetString(serverBuffer, 0, bytes);

            if (player.Team == "O")
            {
                //Zuweisung des Teams des 2. Spielers
                player2.Team = "X";

                //beschreibung der UI
                tbkwinx.Text = $"{player2.Name}  {player2.Wins}";
                tbkwino.Text = $"{player.Wins}  {player.Name}";
                
                //Blau beginnt immer -> ist der Spieler dieses Clients Rot erwartet er gleich am Anfang einen Zug
                await player2turn();
            }
            else if (player.Team == "X")
            {
                //Zuweisung des Teams des 2. Spielers
                player2.Team = "O";
                
                //beschreibung der UI
                tbkwino.Text = $"{player2.Wins}  {player2.Name}";
                tbkwinx.Text = $"{player.Name}  {player.Wins}";
            }
        }

        private void gameloop()
        {

            int isdraw = 0; //empfängt die Anzahl der bereits gedrückten Buttons

            int win = 0;    //erhält die return Werte der Gewinn-funktionen
                            //return 0 -> keiner hat in diesem Zug gewonnen
                            //return 1 -> Rot, also O hat gewonnen
                            //return 2 -> Blau, also X hat gewonnen

            //Prüfen ob ein Spieler in diesem Zug gewonnen hat
            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();
            isdraw = checkfordraw();

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

            //Hat keiner gewonnen aber sind alle Buttons schon gedrückt worden gibt es ein Unentschieden
            else if (isdraw == 9)
            {
                lockbuttons();
                btnnewgame.IsEnabled = true;
            }

            //turn ist 0, lokaler Spieler ist drann, turn = 1, remote Spieler ist drann
            else if (turn == 0)
            {
                unlockbuttons();
            }
            else if (turn == 1)
            {
                player2turn();
            }
            
        }

        //Zug des Gegnerischen Spielers
        private async Task player2turn()
        {
            lockbuttons();

            Byte[] serverBuffer = new Byte[1024];
            p2turn = "";

            //empfängt den Zug
            int bytes = await m1.ClientSocket.ReceiveAsync(new ArraySegment<byte>(serverBuffer), SocketFlags.None);
            p2turn += Encoding.UTF8.GetString(serverBuffer, 0, bytes);


            /*
            if (p2turn == "reset")
            {
                resetfield();
            }*/


            //Immer -'0' weil p2turn[1] entweder 0, 1 oder 2 ist und der char-Wert von diesen 48, 49, und 50 ist
            //subtrahiert man 48 also '0' von diesen chars erhält man den Wert des Integers         
            if (player2.Team == "X")
            {
                tbkturn.Background = new SolidColorBrush(Colors.Red);
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
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
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
            //turn 0 bedeutet der lokale client ist wieder drann 
            turn = 0;
            gameloop();
        }
    
        //überprüft wie viele Buttons beschrieben sind und gibt die Anzahl zurück
        private int checkfordraw()
        {
            int isdraw = 0;

            foreach (Button btn in row1)
            {
                if (btn.Content != null)
                {
                    isdraw++;
                }
            }
            foreach (Button btn in row2)
            {
                if (btn.Content != null)
                {
                    isdraw++;
                }
            }
            foreach (Button btn in row3)
            {
                if (btn.Content != null)
                {
                    isdraw++;
                }
            }

            return isdraw;
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
        private string genbtnstring(string name) //bekommt den Namen des Buttons mit um seine Position im Spielfeld zu ermitteln
        {
            
            //Button namen sind aufgebaut wie folgt: btn{reihe}{position}
            //die Reihe ist entweder top, mid, oder bot; position: left, mid oder right
            
            string btnstring = "";

            //a, b oder c stehen für Reihe 1, 2 oder 3
            //0, 1 oder 2 stehen für den Index innerhalb der jeweiligen Liste und somit für die Buttons
            

            //top
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
            //mid
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
            //bottom
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
                    tbkturn.Background = new SolidColorBrush(Colors.Red);
                }
                else if (player.Team == "O")
                {
                    redturn(pressedbutton);
                    tbkturn.Background = new SolidColorBrush(Colors.Blue);
                }
            }

            //generiert einen string welchen der andere Client als Zug auslesen kann
            string pturn = genbtnstring(pressedbutton.Name);

            //sendet den string an den Server
            await m1.ClientSocket.SendAsync(Encoding.UTF8.GetBytes(pturn), SocketFlags.None);
            turn = 1;
            gameloop();
        }

        private /*async*/ void resetfield()
        {
            tbkturn.Background = new SolidColorBrush(Colors.Blue);  //Farbe der "Wer ist am Zug" Anzeige auf Blau weil Blau beginnt immer

            /*
            if (btnnewgame.IsEnabled == true)
            {
                await m1.ClientSocket.SendAsync(Encoding.UTF8.GetBytes("reset"), SocketFlags.None);
            }*/

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


            btnnewgame.IsEnabled = false;

            //Ist der Client der blaue Spieler so darf er beginnen ist er Rot wartet er auf den Zug des anderen
            if (player.Team == "X")
            {
                unlockbuttons();
            }
            else if (player.Team == "O")
            {
                player2turn();
            }

        }

        private void btnnewgame_Click(object sender, RoutedEventArgs e) //Funktion des Reset Buttons, setzt das spielfeld wieder auf standard zurück
        {
            resetfield();
        }

        private void btnclose_Click(object sender, RoutedEventArgs e)
        {
            //schließt das Programm
            m1.ClientSocket.Close();
            Environment.Exit(0);
        }


    }

    //Klasse um die Variablen der Spieler zu speichern
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
