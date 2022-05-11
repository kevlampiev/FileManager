using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KEvlampiev.TerminalForms;
using System.Text.Json;


namespace FileManager
{
    internal class Program
    {
        const int WINDOW_HEIGHT = 30;
        const int WINDOW_WIDTH = 120;

        const string SETTINGS_FILE = "FileManager.settings.json";

 
        static void Main(string[] arg)
        {
            FMController fMController;
            //Это очень важное дополнение 
            if (!File.Exists(SETTINGS_FILE)) 
            {
                 fMController = new FMController(WINDOW_HEIGHT, WINDOW_WIDTH);
            } 
            else 
            { 
                string json = File.ReadAllText(SETTINGS_FILE);
                Settings settings = JsonSerializer.Deserialize<Settings>(json);
                fMController = new FMController(settings);
            }
            
            fMController.Run();
            fMController.Done();

        }
    }
}