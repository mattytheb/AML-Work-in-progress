using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AML
{
    public partial class Form1 : Form

    {
        List<String> newList = new List<String>();      //New webpage
        List<String> oldList = new List<String>();      //Old Webpage
        DateTime pageChange = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)      //Start button
        {
            using (WebClient client = new WebClient())              
            {

                client.DownloadStringCompleted += (s, ev) =>        //Download complete
                {
                    oldList = ev.Result.Split('\r').ToList();       //spit webpage up into paragraphs using the carriage return
                    Console.WriteLine("Start Timer");
                    timer1.Tick += Timer1_Tick;                     //Start timer, every 10 seconds re-download webpage
                    timer1.Interval = 10 * 1000;
                    timer1.Start();
                };

                string url = "https://www.treasury.gov/ofac/downloads/sdnlist.txt";     //webpage to check
                client.DownloadStringAsync(new Uri(url));                               //Download webpage as one string
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Uri myUri = new Uri("https://www.treasury.gov/ofac/downloads/sdnlist.txt");     // Creates an HttpWebRequest for the specified URL. 
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Console.WriteLine("check");


            if (DateTime.Compare(pageChange, myHttpWebResponse.LastModified) == 1)
            {
                Console.WriteLine("\nThe requested URI was last modified on:{0}", myHttpWebResponse.LastModified);
                // Releases the resources of the response.
                

                using (WebClient client = new WebClient())
                {

                    client.DownloadStringCompleted += (s, ev) =>
                    {
                        newList.AddRange(ev.Result.Split('\r').ToList());       //Dowload the webpage again to check for any changes
                        Console.WriteLine("Get new webpage");

                        for (int i = 0; i < newList.Count; i++)
                        {
                            if (!oldList.Contains(newList[i]))                  //loop through the new webpage to check for any changes or additons in the new webpage
                        {
                                Changes.Items.Add("Updated: " + newList[i] + '\r');     //display in the dialogue box any changes
                        }
                        }

                        for (int j = 0; j < oldList.Count; j++)
                        {
                            if (!newList.Contains(oldList[j]))                  //loop through the old webapage to check if any items have been deleted
                        {
                                Changes.Items.Add("Deleted: " + oldList[j] + '\r');
                            }
                        }

                        oldList.Clear();                //set the new list/webapge to become the old list/webage to check next time, used instead off oldList=newList                             
                        oldList.AddRange(newList);
                        newList.Clear();
                    };

                    string url = "https://www.treasury.gov/ofac/downloads/sdnlist.txt";
                    client.DownloadStringAsync(new Uri(url));
                }
                myHttpWebResponse.Close();
            }

        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Method for checking program using text files on Hard drive
    /*
     *public partial class Form1 : Form

    {
        List<String> newList = new List<String>();
        List<String> oldList = new List<String>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = System.IO.File.ReadAllText(@"C:\Users\Study\Documents\listing1.txt");

            newList = url.Split(',').ToList();

            timer1.Tick += Timer1_Tick;
            timer1.Interval = 5 * 1000;
            timer1.Start();

            
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            string url = System.IO.File.ReadAllText(@"C:\Users\Study\Documents\listing2.txt");

            oldList = newList;
            newList = url.Split(',').ToList();

            for (int i = 0; i < newList.Count; i++)
            {
                if (!oldList.Contains(newList[i]))
                {
                    Changes.Items.Add("Updated: " + newList[i] + '\r');
                }
            }

            for (int j = 0; j < oldList.Count; j++)
            {
                if (!newList.Contains(oldList[j]))
                {
                    Changes.Items.Add("Deleted: " + oldList[j] + '\r');
                }
            }

            oldList = newList;
        }
    }
    */
}
