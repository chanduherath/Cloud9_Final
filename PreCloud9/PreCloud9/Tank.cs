using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreCloud9
{
    class Tank
    {
        String playerName;
        int xcod;
        int ycod;
        int direction;
        int whether_shot;

       
        int health;

        int coins;

        int points;

        
        public Tank()
        {
            this.playerName = "";
            this.Xcod = 0;
            this.Ycod = 0;
            this.Direction = 0;
            this.Whether_shot = 0;
            this.Health = 0;
            this.Coins = 0;
            this.Points = 0;
        }


        public String PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
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


        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public int Whether_shot
        {
            get { return whether_shot; }
            set { whether_shot = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int Coins
        {
            get { return coins; }
            set { coins = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        
    }
}
