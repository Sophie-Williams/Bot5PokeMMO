﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoItX3Lib;

namespace Bot5PokeMMO
{
    public partial class App : Form
    {
        // Creating an instance of AutoItX3
        AutoItX3 autoit = new AutoItX3();

        bool botRunning = false;

        public static string hotkeyUp = "W";
        public static string hotkeyDown = "S";
        public static string hotkeyLeft = "A";
        public static string hotkeyRight = "D";

        int runTimeS = 0; // Run time given in seconds
        int runTimeM = 0; // Run time given in minutes - Variable used for conversion s-m
        int runTimeH = 0;

        Bot5PokeMMO.Framework.BotLogic bot = new Framework.BotLogic();

        public App()
        {
            InitializeComponent();
        }

        private void App_Load(object sender, EventArgs e)
        {
            // Settings recommended settings
            chkOnTop.Checked = true;
            cmbWalk.Text = "Left & Right";
            cmbBot.Text = "VIRIDIAN FOREST";
            chkCatchShiny.Checked = true;
            

            #region Default values for hotkeys and pixel-/coords
            // This parameters should be filled out by a config file with def values.
            bot.GetDefVal(458, 439, 0x87959C, 557, 129, 0x84D18E);

            // Hotkeys being set by a config file - they are default by now though. These are not working
            hotkeyUp = "W";
            hotkeyDown = "S";
            hotkeyLeft = "A";
            hotkeyRight = "D";
            #endregion
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(cmbBot.Text == "")
            {
                printToConsole("Error: A bot needs to be selected");
            }
            else
            {
                if (botRunning == false)
                {
                    // setting sleep interval
                    bot.randomSleepWalkB = Int32.Parse(txtSleepB.Text);
                    bot.randomSleepWalkE = Int32.Parse(txtSleepE.Text);

                    tmrTime.Enabled = true;
                    timerLogic.Enabled = true;
                    botRunning = true;
                    btnStart.Text = "Stop";
                    printToConsole("Bot started " + cmbBot.Text);
                    cmbBot.Enabled = false;
                    setPokeMMO();

                }
                else if(botRunning == true)
                {
                    tmrTime.Enabled = false;
                    timerLogic.Enabled = false;
                    botRunning = false;
                    btnStart.Text = "Start";
                    printToConsole("Bot stopped " + cmbBot.Text);
                    cmbBot.Enabled = true;
                }
            }



        }

        private void timerLogic_Tick(object sender, EventArgs e)
        {
            bot.DoRun();
        }

        private void chkOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOnTop.Checked)
            {
                this.TopMost = true;
            }
            else
            {
                this.TopMost = false;
            }
        }

        public void printToConsole(string text = "Console message is not defined!")
        {
            string _text = text;
            rtbConsole.AppendText(_text + "\n" + " - Time: " + runTimeH.ToString() + "h " + runTimeM.ToString() + "m " + runTimeS.ToString() + "s\n");
        }

        private void rtbConsole_TextChanged(object sender, EventArgs e)
        {
            // Scrolls rtbConsole to end when something is written to the console.
            rtbConsole.ScrollToCaret();
        }

        public void calculateRunTime()
        {
            // Updates lblStatus and converts from seconds to minute when 60 seconds have passed.
            if (runTimeS >= 60)
            {
                runTimeS = 0;
                runTimeM++;

            }
            else if (runTimeM >= 60)
            {
                runTimeM = 0;
                runTimeH++;
            }
            else
            {
                // Only counts seconds when below 60
                lblStatus.Text = "Time: " + runTimeS++.ToString() + "s";

                if (runTimeH >= 1) // Starts to counts hours when the first hour is reached.
                {
                    lblStatus.Text = "Time: " + runTimeH.ToString() + "h " + runTimeM.ToString() + "m " + runTimeS.ToString() + "s";
                }
                else if (runTimeM >= 1) // Start to count minutes when the first minute is reached.
                {
                    lblStatus.Text = "Time: " + runTimeM.ToString() + "m " + runTimeS.ToString() + "s";
                }

            }


        }

        private void tmrTime_Tick(object sender, EventArgs e)
        {
            // Update console
            if (bot.type == 1)
            {
                rtbConsole.AppendText(bot.WriteToConsole());
            }


            // Encounters has to be divided by 2
            lblEncounters.Text = "Encounters: " + bot.encounters.ToString();

            calculateRunTime();
        }

        private void cmbBot_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbBot.Text == "Viridian Forest")
            {
                // setting total pokemon via textbox
                bot.SetTotalPokemon(5);

                bot.randomSleepRunB = 300;
                bot.randomSleepRunB = 400;

                // add all pokemon that needs to be checked
                bot.AssignVars(0x98D048, 668, 234, "run"); // Caterpie
                bot.AssignVars(0xF8E800, 666, 239, "run"); // Pikachu
                bot.AssignVars(0xB8F870, 647, 243, "run"); // Metapod
                bot.AssignVars(0xF8E098, 672, 210, "run"); // Kakuna
                bot.AssignVars(0xE8A840, 659, 265, "run"); // Weedle

                timerLogic.Interval = 300;
            }
            else if(cmbBot.Text == "Mt. Ember")
            {
                // setting total pokemon via textbox
                bot.SetTotalPokemon(5);

                bot.randomSleepRunB = 300;
                bot.randomSleepRunB = 400;

                // add all pokemon that needs to be checked
                bot.AssignVars(0x98D048, 668, 234, "run"); // Caterpie
                bot.AssignVars(0xF8E800, 666, 239, "run"); // Pikachu
                bot.AssignVars(0xB8F870, 647, 243, "run"); // Metapod
                bot.AssignVars(0xF8E098, 672, 210, "run"); // Kakuna
                bot.AssignVars(0xE8A840, 659, 265, "run"); // Weedle

                timerLogic.Interval = 300;

            }
           
          
        }

        public void setPokeMMO() // this method sets window size and position
        {
            autoit.WinMove("PokeMMO", "", 0, 0, 900, 600); // WinMove ( "title", "text", x, y [, width [, height [, speed]]] )
            autoit.WinActivate("PokeMMO"); // this line doesn't work. bah

            printToConsole("Window moved succesfully");
        }

        private void cmbWalk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWalk.Text == "Left & Right")
            {
                // setting walking pattern
                bot.walkPattern = "leftright";
            }
            else if (cmbWalk.Text == "Up & Down")
            {
                // setting walking pattern
                bot.walkPattern = "updown";
            }
            else if (cmbWalk.Text == "Square")
            {
                // setting walking pattern
                bot.walkPattern = "square";
            }

        }

        private void chkCatchShiny_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCatchShiny.Checked)
            {
                bot.catchUnspecifiedPokemon = true;
            }
            else
            {
                bot.catchUnspecifiedPokemon = false;
            }
        }

        private void btnHotkeys_Click(object sender, EventArgs e)
        {
            hotkeyUp = txtUp.Text;
            hotkeyDown = txtDown.Text;
            hotkeyLeft = txtLeft.Text;
            hotkeyRight = txtRight.Text;

            bot.setHotkeys();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {


        }

        private void btnCustomize_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This button does not work yet.");
        }

    }
}