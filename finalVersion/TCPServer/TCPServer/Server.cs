using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TCPServer
{
    class Server
    {
        static List<ClientHandler> serverCH = new List<ClientHandler>();
        static List<RoomHandler> serverRH = new List<RoomHandler>();

        static int waitingC = 0;

        //listen thread
        static void Main(string[] args)
        {   
            

            TcpListener listener = new TcpListener(8888);
            

            
            int numberOfC = 0;
            

            TcpClient temp;
            
           
            Thread checker = new Thread(checkStatus);
            checker.Start();

            //listen
            while (true)
            {
                listener.Start();
                temp = listener.AcceptTcpClient();

                numberOfC += 1;//danh id cho cac client
                serverCH.Add(new ClientHandler(temp,numberOfC));

                waitingC += 1;
                if (waitingC >=2)
                {
                    waitingC-= 2;
                    addRoom();

                }
            }
        }

        static void addRoom()
        {
            ClientHandler a = findWaitingC();
            ClientHandler b = findWaitingC();

            serverCH.Remove(a);
            serverCH.Remove(b);
            serverRH.Add(new RoomHandler(a, b));
            
        }
        //find waiting Client
        static ClientHandler findWaitingC()
        {
            foreach(ClientHandler i in serverCH)
            {
                if (i.status == "waiting")
                {
                    //i.checking.Abort();
                    i.status = "begin";
                    //sendMessages("begin", i);
                    return i;
                }
            }
            return null;
        }

        static void checkStatus()
        {
            while (true)
            {
                Console.Clear();
                
                Console.WriteLine("number of waiting clients:  " + serverCH.Count);
                Console.WriteLine("number of rooms          :  " + serverRH.Count);
                Console.WriteLine("\n \n");
                try
                {
                    foreach (ClientHandler x in serverCH)
                    {

                        if (x.status != "error")
                        {
                            IPEndPoint xx = (IPEndPoint)x.client.Client.RemoteEndPoint;
                            
                            Console.WriteLine("CID: " + x.clientID + "  " + xx.Address + " " + xx.Port + " " + x.client.Connected);
                        }

                        else
                        {

                            x.client.GetStream().Close();
                            serverCH.Remove(x);
                            waitingC -= 1;
                        }

                    }

                    foreach (RoomHandler y in serverRH)
                    {
                        if (y.rStatus == "closed")
                        {
                            serverRH.Remove(y);

                        }
                    }

                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("Updating");
                }
                        
                
                
                Thread.Sleep(500);
                
                
                
            }

        }

        

    }

    class RoomHandler
    {
        ClientHandler black;
        ClientHandler white;
        public string rStatus = "begin";
        public RoomHandler(ClientHandler _b,ClientHandler _w)
        {
            black = _b;
            white = _w;
            black.client.GetStream().Flush();
            white.client.GetStream().Flush();

            Thread match=new Thread(Match);
            match.Start();
            //send("hello from the other side...\n", black);
        }

        public void Match()
        {
            black.send("Black.");
            white.send("White.");

            
            //black.send(white.name+".");
            //white.send(black.name+".");

            while (true)
            {
                string temp = white.receive();
                if (white.status == "error")
                {   
                    black.send("8888");
                    rStatus = "closed";
                }
                else if (TimeOut(temp) == true)
                {
                    black.send("9999");
                    rStatus = "closed";
                }
                else black.send(temp);

                temp = black.receive();
                if (black.status == "error"||temp=="")
                {
                    white.send("8888");
                    rStatus = "closed";
                }
                else if (TimeOut(temp) == true)
                {
                    white.send("9999");
                    rStatus = "closed";
                }
                else white.send(temp);

                if (white.status == "error" && black.status == "error")
                {
                    black.client.Close();
                    white.client.Close();

                    rStatus = "closed";
                }

                if (rStatus == "closed") break;
            }
        }

        public bool TimeOut(string a)
        { string b = "Time Out";
            if (a.Substring(0, b.Length) == b) return true;
            return false;
        }

        

        
    }

    class ClientHandler
    {

        public string name;
        public int clientID = 0;
        public TcpClient client;
        public string status = "";

        
        public Thread checking;


        public ClientHandler(TcpClient c,int number)
        {
            this.clientID = number;
            this.client = c;
            this.status = "waiting";


            client.ReceiveBufferSize = 256;

            send("hello from server.");
            //name = receive();
            checking = new Thread(checkStatusC);


            checking.Start();
        }

        void checkStatusC()
        {
            try
            {
                while (status=="waiting")
                {
                    send("checking");

                    Thread.Sleep(500);

                }
            }
            catch (Exception e)
            {
                client.GetStream().Close();
                status = "error";
            }
            
        }

        public void send(string sentMessage)
        {
            try
            {
                
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];


                buffer = Encoding.ASCII.GetBytes(sentMessage);
                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                
                
            }
            catch (Exception e)
            {
                
                this.status = "error";
            }

            
        }

        public string receive()
        {
            try
            {
                string result = "";

                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];



                stream.Read(buffer, 0, buffer.Length);
                result = Encoding.ASCII.GetString(buffer);

                stream.Flush();
                
                
                
                
                return result;
                
            }
            catch (Exception e)
            {
                
                this.status = "error";
                
            }

            
            return "";
            
        }

        //listen

    }
}
