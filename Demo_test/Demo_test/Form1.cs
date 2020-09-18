using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Demo_test
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.ComponentModel.Container components = null;
        Thread myTh = null;
        bool on = false;
        private System.Windows.Forms.Button button2;
        SerialPort sp = new SerialPort();

		public Form1()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//



        }


         //<summary>
         //清理所有正在使用的资源。
         //</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region  InitializeComponent 
        
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "open";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            //this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(16, 56);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(288, 196);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(120, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "close";
           // this.button2.Click += new System.EventHandler(this.button2_Click);

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(516, 344);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = " 串口测试";
            this.ResumeLayout(false);

		}
		#endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (sp.OpenPort("com1", 9600, 0, 8, 1))
            {
                on = true;
                myTh = new Thread(new ThreadStart(read));
                myTh.Start();
            }
        }

        //private void button2_Click(object sender, System.EventArgs e)
        //{
        //    on = false;
        //    sp.ClosePort();
        //    if (myTh.IsAlive)
        //    {
        //        myTh.Abort();
        //    }

        //}

        private void read()
        {
            while (on)
            {
                if (sp.Opened)
                {
                    try
                    {
                        byte[] buff = new byte[2048];
                        byte[] by = new byte[512];
                        bool getCR = false;
                        int readed = 0;
                        do
                        {
                            int now_read = sp.ReadPort(512, ref by);
                            if (now_read > 0)
                            {
                                Array.Copy(by, 0, buff, readed, now_read);
                                readed += now_read;
                                if ((buff[readed - 1] == 13) || (buff[readed - 1] == 10))
                                {
                                    string str = System.Text.Encoding.ASCII.GetString(buff, 0, readed);
                                    listBox1.Items.Add(str);
                                    getCR = true;
                                }
                            }
                            else
                            {
                                Thread.Sleep(100);
                            }

                        } while (!getCR);
                    }
                    catch
                    {
                        listBox1.Items.Add("exit");
                        break;
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}