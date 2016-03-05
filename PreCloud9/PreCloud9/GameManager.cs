using PreCloud9;
using System;
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
    class GameManager
    {

        public GameEngine gEngine;
        public static bool AI_State = false; //this is used for switching between AI and manual mode
        public GameManager()
        {
            gEngine = new GameEngine();   
        }

        //public void automateGammingTimer()
        //{
        //    System.Timers.Timer tm = new System.Timers.Timer();
        //    tm.Interval = 4000;
        //    tm.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        //    tm.Enabled = true;
        //    tm.Start();    
        //}

        //private void OnTimedEvent(object source, ElapsedEventArgs e)
        //{          
        //    //var timer = (Timer)source;
        //    Console.WriteLine("Executing after a second..........................................................");
        //    //Controller cr = new Controller();
        //    pathFinder AI = new pathFinder();
        //    AI.initializeGraph(gEngine.map);
        //    Node nearest = AI.getNearestCoin(gEngine.myTank);
        //    Stack<Node> st = AI.findPath(nearest);
        //    List<int> directions = AI.getDirections(st);
        //    if (!(directions == null))
        //    {
        //        sendCommands(gEngine.con, directions, gEngine.myTank);
        //    }
            
        //}

        public void startAI()
        {
            Thread thread = new Thread(new ThreadStart(automateProcess));
            thread.Start();
        }

        public void automateProcess()
        {
            while (true)
            {
                try
                {
                    //Controller cr = new Controller();
                    pathFinder AI = new pathFinder();
                    AI.initializeGraph(gEngine.map);
                    Node nearestC = AI.getNearestCoin(gEngine.myTank);
                    Stack<Node> stC = AI.findPath(nearestC);
                    List<int> directionsC = AI.getDirections(stC);//get the direction list for nearest coin

                    AI = new pathFinder();
                    AI.initializeGraph(gEngine.map);
                    Node nearestL = AI.getNearestLifePack(gEngine.myTank);
                    Stack<Node> stL = AI.findPath(nearestL);
                    List<int> directionsL = AI.getDirections(stL);//get the direction list for nearest lifepack

                    if (gEngine.myTank.Health>60)//gives priority to find health packs if my health is redusing below 60
                    {
                        if (!(directionsC == null))
                        {
                            sendCommands(directionsC, gEngine.myTank);
                        }
                    }
                    else
                    {
                        if (!(directionsL == null))
                        {
                            sendCommands(directionsL, gEngine.myTank);
                        }
                    }

                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("An Exception occured in AI automate process method...");
                }
            }
        }

        public void sendCommands(List<int> dir, Tank myTank)//this method is sending moving commands for the AI part
        {
            //for (int i = 0; i < dir.Count; i++)
            //{
            try
            {
                if (myTank.Direction == dir[0])//If the current direction is already set for the required direction
                {
                    switch (dir[0])
                    {
                        case 0:
                            Console.WriteLine("chandu 0.............................");
                            gEngine.con.sendDatatoServer("UP#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 1:
                            Console.WriteLine("chandu 1.............................");
                            gEngine.con.sendDatatoServer("RIGHT#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 2:
                            Console.WriteLine("chandu 2.............................");
                            gEngine.con.sendDatatoServer("DOWN#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 3:
                            Console.WriteLine("chandu 3.............................");
                            gEngine.con.sendDatatoServer("LEFT#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        default:
                            Console.WriteLine("chandu def.............................");
                            Console.WriteLine("An Error occured in the path finding algorithm..");
                            break;
                    }
                }
                else
                {
                    switch (dir[0])//if the current direction is somthing other than required direction
                    {
                        case 0:
                            Console.WriteLine("chandu 2 0.............................");
                            gEngine.con.sendDatatoServer("UP#");
                            System.Threading.Thread.Sleep(1200);
                            gEngine.con.sendDatatoServer("UP#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 1:
                            Console.WriteLine("chandu 2 1.............................");
                            gEngine.con.sendDatatoServer("RIGHT#");
                            System.Threading.Thread.Sleep(1200);
                            gEngine.con.sendDatatoServer("RIGHT#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 2:
                            Console.WriteLine("chandu 2 2 .............................");
                            gEngine.con.sendDatatoServer("DOWN#");
                            System.Threading.Thread.Sleep(1200);
                            gEngine.con.sendDatatoServer("DOWN#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        case 3:
                            Console.WriteLine("chandu  2 3 .............................");
                            gEngine.con.sendDatatoServer("LEFT#");
                            System.Threading.Thread.Sleep(1200);
                            gEngine.con.sendDatatoServer("LEFT#");
                            System.Threading.Thread.Sleep(1200);
                            break;
                        default:
                            Console.WriteLine("chandu 2 default.............................");
                            Console.WriteLine("An Error occured in the path finding algorithm..: " + dir[0]);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("May be Firrst chance exception occured");
            }
                
            //}
        }


        private int[] getFrontCordinate(Tank tk)
        {
            int dir = tk.Direction;
            int[] arr = new int[2];
            switch (dir)
            {
                case 0:
                    arr[0] = tk.Ycod - 1;
                    arr[1] = tk.Xcod;
                    break;
                case 1:
                    arr[0] = tk.Ycod;
                    arr[1] = tk.Xcod + 1;
                    break;
                case 2:
                    arr[0] = tk.Ycod + 1;
                    arr[1] = tk.Xcod;
                    break;
                case 3:
                    arr[0] = tk.Ycod;
                    arr[1] = tk.Xcod - 1;
                    break;
                default:
                    Console.WriteLine("An error occured in finding the front cordinates");
                    break;
            }
            return arr;
        }
    }
}
