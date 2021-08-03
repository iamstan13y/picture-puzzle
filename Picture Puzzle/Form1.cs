using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Picture_Puzzle.Properties;

namespace Picture_Puzzle
{
    public partial class frmPuzzle : Form
    {
        int NullSliceIndex, moves = 0;
        List<Bitmap> lstOrgPicList = new List<Bitmap>();
        Stopwatch timer = new Stopwatch();

        private void frmPuzzle_Load(object sender, EventArgs e)
        {
            Shuffle();
        }

        public void Shuffle()
        {
            do
            {
                int j;
                List<int> indexes = new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 9 });
                Random rand = new Random();
                for (int i = 0; i < 9; i++)
                {
                    indexes.Remove((j = indexes[rand.Next(0, indexes.Count)]));
                    ((PictureBox)gbPuzzleBox.Controls[i]).Image = lstOrgPicList[j];
                    if (j == 9) NullSliceIndex = i;
                }
            } while (CheckWin());
        }

        public frmPuzzle()
        {
            InitializeComponent();
            lstOrgPicList.AddRange(new Bitmap[]
            { Resources._1, Resources._2, Resources._3,
              Resources._4, Resources._5, Resources._6,
              Resources._7, Resources._8, Resources._9, Resources._null
            });
            lblMovesMade.Text += moves;
            lblElapsed.Text = "00:00:00";
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            DialogResult YesOrNo = new DialogResult();
            if (lblElapsed.Text != "00:00:00")
            {
                YesOrNo = MessageBox.Show("Are you sure you want to restart?", "Puzzle Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            if (YesOrNo == DialogResult.Yes || lblElapsed.Text == "00:00:00")
            {
                Shuffle();
                timer.Reset();
                lblElapsed.Text = "00:00:00";
                moves = 0;
                lblMovesMade.Text = "Moves Made: " + moves;
            }
        }

        bool CheckWin()
        {
            int i;
            for (i = 0; i < 8; i++)
            {
                if ((gbPuzzleBox.Controls[i] as PictureBox).Image != lstOrgPicList[i]) break;
            }
            if (i == 8)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void tmrElapsed_Tick(object sender, EventArgs e)
        {
            if (timer.Elapsed.ToString() != "00:00:00")
                lblElapsed.Text = timer.Elapsed.ToString().Remove(8);
            if (timer.Elapsed.ToString() == "00:00:00")
                btnPause.Enabled = false;
            else
                btnPause.Enabled = true;
            if (timer.Elapsed.Minutes.ToString() == "10")
            {
                timer.Reset();
                lblMovesMade.Text = "Moves Made: 0";
                lblElapsed.Text = "00:00:00";
                moves = 0;
                btnPause.Enabled = false;
                MessageBox.Show("Time Is Up!\nTry Again...", "Picture Puzzle");
                Shuffle();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (btnPause.Text == "Pause")
            {
                timer.Stop();
                gbPuzzleBox.Visible=false;
                btnPause.Text = "Resume";
            }
            else
            {
                timer.Start();
                gbPuzzleBox.Visible = true;
                btnPause.Text = "Pause";
            }
        }

        public void SwitchPictureBox(object sender, EventArgs e)
        {
            if (lblElapsed.Text == "00:00:00")
                timer.Start();
            int pbxIndex = gbPuzzleBox.Controls.IndexOf(sender as Control);
            if (NullSliceIndex != pbxIndex)
            {
                List<int> Brothers = new List<int>(new int[] { ((pbxIndex % 3 == 0) ? -1 : pbxIndex - 1), pbxIndex - 3, ((pbxIndex % 3 ==2) ? -1: pbxIndex + 1), pbxIndex + 3 });
                if (Brothers.Contains(NullSliceIndex))
                {
                    ((PictureBox)gbPuzzleBox.Controls[NullSliceIndex]).Image = ((PictureBox)gbPuzzleBox.Controls[pbxIndex]).Image;
                    ((PictureBox)gbPuzzleBox.Controls[pbxIndex]).Image = lstOrgPicList[9];
                    NullSliceIndex = pbxIndex;
                    lblMovesMade.Text = "Moves Made: " + (++moves);
                    if (CheckWin())
                    {
                        timer.Stop();
                        (gbPuzzleBox.Controls[8] as PictureBox).Image = lstOrgPicList[8];
                        MessageBox.Show("CONGRATULATIONS! YOU WON\nTime Elapsed: " + timer.Elapsed.ToString().Remove(8) +
                            "\nTotal Moves Made: " + moves, "Picture Puzzle");
                        moves = 0;
                        lblMovesMade.Text = "Moves Made: " + moves;
                        lblElapsed.Text = "00:00:00";
                        timer.Reset();
                        Shuffle();
                    }
                }
            }
        }
    }
}
