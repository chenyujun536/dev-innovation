using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuaRongDao
{
    public partial class Form1 : Form
    {
        private List<Label> _labels = new List<Label>();
        private Player _player = new Player();
        private int _highlight = 0;

        public Form1()
        {
            InitializeComponent();
            CustomInit();
        }

        private void CustomInit()
        {
            
            stausLabel.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BuildLayout();
            DisplayCurrent();
        }

        private void BuildLayout()
        {
            var labelLength = 75;
            var marginLength = 10;
            panel1.Size = new Size(marginLength+(labelLength + marginLength)* Util.Rank, marginLength+(labelLength+marginLength)*Util.Rank);

            for (int i = 0; i < Util.Rank; i++)
            {
                for (int j = 0; j < Util.Rank; j++)
                {
                    Label label = new Label();
                    label.Anchor = AnchorStyles.None;
                    label.BackColor = Color.CadetBlue;
                    label.Size = new Size(labelLength,labelLength);
                    label.Font = new Font("Arial Narrow", 36);
                    label.TextAlign = ContentAlignment.MiddleCenter;
                    label.Location = new Point(marginLength + (labelLength+marginLength)*j, marginLength+(labelLength+marginLength)*i);
                    panel1.Controls.Add(label);
                    _labels.Add(label);
                }
            }
        }

        private void DisplayCurrent()
        {
            var currentStation = _player.InitialStation;
            
            int[] current = currentStation.Nums;

            for (int i = 0; i < Util.SIZE; i++)
            {
                _labels[i].Text = Util.Format(current[i]);
            }

            DisplaySucceedOrStopInfo();

            Console.WriteLine($@"step {currentStation.Step}, distance {currentStation.Distance}");
        }

        private void DisplaySucceedOrStopInfo()
        {
            stepLabel.Text = _player.CurrentStep.ToString();
            stausLabel.Visible = _player.Success || _player.Stopped;
            if (_player.Success)
                stausLabel.Text = $"Find a solution of {_player.CurrentStep} steps in {_player.ElapsedSeconds.ToString("F2")} seconds";
            else if (_player.Stopped)
                stausLabel.Text = "Stopped";
        }

        private void newGameBtn_Click(object sender, EventArgs e)
        {
            _player.Init();
            DisplayCurrent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            stausLabel.Visible = true;
            stausLabel.Text = "In calculation...";
            await _player.Play();
            timer1.Enabled = false;
            DisplaySucceedOrStopInfo();
        }

        private void stopBtn_Click(object sender, EventArgs e)
        {
            _player.Stop();
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _labels[_highlight].BackColor = Color.CadetBlue;
            _highlight = (_highlight + 1) % Util.SIZE;
            _labels[_highlight].BackColor = Color.Chartreuse;
            stepLabel.Text = _player.TotalStep.ToString();
        }

        private async void moveBtn_Click(object sender, EventArgs e)
        {
            if (_player.Success)
            {
                stausLabel.Text = "Auto move ";
                _player.BeginMove();
                Station station;
                while (_player.NextMove(out station))
                {
                    int[] current = station.Nums;

                    for (int i = 0; i < Util.SIZE; i++)
                    {
                        _labels[i].Text = Util.Format(current[i]);
                        _labels[i].BackColor = Util.IsAbsent(current[i]) ? Color.Chartreuse : Color.CadetBlue;
                    }

                    stepLabel.Text = $@"{station.Step} / {_player.CurrentStep}";

                    await Task.Delay(800);
                }
            }
        }
    }
}
