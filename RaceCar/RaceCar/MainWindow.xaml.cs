//////////////////////////////////////////////////////////////////////////////////
// FINAL PROJECT
// CS371 - Whitworth University
// Members : Bishesh Tuladhar, Pukar Mahat
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Media;


namespace RaceCar
{

    public partial class MainWindow : Window
    {
        //global variables

        double carSpeed = 10; //player car speed
        double roadSpeed = 5; //roadtrack speed
        double trafficSpeed = 5; //AI car speed
        int Score = 0; //Score

        bool carLeft; //bool for left move
        bool carRight; //bool for right move

        Random rnd = new Random(); //random number generator
        DispatcherTimer timer; //timer 

        SoundPlayer music = new SoundPlayer(); //background music
        SoundPlayer hit = new SoundPlayer(); //hit music

        string keyPressing = ""; //DEVELOPER STATS of key

        public MainWindow() 
        {
            InitializeComponent();

            timer = new DispatcherTimer(); //initializing the timer 
            timer.Interval = TimeSpan.FromSeconds(.01); //setting the tick 
            timer.Tick += timer_Tick; 

            string rootLocation = System.IO.Path.GetFullPath("../../Resources/TokyoDrift.wav"); //root location of the background music
            string rootLocation2 = System.IO.Path.GetFullPath("../../Resources/hit.wav"); //root location of the hit msuic
            music.SoundLocation = rootLocation;
            hit.SoundLocation = rootLocation2;
        
            KeyDown += moveCar; //key down event
            KeyUp += stopCar; //key up event

            Reset(); //reseting the game

        }


        //Method to reset the game 
        public void Reset()
        {
            music.Play(); //music is played

            trophy.Visibility = Visibility.Hidden; //trophy is hidden

            explosion.Visibility = Visibility.Hidden; //explosion is hiden

            //Setting default value
            trafficSpeed = 5; 
            roadSpeed = 5;
            Score = 0;

            
            //Reseting Player and its hitbox
            Canvas.SetLeft(player, 164);
            Canvas.SetTop(player, 270);

            Canvas.SetLeft(playerHit, 164);
            Canvas.SetTop(playerHit, 270);


            carLeft = false;
            carRight = false;


            //Reseting Player and its hitbox
            Canvas.SetLeft(AI1, 296);
            Canvas.SetTop(AI1, 78);

            Canvas.SetLeft(AI1Hit, 296);
            Canvas.SetTop(AI1Hit, 78);


            //Reseting Player and its hitbox
            Canvas.SetLeft(AI2, 59);
            Canvas.SetTop(AI2, 78);

            Canvas.SetLeft(AI2Hit, 59);
            Canvas.SetTop(AI2Hit, 78);

            //Reseting the road track
            Canvas.SetLeft(roadTrack1, -3);
            Canvas.SetTop(roadTrack1, -627);

            Canvas.SetLeft(roadTrack2, -5);
            Canvas.SetTop(roadTrack2, -208);

            //Reseting Explosion
            Canvas.SetLeft(explosion, 119);
            Canvas.SetTop(explosion, 240);

            //beginning the timeer
            timer.Start();

        }

        //method to check if two rectangel collide with each other
        public bool intersectsWith(Rectangle ob1, Rectangle ob2)
        {
            Rect myRect1 = new Rect(); //genrating new rect ob for 1st rectangle

            //Setting the rect with same co-ordinate as the rectangle
            double top1 = Canvas.GetTop(ob1); 
            double left1 = Canvas.GetLeft(ob1);

            //Setting the rect with same location and size as the rectangle
            myRect1.Location = new Point(left1, top1); 
            myRect1.Size = new Size(ob1.Width, ob1.Height);

            Rect myRect2 = new Rect(); //genrating new rect ob for 1st rectangle

            //Setting the rect with same co-ordinate as the rectangle
            double top2 = Canvas.GetTop(ob2);
            double left2 = Canvas.GetLeft(ob2);

            //Setting the rect with same location and size as the rectangle
            myRect2.Location = new Point(left2, top2);
            myRect2.Size = new Size(ob2.Width, ob2.Height);


            //Checking if the two rect with each other
            if (myRect1.IntersectsWith(myRect2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //timer_Tick method to circle thorugh each tick
        void timer_Tick(object sender, EventArgs e)
        {
            Score++;

            distance.Content = "" + Score;

            //Movement of the roadtrack
            Canvas.SetTop(roadTrack1, Canvas.GetTop(roadTrack1) + roadSpeed);
            Canvas.SetTop(roadTrack2, Canvas.GetTop(roadTrack2) + roadSpeed);

            //Resetting the road track if it goes out of bound
            if (Canvas.GetTop(roadTrack1) > 500)
            {
                Canvas.SetTop(roadTrack1, -630);
            }

            if (Canvas.GetTop(roadTrack2) > 500)
            {
                Canvas.SetTop(roadTrack2, -630);
            }

            //checking if the left is still within bound
            if (Canvas.GetLeft(player) < 0 || Canvas.GetLeft(playerHit) < 0)
            {
                carLeft = false;
                Canvas.SetLeft(player, 0);
                Canvas.SetLeft(playerHit, 0);
            }

            //Moving car left
            if (carLeft)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - carSpeed);
                Canvas.SetLeft(playerHit, Canvas.GetLeft(playerHit) - carSpeed);
            }

            //checking if the left is still within bound
            if (Canvas.GetLeft(player) > 334 || Canvas.GetLeft(playerHit) > 334)
            {
                carRight = false;
                Canvas.SetLeft(player, 330);
                Canvas.SetLeft(playerHit, 330);
            }

            //Moving car right
            if (carRight)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + carSpeed);
                Canvas.SetLeft(playerHit, Canvas.GetLeft(playerHit) + carSpeed);
            }
            

            //Moving AI cars 
            Canvas.SetTop(AI1, Canvas.GetTop(AI1) + trafficSpeed);
            Canvas.SetTop(AI1Hit, Canvas.GetTop(AI1Hit) + trafficSpeed);


            Canvas.SetTop(AI2, Canvas.GetTop(AI2) + trafficSpeed);
            Canvas.SetTop(AI2Hit, Canvas.GetTop(AI2Hit) + trafficSpeed);

            //Respawing the AI1 cars and changing their images
            if (Canvas.GetTop(AI1) > Base.Height)
            {
                changeAI1();


                double newrand = rnd.Next(0, 160); //generating new random for the AI position 

                Canvas.SetLeft(AI1, newrand);
                Canvas.SetLeft(AI1Hit, newrand);

                double randTop = rnd.Next(400, 400) * -1; //generating new random for the AI position 

                Canvas.SetTop(AI1, randTop);
                Canvas.SetTop(AI1Hit, randTop);
            }

            //Respawing the AI1 cars and changing their images
            if (Canvas.GetTop(AI2) > Base.Height)
            {
                changeAI2(); //changing the image

                double newrand = rnd.Next(161, 300); //generating new random for the AI position 
                Canvas.SetLeft(AI2, newrand);
                Canvas.SetLeft(AI2Hit, newrand);

                double randTop = rnd.Next(400, 600) * -1; //generating new random for the AI position 

                Canvas.SetTop(AI2, randTop);
                Canvas.SetTop(AI2Hit, randTop);


            }

            //Hit Test
            //If the player hits car AI1 or AI2

            if(intersectsWith(playerHit,AI1Hit) || intersectsWith(playerHit, AI2Hit))
            {
                gameOver();
            }

            //Speed up the traffic 
            //Compund increase after every cycle
            trafficSpeed = trafficSpeed * 1.0001;
            roadSpeed = roadSpeed * 1.0001;

        }

        //moveCar method
        private void moveCar(object sender, KeyEventArgs e)
        {
            keyPressing = e.Key.ToString(); //DEVELOPER Stats 

            //if the key pressed is 'A' and if the player is within bound move left
            if (e.Key.ToString() == "A" && Canvas.GetLeft(player) > 0)
            {
                carLeft = true;
            }

            //if the key pressed is 'D' and if the player is within bound move right
            if (e.Key.ToString() == "D" && Canvas.GetLeft(player) < 334)
            {
                carRight = true;
            }
        }
        

        //stopCar method
        private void stopCar(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "A") //stops the car movement to left after key is up
            {
                carLeft = false;
                
            }
            if (e.Key.ToString() == "D") //stops the car movement to right after key is up
            {
                carRight = false;
            }

            keyPressing = ""; //DEVELOPER stats
        }

        //changeAI1 function
        //changes the car images
        private void changeAI1()
        {
            int num = rnd.Next(1, 8); //generating random number from 1 -8

            //Selects a car type according to the random number selection
            switch (num)
            {

                case 1:

                    AI1.Source = new BitmapImage(new Uri("carGreen.png",UriKind.Relative));
                    break;

                case 2:

                    AI1.Source = new BitmapImage(new Uri("carGrey.png", UriKind.Relative));
                    break;

                case 3:

                    AI1.Source = new BitmapImage(new Uri("carOrange.png", UriKind.Relative));
                    break;

                case 4:

                    AI1.Source = new BitmapImage(new Uri("carPink.png", UriKind.Relative));
                    break;

                case 5:

                    AI1.Source = new BitmapImage(new Uri("carRed.png", UriKind.Relative));
                    break;

                case 6:
 
                    AI1.Source = new BitmapImage(new Uri("TruckBlue.png", UriKind.Relative));
                    break;

                case 7:
                  
                    AI1.Source = new BitmapImage(new Uri("TruckWhite.png", UriKind.Relative));
                    break;

                case 8:
               
                    AI1.Source = new BitmapImage(new Uri("ambulance.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }

        private void changeAI2()
        {
            int num = rnd.Next(1, 8); //generating random number from 1 -8

            //Selects a car type according to the random number selection
            switch (num)
            {

                case 1:
                 
                    AI2.Source = new BitmapImage(new Uri("carGreen.png", UriKind.Relative));
                    break;

                case 2:
                 
                    AI2.Source = new BitmapImage(new Uri("carGrey.png", UriKind.Relative));
                    break;

                case 3:
          
                    AI2.Source = new BitmapImage(new Uri("carOrange.png", UriKind.Relative));
                    break;

                case 4:
                
                    AI2.Source = new BitmapImage(new Uri("carPink.png", UriKind.Relative));
                    break;

                case 5:
               
                    AI2.Source = new BitmapImage(new Uri("carRed.png", UriKind.Relative));
                    break;

                case 6:
              
                    AI2.Source = new BitmapImage(new Uri("TruckBlue.png", UriKind.Relative));
                    break;

                case 7:
                 
                    AI2.Source = new BitmapImage(new Uri("TruckWhite.png", UriKind.Relative));
                    break;

                case 8:
                 
                    AI2.Source = new BitmapImage(new Uri("ambulance.png", UriKind.Relative));
                    break;
                default:
                    break;
            }
        }


        //gameOver function
        private void gameOver()
        {
            music.Stop(); //music stops

            trophy.Visibility = Visibility.Visible; //trophy is displayed

            timer.Stop(); //cycle is stoped


            explosion.Visibility = Visibility.Visible; //explosion of the car

            //setting the explosion on top of the player 
            Canvas.SetLeft(explosion, Canvas.GetLeft(player)-45);
            Canvas.SetTop(explosion, Canvas.GetTop(player)-20);


            //Selecting the trophy according to the score
            if (Score < 100)
            {
                trophy.Source = new BitmapImage(new Uri("bronze.png", UriKind.Relative));
            }

            if (Score > 2000)
            {
                trophy.Source = new BitmapImage(new Uri("silver.png", UriKind.Relative));
            }

            if (Score > 3500)
            {
                trophy.Source = new BitmapImage(new Uri("gold.png", UriKind.Relative));
            }

            hit.Play();//hit music

        }

        //Button to reset
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        //DEVELOPER STATS TEXTBOX
        private void DevelopStats()
        {
            Stats.Visibility = Visibility.Visible; //visible the textbox once it is called

            Stats.Text = "R1 = " + Convert.ToString(Canvas.GetLeft(roadTrack1)) + " , " + Convert.ToString(Canvas.GetTop(roadTrack1)) +
                                " \n R2 = " + Convert.ToString(Canvas.GetLeft(roadTrack2)) + " , " + Convert.ToString(Canvas.GetTop(roadTrack2)) +
                                "\n C1 = " + Convert.ToString(Canvas.GetLeft(AI1)) + " , " + Convert.ToString(Canvas.GetTop(AI1)) +
                                "\n C2 = " + Convert.ToString(Canvas.GetLeft(AI2)) + " , " + Convert.ToString(Canvas.GetTop(AI2)) +
                                "\n PLayer = " + Convert.ToString(Canvas.GetLeft(player)) + " , " + Convert.ToString(Canvas.GetTop(player)) +
                                "\n Explosion = " + Convert.ToString(Canvas.GetLeft(explosion)) + " , " + Convert.ToString(Canvas.GetTop(explosion)) +
                                "\n PlayerHIt = " + Convert.ToString(Canvas.GetLeft(playerHit)) + " , " + Convert.ToString(Canvas.GetTop(playerHit)) +
                                "\n AI1hit = " + Convert.ToString(Canvas.GetLeft(AI1Hit)) + " , " + Convert.ToString(Canvas.GetTop(AI1Hit)) +
                                "\n AI2hit = " + Convert.ToString(Canvas.GetLeft(AI2Hit)) + " , " + Convert.ToString(Canvas.GetTop(AI2Hit)) +
                                "\n Key Pressing " + keyPressing;
        }


    }
}
