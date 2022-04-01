using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarkTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(Settings.Default.IP);
            IPEndPoint point = new IPEndPoint(ip, Convert.ToInt32(Settings.Default.Port));
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(point);
            socket.Listen(10);
            Console.WriteLine($"Soket {Settings.Default.IP}:{Settings.Default.Port} is listening...");
            Thread thread = new Thread(new ParameterizedThreadStart(RunListen))
            {
                IsBackground = true
            };
            thread.Start(socket);
            thread.Join();
        }
        static void RunListen(Object o)
        {
            Socket tSock = o as Socket;
            int n;
            string message = String.Empty;
            string[] sdata = {
                "0104607032142591215!:RkV93zn2u",
                "0104607032142591215!:s*v93Wyyj",
                "0104607032142591215!;Geg93R0s9",
                "0104607032142591215!;YXx93pzLd"
            };
            int i = 0;
            while (true)
            {
                try
                {
                    string words = String.Empty;
                    Socket handler = tSock.Accept();
                    Console.WriteLine("Accepted: " + handler.RemoteEndPoint.ToString());
                    if (i > 3)
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        Console.WriteLine("All data have been sent");
                        Thread.Sleep(100);
                        continue;
                    }

                    message = sdata[i++];
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(message + ";;");
                    n = handler.Send(msg);
                    if (n > 0)
                    {
                        words = Encoding.UTF8.GetString(msg, 0, n);
                        Console.WriteLine("Sent : " + words);
                    }
                    else
                    {
                        Console.WriteLine("Data hasn't been sent");
                    }

                    
                    byte[] buff = new byte[1024];
                    do
                    {
                        n = handler.Receive(buff);
                        if (n > 0)
                        {
                            words = Encoding.UTF8.GetString(buff, 0, n);
                            Console.WriteLine("Received: ", words);
                        }
                    }
                    while (handler.Available > 0);

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

    }

}


