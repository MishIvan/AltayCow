using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("192.168.0.100");
            IPEndPoint point = new IPEndPoint(ip, 8081);
            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //socket.Bind(point);
                //socket.Listen(10);
                //Console.WriteLine("Soket 192.168.0.100:8081 is opened. Listening...");
                string message = "Success!";
                int n;
                while (true)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    if (!socket.Connected) socket.Connect(point);
                    do
                    {
                        Console.WriteLine("End Point: " + socket.RemoteEndPoint.ToString());
                        byte[] buff = new byte[1024];
                        n = socket.Receive(buff);
                        if (n > 0)
                        {
                            String s1 = Encoding.UTF8.GetString(buff, 0, n);
                            Console.WriteLine("Received : " + s1 + "\n");
                            byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
                            socket.Send(msg);
                        }
                    } while (socket.Available > 0);
                    if(socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                        socket.Disconnect(true);
                        socket.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}
