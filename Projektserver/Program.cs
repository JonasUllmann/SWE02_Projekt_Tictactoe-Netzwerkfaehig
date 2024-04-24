using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Projektserver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int serverPort = FindAvailablePort();
            Console.WriteLine($"Selected server port: {serverPort}");

            IPHostEntry iphost = Dns.GetHostEntry(Dns.GetHostName());

            foreach (var i in iphost.AddressList)
            {
                Console.WriteLine(i.ToString());
            }

            Console.WriteLine("Listening for messages...");

            Socket serverSock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            IPAddress serverIP = IPAddress.Any;
            IPEndPoint serverEP = new IPEndPoint(serverIP, serverPort);

            try
            {
                serverSock.Bind(serverEP);
            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                int errorCode = sockEx.ErrorCode;
            }

            serverSock.Listen(10);

            while (true)
            {
                Socket connection = serverSock.Accept();

                Byte[] serverBuffer = new Byte[8];
                String message = String.Empty;

                Console.WriteLine(message);
                connection.Close();
            }
        }

        static int FindAvailablePort()
        {
            // Starte bei einem zufälligen Port und prüfe, ob er verfügbar ist
            Random rand = new Random();
            int port = rand.Next(1024, 65535);

            while (!IsPortAvailable(port))
            {
                port = rand.Next(1024, 65535);
            }

            return port;
        }

        static bool IsPortAvailable(int port)
        {
            bool isAvailable = true;
            try
            {
                // Versuche, einen Listener auf dem Port zu binden
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                listener.Stop();
            }
            catch
            {
                // Wenn eine Exception ausgelöst wird, ist der Port nicht verfügbar
                isAvailable = false;
            }
            return isAvailable;
        }
    }
}