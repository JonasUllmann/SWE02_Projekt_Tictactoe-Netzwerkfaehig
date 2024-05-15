using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    class Program
    {
        private static Semaphore semaphore = new Semaphore(1, 1); // Semaphore mit Anfangswert 1
        private static string symbol = ""; // Variable für das Symbol

        // Main Method 
        static void Main(string[] args)
        {
            Console.Title = "Socket Server";

            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress i in hostEntry.AddressList)
            {
                Console.WriteLine(i);
            }

            Console.WriteLine("Listening for messages...");

            Socket serverSock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            IPAddress serverIP = IPAddress.Any;
            IPEndPoint serverEP = new IPEndPoint(serverIP, 11111);

            serverSock.Bind(serverEP);
            /*try

               serverSock.Bind(serverEP);
           }
           catch (System.Net.Sockets.SocketException sockEx)
           {
               int errcode = sockEx.ErrorCode;
               Console.WriteLine(errcode);
           }*/
            serverSock.Listen(2);

            while (true)
            {
                Socket connection = serverSock.Accept();

                // Semaphore übernehmen
                semaphore.WaitOne();
                if (symbol == "")
                {
                    symbol = "X"; // Erster Client bekommt das Symbol "X"
                    Byte[] symbolBytes = Encoding.UTF8.GetBytes(symbol);
                    connection.Send(symbolBytes); // Sende das Symbol "X" an den Client
                }
                else if (symbol == "X")
                {
                    symbol = "O"; // Zweiter Client bekommt das Symbol "O"
                    Byte[] symbolBytes = Encoding.UTF8.GetBytes(symbol);
                    connection.Send(symbolBytes);// Sende das Symbol "O" an den Client
                } 

                Byte[] serverBuffer = new Byte[1024];
                String message = String.Empty;

                int bytes = connection.Receive(serverBuffer, serverBuffer.Length, 0);

                message += Encoding.UTF8.GetString(serverBuffer, 0, bytes);

                Console.WriteLine(message);
               
                

                // Semaphore freigeben
                semaphore.Release();

                connection.Close();
            }
        }
    }
}