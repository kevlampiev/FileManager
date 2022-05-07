using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{
    public static class Utils
    {
        public static (int row, int col) GetCursorPosition()
        {
            return (Console.CursorLeft, Console.CursorTop);
        }

        public static void DisplayError(string message)
        {
            ErrorWindow errorWindow = new ErrorWindow(message);
            errorWindow.Repaint();
            Console.CursorVisible = false;
            Console.ReadKey(true);
            Console.CursorVisible = true;
            
        }
    }
}
