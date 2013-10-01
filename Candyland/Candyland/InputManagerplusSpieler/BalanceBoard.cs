using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MediBalancePro;
using System.Drawing;




namespace Candyland
{
    class BalanceBoard
    {
        DeviceManager medibalancepro;
        static IntPtr windowHandle;
        bool connected = false;
        PointF currentFilteredVal;

        public BalanceBoard() {

            medibalancepro = new DeviceManager();
            medibalancepro.Connect(windowHandle);
            medibalancepro.DataFilter = new DF_DepthPass(20);
            medibalancepro.startCalibrate();
            medibalancepro.NewData += new DeviceManager.NewDataEventHandler(newData);
            medibalancepro.DeviceRemoved += new EventHandler(deviceRemoved);
            connected = true;
            currentFilteredVal = PointF.Empty;
            
        }

        void deviceRemoved(object sender, EventArgs e)
        {
            connected = false;  
        }

        void newData(object sender, NewDataEventArgs args)
        {

            if (args.FilteredDataPresent){
                currentFilteredVal = args.FilteredData;
               // Console.WriteLine(args.FilteredData.ToString());
        }
            else
                currentFilteredVal = PointF.Empty;
        }

        

        public BalanceBoardState getState(){

            if (!connected) return new BalanceBoardState(0, 0, false);

            PointF normedPoint = medibalancepro.normalizePoint(currentFilteredVal, 20.0f, medibalancepro.HardwareBounds);
            PointF normedorigin = medibalancepro.normalizePoint(medibalancepro.Origin , 20.0f, medibalancepro.HardwareBounds);

            float normedx = normedorigin.X - normedPoint.X;
            float normedy =  normedorigin.Y - normedPoint.Y;
       

            return new BalanceBoardState(normedorigin.X - normedPoint.X, normedorigin.Y - normedPoint.Y, connected);
        }

        public void wndproc(ref System.Windows.Forms.Message mes)
        {
            medibalancepro.wndproc(mes);
        }


        public static void initialize(IntPtr ptr){
            windowHandle = ptr;
        }

    }
}
