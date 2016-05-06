using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MoonMinerNew
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
            Node prev = head;

            while (current.Next != null)
            {
                if(newNode.Score > head.Score)
                {
                    Node temp = head;
                    newNode.Next = temp;
                    head = newNode;
                    count++;
                    return;
                }
                if (newNode.Score > current.Score)
                {
                    prev.Next = newNode;
                    newNode.Next = current;
                    count++;
                    return;
                }
                if(current != prev)
                {
                    prev = prev.Next;
                }
                current = current.Next;

                
            }
            current.Next = newNode; // Puts this at the end of the list if it doesn't get caught
            count++;
        }

        public void SaveScores()
        {
            StreamWriter sw = new StreamWriter("highscore.txt");
            Node temp = head;
            while(temp.Next != null)
            {
                sw.WriteLine(temp.Score);
                temp = temp.Next;
            }
            sw.Close();
        }

        public void SaveNames()
        {
            StreamWriter sw = new StreamWriter("highscoreNames.txt");
            Node temp = head;
            while (temp.Next != null)
            {
                sw.WriteLine(temp.Name);
                temp = temp.Next;
            }
            sw.Close();
        }

        public string PrintList()
        {
            string output = "";
            Node temp = head;
            int place = 1;
            for(int i = 0; i < 4; i++)
            {
                output += place + ") " + temp.Name + ": " + temp.Score + "               " + (place + 4) + ") " + temp.Next.Next.Next.Next.Name + ": " + temp.Next.Next.Next.Next.Score + "\n";

                if (temp.Next == null)
                {
                    break;
                }
                else
                {
                    temp = temp.Next;
                    place ++;
                }
            }

            return output;
        }
    }
}
