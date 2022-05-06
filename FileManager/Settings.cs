using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Настройки программы
    /// </summary>
    public class Settings
    {
        //размер окна и буфера для отображения
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        //Текущая директория
        public string CurrentDirectoryStr { get; set; }

        public Settings(int wHeight, int wWidth, string dir)
        { 
            WindowHeight = wHeight;
            WindowWidth = wWidth;   
            CurrentDirectoryStr = dir;
        }

        public Settings() { }
    }
}
