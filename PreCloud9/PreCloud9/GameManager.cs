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
        public static bool AI_State = false;
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
                    List<int> directionsC = AI.getDirections(stC);

                    AI = new pathFinder();
                    AI.initializeGraph(gEngine.map);
                    Node nearestL = AI.getNearestLifePack(gEngine.myTank);
                    Stack<Node> stL = AI.findPath(nearestL);
                    List<int> directionsL = AI.getDirections(stL);

                    if (gEngine.myTank.Health>60)
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

        public void sendCommands(List<int> dir, Tank myTank)
        {
            //for (int i = 0; i < dir.Count; i++)
            //{
            try
            {
                if (myTank.Direction == dir[0])
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
                    switch (dir[0])
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
    }
}
