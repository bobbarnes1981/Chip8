using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace Chip8SDL
{
    class Program
    {
        private static Chip8Library.Computer m_computer;

        private static Surface m_video;

        private static int m_width = 64;

        private static int m_height = 32;

        private static int m_scale = 8;

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
                Events.KeyboardDown += new EventHandler<KeyboardEventArgs>(ApplicationKeyboardEventHandler);
                Events.KeyboardUp += new EventHandler<KeyboardEventArgs>(ApplicationKeyboardEventHandler);
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

        private static void ApplicationKeyboardEventHandler(object sender, KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Key.X:
                    m_computer.SetKey(0x00, args.Down);
                    break;
                case Key.Keypad1:
                    m_computer.SetKey(0x01, args.Down);
                    break;
                case Key.Keypad2:
                    m_computer.SetKey(0x02, args.Down);
                    break;
                case Key.Keypad3:
                    m_computer.SetKey(0x03, args.Down);
                    break;
                case Key.Q:
                    m_computer.SetKey(0x04, args.Down);
                    break;
                case Key.W:
                    m_computer.SetKey(0x05, args.Down);
                    break;
                case Key.E:
                    m_computer.SetKey(0x06, args.Down);
                    break;
                case Key.A:
                    m_computer.SetKey(0x07, args.Down);
                    break;
                case Key.S:
                    m_computer.SetKey(0x08, args.Down);
                    break;
                case Key.D:
                    m_computer.SetKey(0x09, args.Down);
                    break;
                case Key.Z:
                    m_computer.SetKey(0x0A, args.Down);
                    break;
                case Key.C:
                    m_computer.SetKey(0x0B, args.Down);
                    break;
                case Key.Keypad4:
                    m_computer.SetKey(0x0C, args.Down);
                    break;
                case Key.R:
                    m_computer.SetKey(0x0D, args.Down);
                    break;
                case Key.F:
                    m_computer.SetKey(0x0E, args.Down);
                    break;
                case Key.V:
                    m_computer.SetKey(0x0F, args.Down);
                    break;
            }
        }

        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
    }
}
