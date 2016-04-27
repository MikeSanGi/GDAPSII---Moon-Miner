using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonMinerExecutable
{
    class Node
    {
        // Attributes \\
        int score;
        string name;
        Node next;

        // Constructor \\
        public Node(string nm, int scr)
        {
            score = scr;
            name = nm;
            next = null;
        }

        // Properties \\
        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Node Next
        {
            get { return next; }
            set { next = value; }
        }
    }
}
