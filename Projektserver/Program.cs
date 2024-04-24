﻿using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Xml;

namespace Projektserver
{
    class Program
    {
        // Main Method
        static void Main(string[] args)
        {
            Console.Title = "Socket Server";

            IPHostEntry hostEntry = Dns.GetHostByName(Dns.GetHostName());
            Console.WriteLine(hostEntry.AddressList[2]);


            Console.WriteLine("Listening for messages...");

            Socket serverSock = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);


            IPAddress serverIP = IPAddress.Any;
            IPEndPoint serverEP = new IPEndPoint(serverIP, 33367);

            serverSock.Bind(serverEP);
            /*try
            {
                serverSock.Bind(serverEP);
            }
            catch (System.Net.Sockets.SocketException sockEx)
            {
                int errcode = sockEx.ErrorCode;
                Console.WriteLine(errcode);
            }*/
            serverSock.Listen(10);

            while (true)
            {
                Socket connection = serverSock.Accept();

                Byte[] serverBuffer = new Byte[1024];
                String message = String.Empty;

                int bytes = connection.Receive(serverBuffer, serverBuffer.Length, 0);

                message += Encoding.UTF8.GetString(serverBuffer, 0, bytes);

                Console.WriteLine(message);

                connection.Close();
            }
        }
    }
}