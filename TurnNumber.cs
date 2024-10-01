using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_work
{
    public class TurnNumber // Class keeps track of what turn is.
    {
        public int iTurnNumber = 1;

        public void AddTurn() // Function to  increase turn number.
        {
            iTurnNumber++;
        }

        public int GetTurnNumber() // Function to get current turn number.
        {
            return iTurnNumber;
        }

        public void ResetTurnNumber() // Function to reset turn number back to 1.
        {
            iTurnNumber = 1;
        }
    }
}
