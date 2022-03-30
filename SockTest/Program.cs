using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SockTest
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

                socket.Connect(point);
                String smsg = "Ready!";
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(smsg);
                int nbytes = socket.Send(msg, msg.Length, SocketFlags.None);

                if (nbytes > 0)
                {
                    byte[] buff = new byte[256];
                    nbytes = socket.Receive(buff, 0, 256, SocketFlags.None);
                    if (nbytes > 0)
                    {
                        String s1 = Encoding.UTF8.GetString(buff, 0, nbytes);
                        Console.WriteLine("Received: " + s1);
                    }
                }
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
                
            
        }   
    }
}

