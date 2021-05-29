using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPClient
{
    class Client
    {
        static void check()
        {
            Console.WriteLine("hello");
        }

        static void pause() 
        { 
            while (true) { } 
        }

        static string readKey(int dtmax)
        {
            DateTime t0 = DateTime.Now,t1;
            string result = "";

            int dt = 0;
            
            ConsoleKeyInfo cki;

            while (true)
            {
                t1 = DateTime.Now;

                dt = (int)t1.Subtract(t0).TotalMilliseconds;
                if (dt>dtmax) break;

                if (Console.KeyAvailable)
                {

                    cki = Console.ReadKey(true);
                    result += cki.Key + ": " + dt + "\n";
                }
                  
            }
            result += "------------";
            
            return result;
        }
        static void Main(string[] args)
        { 
            
            TcpClient client = new TcpClient();

            try
            {
                client.Connect("127.0.0.1", 8888);
            }
            catch
            {
                Console.WriteLine("Server offline.\nPlease try again later....");
                pause();
            }

            
            client.ReceiveBufferSize = 2000;



            NetworkStream stream;

            string stemp = " ";
            int loop = 0;

            int dt = 500;

            DateTime tSend, tRecv;
            
            while (true)
            {   
            //send
                
                stream = client.GetStream();
                
                
                //readkey
                stemp= "loop:  "+ loop.ToString();
                stemp+= "\n"+readKey(dt);

                loop += 1;
                Console.Clear();
                Console.WriteLine(stemp);
                
                byte[] buffer = Encoding.ASCII.GetBytes(stemp);

                tSend = DateTime.Now;
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();

                //recv
                buffer = new byte[client.ReceiveBufferSize];
                stream.Read(buffer, 0, buffer.Length);

                tRecv = DateTime.Now;
                dt = (int)tRecv.Subtract(tSend).TotalMilliseconds;
                Console.WriteLine("\n   ping: "+dt+" ms");

                Thread.Sleep(1000);

            }
        }
    }
}