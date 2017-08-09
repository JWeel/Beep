using System;
//using System.Collections.Generic;
//using System.Drawing; // Point structure

// TODO
// -generate world
// -possible database functionality => save random generated world seed/parameters to database

namespace Beep {

    // this class is designed to show the generated world
    class BeepConsole {

        // 
        static void Main(string[] args) {
            Console.WriteLine("Hello (generated) world!");
            BeepWorld bw = new BeepWorld(4, 4);
            bw.Show();

            //Beep();

            Console.ReadKey();
        }

        private static void Beep() {
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(659, 500);
            Console.Beep(698, 350);
            Console.Beep(523, 150);
            Console.Beep(415, 500);
            Console.Beep(349, 350);
            Console.Beep(523, 150);
            Console.Beep(440, 1000);
        }
    }
}
