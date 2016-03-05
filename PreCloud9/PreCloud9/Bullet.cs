using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreCloud9
{
    class Bullet
    {
        private bool state;
        private int angel;
        private int xcod;
        private int ycod;

        public Bullet()
        {
            this.State = false;
            this.Angel = 0;
            this.Xcod = 0;
            this.Ycod = 0;
        }

        public bool State
        {
            get { return state; }
            set { state = value; }
        }

        public int Angel
        {
            get { return angel; }
            set { angel = value; }
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
    }
}
