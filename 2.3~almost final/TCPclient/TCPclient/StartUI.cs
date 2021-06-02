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
            loadingPic.Visible = false;
            

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
                string nm = textBox1.Text;
                if (nm == "Enter your name...") nm = "Anonymous";

                if (cc.connectAtemp() == true)
                {

                    cc.name = nm;

                    //this.Hide();

                    cc.main();


                    if (cc.status != "waiting")
                    {
                        this.Hide();
                        cc.createStartScreen();

                    }

                    //cc.client.GetStream().Close();
                    //cc.client.Close();
                    //this.Show();
                    //cc.client.Close();
                    connected = false;
                }
                else
                {
                    label1.Text = "Server offline.\nPlease try again later....";


                }
            }
            
            
            
           
        }

        
        
        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, System.EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void textBox1_Enter(object sender, System.EventArgs e)
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            cc.main();
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {

        }

        private void connectBttn_MouseLeave(object sender, System.EventArgs e)
        {
            label1.Text = "";
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            
        }
    }
}
