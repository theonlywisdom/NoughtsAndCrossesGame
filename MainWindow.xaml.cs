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

namespace NoughtsAndCrossesGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Members
        /// <summary>
        /// Holds the current results of cells in the active game
        /// </summary>
        private MarkType[] mResults;

        /// <summary>
        /// True if it is Player1's turn(X) or Player2's turn (O)
        /// </summary>
        private bool mPlayer1Turn;

        /// <summary>
        /// True if game has ended
        /// </summary>
        private bool mGameEnded;
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }

        #endregion

        // Starts new game clears all values
        private void NewGame()
        {
            // Create a new blank array of free cells
            mResults = new MarkType[9];

            for (var i = 0; i < mResults.Length; i++)
            {
                mResults[i] = MarkType.Free;
            }

            // Make sure Player1 starts the game
            mPlayer1Turn = true;

            // Cast container UI elements to a button
            // Add iterate every button on the grid
            Container.Children.Cast<Button>().ToList().ForEach(
                button =>
                {
                    // Change background, foreground and content to default values
                    button.Content = string.Empty;
                    button.Background = Brushes.White;
                    button.Foreground = Brushes.Blue;
                });

            // Make sure the game hasn't finished
            mGameEnded = false;
        }

        /// <summary>
        /// Handles a button click event
        /// </summary>
        /// <param name="sender">The button that was clicked</param>
        /// <param name="e">The event triggered by the click</param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Start a new game on the click after it finished
            if (mGameEnded)
            {
                NewGame();
                return;
            }

            // Cast the sender to a button
            var button = (Button)sender;

            // Find the button's position in the  array
            var column = Grid.GetColumn(button);
            var row = Grid.GetRow(button);

            var index = column + (row * 3);

            // Don't do anything if the cell already has a value in it
            if (mResults[index] != MarkType.Free)
                return;

            // Set the value based on which player's turn it is
            mResults[index] = mPlayer1Turn ? MarkType.Cross : MarkType.Nought;


            // Set the button content to the result
            button.Content = mPlayer1Turn ? "×" : "○";

            // Change noughts to green
            if (!mPlayer1Turn)
                button.Foreground = Brushes.Red;

            // Toggle the players' turns 
            mPlayer1Turn ^= true;

            // Check for a winner
            CheckForWinner();
        }
        
        // Check if there is a winner of a 3 line straight
        private void CheckForWinner()
        {
            #region Horizontal Wins

            // Check for horizontal wins
            //
            // - Row 0
            //
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[1] & mResults[2]) == mResults[0])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.LightGreen;
            }

            // Check for horizontal wins
            //
            // - Row 1
            //
            if (mResults[3] != MarkType.Free && (mResults[3] & mResults[4] & mResults[5]) == mResults[3])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.LightGreen;
            }

            // Check for horizontal wins
            //
            // - Row 2
            //
            if (mResults[6] != MarkType.Free && (mResults[6] & mResults[7] & mResults[8]) == mResults[6])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.LightGreen;
            }

            #endregion

            #region Vertical Wins
            // Check for vertical wins
            //
            // - Column 0
            //
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[3] & mResults[6]) == mResults[0])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.LightGreen;
            }

            // Check for vertical wins
            //
            // - Column 1
            //
            if (mResults[1] != MarkType.Free && (mResults[1] & mResults[4] & mResults[7]) == mResults[1])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.LightGreen;
            }

            // Check for vertical wins
            //
            // - Column 2
            //
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[5] & mResults[8]) == mResults[2])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.LightGreen;
            }

            #endregion

            #region Diagonal Wins
            // Check for diagonal wins
            //
            // - Top Left to Bottom Right
            //
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[4] & mResults[8]) == mResults[0])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.LightGreen;
            }

            // Check for diagonal wins
            //
            // - Top Right to Bottom Left
            //
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[4] & mResults[6]) == mResults[2])
            {
                // Game ends
                mGameEnded = true;

                // Highlight winning cells in gree
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.LightGreen;
            }

            #endregion

            #region No Winners
            // Check for no winner a and full board
            if (!mResults.Any(result => result == MarkType.Free))
            {
                // Game ended
                mGameEnded = true;

                // Turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(
                button =>
                {
                    // Change background, foreground and content to default values
                    button.Background = Brushes.Orange;
                });
            }

            #endregion
        }
    }
}
