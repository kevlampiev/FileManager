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
            
            FMController fMController = new FMController(WINDOW_HEIGHT, WINDOW_WIDTH);
            fMController.Run();
            fMController.Done();

        }
    }
}