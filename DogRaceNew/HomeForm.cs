using DogRaceNew.RaceEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DogRaceNew
{
    public partial class HomeForm : Form
    {

        Dog[] dogz = new Dog[4];
        Punter[] punters = new Punter[3];
        Dog winnerDog;

        Timer timer1,timer2,timer3,timer4;


        public HomeForm()
        {
            InitializeComponent();
            PrepareRaceData();
        }

        private void PrepareRaceData()
        {
            // Dogs Info
            dogz[0] = new Dog() { DogName = "Dog 1", RaceTrackLength = 1030, MyPictureBox = pictureDog1 };
            dogz[1] = new Dog() { DogName = "Dog 2", RaceTrackLength = 1030, MyPictureBox = pictureDog2 };
            dogz[2] = new Dog() { DogName = "Dog 3", RaceTrackLength = 1030, MyPictureBox = pictureDog3 };
            dogz[3] = new Dog() { DogName = "Dog 4", RaceTrackLength = 1030, MyPictureBox = pictureDog4 };

            //Punter Info
            punters[0] = Factory.GetAPunter("Joe");
            punters[1] = Factory.GetAPunter("Bob");
            punters[2] = Factory.GetAPunter("AI");
            
            punters[0].MyLabel = labelBet;
            punters[0].MyRadioButton = punter1Radio;
            punters[0].MyText = textBoxPunter1;
            punters[0].MyRadioButton.Text = punters[0].Name;

            
            punters[1].MyLabel = labelBet;
            punters[1].MyRadioButton = punter2Radio;
            punters[1].MyText = textBoxPunter2;
            punters[1].MyRadioButton.Text = punters[1].Name;

            
            punters[2].MyLabel = labelBet;
            punters[2].MyRadioButton = punter3Radio;
            punters[2].MyText = textBoxPunter3;
            punters[2].MyRadioButton.Text = punters[2].Name;

            numericCycleNo.Minimum = 1;
            numericCycleNo.Maximum = 4;
            numericCycleNo.Value = 1;
        }


        private void puntersRadio_CheckedChanged(object sender, EventArgs e)
        {
            SetupBetDetails(); //method calling- place bet
        }

        //Place Bet 
        private void SetupBetDetails()
        {
            foreach (Punter punter in punters)
            {
                if (punter.Busted)
                {
                    punter.MyText.Text = "BUSTED";
                }
                else
                {
                    if (punter.MyBet == null)
                    {
                        punter.MyText.Text = punter.Name + " hasn't placed a bet";
                    }
                    else
                    {
                        punter.MyText.Text = punter.Name + " bets $" + punter.MyBet.Amount + " on " + punter.MyBet.dog.DogName;
                    }
                    if (punter.MyRadioButton.Checked)
                    {
                        labelMax.Text = "Max Bet Amount is $" + punter.Cash.ToString();
                        btnAction.Text = "Place Bet for " + punter.Name;
                        punter.MyLabel.Text = punter.Name + " Bets Amount $";
                        numericBetAmount.Minimum = 1;
                        numericBetAmount.Maximum = punter.Cash;
                        numericBetAmount.Value = 1;
                    }
                }
            }
        }

        //Lets Race after placing Bet
        private void btnAction_Click(object sender, EventArgs e)
        {
            if (btnAction.Text.Contains("Place"))
            {
                int count = 0;
                int total_active = 0;
                foreach (Punter punter in punters)
                {
                    if (punter.Busted)
                    {
                        //MessageBox.Show("Bet is Not Placed Because " + punter.Name + " is BUSTED");
                    }
                    else
                    { 
                        total_active++;
                        if (punter.MyRadioButton.Checked)
                        {
                            if (punter.MyBet == null)
                            {
                                int number = (int)numericCycleNo.Value;
                                int amount = (int)numericBetAmount.Value;
                                bool alreadyPlaced = false;
                                foreach (Punter pun in punters)
                                {
                                    if (pun.MyBet != null && pun.MyBet.dog == dogz[number - 1])
                                    {
                                        alreadyPlaced = true;
                                        break;
                                    }
                                }
                                if (alreadyPlaced)
                                {
                                    MessageBox.Show("This Dog Number is Already Taken By Another Better.");
                                }
                                else
                                {
                                    punter.MyBet = new Bet() { Amount = amount, dog = dogz[number - 1] };
                                }

                            }
                            else
                            {
                                MessageBox.Show("You Already Place Bet for " + punter.Name);
                            }
                        }
                        if (punter.MyBet != null)
                        {
                            count++;
                        }
                    }                    
                }
                SetupBetDetails();
                if (count == total_active)
                {
                    btnAction.Text = "Start The Race Now";
                    panelBet.Enabled = false;
                }
            }
            else if (btnAction.Text.Contains("Start"))
            {
                timer1 = new Timer();
                timer1.Interval = 15;
                timer1.Tick += Cycling_Tick;

                timer2 = new Timer();
                timer2.Interval = 15;
                timer2.Tick += Cycling_Tick;

                timer3 = new Timer();
                timer3.Interval = 15;
                timer3.Tick += Cycling_Tick;

                timer4 = new Timer();
                timer4.Interval = 15;
                timer4.Tick += Cycling_Tick;

                timer1.Start();
                timer2.Start();
                timer3.Start();
                timer4.Start();
                
            }
            else if (btnAction.Text.Contains("GAME"))
            {
                MessageBox.Show("GAME OVER!!");
                Application.Exit();
            }
        }


        private void Cycling_Tick(object sender, EventArgs e)
        {
            if(sender is Timer)
            {
                int index = -1;
                Timer timer = sender as Timer;
                if( timer == timer1)
                {
                    index = 0;
                }
                else if (timer == timer2)
                {
                    index = 1;
                }
                else if (timer == timer3)
                {
                    index = 2;
                }
                else if (timer == timer4)
                {
                    index = 3;
                }

                if( index != -1 )
                {
                    PictureBox pbox = dogz[index].MyPictureBox;
                    if (pbox.Location.X + pbox.Width > dogz[index].RaceTrackLength)
                    {  
                        if (winnerDog == null)
                        {
                            winnerDog = dogz[index];
                        }
                        timer1.Stop();
                        timer2.Stop();
                        timer3.Stop();
                        timer4.Stop();
                    }
                    else
                    {
                        int jump = new Random().Next(1, 15);
                        pbox.Location = new Point(pbox.Location.X + jump, pbox.Location.Y);
                    }
                }
            }
            if( winnerDog != null)
            {
                MessageBox.Show("Congratulation! " + winnerDog.DogName + " Win The Race");
                SetupBetDetails();
                foreach (Punter punter in punters)
                {
                    if (punter.MyBet != null)
                    {
                        if (punter.MyBet.dog == winnerDog)
                        {
                            punter.Cash += punter.MyBet.Amount;
                            punter.MyText.Text = punter.Name + " Won and now has $" + punter.Cash;
                            punter.Winner = true;
                        }
                        else
                        {
                            punter.Cash -= punter.MyBet.Amount;
                            if (punter.Cash == 0)
                            {
                                punter.MyText.Text = "BUSTED";
                                punter.Busted = true;
                                punter.MyRadioButton.Enabled = false;
                            }
                            else
                            {
                                punter.MyText.Text = punter.Name + " Lost and now has $" + punter.Cash;
                            }
                        }                        
                    }
                }
                winnerDog = null;
                timer1 = timer2 = timer3 = timer4 = null;
                int count = 0;
                foreach (Punter punter in punters)
                {
                    if (punter.Busted)
                    {
                        count++;
                    }
                    if (punter.MyRadioButton.Enabled && punter.MyRadioButton.Checked)
                    {
                        labelMax.Text = "Max Bet is $" + punter.Cash;
                        numericBetAmount.Maximum = punter.Cash;
                        numericBetAmount.Minimum = 1;
                    }
                    punter.MyBet = null;
                    punter.Winner = false;
                }
                if (count == punters.Length)
                {
                    btnAction.Text = "Game Over";

                }
                else
                {
                    panelBet.Enabled = true;
                }
                foreach (Dog dog in dogz)
                {
                    dog.MyPictureBox.Location = new Point(12, dog.MyPictureBox.Location.Y);
                }
            }
        }
    }
}
