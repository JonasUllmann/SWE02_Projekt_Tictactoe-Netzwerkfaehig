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
            NetworkStream player1Stream = player1Client.GetStream();
            NetworkStream player2Stream = player2Client.GetStream();

            byte[] XMessage = Encoding.UTF8.GetBytes("X");
            byte[] OMessage = Encoding.UTF8.GetBytes("O");
            player1Stream.Write(XMessage, 0, XMessage.Length);
            player2Stream.Write(OMessage, 0, OMessage.Length);

            byte[] buffer = new byte[1024];

            int name1Bytes = player1Stream.Read(buffer, 0, buffer.Length);
            string player1Name = Encoding.UTF8.GetString(buffer, 0, name1Bytes);

            int name2Bytes = player2Stream.Read(buffer, 0, buffer.Length);
            string player2Name = Encoding.UTF8.GetString(buffer, 0, name2Bytes);

            byte[] name1Message = Encoding.UTF8.GetBytes(player1Name);
            byte[] name2Message = Encoding.UTF8.GetBytes(player2Name);
            player1Stream.Write(name2Message, 0, name2Message.Length);
            player2Stream.Write(name1Message, 0, name1Message.Length);

            while (true)
            {
                PerformMove(player1Stream, player2Stream, 'X');
                PerformMove(player2Stream, player1Stream, 'O');

                byte[] endMessageBuffer = new byte[1024];
                int endMessageBytes = player1Stream.Read(endMessageBuffer, 0, endMessageBuffer.Length);
                string endMessage = Encoding.UTF8.GetString(endMessageBuffer, 0, endMessageBytes);

                if (endMessage == "End")
                {
                    player1Client.Close();
                    player2Client.Close();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler im Spiel: " + ex.Message);
        }
    }

    static void PerformMove(NetworkStream currentPlayerStream, NetworkStream otherPlayerStream, char symbol)
    {
        try
        {
            

            // Überprüfe, ob die Verbindung noch geöffnet ist
            if (currentPlayerStream.CanWrite)
            {
                // Empfange den Zug des Spielers
                byte[] moveData = new byte[1024];
                currentPlayerStream.Read(moveData, 0, moveData.Length);
                string moveString = Encoding.UTF8.GetString(moveData); // Erhalten der Zeichenkombi
                otherPlayerStream.Write(moveData, 0, moveData.Length);
             
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Ausführen des Zugs: " + ex.Message);
        }
    }
}
