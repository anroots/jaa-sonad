using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace Jaa_
{
    public class Game
    {
        /// <summary>
        /// Accessor for WPF binding
        /// </summary>
        public string timeString = "-1";

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
        private const int initialTime = 10;

        /// <summary>
        /// Countdown timer for the attempt
        /// </summary>
        private static Timer timer = new Timer(1000);


        /// <summary>
        /// Activate timer
        /// </summary>
        public void StartTimer()
        {
            timeLeft = initialTime;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

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
        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            if (MainWindow.game.timeLeft <= 0)
            {
                timer.Stop();
                MainWindow.game.timeLeft = -1;
            }
            MainWindow.game.timeLeft -= 1;
            
        }
    }
}
