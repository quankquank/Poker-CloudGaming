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

        

        public string name;
        public string status = "off";
        
        

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
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public string whatAmI = "";
        
        public void main()
        {
            status = "waiting";
            try
            {
                
                client.ReceiveBufferSize = 256;

                string sReceive = "";
                sReceive = receive();

                

                //waiting
                while (true)
                {
                    sReceive = this.receive();
                    whatAmI = sReceive;
                    if (status != "matched")
                    {
                        //send("ok");
                    }
                    else break;
                    

                    if (this.whatAmI == "Black"||this.whatAmI=="White")
                    {
                        
                        Console.WriteLine("gamemodel " + whatAmI + "_");
                        status = "matched";
                        break;
                    }
                    
                }

            }
            catch (Exception e)
            {
                status = "off";
            }
        }

        public void createStartScreen()
        {
            Form a = new ChessUI(client.GetStream(), whatAmI);
            a.ShowDialog();
        }

        public string receive()
        {
            NetworkStream stream = client.GetStream();
            string result= "";

            byte[] buffer = new byte[client.ReceiveBufferSize];
            stream.Read(buffer, 0, buffer.Length);
            result = Encoding.ASCII.GetString(buffer);

            // 
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