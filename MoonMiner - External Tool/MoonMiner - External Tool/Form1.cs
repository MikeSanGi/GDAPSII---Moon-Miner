using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MoonMiner___External_Tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Set score modifier
            double scoreModifier = .5;

            //Set availiability of buttons
            button1.Enabled = false;
            btnHard.Enabled = true;
            btnNormal.Enabled = true;
            button2.Enabled = true;

            //Set bar values
            brObstacleFrequency.Value = 1;
            brSpeed.Value = 1;

            //Write to file
            string difficultyText = brObstacleFrequency.Value + " " + brSpeed.Value + " " + scoreModifier;
            System.IO.File.WriteAllText("difficulty.txt", difficultyText);
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            //Set score modifier
            double scoreModifier = 1;

            //Set availiability of buttons
            button1.Enabled = true;
            btnHard.Enabled = true;
            btnNormal.Enabled = false;
            button2.Enabled = true;

            //Set bar values
            brObstacleFrequency.Value = 5;
            brSpeed.Value = 5;

            //Write to file
            string difficultyText = brObstacleFrequency.Value + " " + brSpeed.Value + " " + scoreModifier;
            System.IO.File.WriteAllText("difficulty.txt", difficultyText);
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            //Set score modifier
            double scoreModifier = 2;

            //Set availiability of buttons
            button1.Enabled = true;
            btnHard.Enabled = false;
            btnNormal.Enabled = true;
            button2.Enabled = true;

            //Set bar values
            brObstacleFrequency.Value = 10;
            brSpeed.Value = 10;

            //Write to file
            string difficultyText = brObstacleFrequency.Value + " " + brSpeed.Value + " " + scoreModifier;
            System.IO.File.WriteAllText("difficulty.txt", difficultyText);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Set score modifier
            double scoreModifier;
            double scoreModifierFrequency = .5;
            double scoreModifierSpeed = .5;
            switch (brObstacleFrequency.Value)
            {
                case 1: scoreModifierFrequency = .5; break;
                case 2: scoreModifierFrequency = .6; break;
                case 3: scoreModifierFrequency = .75; break;
                case 4: scoreModifierFrequency = .9; break;
                case 5: scoreModifierFrequency = 1; break;
                case 6: scoreModifierFrequency = 1.2; break;
                case 7: scoreModifierFrequency = 1.4; break;
                case 8: scoreModifierFrequency = 1.6; break;
                case 9: scoreModifierFrequency = 1.8; break;
                case 10: scoreModifierFrequency = 2; break;
            }
            switch (brSpeed.Value)
            {
                case 1: scoreModifierSpeed = .5; break;
                case 2: scoreModifierSpeed = .6; break;
                case 3: scoreModifierSpeed = .75; break;
                case 4: scoreModifierSpeed = .9; break;
                case 5: scoreModifierSpeed = 1; break;
                case 6: scoreModifierSpeed = 1.2; break;
                case 7: scoreModifierSpeed = 1.4; break;
                case 8: scoreModifierSpeed = 1.6; break;
                case 9: scoreModifierSpeed = 1.8; break;
                case 10: scoreModifierSpeed = 2; break;
            }
            scoreModifier = (scoreModifierFrequency + scoreModifierSpeed) / 2;

            //Set availiability of buttons
            button1.Enabled = true;
            btnHard.Enabled = true;
            btnNormal.Enabled = true;
            button2.Enabled = false;
            brObstacleFrequency.Enabled = false;
            brSpeed.Enabled = false;

            //Write to file
            string difficultyText = brObstacleFrequency.Value + " " + brSpeed.Value + " " + scoreModifier;
            System.IO.File.WriteAllText("difficulty.txt", difficultyText);
        }
    }
}
