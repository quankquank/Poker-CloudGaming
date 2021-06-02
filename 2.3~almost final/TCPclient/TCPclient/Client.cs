using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TCPClient
{
    class Client
    {   
        public TcpClient client = new TcpClient();

        static string passing = "";

        public string name;
        public string status = "waiting";
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
            
                Form menu = new StartUI();
                menu.ShowDialog();
            
            
                
            
            
        }

        public bool connectAtemp()
        {
            try
            {
                client.Connect("127.0.0.1", 8888);
                
            }
            catch
            {
                //Console.WriteLine("Server offline.\nPlease try again later....");
                return false;
            }
            return true;
        }
        public string whatAmI = "";
        public string p2name;
        public void main()
        {
            status = "waiting";
            try
            {
                
                client.ReceiveBufferSize = 256;

                string sReceive = "";

                //DateTime tSend, tRecv;

                sReceive = receive();
                //Console.WriteLine(sReceive);

                //send(name);

                

                //waiting
                while (true)
                {
                    sReceive = this.receive();
                    whatAmI = sReceive;
                    //Console.WriteLine(sReceive);

                    if (this.whatAmI == "Black" || this.whatAmI == "White")
                    {
                        //this.p2name = this.receive();
                        status = "matched";
                        //Console.WriteLine(p2name);

                        

                        break;
                    }
                        
                    
                }

                //createStartScreen();
            }
            catch
            {
                //Console.WriteLine("Disconnected!\nPlease try again later....");
                Thread.Sleep(5000);
            }
        }

        public void createStartScreen()
        {
            Form a = new ChessUI(client.GetStream(), whatAmI);//,name,p2name);
            
            a.ShowDialog();
        }

        public string receive()
        {
            
            NetworkStream stream = client.GetStream();

            string result= "";


            byte[] buffer = new byte[client.ReceiveBufferSize];
            stream.Read(buffer, 0, buffer.Length);
            result = Encoding.ASCII.GetString(buffer);
            string temp = "";
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '.')
                {
                    result = result.Substring(0, i);
                }
            }
            
            return result;
        }

        public void send(string message)
        {

            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            buffer = Encoding.ASCII.GetBytes(message);

            stream.Write(buffer, 0, buffer.Length);

            stream.Flush();
            
        }
    }

    
}