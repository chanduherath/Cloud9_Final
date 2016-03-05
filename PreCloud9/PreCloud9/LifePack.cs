using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GameStructure
{
    class LifePack
    {
        private int xcod;
        private int ycod;
        private int lifeTime;
        private bool state;
  
        public LifePack()
        {
            this.xcod = 0;
            this.ycod = 0;
            this.lifeTime = 0;
            this.state = true;
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

        public int LifeTime
        {
            get { return lifeTime; }
            set { lifeTime = value; }
        }

        public bool State
        {
            get { return state; }
            set { state = value; }
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
