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

namespace RaceCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global variables
        int carSpeed = 5;
        int roadSpeed = 5;
        bool carLeft;
        bool carRight;
        int trafficSpeed = 5;
        int Score = 0;
        Random rnd = new Random();
        System.Timers.Timer gTimerFlag = new System.Timers.Timer();
        DispatcherTimer timer;

        public MainWindow() 
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.001);
            timer.Tick += timer_Tick;
            timer.Start();

            Reset();


        }

        public void Reset()
        {
            trophy.Visibility = Visibility.Collapsed;

            button1.IsEnabled = false;

            explosion.Visibility = Visibility.Collapsed;

            trafficSpeed = 5;

            roadSpeed = 5;

            Score = 0;

            player.Margin = new Thickness(161, 286, 0, 0);

            carLeft = false;
            carRight = false;

            AI1.Margin = new Thickness(66, -120,0,0);

            AI2.Margin = new Thickness(294, -185,0,0);

            roadTrack2.Margin = new Thickness(-3, 222,0,0);
            roadTrack1.Margin = new Thickness(-2, -638, 0, 0);

            timer.Start();

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

            HitTestResult result_topLeft = VisualTreeHelper.HitTest(img2.Parent as Grid, img1_topLeft);
            HitTestResult result_bottomRight = VisualTreeHelper.HitTest(img2.Parent as Grid, img1_bottomRight);

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

            Thickness r2 = roadTrack1.Margin;
            r2.Top += roadSpeed;

            roadTrack1.Margin = r1;
            roadTrack2.Margin = r2;

            if (roadTrack1.Margin.Top > 630)
            {
                r1.Top = -630;
                roadTrack1.Margin = r1;
            }

            if (roadTrack2.Margin.Top > 630)
            {
                r2.Top = -630;
                roadTrack1.Margin = r2;
            }

            Thickness p1 = player.Margin;

            //Moving car left
            if (carLeft)
            {
                p1.Left -= carSpeed;
                player.Margin = p1;
            }

            //Moving car right
            if (carRight)
            {
                p1.Left += carSpeed;
                player.Margin = p1;
            }

            if (player.Margin.Left < 1)
            {
                carLeft = false;
            }
            else if (player.Margin.Left + player.Width > 380)
            {
                carRight = false;
            }

            //Moving AI cars 
            Thickness a1T = AI1.Margin;
            a1T.Top += trafficSpeed;

            AI1.Margin = a1T;

            Thickness a2T = AI2.Margin;
            a2T.Top += trafficSpeed;

            AI2.Margin = a2T;

            Thickness a1L = AI1.Margin;
            Thickness a2L = AI1.Margin;

            //Respawing the AI cars and changing their images
            if (AI1.Margin.Top > Base.Height)
            {
                //changeAI1();
                a1L.Left = rnd.Next(2, 160);
                AI1.Margin = a1L;

                a1T.Top = rnd.Next(100, 200) * -1;
                AI1.Margin = a1T;

            }

            if (AI2.Margin.Top > Base.Height)
            {
                //changeAI2();
                a2L.Left = rnd.Next(2, 160);
                AI2.Margin = a2L;

                a2T.Top = rnd.Next(100, 200) * -1;
                AI2.Margin = a2T;

            }

            //Hit Test
            //If the player hits car AI1 or AI2
            if (isOn(player,AI1) || isOn(player, AI2))
            {
                //gameover();
            }

            //Speed up the traffic 
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
            }

        }

        private void moveCar(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Left" && player.Margin.Left > 0)
            {
                carLeft = true;
            }

            if (e.Key.ToString() == "Right" && player.Margin.Right + player.Width < Base.Width)
            {
                carRight = true;
            }
        }

        private void stopCar(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Left")
            {
                carLeft = false;
            }
            if (e.Key.ToString() == "Right")
            {
                carRight = false;
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Reset();
        }

    }
}
