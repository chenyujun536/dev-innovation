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
        private List<Label> _labels;
        private Player _player = new Player();
        private int _highlight = 0;

        public Form1()
        {
            InitializeComponent();
            CustomInit();
        }

        private void CustomInit()
        {
            _labels = new List<Label>
            {
                label1,
                label2,
                label3,
                label4,

                label5,
                label6,
                label7,
                label8,

                label9,
                label10,
                label11,
                label12,

                label13,
                label14,
                label15,
                label16,
            };

            succeedLabel.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayCurrent();

        }

        private void DisplayCurrent()
        {
            var currentStation = _player.Current;
            
            int[] current = currentStation.Nums;

            for (int i = 0; i < Util.SIZE; i++)
            {
                _labels[i].Text = Util.Format(current[i]);
            }

            step.Text = _player.CurrentStep.ToString();
            succeedLabel.Visible = _player.Success || _player.Stopped;
            if (_player.Success)
                succeedLabel.Text = "Success";
            else if (_player.Stopped)
                succeedLabel.Text = "Stopped";

            //string result = current.Aggregate("", (s, i) => s += " " + i.ToString());
            //Console.WriteLine(result);
            Console.WriteLine($@"step {currentStation.Step}, distance {currentStation.Distance}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _player.Init();
            DisplayCurrent();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Enabled = true;
            succeedLabel.Visible = true;
            succeedLabel.Text = "In calculation...";
            await _player.Play();
            timer1.Enabled = false;
            DisplayCurrent();
            //while (true)
            //{
            //    await _player.Next();
            //    //await Task.Delay(300);
            //    DisplayCurrent();
            //    if (_player.Success || _player.Stopped)
            //        break;
            //}

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _player.Stop();
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _labels[_highlight].BackColor = Color.CadetBlue;
            _highlight = (_highlight + 1) % Util.SIZE;
            _labels[_highlight].BackColor = Color.Chartreuse;
            step.Text = _player.TotalStep.ToString();
        }
    }
}
