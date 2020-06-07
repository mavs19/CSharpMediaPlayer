using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CsvHelper;

namespace ProjectMediaPlayer
{
    public partial class FormMediaPlayer : Form
    {
        // Doubly linked list playlist declared
        DoublyLinkedList<string> playlist = new DoublyLinkedList<string>();
        // A variable of Node class declared as the current song
        Node currentSong;

        public FormMediaPlayer()
        {
            InitializeComponent();
        }

        // Play button event plays the selected song on the list
        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer.Ctlcontrols.play();
                toolStripStatusLabel.Text = "Now playing : " + currentSong.Title;
            }
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text = "No song selected";
            }
        }

        // Pause button event pauses the current song
        private void ButtonPause_Click(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer.Ctlcontrols.pause();
                toolStripStatusLabel.Text = currentSong.Title + " has been paused.";
            }
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text = "No song selected";
            }
        }

        // Next button event selects the next song on the list and plays
        private void ButtonNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSong != null)
                {
                    //currentSong = playlist.PlayNext(currentSong);
                    //Play(currentSong);
                    listBoxPlaylist.SelectedItem = currentSong.next.Title;
                    toolStripStatusLabel.Text = "Now playing : " + currentSong.Title;
                }
                else
                {
                    toolStripStatusLabel.Text = "No song selected";
                }
                
            }
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text = "Now playing : " + currentSong.Title + " [End of the list has been reached]";
            }
        }

        // Next button event selects the previous song on the list and plays
        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSong != null)
                {
                    //currentSong = playlist.PlayPrev(currentSong);
                    //Play(currentSong);
                    listBoxPlaylist.SelectedItem = currentSong.prev.Title;
                    toolStripStatusLabel.Text = "Now playing : " + currentSong.Title;
                }
                else
                {
                    toolStripStatusLabel.Text = "No song selected";
                }
                
            } 
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text = "Now playing : " + currentSong.Title + " [Start of the list has been reached]";
            }
        }

        // Stop button event stops the current song being played
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentSong != null)
                {
                    axWindowsMediaPlayer.Ctlcontrols.stop();
                    toolStripStatusLabel.Text = "";
                }
                else
                {
                    toolStripStatusLabel.Text += "No song selected";
                }
            }
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text += "No song selected";
            }
        }

        // Delete button event will prompt a dialog box before calling a method to remove item from playlist
        // The updated list will be displayed
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxPlaylist.SelectedIndex != -1)
                {
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete song?", caption: "Options", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string selectedSong = listBoxPlaylist.SelectedItem.ToString();
                        playlist.head = playlist.Delete(playlist.head, selectedSong);
                        axWindowsMediaPlayer.Ctlcontrols.stop();
                        toolStripStatusLabel.Text = selectedSong + " has been deleted from playlist";
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                    DisplayPlayList();
                }
                else
                {
                    toolStripStatusLabel.Text = "No song selected";
                }
                
            }
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text = "No song selected";
            }
        }

        // Add button event, opens windows explorer so songs can be selected after navigating to location
        // Allows multi select and offers various file types
        // Items are added to the linked list with title and path attributes determined in foreach loop
        // The sort method is called which calls a merge sort and displays the list
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "MP3|*.mp3|WMV|*.wmv|WAV|*wav|MP4|*.mp4|MKV|*.mkv"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(fileName);
                        string title = Path.GetFileNameWithoutExtension(fi.FullName),
                        path = fi.FullName;
                        playlist.AddLast(title, path);
                    }
                    Sort();
                    toolStripStatusLabel.Text = "";
                }
            }
        }

        // Save button event opens explorer to allow a foler and file name to be selected in csv format
        // CSV Helper library used, data from the linked list is added to a generic list in while loop
        // The CSV helper's Write records method writes the entire list to CSV 
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "CSV|*.csv", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var sw = new StreamWriter(sfd.FileName))
                    using (var writer = new CsvWriter(sw, CultureInfo.InvariantCulture))
                    {
                        List<Node> records = new List<Node>();
                        Node temp = playlist.head;
                        while (temp != null)
                        {
                            records.Add(temp);
                            temp = temp.next;
                        }
                        writer.WriteRecords(records);
                    }
                    toolStripStatusLabel.Text = "Data has been saved to : " + sfd.FileName;
                }
            }
        }

        // Load button event opens explorer to locate file with the filter set to CSV file type
        // CSV Helper libray used with the Get Records method, an I Enurable list stores all the data,
        // A for each loop is used to traverse this list and add the records to the linked list
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    using (var sr = new StreamReader(new FileStream(ofd.FileName, FileMode.Open)))
                    using (var reader = new CsvReader(sr, CultureInfo.InvariantCulture))
                    {
                        IEnumerable<Node> records = reader.GetRecords<Node>();
                        foreach (Node item in records)
                        {
                            playlist.AddLast(item.Title, item.Path);
                        }
                        DisplayPlayList();
                    
                    }
                    toolStripStatusLabel.Text = ofd.FileName + " successfully opened";
                }
            }
        }

        // Search button event, list sorted first. 
        // Text input target used in binary search method, variable of Node retriving return data
        // The selected song will be added to the media player to play, target will select on list 
        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textSearch.Text))
            {
                Sort();
                string target = textSearch.Text;
                textSearch.Clear();
                Node selectedSong = playlist.BinarySearch(target);
                if (selectedSong != null)
                {
                    listBoxPlaylist.SelectedItem = target;
                    currentSong = selectedSong;
                    axWindowsMediaPlayer.URL = currentSong.Path;
                    axWindowsMediaPlayer.Ctlcontrols.play();
                    toolStripStatusLabel.Text = "Now playing : " + currentSong.Title;
                }
                else
                {
                    toolStripStatusLabel.Text = target + " not found in the list!";
                }
            }
            else
            {
                toolStripStatusLabel.Text = "Please enter a title in the text field";
            }
            
        }

        // Button sort event calls the sort method
        private void ButtonSort_Click(object sender, EventArgs e)
        {
            Sort();
            toolStripStatusLabel.Text = "Playlist has been sorted";
        }

        // Button Shuffle event, the shuffled list is returned to the variable shuffled
        // This is done by calling a method using the playlist,
        // The playlist's data is then changed to the shuffled result before displaying
        private void ButtonShuffle_Click(object sender, EventArgs e)
        {
            Node shuffled = playlist.Shuffle(playlist.head);
            playlist.head = shuffled;
            listBoxPlaylist.Items.Clear();
            DisplayPlayList();
            toolStripStatusLabel.Text = "Playlist has been shuffled";
        }

        // Event for clicking am item on the list box, which promts the item to play
        // A string variable is used as the title in the Get Song method which returns the path
        private void ListBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlaylist.SelectedIndex != -1)
            {
                string selectedTitle = listBoxPlaylist.SelectedItem.ToString();
                Node selectedPath = playlist.GetSong(playlist.head, selectedTitle);
                currentSong = selectedPath;
                axWindowsMediaPlayer.URL = currentSong.Path;
                axWindowsMediaPlayer.Ctlcontrols.play();
                toolStripStatusLabel.Text = "Now playing : " + currentSong.Title;
            }
        }

        // Method to display the list into the list box, which is cleared before displaying data
        // While loop will iterate until end of list is reached (null)
        // The variable temp is added to the list, the changed to the next item in each iteration
        private void DisplayPlayList()
        {
            listBoxPlaylist.Items.Clear();
            Node temp = playlist.head;
            while (temp != null)
            {
                listBoxPlaylist.Items.Add(temp.GetTitle());
                temp = temp.next;
            }
        }

        // Method to sort the list, A Node variable will store the returned data from the Merge Sort method
        // The playlist's data is then changed to the sorted result before displaying
        private void Sort()
        {
            Node sorted = playlist.MergeSort(playlist.head);
            playlist.head = sorted;
            listBoxPlaylist.Items.Clear();
            DisplayPlayList();
        }

        // Event for the drop down option sign out
        // Closes the current form and opens the sign in form
        private void SignOutToolStripMenuItem_Click(object sender, EventArgs e)

        {
            this.Hide();
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
        }

        // Event for the drop down option clear list
        // Displays a yes/no dialog box, clears the list from th list box, makes list null
        private void ClearPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete song?", caption: "Options", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                playlist.head = null;
                DisplayPlayList();
                toolStripStatusLabel.Text = "Playlist has been deleted";
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        //private void Play(Node node)
        //{
        //    Node song = node;
        //    axWindowsMediaPlayer.URL = song.path;
        //    axWindowsMediaPlayer.Ctlcontrols.play();
        //}
    }
}
