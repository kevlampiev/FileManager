using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KEvlampiev.TerminalForms;

namespace FileManager
{
    internal class Program
    {
        const int WINDOW_HEIGHT = 30;
        const int WINDOW_WIDTH = 120;

        static void Main(string[] arg)
        {
            Console.Title = "File manager";
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            Console.SetBufferSize(WINDOW_WIDTH, WINDOW_HEIGHT);

            ConsoleWindow dirTree = new ConsoleWindow(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT-10, "Directory tree");
            ConsoleWindow infoPanel = new ConsoleWindow(0, WINDOW_HEIGHT-10, WINDOW_WIDTH, 8, "Info");
            BashTerminal bash = new BashTerminal(0, WINDOW_HEIGHT-1, WINDOW_WIDTH, @"c:\" );
            Console.Clear();
            dirTree.Repaint();
            infoPanel.Repaint();
            bash.Repaint();

            Console.ReadKey(true);

        }
    }
}