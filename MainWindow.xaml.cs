using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;

namespace Jaa_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Random color generator
        /// </summary>
        private Random _random;


        /// <summary>
        /// Whether to use the time limit
        /// </summary>
        private bool timeLimit = false;

        /// <summary>
        /// Whether to use foreign letters
        /// </summary>
        private bool foreignLetters = false;

        /// <summary>
        /// The standard alphabet
        /// </summary>
        private string[] alphabet = new string[] { "A", "B", "D", "E", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "Õ", "Ä", "Ö", "Ü" };

        /// <summary>
        /// The foreign alphabet
        /// </summary>
        private string[] foreignAlphabet = new string[] { "C", "F", "Q", "Š", "Z", "Ž", "W", "X", "Y"};

        /// <summary>
        /// The join of the standard and the foreign alphabet
        /// </summary>
        private string[] combinedAlphabet = new string[] { "C", "F", "Q", "Š", "Z", "Ž", "W", "X", "Y","A", "B", "D", "E", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "V", "Õ", "Ä", "Ö", "Ü" };


        /// <summary>
        /// The number of guessed words
        /// </summary>
        private int wordCount = 0;

        /// <summary>
        /// Holds player points
        /// </summary>
        private int points = 0;

        /// <summary>
        /// Global multiplier for points calculation formula
        /// </summary>
        private const double pointsMultiplier = 4.2;

        public static Game game = new Game();

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            lblTimeLeft.DataContext = game;
            _random = new Random(22);
            Reset();
           }

        /// <summary>
        /// Returns a random pastel color
        /// </summary>
        /// <see cref="http://blog.functionalfun.net/2008/07/random-pastel-colour-generator.html"/>
        /// <returns>Random brush</returns>
        public SolidColorBrush RandomBrush()
        {
            // to create lighter colours:
            // take a random integer between 0 & 128 (rather than between 0 and 255)
            // and then add 127 to make the colour lighter
            byte[] colorBytes = new byte[3];
            colorBytes[0] = (byte)(_random.Next(128) + 127);
            colorBytes[1] = (byte)(_random.Next(128) + 127);
            colorBytes[2] = (byte)(_random.Next(128) + 127);

            Color color = new Color();

            // make the color fully opaque
            color.A = 255;
            color.R = colorBytes[0];
            color.B = colorBytes[1];
            color.G = colorBytes[2];

            return new SolidColorBrush(color);
        }

        /// <summary>
        /// Focus input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_Loaded(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void entry_KeyDown(object sender, KeyEventArgs e)
        {
            string guess = entry.Text.Trim();

            if (guess.Length == 0 || guess.Length == 1)
            {
                return;
            }

            // Timer enabled but not yet started
            if (timeLimit && game.timeLeft == -1)
            {
                game.StartTimer();
            }

            // Enter / space pressed
            if (e.Key.Equals(Key.Enter))
            {
                SaveHistory();
                AddPoints();

                letter.Foreground = RandomBrush();
                SwitchLetter();

                // Limit
                if (wordCount > 998)
                {
                    MessageBox.Show("Hei...oli tore...aga aitab nüüd ka!");
                    entry.IsEnabled = false;
                }
                wordCount += 1;
                lblWordCounter.Content = wordCount.ToString();


                // Reset timer
                if (timeLimit)
                {
                    game.RenewTime();
                }
            }
        }

        /// <summary>
        /// Add points according to the formula
        /// </summary>
        private void AddPoints()
        {
            double multiply = pointsMultiplier;
            double newPoints = 0;

            // More points for higher stakes
            if (foreignLetters)
            {
                multiply += pointsMultiplier * 0.20;
            }

            // More points for timelimit
            if (timeLimit)
            {
                multiply += pointsMultiplier * 0.35;
            }

            newPoints += Math.Ceiling((entry.Text.Length / 2.2)*multiply);
            points += (int)newPoints;
            lblPoints.Content = points.ToString();
        }

        /// <summary>
        /// Reset the state
        /// </summary>
        private void Reset()
        {
            entry.IsEnabled = true;
            entry.Focus();
            letter.Foreground = RandomBrush();
            wordCount = 0;
            game.StopTimer();
            game.timeLeft = -1;
            timeLimit = false;
            foreignLetters = false;
            checkBox1.IsChecked = false;
            checkBox2.IsChecked = false;
            history.Text = "";
            points = 0;
            lblPoints.Content = "0";
            lblWordCounter.Content = "0";
        }

        /// <summary>
        /// Switch the current letter to a next one
        /// </summary>
        private void SwitchLetter()
        {
            string nextLetter = NextLetter();
            letter.Content = nextLetter;
            entry.Text = nextLetter;
            entry.Focus();
        }

        /// <summary>
        /// Save history
        /// </summary>
        private void SaveHistory()
        {
            if (history.Text.Length > 0)
            {
                history.Text += " -> ";
            }
            history.Text += letter.Content+": "+entry.Text.Trim();
        }

        /// <summary>
        /// Get a random letter
        /// </summary>
        /// <returns>A random alphabet letter from either the standard or combined alphabet</returns>
        private string NextLetter()
        {           
            System.Random RandNum = new System.Random();
            return foreignLetters ? combinedAlphabet[RandNum.Next(0, combinedAlphabet.Length-1)] : alphabet[RandNum.Next(0, alphabet.Length-1)];
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        /// <summary>
        /// Help window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Jaa_.Help help = new Jaa_.Help();
            help.Show();
        }

        /// <summary>
        /// Time limit ON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            timeLimit = true;
            lblTimeLeft.Visibility = Visibility.Visible;
            label1.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Time limit OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            timeLimit = false;
            lblTimeLeft.Visibility = Visibility.Hidden;
            label1.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Foreign letters OFF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            foreignLetters = false;
        }

        /// <summary>
        /// Foreign letters ON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            foreignLetters = true;
        }

        /// <summary>
        /// Reset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

      
    }
}
