namespace KEvlampiev.TerminalForms
{
    public class ConsoleWindow:ConsoleElement
    {
        public char _leftTopCornerSymbol = '╔';
        public char _rightTopCornerSymbol = '╗';
        public char _leftBottomCornerSymbol = '╚';
        public char _rightBottomCornerSymbol = '╝';
        public char _horizontalFrameSymbol = '═';
        public char _verticalFrameSymbol = '║';
        public char _startTitleChar = '╡';
        public char _endTitleChar = '╞';

        //TODO уточнить а можно ли написать такой get
        public string Title { get; set; }

        

        public ConsoleWindow(int left, int top, int width, int height, string title):base(left, top, width, height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Color = ConsoleColor.Cyan;
            BackgroundColor = ConsoleColor.Blue;
            Title = title;
        }
        override public void Repaint() 
        {
            DrawFrame();
            DrawTitle();
            Console.ResetColor();
        }


        private void DrawFrame() 
        {
            Console.ForegroundColor = Color;
            Console.BackgroundColor = BackgroundColor;
            string strToDraw = "";
            for (int i = Top; i <= Bottom; i++) 
            {
                if (i == Top)
                {
                    strToDraw = _leftTopCornerSymbol.ToString().PadRight(Width - 2, _horizontalFrameSymbol) +
               _rightTopCornerSymbol;
                }
                else if (i == Bottom)
                {
                    strToDraw = _leftBottomCornerSymbol.ToString().PadRight(Width - 2, _horizontalFrameSymbol) +
               _rightBottomCornerSymbol;
                }
                else {
                    strToDraw = _verticalFrameSymbol.ToString().PadRight(Width - 2, ' ') + _verticalFrameSymbol;
                }
                Console.SetCursorPosition(Left, i);
                Console.WriteLine(strToDraw);
            }
        }

        private void DrawTitle() 
        {
            Console.ForegroundColor = Color;
            Console.BackgroundColor = BackgroundColor;
            int startTitlePosition = Math.Max(MiddleHorizontal - Title.Length / 2 - 2, Left +1);
            int titleLength = Math.Min(Title.Length, Width-5);
            string resultString = _startTitleChar + Title.Substring(0, titleLength) + _endTitleChar;
            Console.SetCursorPosition(startTitlePosition, Top);
            Console.Write(resultString);
        }

       


    }
}