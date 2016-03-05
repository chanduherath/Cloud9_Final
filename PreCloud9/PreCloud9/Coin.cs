using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GameStructure
{
    class Coin
    {
        private int xcod;
        private int ycod;
        private int lifetime;
        private int val;
        private bool state;

        

        public Coin()
        {
            this.state = true;
            this.xcod = 0;
            this.ycod = 0;
            this.lifetime = 0;
            this.val = 0;
        }

        public bool State
        {
            get { return state; }
            set { state = value; }
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
       
        public int Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }
       
        public int Val
        {
            get { return val; }
            set { val = value; }
        }

        public void startTimer(int miseconds)
        {
            System.Timers.Timer tm = new System.Timers.Timer();
            tm.Interval = miseconds;
            tm.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            tm.Enabled = true;
            tm.Start();          
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.State = false;
            var timer = (Timer)source;
            timer.Stop();
        }
    }
}
