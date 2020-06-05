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
        DoublyLinkedList<string> playlist = new DoublyLinkedList<string>();
        DoublyLinkedList<string> sortedPlaylist = new DoublyLinkedList<string>();
        Node currentSong;

        public FormMediaPlayer()
        {
            InitializeComponent();
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.play();
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.pause();
        }

        private void ButtonNext_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxPlaylist.SelectedItem = currentSong.next.title;
            }
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text += "Error";
            }
        }

        private void ButtonPrev_Click(object sender, EventArgs e)
        {
            try
            {
                listBoxPlaylist.SelectedItem = currentSong.prev.title;
            } 
            catch (NullReferenceException)
            {
                toolStripStatusLabel.Text += "Error";
            }
            
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.Ctlcontrols.stop();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete song?", caption: "Options", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string selectedSong = listBoxPlaylist.SelectedItem.ToString();
                playlist.head = playlist.Delete(playlist.head, selectedSong);
                axWindowsMediaPlayer.Ctlcontrols.stop();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
            DisplayPlayList();
            //MessageBox.Show(playlist.head.ToString());
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Multiselect = true,
                //Filter = "MP3|*.mp3"
                Filter = "WMV|*.wmv|WAV|*wav|MP3|*.mp3|MP4|*.mp4|MKV|*.mkv"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //playlist = new List<MediaFile>();
                    foreach (string fileName in ofd.FileNames)
                    {
                        FileInfo fi = new FileInfo(fileName);
                        string title = Path.GetFileNameWithoutExtension(fi.FullName),
                        path = fi.FullName;
                        playlist.AddLast(title, path);
                            
                    }
                    Node sorted = playlist.MergeSort(playlist.head);
                    playlist.head = sorted;
                    DisplayPlayList();
                    //listBoxPlaylist.DataSource = playlist;

                }
            }
        }

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
                    toolStripStatusLabel.Text = "Data has been saved to CSV";
                }
            }
        }

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
                            playlist.AddLast(item.title, item.path);
                        }
                        DisplayPlayList();
                    
                    }
                    toolStripStatusLabel.Text = "CSV file successfully opened";
                }
            }
        }

        private void ListBoxPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPlaylist.SelectedIndex != -1)
            {
                string selectedTitle = listBoxPlaylist.SelectedItem.ToString();
                Node selectedPath = playlist.GetSong(playlist.head, selectedTitle);
                currentSong = selectedPath;
                axWindowsMediaPlayer.URL = currentSong.path;
                axWindowsMediaPlayer.Ctlcontrols.play();
            }
        }

        private void DisplayPlayList()
        {
            listBoxPlaylist.Items.Clear();
            Node temp = playlist.head;
            while (temp != null)
            {
                listBoxPlaylist.Items.Add(temp.getTitle());
                temp = temp.next;
            }
                
        }

        private void FormMediaPlayer_Load(object sender, EventArgs e)
        {
            //listBoxPlaylist.ValueMember = "path";
            //listBoxPlaylist.DisplayMember = "title";
        }
    }
}
