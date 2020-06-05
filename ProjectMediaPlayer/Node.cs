using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMediaPlayer
{
    class Node
    {
        // Previous and getNext instances used for Linked List
        // The String title and path are the song attributes
        public Node prev;
        public Node next;
        public string title { get; set; }
        public string path { get; set; }

        // Default constructor
        public Node()
        {
        }

        // Overloaded constructor accepting the song attributes
        public Node(string title, string path)
        {
            this.title = title;
            this.path = path;
        }

        // Getters and setters
        public string getTitle()
        {
            return title;
        }

        public string getPath()
        {
            return path;
        }

        public void setTitle(string title)
        {
            this.title = title;
        }

        public void setPath(string path)
        {
            this.path = path;
        }

        public Node getPrev()
        {
            return prev;
        }

        public Node getNext()
        {
            return next;
        }

    //    // Overriding the toString method to dispaly title only in list view
    //    @Override
    //public String toString()
    //    {
    //        return title;
    //    }
    }
}
