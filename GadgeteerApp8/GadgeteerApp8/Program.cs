using Gadgeteer.Modules.GHIElectronics;
using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;
using Microsoft.SPOT.Net;   
using Microsoft.SPOT.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using System.Text;

namespace GadgeteerApp8
{
    public partial class Program : Gadgeteer.Program
    {



        // This method is run when the mainboard is powered up or reset.   
        Socket s;
        byte[] HTML = Encoding.UTF8.GetBytes(
"<html><body>" +
"<h1>Hosted on .NET Gadgeteer</h1>" +
"<p>Lets scare someone!</p>" +
"<form action=\"\" method=\"post\">" +
"<input type=\"submit\" value=\"Take a photo!\">" +
"</form>" +
"</body></html>");


        public void ProgramStarted()
        {
            
            camera.PictureCaptured += new Camera.PictureCapturedEventHandler(Myfunc);
            ethernetJ11D.UseThisNetworkInterface();
            string[] dns = { "8.8.8.8", "8.8.4.4" };
            ethernetJ11D.UseStaticIP("192.168.1.2", "255.255.255.0", "192.168.1.1", dns);
            ethernetJ11D.NetworkUp += NetUpH;
            ethernetJ11D.NetworkDown += NetDownH;

            //s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //EndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.1.1"), 1500);
            //s.Connect(ep);

           



            Thread.Sleep(4000);


            //  timer.Tick += new GT.Timer.TickEventHandler(timerFunc);
            //timer.Start();


          //  new Thread(RunWebServer).Start();


        }
        public void Myfunc(Camera sender, GT.Picture e)
        {
            //timer.Stop();
        //displayT35.SimpleGraphics.DisplayImage(e, 0, 0);
          /*       Bitmap bmap = e.MakeBitmap();
            int Wcnt = 0, Bcnt = 0; float Tcnt = 0;
            int Hstart = 0, Hstop = bmap.Height;
            int Wstart = 0, Wstop = bmap.Width;

            for (int i = Hstart; i < Hstop; i++)
            {
                for (int j = Wstart; j < Wstop; j++)
                {
                    Color c = bmap.GetPixel(j, i);
                    Tcnt++;
                    byte B = ColorUtility.GetBValue(c);
                    byte R = ColorUtility.GetRValue(c);
                    byte G = ColorUtility.GetGValue(c);
                    if (B >= 220 && R >= 220 && G >= 220)
                        Wcnt++;
                    if (B <= 50 && R <= 50 && G <= 50)
                        Bcnt++;

                }




            }
            double x = Hstop * Wstart;
            float Pwhite = Wcnt / Tcnt;
            float Pblack = Bcnt / Tcnt;

            if (Pwhite >= 0.4 && Pblack >= 0.4)
                displayT35.SimpleGraphics.DisplayText("Codice a barre rilevato\n", Resources.GetFont(Resources.FontResources.NinaB), Colors.Green, 10, 0);

            else {
                displayT35.SimpleGraphics.DisplayText(" B: " + Pblack.ToString() + " W: " + Pwhite.ToString() + "\n", Resources.GetFont(Resources.FontResources.NinaB), Colors.Green, 10, 0);
            */    displayT35.SimpleGraphics.DisplayImage(e, 0, 0);
            //}
           // timer.Start();
        }
        public void timerFunc(GT.Timer timer)
        {
            //  camera.TakePicture();


        }
        public void NetUpH(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            displayT35.SimpleGraphics.DisplayText("Net up" , Resources.GetFont(Resources.FontResources.NinaB), Colors.Green, 10, 0);
            camera.TakePicture();


        }
        public void NetDownH(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {

            displayT35.SimpleGraphics.DisplayText("Net down", Resources.GetFont(Resources.FontResources.NinaB), Colors.Green, 10, 0);
        }



        void RunWebServer()
        {



            // Wait for the network...
            while (ethernetJ11D.IsNetworkUp == false)
            {
                Debug.Print("Waiting...");
                Thread.Sleep(1000);
            }
            // Start the server
            WebServer.StartLocalServer(ethernetJ11D.NetworkSettings.IPAddress, 80);
            WebServer.DefaultEvent.WebEventReceived += DefaultEvent_WebEventReceived;
            while (true)
            {
                Thread.Sleep(1000);
            }
        }


        void DefaultEvent_WebEventReceived(string path, WebServer.HttpMethod method, Responder
        responder)
        {
        
            // We always send the same page back
            responder.Respond(HTML, "text/html;charset=utf-8");
            // If a button was clicked
            if (method == WebServer.HttpMethod.POST)
            {
                camera.TakePicture();
            }
        }

    }
}
