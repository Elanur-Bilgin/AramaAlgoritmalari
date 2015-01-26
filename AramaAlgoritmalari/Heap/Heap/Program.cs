using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heap
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Heap theHeap = new Heap(10); 
            theHeap.insert(70); 
            theHeap.insert(40);
            theHeap.insert(50);
            theHeap.insert(20);
            theHeap.insert(60);
            theHeap.insert(100);
            theHeap.insert(80);
            theHeap.insert(30);
            theHeap.insert(10);
            theHeap.insert(90);
            Console.WriteLine("Heap' den eklediğimiz 10 elemandan silinen 5 eleman:");
        
            Console.WriteLine("**********************************************************");
              for(int i=0;i<5;i++)
                Console.WriteLine(theHeap.remove().getKey());
            Console.ReadLine();
        }
    }
    class Node
    {
        private int iData;
        public Node(int key)
        { iData = key; }
        // -------------------------------------------------------------
        public int getKey()
        { return iData; }
        // -------------------------------------------------------------
        public void setKey(int id)
        { iData = id; }
        // -------------------------------------------------------------
    }
    class Heap
    {
        private Node[] heapArray;
        private int maxSize; // size of array
        private int currentSize; // number of nodes in array
        // -------------------------------------------------------------
        public Heap(int mx) // constructor
        {
            maxSize = mx;
            currentSize = 0;
            heapArray = new Node[maxSize]; // create array
        }
        // -------------------------------------------------------------
        public bool isEmpty()
        { return currentSize == 0; }
        // -------------------------------------------------------------
        public bool insert(int key)
        {
            if (currentSize == maxSize)
                return false;
            Node newNode = new Node(key);
            heapArray[currentSize] = newNode;
            trickleUp(currentSize++);
            return true;
        }
        public void trickleUp(int index)
        {
            int parent = (index - 1) / 2;
            Node bottom = heapArray[index];
            while (index > 0 && heapArray[parent].getKey() > bottom.getKey())
            {
                heapArray[index] = heapArray[parent]; // move it down
                index = parent;
                parent = (parent - 1) / 2;
            } // end while
            heapArray[index] = bottom;
        }
        public Node remove() // delete item with max key
        {
            Node root = heapArray[0];
            heapArray[0] = heapArray[--currentSize];
            trickleDown(0);
            return root;
        }
        public void trickleDown(int index)
        {
            int largerChild;
            Node top = heapArray[index]; // save root
            while (index < currentSize / 2) // while node has at
            { 
                int leftChild = 2 * index + 1;
                int rightChild = leftChild + 1;
             
                if (rightChild < currentSize && heapArray[leftChild].getKey() > heapArray[rightChild].getKey())
                    largerChild = rightChild;
                else
                    largerChild = leftChild;
                
                if (top.getKey() <= heapArray[largerChild].getKey())
                    break;
                // shift child up
                heapArray[index] = heapArray[largerChild];
                index = largerChild; // go down
            }
            heapArray[index] = top;
        }
       
    }
}
