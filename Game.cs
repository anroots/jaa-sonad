using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Jaa_
{
    public class Game
    {
        /// <summary>
        /// Accessor for WPF binding
        /// </summary>
        public string timeString = "-1";

        /// <summary>
        /// How long the game lasted in seconds
        /// </summary>
        public int gameDuration = 0;

        /// <summary>
        /// Holds timeLeft
        /// </summary>
        private int _timeLeft = -1;

        /// <summary>
        /// This holds the number of seconds left 
        /// for each attempt when the timer is enabled.
        /// -1 stands for not yet initialized (paused)
        /// </summary>
        public int timeLeft { get { return _timeLeft; } set { _timeLeft = value; timeString = "param"; } }

        /// <summary>
        /// The number of seconds for each attempt
        /// </summary>
        public static readonly int initialTime = 6;

        /// <summary>
        /// Countdown timer for the attempt
        /// </summary>
        private static DispatcherTimer timer = new DispatcherTimer();


        /// <summary>
        /// Activate timer
        /// </summary>
        public void StartTimer()
        {
            timer = new DispatcherTimer();
            gameDuration = 0;
            timeLeft = initialTime;
            timer.Tick += new EventHandler(timer_Elapsed);
            timer.Interval = new TimeSpan(0, 0, 1);

            timer.Start();
        }

        /// <summary>
        /// Renew timer
        /// </summary>
        public void RenewTime()
        {
            timeLeft = initialTime;
        }

        /// <summary>
        /// Stop timer
        /// </summary>
        public void StopTimer()
        {
            timer.Stop();
        }

        /// <summary>
        /// Time left timer tic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, EventArgs e)
        {

            MainWindow.game.timeLeft -= 1;
            gameDuration += 1;
            MainWindow.instance.UpdateTimeLeft();

            if (MainWindow.game.timeLeft <= 0)
            {
                timer.Stop();
                MainWindow.game.timeLeft = -1;
                MainWindow.instance.TimeRanOut();
            }
        }
    }
}
