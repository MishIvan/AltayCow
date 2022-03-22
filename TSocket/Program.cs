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
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(point);
                socket.Listen(10);
                Console.WriteLine("Soket 192.168.0.100:8081 is opened. Listening...");
                string message = "Success!";
                int n;
                while (true)
                {
                    Thread th = new Thread(() => { 
                    Socket handler = socket.Accept();
                    byte[] buff = new byte[1024];
                    try
                    {
                        n = handler.Receive(buff);
                        if (n > 0)
                        {
                            String s1 = Encoding.UTF8.GetString(buff, 0, n);
                            Console.WriteLine("Received : " + s1 + "\n");
                            if (s1.ToLower() == "q")
                            {
                                buff = Encoding.UTF8.GetBytes("Shut down...");
                                handler.Send(buff);
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                return;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                        Console.WriteLine(ex.Message);
                        return;
                    }
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(message);
                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                });
                    th.Start();
                    th.Join();
                }                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}
