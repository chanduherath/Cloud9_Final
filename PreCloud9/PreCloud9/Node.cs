using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreCloud9
{
    class Node
    {

        String contain;
        Node parent;
        String color;
        int distance;
        int xcod;
        int ycod;
        int dir;

        public String Contain
        {
            get { return contain; }
            set { contain = value; }
        }
        

        internal Node Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        

        public String Color
        {
            get { return color; }
            set { color = value; }
        }
        

        public int Distance
        {
            get { return distance; }
            set { distance = value; }
        }
        

        public int Xcod
        {
            get { return xcod; }
            set { xcod = value; }
        }
        

        public int Ycod
        {
            get { return ycod; }
            set { ycod = value; }
        }
        

        public int Dir
        {
            get { return dir; }
            set { dir = value; }
        }
    }
}
