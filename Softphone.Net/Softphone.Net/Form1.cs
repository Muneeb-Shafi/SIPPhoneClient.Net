using Ozeki.VoIP.SDK;
using Ozeki.VoIP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ozeki.voip.sip.client;
using Ozeki.Media.MediaHandlers;
using System.Threading;

namespace Softphone.Net
{
    public partial class Form1 : Form
    {
        static ISoftPhone softphone;
        static IPhoneLine phoneLine;
        CallHandlerSample call;
        string password;string username; string address;string recipent;
        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;

        }

        public Form1(string name, string pass, string add, string recp)
        {
            InitializeComponent();
            button1.Enabled = false;
            password = pass;
            username = name;
            address = add;
            recipent = recp;
            call = new CallHandlerSample(username, address, password);

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegState rs = call.Call($"{recipent}@{address}");

            while (true)
            {
                if (rs == RegState.RegistrationRequested)
                {
                    Thread.Sleep(1000);
                    rs = call.Call("1208@192.168.12.5");
                    if (rs == RegState.RegistrationSucceeded)
                    {
                        break;
                    }
                }
            }

            

        }


        

        private void buttonChange()
        {
            button1.Enabled = true;
            button1.Text = "End Call";
            button1.BackColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            call.call.HangUp();
            this.Close();
        }


        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            // Ensure the call is ended before closing
            if ( call.call != null && call.call.CallState != CallState.Completed)
            {
                call.call.HangUp();
                softphone.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (call.Call_State == CallState.InCall || call.Call_State == CallState.Ringing || call.Call_State == CallState.Setup)
            {
                buttonChange();
            }
        }
    }
}
