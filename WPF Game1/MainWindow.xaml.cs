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

using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Media;


using WPF_Game1;



delegate void FunctionToCall();

namespace WPF_Game1
{   public partial class MainWindow : Window
    {
        public static MainWindow Game;
        //MediaElement Hello = new MediaElement();
        DispatcherTimer DP = new DispatcherTimer();

        int speed = 20;
        bool IsMovingLeft = false, IsMovingRight = false;
        SoundPlayer music = new SoundPlayer("music.wav");
       

        animation classA = new animation();
        FunctionToCall exeA; //function delegate who will allow you to store a func method from classA
        FunctionToCall exeAS;

        //Initialize pointers
        public MainWindow()
        {   Game = this;
            exeA = classA.idle;
            exeAS = classA.AS1;
            
           
            
           

            
            InitializeComponent();
            music.Play();
            DP.Interval = TimeSpan.FromMilliseconds(160);
            DP.Tick += GameTime;
            DP.Start();
            GameBack.Margin = new Thickness(0, -40, -600, 0);
            Pause.Margin = new Thickness(0, 0, 0, 0);
            Pause.Width = 800;
            Pause.Height = 670;
            Pause.Visibility = Visibility.Hidden;
            inventory1.Visibility = Visibility.Hidden;
            InitializeComponent();
        }

        public float energyPT = 0.5f;
        private void GameTime(object sender, EventArgs e)
        { if ((IsMovingLeft && IsMovingRight)||(!IsMovingLeft&&!IsMovingRight))
            { IsMovingLeft = false; IsMovingRight = false;
            } else
            { if (Player.Margin.Left >= 200 && Player.Margin.Left <= 450)
                {
                    if (IsMovingLeft &&Player.Margin.Left>211) { PlayerMove(-speed); } else if (IsMovingLeft) { GameMove(-speed); }
                    if (IsMovingRight&&Player.Margin.Left<439) { PlayerMove(speed); } else if (IsMovingRight) { GameMove(speed); }
                }
            }
            
            exeA(); // execute specific animation
            exeAS(); // increment animation state
            
            if (mana.Value < 100f) mana.Value +=energyPT; // regenerate mana/tick
            if (health.Value < 100f) health.Value += 0.1f; // regenerate health/tick
        }


        private void PlayerMove(float _speed)
        { Player.Margin = new Thickness(Player.Margin.Left + _speed, Player.Margin.Top, 0, 0);}

        private void GameMove(float _speed) //the 'camera' will follow the player
        {GameBack.Margin = new Thickness(GameBack.Margin.Left , GameBack.Margin.Top, GameBack.Margin.Right+_speed, 0);}

        //trigger events for specific keys
        public void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    IsMovingLeft = true;
                    exeA = classA.run;
                    Player.RenderTransform = new ScaleTransform { ScaleX = -1, ScaleY = 1 };
                    break;
                case Key.D:
                    IsMovingRight = true;
                    exeA = classA.run;
                    Player.RenderTransform = new ScaleTransform { ScaleX = 1, ScaleY = 1 };
                    break;
                case Key.S:
                    { if (IsMovingLeft || IsMovingRight)
                            exeA = classA.roll;
                        else exeA = classA.block;
                        break;
                    }
                case Key.Space:
                    if (mana.Value > 30)
                    { DP.Interval = TimeSpan.FromMilliseconds(300); mana.Value -= 30; }
                    else DP.Interval = TimeSpan.FromMilliseconds(160);
                    break;
                case Key.D1:
                    exeA = classA.attack1;
                    break;
                case Key.D2:
                    exeA = classA.attack2;
                    break;
                case Key.D3:
                    exeA = classA.attack3;
                    break;
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.A:
                    IsMovingLeft = false;
                    exeA = classA.idle;
                   
                    break;
                case Key.D:
                    IsMovingRight = false;
                    exeA = classA.idle;
                    break;
                case Key.S:
                    if (IsMovingLeft || IsMovingRight)
                        exeA = classA.run;
                    else exeA = classA.idle;
                    break;
                case Key.Space:
                    DP.Interval = TimeSpan.FromMilliseconds(160);
                    energyPT = 0.5f;
                    break;
                case Key.D1:
                case Key.D2:
                case Key.D3:
                    exeA = classA.idle;
                    break;

            }

        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e) { }
        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e) { }
        



        private void Form1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 

        }

        private void Form1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            
            InitializeComponent();
            exeA = classA.idle;
        }

        private void Form1_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DP.Stop();
            GameBack.LoadedBehavior = MediaState.Stop;
            Pause.Visibility = Visibility.Visible;
            inventory1.Visibility = Visibility.Visible;

            
        }

        private void Form1_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            DP.Start();
            GameBack.LoadedBehavior = MediaState.Play;
           
            Pause.Visibility = Visibility.Hidden;
            inventory1.Visibility = Visibility.Hidden;
          
        }









        //Reload Background
        private void GameBack_MediaEnded(object sender, RoutedEventArgs e)
        {
            GameBack.Position = new TimeSpan(0, 0, 0, 0, 1000);
        }
    }

    public class animation
    {public int AS = 0; //Animation State
     public void AS1() { AS += 1; }// Increment AS, AS1() it's called from main class
   
        //Change Player model
        public void cp(string a){WPF_Game1.MainWindow.Game.Player.Source = 
                new BitmapImage(new Uri("HeroKnight/" + a + ".png", UriKind.Relative));}


        //Animations
        public void run()
        {   string arg = "Run/HeroKnight_Run_";
            if (AS >= 9) { AS = 0; } else { AS += 1; }cp(arg + AS);
        }

        public void idle()
        {   string arg = "Idle/HeroKnight_Idle_";
            if (AS >= 7) { AS = 0; } else {AS += 1;} cp(arg + AS);
        }

        public void attack1()
        {   string arg = "Attack1/HeroKnight_Attack1_"; 
            if (AS >= 5) { AS = 0; } else { AS += 1; }; cp(arg + AS);
        }

        public void attack2()
        {   string arg = "Attack2/HeroKnight_Attack2_";
            if (AS >= 5) { AS = 0; } else { AS += 1; }cp(arg + AS);
        }

        public void attack3()
        {   string arg = "Attack3/HeroKnight_Attack3_"; 
            if (AS >= 7) { AS = 0; } else { AS += 1; }cp(arg + AS);
        }

        public void roll()
        {   string arg = "Roll/HeroKnight_Roll_"; 
            if (AS >= 8) { AS = 0; } else { AS += 1; }cp(arg + AS);
        }

        public void block()
        {   string arg = "BlockIdle/HeroKnight_Block Idle_"; 
            if (AS >= 7) { AS = 0; } else { AS += 1; }cp(arg + AS);
        }


    }
}
