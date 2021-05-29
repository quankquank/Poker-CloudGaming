using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    class Server
    {
        static List<ClientHandler> serverCH = new List<ClientHandler>();

        static void check()
        {
            Console.WriteLine("hello");
        }

        static void pause() 
        { 
            while (true) { } 
        }

        static void Main(string[] args)
        {   
            

            TcpListener listener = new TcpListener(8888);
            

            
            int numberOfC = 0;
            TcpClient temp;
            
            NetworkStream stream;
            Thread checker = new Thread(checkAll);
            checker.Start();

            //listen
            while (true)
            {
                


                listener.Start();
                temp = listener.AcceptTcpClient();
                numberOfC += 1;
                serverCH.Add(new ClientHandler(temp,numberOfC));
            }
            

            
        }

        static void checkAll()
        {
            while (true)
            {
                try
                {
                    foreach (ClientHandler x in serverCH)
                    {
                        if (x.status == "error") serverCH.Remove(x);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Empty");
                }
                Console.Clear();
                Console.WriteLine("number of online clients:  " + serverCH.Count);
                Thread.Sleep(1000);
            }



            
        }
    }

    class ClientHandler
    {

        TcpClient client = default(TcpClient);
        int clientID = 0;
        Thread main;
        public string status = "ok";

        public ClientHandler(TcpClient c,int number)
        {
            this.client = c;
            this.clientID = number;
            main  = new Thread(this.run);
            main.Start();

        }

        //listen
        void run()
        { 
            NetworkStream stream;

            client.ReceiveBufferSize = 2000;

            //clientHandle

            try
            {
                while (true)
                {

                    //recv
                    stream = client.GetStream();

                    byte[] buffer = new byte[client.ReceiveBufferSize];
                    stream.Read(buffer, 0, buffer.Length);

                    //showResult(Encoding.ASCII.GetString(buffer));

                    //send confirm
                    buffer = Encoding.ASCII.GetBytes("OK\n");
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(">> UserID "+clientID+" offline...");
                Thread.Sleep(1000);
                status = "error";
            }

            
        }

        void showResult(string a)
        {
            //Console.Clear();
            Console.WriteLine("clientID:  "+ clientID+"\n"+a);
        }
        
}
}
