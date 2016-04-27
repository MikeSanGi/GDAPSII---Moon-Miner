using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonMinerExecutable
{
    class ScoreLinkedList
    {
        // Attributes \\
        private Node head = null;
        private int count = 0;

        // Add Method \\
        public void Add(string nm, int scr)
        {
            Node newNode = new Node(nm, scr);

            //If nothing in the list
            if (head == null)
            {
                head = newNode;
                count++;
                return;
            }

            //If list is already populated
            Node current = head;
            Node link = null;
            while (current.Next != null)
            {
                if (newNode.Score < current.Score)
                {
                    current = current.Next;
                }
                if (newNode.Score >= current.Next.Score)
                {
                    link = current.Next;
                    newNode = current.Next;
                    newNode.Next = link;
                    count++;
                    return;
                }
            }
            current.Next = newNode; // Puts this at the end of the list if it doesn't get caught
            count++;
        }
    }
}
