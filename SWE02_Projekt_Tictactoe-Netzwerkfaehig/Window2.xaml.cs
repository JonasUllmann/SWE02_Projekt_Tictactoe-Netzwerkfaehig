﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SWE02_Projekt_Tictactoe_Netzwerkfaehig
{
    /// <summary>
    /// Interaktionslogik für Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        //((MainWindow)Application.Current.MainWindow) -> Zugriff auf Mainwindow
        //private MainWindow m1;


        private int turn;   //Zählt die Anzahl der Züge, von dieser Variable abhängig wer gerade drann ist
        private int winrot;     //Zählt die Anzahl für gewonnene Runden von Rot
        private int winblau;    //Zählt die Anzahl für gewonnene Runden von Blau

        SolidColorBrush backgroundcolor = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFDDDDDD");   //Standardfarbe der Buttons in XAML, wichtig für richtige Farbe nach Reset

        Button pressedbutton;   //der gedrückte Button

        private MainWindow m1; //erstellt Mainwindowobjekt
        private Window1 win1;


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

        public Window2(MainWindow m1)
        {
            InitializeComponent();
            this.m1 = m1;


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



        private int checkfordraw()
        {
            int isdraw = 0;

            foreach(Button btn in row1)
            {
                if(btn.Content != null)
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

        private void btnclick(object sender, RoutedEventArgs e) //Funktion löst wenn einer der 9 Hauptbuttons gedrückt wird
        {
            int isdraw = 0;
            int win = 0;    //erhält die return Werte der Gewinn-funktionen
                            //return 0 -> keiner hat in diesem Zug gewonnen
                            //return 1 -> Rot, also O hat gewonnen
                            //return 2 -> Blau, also X hat gewonnen

            pressedbutton = (Button)sender; //Funktion wird ausgelöst, egal welcher der 9 Buttons gedrückt wird, gibt an welcher der 9 es war


            if (turn % 2 == 0 && pressedbutton.Content == null) //ist der Wert von turn gerade ist Blau an der Reihe
            {
                blueturn(pressedbutton); //der gedrückte Button wird in der entsprechenden Farbe markiert und der Content wird geändert
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);   //Anzeige wer jetzt an der Reihe ist wird entsprechend geändert


            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)    //ist der Wert von turn gerade ist Rot an der Reihe
            {
                redturn(pressedbutton); //der gedrückte Button wird in der entsprechenden Farbe markiert und der Content wird geändert
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);   //Anzeige wer jetzt an der Reihe ist wird entsprechend geändert
            }


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
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                //MessageBox.Show("Rot hat gewonnen!");
            }
            //Blau hat gewonnen
            else if (win == 2)
            {
                lockbuttons();
                btnnewgame.IsEnabled = true; //entsperrt Reset-button
                winblau++;
                tbkwinx.Text = $"X    {winblau}";
                //MessageBox.Show("Blau hat gewonnen");
            }
            else if(isdraw == 9)
            {
                lockbuttons();
                btnnewgame.IsEnabled = true;
            }
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
            Environment.Exit(0);        
        }


    }

}
