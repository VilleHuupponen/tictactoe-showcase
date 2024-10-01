using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_work
{
    public class ComputerOpponent
    {
        public static bool bIsInPlay = false; // By default computer opponent is not in play.

        public void ComputerInPlay() // A function to put computer opponent in play.
        {
            bIsInPlay = true;
        }

        public void ComputerOutPlay() // A function to get computer opponent out of the game.
        {
            bIsInPlay = false;
        }

        public bool IsComputerInPlay() // A function to ask if the computer opponent is in play.
        {
            return bIsInPlay;
        }
    }
}
