using PreCloud9;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameStructure
{
    class Parser
    {
        

        public LifePack createLifePack(String str)
        {
            LifePack lf = new LifePack();
            char[] delimiters = new char[] { ':', '#', ',' };
            string[] arr = str.Split(delimiters);

            lf.Xcod = Convert.ToInt32(arr[1]);
            lf.Ycod = Convert.ToInt32(arr[2]);
            lf.LifeTime = Convert.ToInt32(arr[3]);

            return lf;
        }

        public Coin createCoin(String str)
        {
            Coin coin = new Coin();
            char[] delimiters = new char[] { ':', '#', ',' };
            string[] arr = str.Split(delimiters);

            coin.Xcod = Convert.ToInt32(arr[1]);
            coin.Ycod = Convert.ToInt32(arr[2]);
            coin.Lifetime = Convert.ToInt32(arr[3]);
            coin.Val = Convert.ToInt32(arr[4]);

            return coin;
        }

        public List<String []> createMapList(String str)
        {
            List<String []> mapList = new List<String []>();
            char[] predelimiters = new char[] { ':', '#' };
            char[] postdelimiters = new char[] { ',', ';' };
            string[] arr = str.Split(predelimiters);

            for (int i = 2; i < arr.Length; i++)
            {
                String[] subArray = arr[i].Split(postdelimiters);
                mapList.Add(subArray);
            }
            return mapList;
        }

        public String getMyPlayerName(String str)
        {
            return str.Substring(2, 2);
        }

        public Tank getMydetails(String str,String name)
        {
            Tank myTank = new Tank();
            char[] predelimiters = new char[] { ':', '#' };
            char[] postdelimiters = new char[] { ',', ';' };
            string[] arr = str.Split(predelimiters);
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].StartsWith(name))
                {
                    string[] arr1 = arr[i].Split(postdelimiters);
                    myTank.Xcod = Int32.Parse(arr1[1]);
                    myTank.Ycod = Int32.Parse(arr1[2]);
                    myTank.Direction = Int32.Parse(arr1[3]);
                }
            }
            myTank.PlayerName = name;
            return myTank;
        }

        private Tank getTankDetails(String str)
        {
            Tank tnk = new Tank();
            //Console.WriteLine("One string related to a one tank : " + str);
            char[] delimiters = new char[] { ',', ';' };
            string[] arr = str.Split(delimiters);
            tnk.PlayerName = arr[0];
            tnk.Xcod = Int32.Parse(arr[1]);
            tnk.Ycod = Int32.Parse(arr[2]);
            tnk.Direction = Int32.Parse(arr[3]);
            tnk.Whether_shot = Int32.Parse(arr[4]);
            tnk.Health = Int32.Parse(arr[5]);
            tnk.Coins = Int32.Parse(arr[6]);
            tnk.Points = Int32.Parse(arr[7]);
            return tnk;
        }

        

        public List<Tank> getTankList(String str)
        {
            List<Tank> tanklist = new List<Tank>();
            //str = "G:P0;0,0;1;0;100;0;0:P1;0,9;1;0;100;0;0:P2;9,0;3;0;100;0;0:P3;9,9;0;0;100;0;0:8,6,0;9,3,0;1,7,0;7,1,0;6,8,0#";
            char[] predelimiters = new char[] { ':', '#' };
            string[] arr = str.Split(predelimiters);
            for (int i = 1; i < arr.Length - 2; i++)
            {
                
                Tank tnk = getTankDetails(arr[i]);
                tanklist.Add(tnk);
            }
            Console.WriteLine("Number of tanks : " + tanklist.Count);
            return tanklist;
        }

        public List<Brick> getBrickList(String str)
        {
            List<Brick> brickList = new List<Brick>();
            char[] predelimeters = new char[] { ':', '#' };
            string[] arr = str.Split(predelimeters);
            String brickString = arr[arr.Length - 2];

            char[] postdelimeters = new char[] { ';' };
            String[] brickarr = brickString.Split(postdelimeters);
            for (int i = 0; i < brickarr.Length; i++)
            {
                brickList.Add(getBrickDetails(brickarr[i]));
            }
            return brickList;
        }

        private Brick getBrickDetails(String str)
        {
            Brick br = new Brick();
            char[] delimeters = new char[] { ',' };
            string[] arr = str.Split(delimeters);
            br.Ycod = Int32.Parse(arr[0]);
            br.Xcod = Int32.Parse(arr[1]);
            br.DamageLevel = Int32.Parse(arr[2]);
            return br;
        }


    }
}
