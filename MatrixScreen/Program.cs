using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixScreen
{

    class FallingString
    {
        int col;        // Column the string is falling in
        int row;        // Current row to start drawing in (goes up as we go left to right)
        int rate;       // Rate at which the string falls (bigger value = slower)
        String str;     // String of characters that is falling

        public FallingString( Random rand )
        {
            this.Reset( rand );

            return;
        }
        
        void Reset( Random rand )
        {
            int size;

            this.row = 0;
            this.col = rand.Next(Console.WindowWidth);

            // Prevent falling string in last column, because it will cause the screen to 
            // to scroll
            if( this.col == (Console.WindowWidth -1 ))
            {
                --this.col;
            }

            // Set the size of the string
            size = (rand.Next(5, Console.WindowHeight)) / 2;

            // Fill the string with random characters
            for (int ii = 0; ii < size; ++ii)
            {
                this.str += (char)rand.Next(32, 256);
            }

            this.rate = 10 * ( rand.Next(1,11));

            return;
        }

        public void Update( UInt32 count, Random rand )
        {
            if(( count % this.rate ) == 0 )
            {
                Console.ForegroundColor = ConsoleColor.Green;

                // Print the string from the current row up the screen (stop at the top)
                for (int jj = 0; jj < this.str.Length; ++jj)
                {
                    Console.SetCursorPosition(this.col, this.row - jj);
                    Console.Write(this.str[jj]);

                    if ((this.row - jj) == 0)
                        break;
                }

                // If the row has reached bottom, we want to have the string "disappear" off the bottom
                // as it moves down, so we keep the row at the bottom, but remove the first character
                if( this.row == (Console.WindowHeight - 1))
                {
                    if (this.str.Length > 0)
                        this.str = this.str.Substring(1);
                    else
                    {
                        this.Reset( rand );
                    }
                }
                else
                {
                    ++this.row;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.SetWindowSize( 80, 40 );
            Console.SetBufferSize( 80, 40 );

            Console.CursorVisible = false;

            int col, row;

            UInt32 counter = 0;

            Random rand = new Random();

            Console.WriteLine("Press ESC to stop");

            FallingString fs1 = new FallingString(rand);
            FallingString fs2 = new FallingString(rand);
            FallingString fs3 = new FallingString(rand);
            FallingString fs4 = new FallingString(rand);

            // Loop until 'ESC' key is hit
            do
            {
                while (!Console.KeyAvailable)
                {
                    // Manage the random full window 1's and 0's "background"
                    // Set the cursor position of the next character
                    col = rand.Next((int)(Console.WindowWidth));
                    row = rand.Next((int)(Console.WindowHeight));

                    // Prevent scrolling of window by printing in the last col and last row
                    if(( col == (Console.WindowWidth - 1)) && ( row == (Console.WindowHeight -1 )))
                    {
                        --col;
                        --row;
                    }

                    Console.SetCursorPosition(col, row);

                    // Select the color between green and dark green
                    if (rand.Next(10) > 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }

                    // Decide whether a 1, 0, or space gets placed.  Spaces should be placed about 75% of the time.
                    if (( counter % 5 ) == 0 )
                    {
                        int n = rand.Next(2);
                        Console.Write(n);
                    }
                    else
                    {
                        Console.Write(' ');
                    }

                    // Update the falling strings
                    fs1.Update(counter, rand );
                    fs2.Update(counter, rand);
                    fs3.Update(counter, rand);
                    fs4.Update(counter, rand);

                    ++counter;
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
