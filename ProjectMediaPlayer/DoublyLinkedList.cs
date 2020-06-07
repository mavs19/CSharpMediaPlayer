using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectMediaPlayer
{
    class DoublyLinkedList<T>
    {
        public Node head;
        public Node last = null;
        public Node tail;

        // Method to add data to the lastNode node, if the list is empty the new node will be listHead
        // The New Node will traverse the list and up in the lastNode position 
        public void AddLast(string newTitle, string newPath)
        {

            Node newNode = new Node(newTitle, newPath);
            tail = newNode;
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

        // Method to Split doubly linked list into two halves and return middle of the list
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
            if (first.Title.CompareTo(second.Title) < 0)
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

        // Method to assign the last the last item on the list to the variable node
        // Returns this value for the purpose of the Binary search
        public Node FindTail(Node node)
        {
            while (node.next != null)
            {
                node = node.next;
            }
            return node;
        }

        // Method to complete a binary search using a string as argument
        // If blocks compare if the target match the middle, next to or previous to middle
        // Continues until found which will return the node, or return null if not found
        public Node BinarySearch(string target)
        {
            try
            {
                if (head == null)
                {
                    return null;
                }
                Node first = head;
                Node lastNode = FindTail(head);
                while (first != lastNode)
                {
                    Node middle = GetMiddle(first, lastNode);
                    if (middle.Title.Equals(target))
                    {
                        return middle;
                    }
                    else if (middle.Title.CompareTo(target) < 0)
                    {
                        if (middle.next != null)
                        {
                            first = middle.next;
                        }
                        else
                        {
                            first = middle;
                        }
                    }
                    else if (middle.Title.CompareTo(target) > 0)
                    {
                        if (middle.prev != null)
                        {
                            lastNode = middle.prev;
                        }
                        else
                        {
                            lastNode = middle;
                        }
                    }
                    
                }
                if (first.Title.Equals(target))
                {
                    return first;
                }
                else
                {
                    return null;
                }
            }
            catch (NullReferenceException)
            {
                return null;
            }
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
            while ((!(fast.Title.Equals(last.Title))) && (!(fast.next.Title.Equals(last.Title))))
            {
                fast = fast.next.next;
                slow = slow.next;
            }
            return slow;
        }

        // Method to delete a song from the playlist, receives target as String
        // A method is called to retrive the object with the target's attribute as its title
        // If the target matches head, the new head will be the node next to current head
        // If the node next to target isn't null, the target node becomes the node previous to target
        // If the node previous to target isn't null, the target node becomes the node next to target
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
        // If the string input equals an node objects title, this data is added to the result node
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
                if (target.Equals(tempHead.Title))
                {
                    result = tempHead;
                    found = true;
                }
            }
            return result;
        }

        // Method to shuffle the list, calls method to retrive the middle of list
        // Uses head and middle of list as arguments in Interweave method will performs the shuffle
        public Node Shuffle(Node head)
        {
            if (head == null)
            {
                return null;
            }
            else
            {
                Node middle = Split(head);
                Interweave(head, middle);
                head = middle;
            }
            return head;
        }

        // Method recieving head and middle of list, 
        // adds both of these plus tail variable to call recursive interweave method
        public void Interweave(Node first, Node second)
        {
            Node tail = null;
            RecInterweave(first, second, tail);
        }

        // Method to relocate the data on the list
        // Middle of original list becomes head with rest of the data changing positions 
        public Node RecInterweave(Node first, Node second, Node tail)
        {
            if (second == null)
            {
                return null;
            }
            if (tail == null)
            {
                tail = second;
            }
            else
            {
                tail.next = second;
                tail = second;
            }
            second.next = RecInterweave(second.next, first, tail);
            return second;

        }

        //// Mehtod to retrieve the number of data in teh list using int counter
        //public int Length()
        //{
        //    int length = 0;
        //    Node current = head;
        //    while (current != null)
        //    {
        //        length++;
        //        current = current.next;
        //    }
        //    return length;
        //}

        //// Method to add a node at the start of list, a new node object is created
        //// The New node added will end up as the listHead, if the list was empty prev will be set to null
        //public void AddFirst(string newTitle, string newPath)
        //{

        //    Node newNode = new Node(newTitle, newPath);
        //    newNode.next = head;
        //    newNode.prev = null;
        //    if (head != null)
        //    {
        //        head.prev = newNode;
        //    }
        //    head = newNode;
        //}

        //// Method to add node after lastNode entry, the parameter prevNode requires the listHead data when method called
        //// This method will display a message if the list is empty, otherwise data will be added after listHead node
        //public void AddAfter(Node prevNode, string newTitle, string newPath)
        //{

        //    if (prevNode == null)
        //    {
        //        return;
        //    }
        //    Node newNode = new Node(newTitle, newPath);
        //    newNode.next = prevNode.next;
        //    prevNode.next = newNode;
        //    newNode.prev = prevNode;
        //    if (newNode.next != null)
        //    {
        //        newNode.next.prev = newNode;
        //    }
        //}

        //// Method to return the next item in the list
        //public Node PlayNext(Node song)
        //{
        //    Node nextSong = song.next;
        //    return nextSong;
        //}

        //// Mehtod to return the previous item on the list
        //public Node PlayPrev(Node song)
        //{
        //    Node prevSong = song.prev;
        //    return prevSong;
        //}
    }
}
