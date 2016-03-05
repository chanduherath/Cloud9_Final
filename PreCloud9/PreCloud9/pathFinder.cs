using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PreCloud9
{
    class pathFinder
    {
        //List<Node> graph;
        Node[,] nodeMap;
        Queue<Node> discovered;

        public pathFinder()
        {
            nodeMap = new Node[10,10];            
            discovered = new Queue<Node>();
            createGraph();
        }

        private void createGraph()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Node n = new Node();
                    nodeMap[i, j] = n;
                }
            }
        }

        public void initializeGraph(String[,] map){
            for(int i=0; i<10 ;i++){
                for(int j=0; j<10 ; j++){
                    nodeMap[i,j].Color = "WHITE";
                    nodeMap[i,j].Distance = int.MaxValue;
                    nodeMap[i, j].Parent = null;
                    nodeMap[i,j].Contain = map[i,j];
                    nodeMap[i, j].Xcod = i;
                    nodeMap[i, j].Ycod = j;
                    nodeMap[i, j].Dir = 4;
                }
            }
        }

        public Stack<Node> findPath(Node n){
            if (!(n == null)) {
                Stack<Node> nodePath = new Stack<Node>();
                nodePath.Push(n);
                Node temp = n;
                while (!(temp.Parent == null))
                {
                    temp = temp.Parent;
                    nodePath.Push(temp);
                }
                //Console.WriteLine("here is the path");
                //foreach (Node p in nodePath)
                //{
                //    Console.WriteLine("nodePath X : " + p.Xcod + " nodePath Y : " + p.Ycod);
                //}
                return nodePath;
            }
            else
            {
                return null;
            }
                        
        }

        public List<int> getDirections(Stack<Node> st)
        {
            if (!(st == null))
            {
                st.Pop();
                List<int> directions = new List<int>();
                while (st.Count > 0)
                {
                    Node n = st.Pop();
                    directions.Add(n.Dir);
                }
                return directions;
            }
            else
            {
                return null;
            }
            
        }
       
        public Node getNearestCoin(Tank t1)
        {
            Node tankNode = this.nodeMap[t1.Ycod, t1.Xcod];
            tankNode.Color = "GRAY";
            tankNode.Distance = 0;
            tankNode.Parent = null;
            discovered.Enqueue(tankNode);

            while (!(discovered.Count == 0))
            {
                Node u = discovered.Dequeue();
                if (u.Contain.Equals("C"))
                {
                    return u;                    
                }
                List<Node> neigh = getAllNeighbours(u);
               
                for (int i = 0; i < neigh.Count; i++)
                {
                    neigh[i].Color = "GRAY";
                    neigh[i].Distance = u.Distance + 1;
                    neigh[i].Parent = u;
                    discovered.Enqueue(neigh[i]);
                }
                u.Color = "BLACK";
            }

            return null;
        }

        public Node getNearestLifePack(Tank t1)
        {
            Node tankNode = this.nodeMap[t1.Ycod, t1.Xcod];
            tankNode.Color = "GRAY";
            tankNode.Distance = 0;
            tankNode.Parent = null;
            discovered.Enqueue(tankNode);

            while (!(discovered.Count == 0))
            {
                Node u = discovered.Dequeue();
                if (u.Contain.Equals("L"))
                {
                    return u;
                }
                List<Node> neigh = getAllNeighbours(u);

                for (int i = 0; i < neigh.Count; i++)
                {
                    neigh[i].Color = "GRAY";
                    neigh[i].Distance = u.Distance + 1;
                    neigh[i].Parent = u;
                    discovered.Enqueue(neigh[i]);
                }
                u.Color = "BLACK";
            }

            return null;
        }


        private List<Node> getAllNeighbours(Node n)
        {
            List<Node> neigh = new List<Node>();
            if (n.Xcod - 1 >= 0)
            {
                if (nodeMap[n.Xcod - 1, n.Ycod].Color.Equals("WHITE") && !(nodeMap[n.Xcod - 1, n.Ycod].Contain.Equals("S") || nodeMap[n.Xcod - 1, n.Ycod].Contain.Equals("B") || nodeMap[n.Xcod - 1, n.Ycod].Contain.Equals("W")))
                {
                    nodeMap[n.Xcod - 1, n.Ycod].Dir = 0;
                    neigh.Add(nodeMap[n.Xcod-1,n.Ycod]);
                }
            }

            if (n.Ycod - 1 >= 0)
            {
                if (nodeMap[n.Xcod, n.Ycod - 1].Color.Equals("WHITE") && !(nodeMap[n.Xcod, n.Ycod - 1].Contain.Equals("S") || nodeMap[n.Xcod, n.Ycod - 1].Contain.Equals("B") || nodeMap[n.Xcod, n.Ycod - 1].Contain.Equals("W")))
                {
                    nodeMap[n.Xcod, n.Ycod - 1].Dir = 3;
                    neigh.Add(nodeMap[n.Xcod, n.Ycod - 1]);
                }
               
            }
            if (n.Xcod + 1 <= 9)
            {
                if (nodeMap[n.Xcod + 1, n.Ycod].Color.Equals("WHITE") && !(nodeMap[n.Xcod + 1, n.Ycod].Contain.Equals("S") || nodeMap[n.Xcod + 1, n.Ycod].Contain.Equals("B") || nodeMap[n.Xcod + 1, n.Ycod].Contain.Equals("W")))
                {
                    nodeMap[n.Xcod + 1, n.Ycod].Dir = 2;
                    neigh.Add(nodeMap[n.Xcod + 1, n.Ycod]);
                }
               
            }
            if (n.Ycod + 1 <= 9)
            {
                if (nodeMap[n.Xcod, n.Ycod + 1].Color.Equals("WHITE") && !(nodeMap[n.Xcod, n.Ycod + 1].Contain.Equals("S") || nodeMap[n.Xcod, n.Ycod + 1].Contain.Equals("B") || nodeMap[n.Xcod, n.Ycod + 1].Contain.Equals("W")))
                {
                    nodeMap[n.Xcod, n.Ycod + 1].Dir = 1;
                    neigh.Add(nodeMap[n.Xcod, n.Ycod + 1]);
                }
                
            }
            return neigh;
        }

        private void addNeighbousToQueue(Node n)
        {
            List<Node> allneigh = getAllNeighbours(n);
            List<Node> addToTheQueue = new List<Node>();
            //List<Node> toBeVisited = tobeVisited.ToList();
            for(int i=0; i<allneigh.Count ;i++){
                foreach(var j in discovered)
                {
                    if (!(allneigh[i].Xcod == j.Xcod && allneigh[i].Ycod == j.Ycod))
                    {
                        addToTheQueue.Add(allneigh[i]);
                    }
                }
            }
            
        }

       
    }
}
