using System;
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
        IPAddress ipAddress = IPAddress.Any;
        int port = 11111;
        TcpListener listener = new TcpListener(ipAddress, port);

        try
        {
            listener.Start();
            Console.WriteLine("Tic Tac Toe Server gestartet. Warte auf Verbindungen...");

            TcpClient player1Client = listener.AcceptTcpClient();
            Console.WriteLine("Spieler X verbunden.");
            TcpClient player2Client = listener.AcceptTcpClient();
            Console.WriteLine("Spieler O verbunden.");

            Thread gameThread = new Thread(() => PlayGame(player1Client, player2Client));
            gameThread.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler: " + ex.Message);
        }
        finally
        {
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
            Console.WriteLine("Spieler X Name: " + player1Name);

            int name2Bytes = player2Stream.Read(buffer, 0, buffer.Length);
            string player2Name = Encoding.UTF8.GetString(buffer, 0, name2Bytes);
            Console.WriteLine("Spieler O Name: " + player2Name);

            byte[] name1Message = Encoding.UTF8.GetBytes(player1Name);
            byte[] name2Message = Encoding.UTF8.GetBytes(player2Name);
            player1Stream.Write(name2Message, 0, name2Message.Length);
            player2Stream.Write(name1Message, 0, name1Message.Length);

            while (true)
            {
                PerformMove(player1Stream, player2Stream, 'X');
                PerformMove(player2Stream, player1Stream, 'O');
            }
        }
        catch (Exception ex) { 
             Console.WriteLine("Fehler im Spiel: " + ex.Message);
        }
        finally
        {
            player1Client.Close();
            player2Client.Close();
        }
    }

    static void PerformMove(NetworkStream currentPlayerStream, NetworkStream otherPlayerStream, char symbol)
    {
        try
        {
            byte[] moveData = new byte[1024];
            int bytesRead = currentPlayerStream.Read(moveData, 0, moveData.Length);
            if (bytesRead == 0) return;  // Verbindung geschlossen
            string moveString = Encoding.UTF8.GetString(moveData, 0, bytesRead);
            Console.WriteLine($"{symbol} spielt Zug: {moveString}");
            otherPlayerStream.Write(moveData, 0, bytesRead);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fehler beim Ausführen des Zugs: " + ex.Message);
        }
    }
}
