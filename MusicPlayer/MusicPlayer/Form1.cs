using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        private List<string> musicFiles;
        private string currentSong;
        private bool isPaused;
        private bool isChangingPosition;
        public Form1()
        {
            InitializeComponent();
            musicFiles = new List<string>();
            isPaused = false;
            isChangingPosition = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "MP3 Files | *.mp3";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    musicFiles.Add(file);
                    listBox1.Items.Add(Path.GetFileName(file));
                }
                if(musicFiles.Count > 0)
                {
                    btnStart.Enabled = true;
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                if (isPaused)
                {
                    musicPlayer.Ctlcontrols.play();
                    isPaused = false;
                }
                else
                {
                    currentSong = musicFiles[listBox1.SelectedIndex];
                    musicPlayer.URL = currentSong;
                    musicPlayer.Ctlcontrols.play();
                }
                timerPlayBack.Enabled = true;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            musicPlayer.Ctlcontrols.stop();
            isPaused = false;
            timerPlayBack.Enabled= false;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if(isPaused == false)
            {
                musicPlayer.Ctlcontrols.pause();
                isPaused = true;
            }
            else
            {
                musicPlayer.Ctlcontrols.play();
                isPaused = false;
            }
        }

        private void timerPlayBack_Tick(object sender, EventArgs e)
        {
            if (!isChangingPosition)
            {
                label2.Text = "Length: " + FormatTime(musicPlayer.Ctlcontrols.currentPosition) + "/" + FormatTime(musicPlayer.currentMedia.duration);
            }
        }
        private string FormatTime(double seconds)
        {
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            return time.ToString(@"mm\:ss");
        }

        private void musicPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if(e.newState == 8)
            {
                int nextIndex = listBox1.SelectedIndex += 1;

                if(nextIndex < musicFiles.Count)
                {
                    listBox1.SelectedIndex = nextIndex;
                    currentSong = musicFiles[nextIndex];
                    musicPlayer.URL = currentSong;
                    musicPlayer.Ctlcontrols.play();
                    isPaused = false;
                }
                else
                {
                    musicPlayer.Ctlcontrols.stop();
                    isPaused=false;
                }
            }
        }

        private void VolumeBar_Scroll(object sender, ScrollEventArgs e)
        {
            musicPlayer.settings.volume = VolumeBar.Value;
        }
    }
}
