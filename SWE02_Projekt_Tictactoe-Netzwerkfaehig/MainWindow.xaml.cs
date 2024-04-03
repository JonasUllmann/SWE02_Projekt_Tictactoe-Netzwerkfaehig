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

        private int turn;
        private int winrot;
        private int winblau;
        Button pressedbutton;

        private List<Button> row1;
        private List<Button> row2;
        private List<Button> row3;

        private List<Button> column1;
        private List<Button> column2;
        private List<Button> column3;

        private List<Button> diagonal1;
        private List<Button> diagonal2;


        public MainWindow()
        {
            InitializeComponent();

            turn = 0;
            winrot = 0;
            winblau = 0;

            row1 = new List<Button>()
            {
                btntopleft, btntopmid, btntopright
            };
            row2 = new List<Button>()
            {
                btnmidleft, btnmidmid, btnmidright
            };
            row3 = new List<Button>()
            {
                btnbotleft, btnbotmid, btnbotright
            };

            column1 = new List<Button>()
            {
                btntopleft, btnmidleft, btnbotleft
            };
            column2 = new List<Button>()
            {
                btntopmid, btnmidmid, btnbotmid
            };
            column3 = new List<Button>()
            {
                btntopright, btnmidright, btnbotright
            };

            diagonal1 = new List<Button>()
            {
                btntopleft, btnmidmid, btnbotright
            };
            diagonal2 = new List<Button>()
            {
                btntopright, btnmidmid, btnbotleft
            };
        }

        private int checkforrowwin()
        {
            //return 0 -> keiner hat in diesem Zug gewonnen
            //return 1 -> Rot, also der Spieler mit dem O hat gewonnen
            //return 2 -> Blaut, also X hat gewonnen
            int checksum = 0;


            foreach (Button i in row1)
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

            return 0;
        }

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



        private void redturn(Button pressedbutton)
        {
            pressedbutton.Content = "O";
            pressedbutton.Background = new SolidColorBrush(Colors.Red);


        }

        private void blueturn(Button pressedbutton)
        {
            pressedbutton.Content = "X";
            pressedbutton.Background = new SolidColorBrush(Colors.Blue);


        }

        private void btntopleft_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = sender as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }

        }

        private void btntopmid_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btntopright_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btnmidleft_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btnmidmid_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btnmidright_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btnbotleft_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btnbotmid_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }

        private void btnbotright_Click(object sender, RoutedEventArgs e)
        {
            int win = 0;
            pressedbutton = e.Source as Button;


            if (turn % 2 == 0 && pressedbutton.Content == null)
            {
                blueturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Red);
            }
            else if (turn % 2 == 1 && pressedbutton.Content == null)
            {
                redturn(pressedbutton);
                turn++;
                tbkturn.Background = new SolidColorBrush(Colors.Blue);
            }

            win += checkforrowwin();
            win += checkforcolumnwin();
            win += checkfordiagonalwin();

            if (win == 1)
            {
                winrot++;
                tbkwino.Text = $"{winrot}    O";
                MessageBox.Show("Rot hat gewonnen!");
            }
            if (win == 2)
            {
                winblau++;
                tbkwino.Text = $"{winblau}    O";
                MessageBox.Show("Blau hat gewonnen");
            }
        }


        private void btnnewgame_Click(object sender, RoutedEventArgs e)
        {
            foreach (Button btn in row1)
            {
                btn.Background = new SolidColorBrush();
            }
        }

    }
}