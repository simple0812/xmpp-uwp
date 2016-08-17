using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Matrix;
using Matrix.Net;
using Matrix.Xml;
using Matrix.Xmpp;
using Matrix.Xmpp.Client;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace XApp
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private XmppClient xmppClient;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void BtnSend_OnClick(object sender, RoutedEventArgs e)
        {
            xmppClient.Send(new Message()
            {
                To = "zl@desktop-b80818f",
                Type = MessageType.Chat,
                Body = txtSend.Text
            });

        }

        private void BtnOpen_OnClick(object sender, RoutedEventArgs e)
        {
            string lic = @"eJxkkFtTwjAQhf8K46ujIQxWcNaM2IsilxawCr5FGttq0pQkpS2/XlQELy87
                e/bby5mFYbpkmWaNSvBMXx7R+ETLF1NSxS74FzoiECgZFUvTj8jMFFEqAR0q
                MCloZlJTEwxon4NdaCMFUwTGVDDirikvqJEK0KcGW4qcZvU3SGXW2FkB9M3A
                FTTlRFPO9NUPZ6fRtumLbZv3h8I8ooa5VZ4q5mwz0mri82YHnwP6h6CvHSYk
                MarY7toJ+Ii/561mF1uA/gCYpXFGTaEYyfN2ieOHu5BPB26QOL5fcpTXE8ss
                55vhjTUMXd1Dbk/ZVF/PvRY2x7Nr33P80Kvshzipzyp3JPBgxW49f5OsVass
                s8Wozd78tTdOnEHe6T1nTUt37+h92K9aE6w3q0cpPI7TJ6f9OAqSxZKN34IQ
                y/q107kp5tOXKEgq0RvgY8XnjNmrdjzt6ktAB9+Adu8m7wI=";

            Matrix.License.LicenseManager.SetLicense(lic);

            xmppClient = new XmppClient();
            xmppClient.Compression = false;
            xmppClient.Hostname = "desktop-b80818f";
            xmppClient.ResolveSrvRecords = true;
            xmppClient.StartTls = false;
            xmppClient.Status = "Online";
            xmppClient.Show = Show.None;

            Jid jid = new Jid("test@desktop-b80818f");
            xmppClient.Password = "123456789";
            xmppClient.Username = jid.User;
            xmppClient.SetXmppDomain(jid.Server);


            xmppClient.OnRosterEnd += XmppClient_OnRosterEnd;
            xmppClient.OnMessage += XmppClient_OnMessage;
            xmppClient.OnBind += XmppClient_OnBind;
            xmppClient.OnBindError += XmppClient_OnBindError;
            xmppClient.OnLogin += XmppClient_OnLogin;
            xmppClient.OnAuthError += XmppClient_OnAuthError;
            xmppClient.OnError += XmppClient_OnError;
            xmppClient.OnReceiveXml += new EventHandler<TextEventArgs>(XmppClientOnReceiveXml);
            xmppClient.OnSendXml += new EventHandler<TextEventArgs>(XmppClientOnSendXml);
            xmppClient.OnReceiveBody += new EventHandler<BodyEventArgs>(XmppClientOnReceiveBody);
            xmppClient.OnSendBody += new EventHandler<BodyEventArgs>(XmppClientOnSendBody);

            xmppClient.Open();
            //xmppClient.SendPresence(Show.Chat, "Online");
        }


        void XmppClientOnReceiveBody(object sender, BodyEventArgs e)
        {
            Debug.WriteLine(e.Body.ToString());
        }

        void XmppClientOnSendBody(object sender, BodyEventArgs e)
        {
            Debug.WriteLine(e.Body.ToString());
        }

        void XmppClientOnSendXml(object sender, TextEventArgs e)
        {
            Debug.WriteLine("SEND: " + e.Text);
        }

        void XmppClientOnReceiveXml(object sender, TextEventArgs e)
        {
            Debug.WriteLine("RECV: " + e.Text);
        }

        private void XmppClient_OnError(object sender, Matrix.ExceptionEventArgs e)
        {
            Debug.WriteLine("XmppClient_OnError " + e.Exception.Message);
        }

        private void XmppClient_OnAuthError(object sender, Matrix.Xmpp.Sasl.SaslEventArgs e)
        {
            Debug.WriteLine("XmppClient_OnAuthError " + e.Exception.Message);
        }

        private void XmppClient_OnLogin(object sender, Matrix.EventArgs e)
        {
            Debug.WriteLine("XmppClient_OnLogin");
        }

        private void XmppClient_OnBindError(object sender, IqEventArgs e)
        {
            Debug.WriteLine("XmppClient_OnBindError");
        }

        private void XmppClient_OnBind(object sender, Matrix.JidEventArgs e)
        {
            Debug.WriteLine("XmppClient_OnBind");
        }

        private async void XmppClient_OnMessage(object sender, MessageEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                txtRece.Text += e.Message.Body + "\r\n";
            });
        }

        private void XmppClient_OnRosterEnd(object sender, Matrix.EventArgs e)
        {
            Debug.WriteLine("XmppClient_OnRosterEnd");
            xmppClient.Send(new Message()
            {
                To = "zl@jabber.org",
                Type = MessageType.Chat,
                Body = "hello world"
            });
        }
    }
}
