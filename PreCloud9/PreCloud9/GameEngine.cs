using PreCloud9;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace GameStructure
{
    class GameEngine
    {
        public static TcpListener listener;
        public static NetworkStream incommingStream;
        private Parser p;
        public Connection con;

        List<String[]> mapList = new List<String[]>();
        public String[,] map;
        public int gridSize;
        public List<Coin> coinList;
        public List<LifePack> lifePackList;
        public List<Tank> tankList;
        public List<Brick> brickList;
        public Tank myTank;
        public Bullet blt;
    

        public GameEngine()
        {
            this.p = new Parser();
            this.con = new Connection();
            this.blt = new Bullet();
            myTank = new Tank();
            this.gridSize = 10;
            initializeMap();
            Thread listenThread = new Thread(listentoServer);
            listenThread.Start();
            coinList = new List<Coin>();
            lifePackList = new List<LifePack>();
            tankList = new List<Tank>();
            brickList = new List<Brick>();
            coinObserver();
            lifePackObserver();
        }

        private void initializeMap()
        {
            map = new String[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    map[i, j] = "N";
                }
            }
        }

        public void callParserMethods(String str)
        {
            if (str.StartsWith("I"))
            {
                this.mapList = p.createMapList(str);
                this.myTank.PlayerName = p.getMyPlayerName(str);
                markOnMap(mapList);
                drawMap();
            }if(str.StartsWith("S")){
                myTank = p.getMydetails(str,myTank.PlayerName);
            }
            if (str.StartsWith("G"))
            {
                this.tankList = p.getTankList(str);
                this.myTank = updateMyTankDetails();
                this.brickList = p.getBrickList(str);
            }
            if (str.StartsWith("L"))
            {
                LifePack lf = p.createLifePack(str);
                Console.WriteLine("Before marking on map");
                markLifePackOnMap(lf, map);
                lifePackList.Add(lf);
                lf.startTimer(lf.LifeTime);
                
            } if (str.StartsWith("C:"))
            {
                Coin coin = p.createCoin(str);
                markCoinOnMap(coin, map);        
                coinList.Add(coin);
                coin.startTimer(coin.Lifetime);
            }
        }

        public void markOnMap(List<String[]> maplist)
        {
            for (int i = 0; i < maplist.Count; i++)
            {
                String[] tempString = maplist[i];
                if (i == 0)//Bricks
                {
                    for (int j = 0; j < tempString.Length; j += 2)
                    {
                        map[Convert.ToInt32(tempString[j + 1]), Convert.ToInt32(tempString[j])] = "B";
                    }
                }
                if (i == 1)//Stones
                {
                    for (int j = 0; j < tempString.Length; j += 2)
                    {
                        map[Convert.ToInt32(tempString[j + 1]), Convert.ToInt32(tempString[j])] = "S";
                    }
                }
                if (i == 2)//Water
                {
                    for (int j = 0; j < tempString.Length; j += 2)
                    {
                        map[Convert.ToInt32(tempString[j + 1]), Convert.ToInt32(tempString[j])] = "W";
                    }
                }

            }
        }

        public void drawMap()
        {
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Console.Write(map[i, j] + " ");
                }
                Console.WriteLine();
            }

        }

        public void listentoServer()
        {
            try
            {
                //listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000);
                listener = new TcpListener(IPAddress.Any, 7000);
                listener.Start();

                while (true)
                {
                    TcpClient cln = listener.AcceptTcpClient();
                    incommingStream = cln.GetStream();
                    byte[] bytesToRead = new byte[cln.ReceiveBufferSize];
                    int bytesRead = incommingStream.Read(bytesToRead, 0, cln.ReceiveBufferSize);
                    Console.WriteLine(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                    this.callParserMethods(Encoding.ASCII.GetString(bytesToRead, 0, bytesRead));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void markLifePackOnMap(LifePack lf,String[,] tempmap)
        {
            tempmap[lf.Ycod, lf.Xcod] = "L";
            Console.WriteLine("After creating a lifepack.. ");
            //drawMap();
            this.map = tempmap;
        }

        public void markCoinOnMap(Coin coin, String[,] tempmap)
        {
            tempmap[coin.Ycod, coin.Xcod] = "C";
            Console.WriteLine("After creating a Coin.. ");
            //drawMap();
            this.map = tempmap;
            
        }

        public void coinObserver()
        {
            Thread thread = new Thread(new ThreadStart(workThreadMethod));
            thread.Start();
        }

        public void lifePackObserver()
        {
            Thread thread = new Thread(new ThreadStart(detectLifePacks));
            thread.Start();
        }

        public void detectLifePacks()
        {
            while (true)
            {
                try
                {
                    if (lifePackList.Count > 0)
                    {
                        for (int i = 0; i < lifePackList.Count; i++)
                        {
                            if (lifePackList[i].State == false)
                            {
                                removeLifePackFromMap(lifePackList[i], this.map);
                                lifePackList.Remove(lifePackList[i]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception occured in lifePack Thread");
                }
            }
        }

        public void workThreadMethod()
        {
            while (true)
            {
                try
                {
                    if (coinList.Count > 0)
                    {
                        for (int i = 0; i < coinList.Count; i++)
                        {
                            if (coinList[i].State == false)
                            {
                                removeCoinFromMap(coinList[i], this.map);
                                coinList.Remove(coinList[i]);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An Exception occured in Coin Thread");
                }
            }
        }

        public void removeCoinFromMap(Coin coin, string[,] tempmap)
        {
            tempmap[coin.Ycod, coin.Xcod] = "N";
            Console.WriteLine(coin.Ycod + " " + coin.Xcod + " " + "removed form string map");
            this.map = tempmap;
        }

        public void removeLifePackFromMap(LifePack lf, string[,] tempmap)
        {
            tempmap[lf.Ycod, lf.Xcod] = "N";
            Console.WriteLine(lf.Ycod + " " + lf.Xcod + " " + "removed form string map");
            this.map = tempmap;
        }

        private Tank updateMyTankDetails()
        {
            Tank myTnk = this.myTank;
            List<Tank> tlist = this.tankList;
            for (int i = 0; i < tlist.Count; i++)
            {
                if (myTnk.PlayerName.Equals(tlist[i].PlayerName))
                {
                    myTnk.Xcod = tlist[i].Xcod;
                    myTnk.Ycod = tlist[i].Ycod;
                    myTnk.Direction = tlist[i].Direction;
                    myTnk.Whether_shot = tlist[i].Whether_shot;
                    myTnk.Health = tlist[i].Health;
                    myTnk.Coins = tlist[i].Coins;
                    myTnk.Points = tlist[i].Points;

                    this.blt.Angel = myTank.Direction;// give same direction for the bullet as well.
                }
            }
            return myTnk;
        }
    }
}
