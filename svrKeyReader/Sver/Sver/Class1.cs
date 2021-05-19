using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sver
{
    class Class1
    {   
        //writeFile: input, fileName,append or new?
        static void writeTo(string text, string fileName, Boolean doAppend)
        {
            StreamWriter sW;
            if (doAppend==true) sW = new StreamWriter(fileName,append: true);
            else sW = new StreamWriter(fileName, append: false);

            string a = "testest";

            sW.WriteLine(text);

            sW.Close();
        }

        //readFile: fileName
        static string readFrom(string fileName)
        {
            StreamReader sR = new StreamReader("test.txt");
            string a = sR.ReadToEnd();
            sR.Close();
            return a;
        }

        static string empty(int times)
        {   string a = "";
            for (int i = 0; i < times; i++) a += " ";
            return a;
        }

        static void Main(String[] args)
        {
            //file
            string outFile = "test.txt";

            
            //time for frames
            DateTime t1,t2;
            double dT;

            //all frame key, each frame key, bar
            string kPerFrame = "", currKey = "",bar="===================================";
           
            int frameTime = 1000; //millisec
            
            //loop for vector
            int loop = -1;

            //empty file
            writeTo("", outFile, false);


            while (true)
            {
                writeTo(kPerFrame+"\n"+bar+loop.ToString()+bar+"\n", outFile, true);
                loop += 1;
                Console.Clear();
                kPerFrame = "";
                t1 = DateTime.Now;
                while (true)
                {
                    t2 = DateTime.Now;
                    dT = t2.Subtract(t1).TotalMilliseconds;
                    if (dT > 3000) break;
                    
                    if (Console.KeyAvailable)
                    {
                        
                        currKey= Console.ReadKey(true).Key.ToString();
                        kPerFrame += currKey+empty(20-currKey.Length)+"\t at "+dT.ToString()+"\n";
                        Console.WriteLine(currKey);
                        //if (toets.Key.Equals(ConsoleKey.UpArrow)
                    }

                }
            }





        }
        


        

    }
}
