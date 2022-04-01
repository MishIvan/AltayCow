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
            IPAddress ip = IPAddress.Parse("192.168.1.10");
            IPEndPoint point = new IPEndPoint(ip, 45678);
            string[] sdata = {
                "0104607032142591215!:RkV93zn2u",
                "0104607032142591215!:s*v93Wyyj",
                "0104607032142591215!;Geg93R0s9",
                "0104607032142591215!;YXx93pzLd"
            };
            int i = 0;

            try
            {
                while(i < 4)
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Connect(point);
                    if(socket.Connected)
                    {
                        Console.WriteLine("Connected: " + socket.RemoteEndPoint.ToString());
                    }
                    String smsg = sdata[i++];
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(smsg);
                    int nbytes = socket.Send(msg);
                    if (nbytes > 0)
                    {
                        String s1 = Encoding.UTF8.GetString(msg, 0, nbytes);
                        Console.WriteLine("Sent: " + s1);
                    }


                    byte[] buff = new byte[256];
                    nbytes = socket.Receive(buff);
                    if (nbytes > 0)
                    {
                        String s1 = Encoding.UTF8.GetString(buff, 0, nbytes);
                        Console.WriteLine("Received: " + s1);
                    }
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
                
            
        }   
    }
}

