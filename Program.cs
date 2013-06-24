using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;

namespace Agent.Notify
{
    public class Program
    {
        static Bitmap _display;
        static Timer _updateClockTimer;

        static DateTime currentTime = DateTime.Now;

        public static void Main()
        {
            // initialize our display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // display the time immediately
            UpdateTime(null);

            // obtain the current time
            DateTime currentTime = DateTime.Now;
            // set up timer to refresh time every minute
            TimeSpan dueTime = new TimeSpan(0, 0, 0, 59 - currentTime.Second, 1000 - currentTime.Millisecond); // start timer at beginning of next minute
            TimeSpan period = new TimeSpan(0, 0, 0, 1, 0); // update time every minute
            _updateClockTimer = new Timer(UpdateTime, null, dueTime, period); // start our update timer

            // go to sleep; time updates will happen automatically every minute
            Thread.Sleep(Timeout.Infinite);
        }

        static void UpdateTime(object state)
        {
            // obtain the current time
            currentTime = currentTime.AddMinutes(1);
            // clear our display buffer
            _display.Clear();


            Font bigFont = Resources.GetFont(Resources.FontResources.ubuntu12);
            Font smallFont = Resources.GetFont(Resources.FontResources.ubuntu12c);

            // add your watchface drawing code here
            Bitmap hoursPanel = new Bitmap(Resources.GetBytes(Resources.BinaryResources.minima), Bitmap.BitmapImageType.Gif);
            // decide on angle
            int angle = 360 / 12 * (currentTime.Hour % 12);
            Debug.Print(angle.ToString());
            // copy with rotation
            _display.RotateImage(angle, 0, 0, hoursPanel, 0, 0, _display.Width, _display.Height, 0x00);

            // add data
            _display.DrawTextInRect(
                currentTime.ToString("dd MMM").ToUpper(),
                32, 32, 64, 22, Bitmap.DT_AlignmentCenter,
                Color.White,
                bigFont
                );

            // add notif count
            _display.DrawImage(34, 56, new Bitmap(Resources.GetBytes(Resources.BinaryResources.icons), Bitmap.BitmapImageType.Gif), 0, 0, 64, 20);
            _display.DrawText("12",  smallFont, Color.White, 38,  72);
            _display.DrawText("101", smallFont, Color.White, 60,  72);
            _display.DrawText("9",   smallFont, Color.White, 86,  72);

            // draw numerals
            _display.DrawText("12", bigFont, Color.White, _display.Width / 2 - 8, 2);
            _display.DrawText("3", bigFont, Color.White, _display.Width - 13, _display.Height / 2 - 10);
            _display.DrawText("6", bigFont, Color.White, _display.Width / 2 - 4, _display.Height - 20);
            _display.DrawText("9", bigFont, Color.White, 6, _display.Height / 2 - 10);


            // draw minutes circle
            _display.DrawEllipse(Color.White, 1, _display.Width/2, _display.Height/2, 60, 60, 0, 0, 0, 0, 0, 0, 0 );

            // draw minutes marker
            DrawMinutePointer(currentTime.Minute);

            // flush the display buffer to the display
            _display.Flush();
        }

        private static void DrawMinutePointer(int minutes)
        {
            // get start coordinates
            float angleInDegrees = 360/60 * minutes - 90;

            //const int INNER_RADIUS = 43;
            //const int OUTER_RADIUS = 53;

            int start_x =  (int) (60*System.Math.Cos(angleInDegrees*System.Math.PI/180F)) + _display.Width/2;
            int start_y =  (int) (60*System.Math.Sin(angleInDegrees*System.Math.PI/180F)) + _display.Height/2;

            //int start_x = (int)(INNER_RADIUS * System.Math.Cos(angleInDegrees * System.Math.PI / 180F)) + _display.Width / 2;
            //int start_y = (int)(INNER_RADIUS * System.Math.Sin(angleInDegrees * System.Math.PI / 180F)) + _display.Height / 2;

            //int end_x =  (int) (OUTER_RADIUS*System.Math.Cos(angleInDegrees*System.Math.PI/180F)) + _display.Width/2;
            //int end_y =  (int) (OUTER_RADIUS*System.Math.Sin(angleInDegrees*System.Math.PI/180F)) + _display.Height/2;

            // _display.DrawLine(Color.White, 2, start_x, start_y, end_x, end_y);
            _display.DrawEllipse(Color.White, 0, start_x, start_y, 5, 5, Color.White, 0, 0, Color.White, 10, 10, 0xFF);

        }

    }
}
