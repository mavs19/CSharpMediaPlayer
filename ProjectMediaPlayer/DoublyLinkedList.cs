﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMediaPlayer
{
    class DoublyLinkedList<T>
    {
        // Node listHead set globally to the node at front of list
        public Node head;
        public Node last = null;
        //public Node current;

        //    //Doubly Linked list Node
        //    // Method to check the length of the list
        public int Length()
        {
            int length = 0;
            Node current = head;
            while (current != null)
            {
                length++;
                current = current.next;
            }
            return length;
        }

        // Method to add a node at the start of list, a new node object is created
        // The New node added will end up as the listHead, if the list was empty prev will be set to null
        public void AddFirst(string newTitle, string newPath)
        {

            Node newNode = new Node(newTitle, newPath);
            newNode.next = head;
            newNode.prev = null;
            if (head != null)
            {
                head.prev = newNode;
            }
            head = newNode;
        }

        // Method to add node after lastNode entry, the parameter prevNode requires the listHead data when method called
        // This method will display a message if the list is empty, otherwise data will be added after listHead node
        public void AddAfter(Node prevNode, string newTitle, string newPath)
        {

            if (prevNode == null)
            {
                //Messa("The given previous node cannot be NULL ");
                return;
            }
            Node newNode = new Node(newTitle, newPath);
            newNode.next = prevNode.next;
            prevNode.next = newNode;
            newNode.prev = prevNode;
            if (newNode.next != null)
            {
                newNode.next.prev = newNode;
            }
        }

        // Method to add data to the lastNode node, if the list is empty the new node will be listHead
        // The New Node will traverse the list and up in the lastNode position 
        public void AddLast(string newTitle, string newPath)
        {

            Node newNode = new Node(newTitle, newPath);
            last = head;
            newNode.next = null;
            if (head == null)
            {
                newNode.prev = null;
                head = newNode;
                return;
            }
            while (last.next != null)
            {
                last = last.next;
            }
            last.next = newNode;
            newNode.prev = last;
        }

        public Node PlayNext(Node song)
        {
            Node nextSong = song.next;
            return nextSong;
        }

        public Node PlayPrev(Node song)
        {
            Node prevSong = song.prev;
            return prevSong;
        }


        // Method to Split doubly linked list (DLL) into two halves and return middle of the list
        // Slow and fast variable traverse the list with fast taking two stpes for slow's one step
        public Node Split(Node head)
        {
            Node fast = head, slow = head;
            while (fast.next != null && fast.next.next != null)
            {
                fast = fast.next.next;
                slow = slow.next;
            }
            Node temp = slow.next;
            slow.next = null;
            return temp;
        }

        // Method to Merge Sort the list, the variable second is allocated the node at middle of the list
        // Recurs by recalling the merge sort with original listHead and second listHead node
        // Calls the Merge method to merge the split list back together 
        public Node MergeSort(Node node)
        {
            if (node == null || node.next == null)
            {
                return node;
            }
            Node second = Split(node);
            node = MergeSort(node);
            second = MergeSort(second);
            return Merge(node, second);
        }

        // Method to Merge the two lists after sorting, Compares the first list listHead to the second, 
        // Swaps the data appropriately and recursivley calls the merge until complete
        public Node Merge(Node first, Node second)
        {
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }
            if (first.title.CompareTo(second.title) < 0)
            {
                first.next = Merge(first.next, second);
                first.next.prev = first;
                first.prev = null;
                return first;
            }
            else
            {
                second.next = Merge(first, second.next);
                second.next.prev = second;
                second.prev = null;
                return second;
            }
        }

        // Method to complete a binary search using a node and string as arguments
        // returns null if the list to search is empty
        // While loop to iterate until reached end 
        // If blocks compare if the target match the middle, next to or previous to middle
        // If not found new lastNode node (null) will become the next to meddle or first will become middle
        // Continues until found which will return the node, or return null ifnot found
        public Node BinarySearch(Node newHead, string target)
        {
            head = newHead;
            try
            {
                if (head == null)
                {
                    //System.out.println("No list to search!!!");
                    return null;
                }
                Node first = head;
                Node lastNode = null;
                while (lastNode != null || first != lastNode)
                {
                    Node middle = GetMiddle(first, lastNode);
                    //System.out.println("Target : " + target);
                   // System.out.println("Middle : " + middle.title);
                    //System.out.println("First : " + first.title);
                    if (middle.title.Equals(target))
                    {
                        //System.out.println("Target found at mid : " + target);
                        return middle;
                    }
                    else if (middle.prev.title.Equals(target))
                    {
                        //System.out.println("Found at prev : " + target);
                        return middle.prev;
                    }
                    else if (middle.next.title.Equals(target))
                    {
                        //System.out.println("Found at next : " + target);
                        return middle.next;
                    }
                    else if (middle.title.CompareTo(target) > 0)
                    {
                        lastNode = middle.next;
                    }
                    else
                    {
                        first = middle;
                    }
                }
            }
            catch (NullReferenceException e)
            {
                //System.out.println("Song not found");
                return null;
            }
            return null;
        }

        // Method to find the middle but taking two arguments, used in the binary search 
        // Slow and fast variable traverse the list with fast taking two stpes for slow's one step
        public Node GetMiddle(Node first, Node last)
        {
            if (first == null)
            {
                return null;
            }
            Node fast = first, slow = first;
            while (fast != last && fast.next != last)
            {
                fast = fast.next.next;
                slow = slow.next;
            }
            return slow;
        }

        // Method to target a song from the playlist, receives target as String
        // A method is called to retrive the object with the target's attribute as its title
        // If the target matches head, the new head will be the node next to current head
        // If the node next to target isn't null, the target node becomes the node previous to target
        // If the node previous to target isn't null, the target node becomes the node next to target
        // A node is returned of teh updated list
        public Node Delete(Node newHead, string target)
        {

            Node listHead = newHead;
            Node targetNode = GetSong(listHead, target);
            if (listHead == null || targetNode == null)
            {
                return null;
            }
            if (listHead == targetNode)
            {
                listHead = targetNode.next;
            }
            if (targetNode.next != null)
            {
                targetNode.next.prev = targetNode.prev;
            }
            if (targetNode.prev != null)
            {
                targetNode.prev.next = targetNode.next;
            }
            return listHead;
        }

        // Method to retrieve a Node object with a String value as the input
        // A boolean is used to break out of loop if found, otherwise continues until until end of list (null)
        //  If the string input equals an node objects title, this data is added to the result node
        // Result node will return the object if found or null if not found
        public Node GetSong(Node head, string target)
        {
            Node result = null;
            Node tempHead = head;
            bool found = false;
            while (tempHead != null && found == false)
            {
                tempHead = head;
                head = head.next;
                if (target.Equals(tempHead.title))
                {
                    result = tempHead;
                    found = true;
                }
            }
            return result;
        }

        // Method to print the list for testing purposes
        public void Printlist(Node node)
        {
            Node lastNode = null;

            while (node != null)
            {
                //System.out.print(node.title + "\n");
                lastNode = node;
                node = node.next;
            }
            //System.out.println();
        }
    }
}
