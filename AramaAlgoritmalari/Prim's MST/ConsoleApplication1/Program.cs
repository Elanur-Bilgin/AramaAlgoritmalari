using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prims_MST
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph theGraph = new Graph();
            theGraph.addVertex('A');    // 0  (start for mst)
            theGraph.addVertex('B');    // 1
            theGraph.addVertex('C');    // 2
            theGraph.addVertex('D');    // 3
            theGraph.addVertex('E');    // 4
            theGraph.addVertex('F');    // 5

            theGraph.addEdge(0, 1, 6);  // AB  6
            theGraph.addEdge(0, 3, 4);  // AD  4
            theGraph.addEdge(1, 2, 10); // BC 10
            theGraph.addEdge(1, 3, 7);  // BD  7
            theGraph.addEdge(1, 4, 7);  // BE  7
            theGraph.addEdge(2, 3, 8);  // CD  8
            theGraph.addEdge(2, 4, 5);  // CE  5
            theGraph.addEdge(2, 5, 6);  // CF  6
            theGraph.addEdge(3, 4, 12); // DE 12
            theGraph.addEdge(4, 5, 7);  // EF  7

            Console.WriteLine("Minimum spanning tree: ");
            theGraph.mstw();            // minimum spanning tree
            Console.WriteLine();
            Console.ReadLine();
        }
    }
    class Edge
    {
        public int srcVert;   // index of a vertex starting edge
        public int destVert;  // index of a vertex ending edge
        public int distance;  // distance from src to dest
        // -------------------------------------------------------------
        public Edge(int sv, int dv, int d)  // constructor
        {
            srcVert = sv;
            destVert = dv;
            distance = d;
        }
        // -------------------------------------------------------------
    }  


    class PriorityQ
    {    
        // array in sorted order, from max at 0 to min at size-1
        private int SIZE = 20;
        private Edge[] queArray;
        private int sze;
        // -------------------------------------------------------------
        public PriorityQ()            // constructor
        {
            queArray = new Edge[SIZE];
            sze = 0;
        }
        // -------------------------------------------------------------
        public void insert(Edge item)  // insert item in sorted order
        {
            int j;

            for (j = 0; j < sze; j++)           // find place to insert
                if (item.distance >= queArray[j].distance)
                    break;

            for (int k = sze - 1; k >= j; k--)    // move items up
                queArray[k + 1] = queArray[k];

            queArray[j] = item;             // insert item
            sze++;
        }
        // -------------------------------------------------------------
        public Edge removeMin()            // remove minimum item
        { return queArray[--sze]; }
        // -------------------------------------------------------------
        public void removeN(int n)         // remove item at n
        {
            for (int j = n; j < sze - 1; j++)     // move items down
                queArray[j] = queArray[j + 1];
            sze--;
        }
        // -------------------------------------------------------------
        public Edge peekMin()          // peek at minimum item
        { return queArray[sze - 1]; }
        // -------------------------------------------------------------
        public int size()              // return number of items
        { return sze; }
        // -------------------------------------------------------------
        public bool isEmpty()      // true if queue is empty
        { return (sze == 0); }
        // -------------------------------------------------------------
        public Edge peekN(int n)      // peek at item n
        { return queArray[n]; }
        // -------------------------------------------------------------
        public int find(int findDex)  // find item with specified
        {                          // destVert value
            for (int j = 0; j < sze; j++)
                if (queArray[j].destVert == findDex)
                    return j;
            return -1;
        }
        // -------------------------------------------------------------
    }  

    class Vertex
    {
        public char label;       
        public bool gezildi;

        public Vertex(char lab)   // constructor
        {
            label = lab;
            gezildi = false;
        }
    }  

    class Graph
    {
        private int MAX_VERTS = 20;
        private int INFINITY = 1000000;
        private Vertex[] vertexList; // list of vertices
        private int[,] adjMat;      // adjacency matrix
        private int nVerts;          // current number of vertices
        private int currentVert;
        private PriorityQ thePQ;
        private int nTree;           // number of verts in tree
        // -------------------------------------------------------------
        public Graph()               // constructor
        {
            vertexList = new Vertex[MAX_VERTS];
            // adjacency matrix
            adjMat = new int[MAX_VERTS, MAX_VERTS];
            nVerts = 0;
            for (int j = 0; j < MAX_VERTS; j++)      // set adjacency
                for (int k = 0; k < MAX_VERTS; k++)   //    matrix to 0
                    adjMat[j, k] = INFINITY;
            thePQ = new PriorityQ();
        }  // end constructor
        // -------------------------------------------------------------
        public void addVertex(char lab)
        {
            vertexList[nVerts++] = new Vertex(lab);
        }
        // -------------------------------------------------------------
        public void addEdge(int start, int end, int weight)
        {
            adjMat[start, end] = weight;
            adjMat[end, start] = weight;
        }
        // -------------------------------------------------------------
        //public void displayVertex(int v)
        //{
        //    Console.Write(vertexList[v].label);
        //}
        // -------------------------------------------------------------
        public void mstw()           // minimum spanning tree
        {
            currentVert = 0;         

            while (nTree < nVerts - 1)   // while not all verts in tree
            {                     
                vertexList[currentVert].gezildi = true;
                nTree++;

                for (int j = 0; j < nVerts; j++)  
                {
                    if (j == currentVert)         // skip if it's us
                        continue;
                    if (vertexList[j].gezildi) // skip if in the tree
                        continue;
                    int distance = adjMat[currentVert, j];
                    if (distance == INFINITY)  // skip if no edge
                        continue;
                    putInPQ(j, distance);      
                }
                if (thePQ.size() == 0)      
                {
                    Console.WriteLine(" GRAPH NOT CONNECTED");
                    return;
                }
                
                Edge theEdge = thePQ.removeMin();
                int sourceVert = theEdge.srcVert;
                currentVert = theEdge.destVert;

                Console.Write(vertexList[sourceVert].label);
                Console.Write(vertexList[currentVert].label);
                Console.Write(" ");
            }  

            for (int j = 0; j < nVerts; j++) 
                vertexList[j].gezildi = false;
        } 

        public void putInPQ(int newVert, int newDist)
        {
            // is there another edge with the same destination vertex?
            int queueIndex = thePQ.find(newVert);
            if (queueIndex != -1)              // got edge's index
            {
                Edge tempEdge = thePQ.peekN(queueIndex);  // get edge
                int oldDist = tempEdge.distance;
                if (oldDist > newDist)          // if new edge shorter,
                {
                    thePQ.removeN(queueIndex);  // remove old edge
                    Edge theEdge = new Edge(currentVert, newVert, newDist);
                    thePQ.insert(theEdge);      // insert new edge
                }
            }
            else
            {
                Edge theEdge = new Edge(currentVert, newVert, newDist);
                thePQ.insert(theEdge);
            }
        }
    }
}
