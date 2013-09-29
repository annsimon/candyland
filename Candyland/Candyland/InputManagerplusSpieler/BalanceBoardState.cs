using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Candyland
{
    class BalanceBoardState 
    {
        public float X {get;private  set;}
        public float Y {get;private set;}
        public bool isConnected {get;private set;}

        public BalanceBoardState(float x, float y, bool isConnected){
            this.X = x;
            this.Y = y;
            this.isConnected = isConnected;
        }

        public static bool operator != (BalanceBoardState left, BalanceBoardState right)
        {
            return !((left.X == right.X) && (left.Y == right.Y) && (left.isConnected == right.isConnected));
        }

        public static bool operator == (BalanceBoardState left, BalanceBoardState right)
        {
            return ((left.X == right.X) && (left.Y == right.Y) && (left.isConnected == right.isConnected));
        }

        public override bool Equals(object o)
        {

            if (!(o is BalanceBoardState)) return false;

            return ((this.X == ((BalanceBoardState)o).X)
                &&  (this.Y == ((BalanceBoardState)o).Y)
                &&  (this.isConnected == ((BalanceBoardState)o).isConnected));
        }

        public String toString() {
            String str = "";

            str += "Boardstate(X: " + X + ", Y: " + Y + ", Connected: " + isConnected;
            return str;
        }
    }
}
