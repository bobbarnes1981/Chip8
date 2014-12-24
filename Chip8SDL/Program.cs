using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Core;
using SdlDotNet.Graphics;

namespace Chip8SDL
{
    class Program
    {
        private static Chip8Library.Computer m_computer;

        private static Surface m_video;

        private static int m_width = 64;

        private static int m_height = 32;

        private static int m_scale = 2;

        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                m_computer = new Chip8Library.Computer();
                m_computer.Load(0x0200, File.ReadAllBytes(args[0]));

                // todo: this needs to be better
                new System.Threading.Thread(m_computer.Run).Start();

                m_video = Video.SetVideoMode(m_width * m_scale, m_height * m_scale, 32, false, false, false, true);
                Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);
                Events.Tick += new EventHandler<TickEventArgs>(ApplicationTickEventHandler);
                Events.Run();

                m_computer.Stop();
            }
            else
            {
                Console.WriteLine("Usage: Chip8SDL <filename>");
            }
        }

        private static void ApplicationTickEventHandler(object sender, TickEventArgs args)
        {
            for (int y = 0; y < m_height; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    int xa = x * m_scale;
                    int xb = (x * m_scale) + m_scale;
                    int ya = y * m_scale;
                    int yb = (y * m_scale) + m_scale;
                    if (m_computer.Video.Read(x, y))
                    {
                        m_video.Draw(new SdlDotNet.Graphics.Primitives.Box(new Point(xa, ya), new Point(xb, yb)), Color.White, false, true);
                    }
                    else
                    {
                        m_video.Draw(new SdlDotNet.Graphics.Primitives.Box(new Point(xa, ya), new Point(xb, yb)), Color.Black, false, true);
                    }
                }
            }
            m_video.Update();
        }

        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
    }
}
