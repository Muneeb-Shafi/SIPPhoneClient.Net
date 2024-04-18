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
using Ozeki.VoIP;
using Ozeki.VoIP.SDK;
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
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            call = new CallHandlerSample(username, address, password);
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

            if(call.Call_State == CallState.InCall)
            {
                buttonChange();
            }

        }


        

        private void buttonChange()
        {
            button1.Enabled = true;
            button1.Text = "End Call";
            button1.ForeColor = Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            call.call.HangUp();
            softphone.Close();
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
    }
}
