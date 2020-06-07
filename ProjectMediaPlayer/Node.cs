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
        public string Title { get; set; }
        public string Path { get; set; }

        // Default constructor
        public Node()
        {
        }

        // Overloaded constructor accepting the song attributes
        public Node(string title, string path)
        {
            this.Title = title;
            this.Path = path;
        }

        // Getters and setters
        public string GetTitle()
        {
            return Title;
        }

        //public string getPath()
        //{
        //    return Path;
        //}

        //public void setTitle(string title)
        //{
        //    this.Title = title;
        //}

        //public void setPath(string path)
        //{
        //    this.Path = path;
        //}

        //public Node getPrev()
        //{
        //    return prev;
        //}

        //public Node getNext()
        //{
        //    return next;
        //}

    }
}
