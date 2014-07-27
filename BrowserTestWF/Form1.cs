using BrowserTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WatiN.Core;
using WatiN.Core.DialogHandlers;

namespace BrowserTestWF
{
    public partial class Form1 : System.Windows.Forms.Form
    {

        public Form1()
        {
            InitializeComponent();
            SimpleHttpServer.Start("mmmn");
            this.textBox1.Text = "http://localhost:2000/test?s=</script><script>alert(1)</script>";
            System.Threading.Thread.Sleep(500);

            browser = new IE("http://www.google.com");
            //{
            //    browser.TextField(Find.ByName("q")).TypeText("WatiN");
            //    browser.Button(Find.ByName("btnG")).Click();

            //    Assert.IsTrue(browser.ContainsText("WatiN"));
            //}
        }

        IE browser = null;

        //private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        //{
        //    var htmlText = this.webBrowser1.DocumentText;
        //    if (htmlText.Contains("</script><script>alert(1)</script>"))
        //        MessageBox.Show("ok: " + position);

        //    if (fuzzing)
        //        NavigateNext();
        //}

        private void StartFuzz()
        {
            var x = 0;
            var posX = 0;

            while (true)
            {
                var c = Encoding.ASCII.GetString(new byte[] { (byte)x });
                var c2 = Encoding.ASCII.GetString(new byte[] { (byte)(x + 10007) });
                var c3 = Encoding.ASCII.GetString(new byte[] { (byte)(x + 10099) });
                var c4 = Encoding.ASCII.GetString(new byte[] { (byte)(x + 23) });
                //c+= Encoding.ASCII.GetString(new byte[] { (byte)y });
                
                //var payload = "</script><script>a(1);</script>";
                var payload = "<iframe/**/src=test2>";
                payload = payload.Insert(posX % payload.Length, c);
                
                if(x==255)
                {
                    x = 0;
                    posX++;
                }
                
                if (posX >= payload.Length)
                    break;

                x++;

                var url = "http://localhost:2000/test?s=" + payload;

                this.GoToString(url);

                var k = this.browser.Html;
                AlertDialogHandler alertDialogHandler = new AlertDialogHandler();
                using (new UseDialogOnce(browser.DialogWatcher, alertDialogHandler))
                {
                    if (alertDialogHandler.Exists())
                        MessageBox.Show("sdf");
                }

                //if(k.ToLower().Contains("<iframe"))
                //    MessageBox.Show("sdf");
             
            }
        }
        private int position = 0;
        private bool fuzzing = false;

        private void button1_Click(object sender, EventArgs e)
        {
            this.StartFuzz();
        }

        private void GoToString(string url)
        {
            dynamic dy = this.browser.InternetExplorer;
            dy.Navigate(url);
            this.browser.WaitForComplete();

        }

        private void textBox1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.GoToString(this.textBox1.Text);
        }
    }
}
