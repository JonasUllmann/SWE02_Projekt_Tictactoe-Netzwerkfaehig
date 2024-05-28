using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class TicTacToeServer
{
    static void Main(string[] args)
    {
        StartServer();
    }

    static void StartServer()
    {
        // Definiere die IP-Adresse und den Port, auf dem der Server lauschen soll
        IPAddress ipAddress = IPAddress.Any; // Lauscht auf allen verfügbaren IP-Adressen des Hosts
        int port = 11111; // Der gleiche Port wie im Clientcode

        // Erstelle einen TCP-Listener
        TcpListener listener = new TcpListener(ipAddress, port);

        try
        {
            // Beginne mit dem Lauschen auf Verbindungen von Clients
            listener.Start();
            Console.WriteLine("Tic Tac Toe Server gestartet. Warte auf Verbindungen...");

            // Warte auf Verbindung des ersten Spielers
            TcpClient player1Client = listener.AcceptTcpClient();
            Console.WriteLine("Spieler X verbunden.");
            // Warte auf Verbindung des zweiten Spielers
            TcpClient player2Client = listener.AcceptTcpClient();
            Console.WriteLine("Spieler O verbunden.");

            // Erstelle einen Thread für jedes Spiel
            Thread gameThread = new Thread(() => PlayGame(player1Client, player2Client));
            gameThread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler: " + ex.Message);
        }
        finally
        {
            // Schließe den Listener, wenn er nicht mehr benötigt wird
            listener.Stop();
        }
    }

    static void PlayGame(TcpClient player1Client, TcpClient player2Client)
    {
        try
        {
            // Erhalte die Netzwerkstreams für die Kommunikation mit den Spielern
            NetworkStream player1Stream = player1Client.GetStream();
            NetworkStream player2Stream = player2Client.GetStream();

            // Sende eine Nachricht an die Spieler welches Team sie sind 
            byte[] XMessage = Encoding.UTF8.GetBytes("X");
            byte[] OMessage = Encoding.UTF8.GetBytes("O");
            player1Stream.Write(XMessage, 0, XMessage.Length);
            player2Stream.Write(OMessage, 0, OMessage.Length);
            //Empfangen der Spielernamen
            // Puffer für eingehende Daten
            byte[] buffer = new byte[1024];

            // Anzahl der gelesenen Bytes
            int name1Bytes = player1Stream.Read(buffer, 0, buffer.Length);
            int name2Bytes = player2Stream.Read(buffer, 0, buffer.Length);

            // Empfangene Daten in einen String umwandeln
            string player1Name = Encoding.UTF8.GetString(buffer, 0, name1Bytes);
            string player2Name = Encoding.UTF8.GetString(buffer, 0, name2Bytes);
            //name dem anderen spieler schicken 
            byte[] name1Message = Encoding.UTF8.GetBytes(player1Name);
            byte[] name2Message = Encoding.UTF8.GetBytes(player1Name);
            player1Stream.Write(name2Message, 0, name2Message.Length);
            player2Stream.Write(name1Message, 0, name1Message.Length);

            // Erstelle ein leeres Spielfeld
            char[] board = { '-', '-', '-', '-', '-', '-', '-', '-', '-' };

            // Solange das Spiel läuft
            while (true)
            {
                // Spieler 1 ist am Zug
                PerformMove(player1Stream, player2Stream, board, 'X');

                // Überprüfe, ob Spieler 1 gewonnen hat oder das Spiel unentschieden ist
                if (CheckForWin(board, 'X'))
                {
                    SendWinMessage(player1Stream, player2Stream, "Spieler 1 hat gewonnen!");
                    break;
                }
                else if (CheckForDraw(board))
                {
                    SendDrawMessage(player1Stream, player2Stream, "Unentschieden!");
                    break;
                }

                // Spieler 2 ist am Zug
                PerformMove(player2Stream, player1Stream, board, 'O');

                // Überprüfe, ob Spieler 2 gewonnen hat oder das Spiel unentschieden ist
                if (CheckForWin(board, 'O'))
                {
                    SendWinMessage(player1Stream, player2Stream, "Spieler 2 hat gewonnen!");
                    break;
                }
                else if (CheckForDraw(board))
                {
                    SendDrawMessage(player1Stream, player2Stream, "Unentschieden!");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler im Spiel: " + ex.Message);
        }
        finally
        {
            // Schließe die Verbindungen zu den Spielern
            player1Client.Close();
            player2Client.Close();
        }
    }

    static void PerformMove(NetworkStream currentPlayerStream, NetworkStream otherPlayerStream, char[] board, char symbol)
    {
        try
        {
            // Sende das aktuelle Spielfeld an den Spieler
            byte[] boardData = Encoding.UTF8.GetBytes(new string(board));
            currentPlayerStream.Write(boardData, 0, boardData.Length);

            // Überprüfe, ob die Verbindung noch geöffnet ist
            if (currentPlayerStream.CanWrite)
            {
                // Empfange den Zug des Spielers
                byte[] moveData = new byte[1];
                currentPlayerStream.Read(moveData, 0, moveData.Length);
                char moveChar = Encoding.UTF8.GetChars(moveData)[0]; // Erhalte das Zeichen anstelle der Ganzzahl

                // Überprüfe, ob das empfangene Zeichen eine Ganzzahl darstellt
                if (char.IsDigit(moveChar))
                {
                    // Führe den Zug des Spielers aus
                    int moveIndex = int.Parse(moveChar.ToString()); // Konvertiere das Zeichen in eine Ganzzahl
                    board[moveIndex] = symbol;
                }
                else
                {
                    // Sende eine Fehlermeldung an den Spieler
                    byte[] errorMessage = Encoding.UTF8.GetBytes("ERROR: Ungültiger Zug");
                    currentPlayerStream.Write(errorMessage, 0, errorMessage.Length);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Ausführen des Zugs: " + ex.Message);
        }
    }


    static bool CheckForWin(char[] board, char symbol)
    {
        // Überprüfe alle möglichen Gewinnkombinationen
        return (board[0] == symbol && board[1] == symbol && board[2] == symbol) ||
               (board[3] == symbol && board[4] == symbol && board[5] == symbol) ||
               (board[6] == symbol && board[7] == symbol && board[8] == symbol) ||
               (board[0] == symbol && board[3] == symbol && board[6] == symbol) ||
               (board[1] == symbol && board[4] == symbol && board[7] == symbol) ||
               (board[2] == symbol && board[5] == symbol && board[8] == symbol) ||
               (board[0] == symbol && board[4] == symbol && board[8] == symbol) ||
               (board[2] == symbol && board[4] == symbol && board[6] == symbol);
    }

    static bool CheckForDraw(char[] board)
    {
        // Überprüfe, ob das Spielfeld voll ist (unentschieden)
        foreach (char cell in board)
        {
            if (cell == '-')
            {
                return false;
            }
        }
        return true;
    }

    static void SendWinMessage(NetworkStream player1Stream, NetworkStream player2Stream, string message)
    {
        // Sende Gewinnnachricht an beide Spieler
        byte[] winMessage = Encoding.UTF8.GetBytes("WIN:" + message);
        player1Stream.Write(winMessage, 0, winMessage.Length);
        player2Stream.Write(winMessage, 0, winMessage.Length);
    }

    static void SendDrawMessage(NetworkStream player1Stream, NetworkStream player2Stream, string message)
    {
        // Sende Unentschiedennachricht an beide Spieler
        byte[] drawMessage = Encoding.UTF8.GetBytes("DRAW:" + message);
        player1Stream.Write(drawMessage, 0, drawMessage.Length);
        player2Stream.Write(drawMessage, 0, drawMessage.Length);
    }
}
