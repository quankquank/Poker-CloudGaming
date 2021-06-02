using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class StartUI: Form
    {
        Client cc;
        bool connected = false;
        public StartUI()
        {
            InitializeComponent();
            //string a = "9999";
            //label1.Text=a.Substring(0,4);
        }
        
        private void button1_Click(object sender, System.EventArgs e)

        {
            
            if (connected == false)
            {
                cc = new Client();
                connected = true;
            }
            else
            {
                
                if (cc.connectAtemp() == true)
                {
                    label1.Text = "Disconnecting....";
                    cc.main();
                    if (cc.status =="matched")
                    {
                        this.Hide();
                        cc.createStartScreen();
                    }
                    connected = false;
                }
                else
                {
                    label1.Text = "Server offline.\nPlease try again later....";
                }
            }
        }

        private void pictureBox2_MouseLeave(object sender, System.EventArgs e)
        {
            label1.Text = "";
        }
    }
}
