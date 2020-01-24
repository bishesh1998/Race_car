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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string keyPressing="";
        //global variables
        int carSpeed = 10;
        double roadSpeed = 5;
        bool carLeft;
        bool carRight;
        double trafficSpeed = 5;
        int Score = 0;
        Random rnd = new Random();
        System.Timers.Timer gTimerFlag = new System.Timers.Timer();
        DispatcherTimer timer;

        SoundPlayer music = new SoundPlayer();
        SoundPlayer hit = new SoundPlayer();



        public MainWindow() 
        {
            InitializeComponent();
            

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.01);
            timer.Tick += timer_Tick;

            string rootLocation = System.IO.Path.GetFullPath("../../Resources/TokyoDrift.wav");
            string rootLocation2 = System.IO.Path.GetFullPath("../../Resources/hit.wav");
            music.SoundLocation = rootLocation;
            hit.SoundLocation = rootLocation2;
        
            KeyDown += moveCar;
            KeyUp += stopCar;

            Reset();

        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
                moveCar(sender,e);
        }

        private void OnKeyUpHandler(object sender, KeyEventArgs e)
        {
            stopCar(sender, e);
        }

        public void Reset()
        {
            music.Play();

            trophy.Visibility = Visibility.Hidden;

            explosion.Visibility = Visibility.Hidden;

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

            roadTrack1.Margin = new Thickness(-2,-638, 0, 0);
            roadTrack2.Margin = new Thickness(-3, -222, 0, 0);

            //Reseting Explosion
            Canvas.SetLeft(explosion, 119);
            Canvas.SetTop(explosion, 240);

            timer.Start();

        }

        public bool intersectsWith(Rectangle ob1, Rectangle ob2)
        {
            Rect myRect1 = new Rect();

            double top1 = Canvas.GetTop(ob1);
            double left1 = Canvas.GetLeft(ob1);

            myRect1.Location = new Point(left1, top1);
            myRect1.Size = new Size(ob1.Width, ob1.Height);

            Rect myRect2 = new Rect();
            double top2 = Canvas.GetTop(ob2);
            double left2 = Canvas.GetLeft(ob2);

            myRect2.Location = new Point(left2, top2);
            myRect2.Size = new Size(ob2.Width, ob2.Height);

            if (myRect1.IntersectsWith(myRect2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isOn(Image img1, Image img2)
        {
            if (img1 == null || img1.Visibility != System.Windows.Visibility.Visible)
            {
                return false;
            }

            double img1_topLeft_X = img1.Margin.Left;
            double img1_topLeft_Y = img1.Margin.Top;
            double img1_bottomRight_X = img1_topLeft_X + img1.Width;
            double img1_bottomRight_Y = img1_topLeft_Y + img1.Height;

            Point img1_topLeft = new Point(img1_topLeft_X, img1_topLeft_Y);
            Point img1_bottomRight = new Point(img1_bottomRight_X, img1_bottomRight_Y);

            HitTestResult result_topLeft = VisualTreeHelper.HitTest(img2.Parent as Canvas, img1_topLeft);
            HitTestResult result_bottomRight = VisualTreeHelper.HitTest(img2.Parent as Canvas, img1_bottomRight);

            if (result_topLeft != null && result_bottomRight != null)
            {
                if (result_topLeft.VisualHit.GetType() == typeof(Image) && result_bottomRight.VisualHit.GetType() == typeof(Image) &&
                    (result_topLeft.VisualHit as Image).Name.Equals(img2.Name) && (result_bottomRight.VisualHit as Image).Name.Equals(img2.Name))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        void timer_Tick(object sender, EventArgs e)
        {
            Score++;

            distance.Content = "" + Score;

            Thickness r1 = roadTrack1.Margin;
            r1.Top += roadSpeed;

            Thickness r2 = roadTrack2.Margin;
            r2.Top += roadSpeed;

            roadTrack1.Margin = r1;
            roadTrack2.Margin = r2;

            Stats.Text = "R1 = " + Convert.ToString(roadTrack1.Margin) + " \n R2 = " + Convert.ToString(roadTrack2.Margin) +
                                "\n C1 = " + Convert.ToString(Canvas.GetLeft(AI1)) + " , " + Convert.ToString(Canvas.GetTop(AI1)) + 
                                "\n C2 = " + Convert.ToString(Canvas.GetLeft(AI2)) + " , " + Convert.ToString(Canvas.GetTop(AI2)) +
                                "\n PLayer = " + Convert.ToString(Canvas.GetLeft(player)) + " , " + Convert.ToString(Canvas.GetTop(player)) +
                                "\n Explosion = " + Convert.ToString(Canvas.GetLeft(explosion)) + " , " + Convert.ToString(Canvas.GetTop(explosion)) +
                                "\n PlayerHIt = " + Convert.ToString(Canvas.GetLeft(playerHit)) + " , " + Convert.ToString(Canvas.GetTop(playerHit)) +
                                "\n AI1hit = " + Convert.ToString(Canvas.GetLeft(AI1Hit)) + " , " + Convert.ToString(Canvas.GetTop(AI1Hit)) +
                                "\n AI2hit = " + Convert.ToString(Canvas.GetLeft(AI2Hit)) + " , " + Convert.ToString(Canvas.GetTop(AI2Hit))+
                                "\n Key Pressing " + keyPressing;



            if (roadTrack1.Margin.Top > 400)
            {
                r1.Top = -630;
                roadTrack1.Margin = r1;
            }

            if (roadTrack2.Margin.Top > 400)
            {
                r2.Top = -630;
                roadTrack2.Margin = r2;
            }

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

            //Respawing the AI cars and changing their images
            if (Canvas.GetTop(AI1) > Base.Height)
            {
                changeAI1();


                double newrand = rnd.Next(0, 160);

                Canvas.SetLeft(AI1, newrand);
                Canvas.SetLeft(AI1Hit, newrand);

                double randTop = rnd.Next(400, 400) * -1;

                Canvas.SetTop(AI1, randTop);
                Canvas.SetTop(AI1Hit, randTop);
            }

            if (Canvas.GetTop(AI2) > Base.Height)
            {
                changeAI2();


                double newrand = rnd.Next(161, 300);
                Canvas.SetLeft(AI2, newrand);
                Canvas.SetLeft(AI2Hit, newrand);

                double randTop = rnd.Next(400, 600) * -1;

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
            trafficSpeed = trafficSpeed * 1.0001;
            roadSpeed = roadSpeed * 1.0001;
            /*
            if (Score > 100 && Score < 500)
            {
                trafficSpeed = 6;
                roadSpeed = 7;
            }
            else if (Score >= 500 && Score < 1000)
            {
                trafficSpeed = 7;
                roadSpeed = 8;
            }
            else if (Score > 1200)
            {
                trafficSpeed = 9;
                roadSpeed = 10;
            }*/

        }

        private void moveCar(object sender, KeyEventArgs e)
        {
            keyPressing = e.Key.ToString();
            if (e.Key.ToString() == "A" && Canvas.GetLeft(player) > 0)
            {
                carLeft = true;
            }

            if (e.Key.ToString() == "D" && Canvas.GetLeft(player) < 334)
            {
                carRight = true;
            }
        }

        private void stopCar(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "A")
            {
                carLeft = false;
                
            }
            if (e.Key.ToString() == "D")
            {
                carRight = false;
            }
            keyPressing = "";
        }

        private void changeAI1()
        {
            int num = rnd.Next(1, 8);
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
            int num = rnd.Next(1, 8);
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

        private void gameOver()
        {
            music.Stop();

            trophy.Visibility = Visibility.Visible;

            timer.Stop();


            explosion.Visibility = Visibility.Visible;

            Canvas.SetLeft(explosion, Canvas.GetLeft(player)-45);
            Canvas.SetTop(explosion, Canvas.GetTop(player)-20);


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

            hit.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Reset();
        }


    }
}
