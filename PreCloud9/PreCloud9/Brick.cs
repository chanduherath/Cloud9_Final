using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreCloud9
{
    class Brick
    {

        private int xcod;
        private int ycod;     
        private int damageLevel;

        public Brick()
        {
            this.xcod = 0;
            this.ycod = 0;
            this.damageLevel = 4;
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

        public int DamageLevel
        {
            get { return damageLevel; }
            set { damageLevel = value; }
        }
    }
}
