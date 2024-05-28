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
            byte[] name2Message = Encoding.UTF8.GetBytes(player2Name);
            player1Stream.Write(name2Message, 0, name2Message.Length);
            player2Stream.Write(name1Message, 0, name1Message.Length);




            // Solange das Spiel läuft
            while (true)

            {

                // Spieler 1 ist am Zug
                PerformMove(player1Stream, player2Stream, 'X');

                // Spieler 2 ist am Zug
                PerformMove(player2Stream, player1Stream ,'O');

                //buffer für die ENd message spielzug 
                byte[] endmessagebuffer = new byte[1024];
                int endmessageBytes = player1Stream.Read(endmessagebuffer, 0, endmessagebuffer.Length);
                string endmessage = Encoding.UTF8.GetString(endmessagebuffer, 0, endmessageBytes);

                if (endmessage=="End")
                {
                    player1Client.Close();
                    player2Client.Close();
                    break;
                }
                else
                {
                    continue;
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
