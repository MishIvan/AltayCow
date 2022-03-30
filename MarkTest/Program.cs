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
            while (true)
            {
                try
                {

                    Socket handler = tSock.Accept();
                    do
                    {
                        Console.WriteLine("End Point: " + handler.RemoteEndPoint.ToString());
                        byte[] buff = new byte[1024];
                        n = handler.Receive(buff);
                        if (n > 0)
                        {
                            String s1 = Encoding.UTF8.GetString(buff, 0, n);
                            Console.WriteLine("Received : " + s1 + "\n");

                            Console.WriteLine("Enter message to send: ");
                            message = Console.ReadLine();
                            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
                            n = handler.Send(msg);
                            s1 = Encoding.UTF8.GetString(buff, 0, n);
                            Console.WriteLine("Sent : " + s1 + "\n");
                        }
                    } while (handler.Available > 0);
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


