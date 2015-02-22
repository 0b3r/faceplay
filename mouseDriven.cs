using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using WindowsInput;
using System.IO;
using System.Runtime.InteropServices;

public class mouseDriven
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    public bool _ShouldMouseUp = false;
    public bool _ShouldMouseDown = false;
    public bool _ShouldMouseLeft = false;
    public bool _ShouldMouseRight = false;
    public bool _ShouldLeftClick = false;
    public bool _ShouldRightClick = false;
    public bool _IsLeftClicking = false;
    public bool _IsRightClicking = false;
    public bool _ShouldDoubleClick = false;
    public bool _ShouldScrollUp = false;
    public bool _ShouldScrollDown = false;
    public bool _ShouldRun = true;
    public System.Timers.Timer aTimer;
    public int mouseSens = 10;

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const int MOUSEEVENTF_RIGHTUP = 0x10;

    public void leftClick()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void leftClickDown()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void leftClickUp()
    {
        mouse_event(MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void rightClick()
    {
        mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void rightClickDown()
    {
        mouse_event(MOUSEEVENTF_RIGHTDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void rightClickUp()
    {
        mouse_event(MOUSEEVENTF_RIGHTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void doubleClick()
    {
        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void checkInputs()
    {
        if (InputSimulator.IsKeyDown(VirtualKeyCode.UP))
        {
            _ShouldMouseUp = true;
        }
        else
        {
            _ShouldMouseUp = false;
        }

        if (InputSimulator.IsKeyDown(VirtualKeyCode.DOWN))
        {
            _ShouldMouseDown = true;
        }
        else
        {
            _ShouldMouseDown = false;
        }

        if (InputSimulator.IsKeyDown(VirtualKeyCode.LEFT))
        {
            _ShouldMouseLeft = true;
        }
        else
        {
            _ShouldMouseLeft = false;
        }

        if (InputSimulator.IsKeyDown(VirtualKeyCode.RIGHT))
        {
            _ShouldMouseRight = true;
        }
        else
        {
            _ShouldMouseRight = false;
        }
        if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_1))
        {
            _IsLeftClicking = true;
        }
        if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_2))
        {
            _IsRightClicking = true;
        }
        if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_3))
        {
            _ShouldDoubleClick = true;
        }
        else
        {
            _ShouldDoubleClick = false;
        }

        if (!InputSimulator.IsKeyDown(VirtualKeyCode.VK_1) && _IsLeftClicking)
        {
            _ShouldLeftClick = true;
            _IsLeftClicking = false;
        }

        if (!InputSimulator.IsKeyDown(VirtualKeyCode.VK_2) && _IsRightClicking)
        {
            _ShouldRightClick = true;
            _IsRightClicking = false;
        }

    }

    public void OnTimedEvent(Object source, ElapsedEventArgs e)
    {
        checkInputs();

        if (_ShouldMouseDown && _ShouldMouseUp)
        {
            Console.WriteLine("Cannot move mouse up and down at the same time.");
        }
        else if (_ShouldMouseDown)
        {
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + mouseSens);
        }
        else if (_ShouldMouseUp)
        {
            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - mouseSens);
        }

        if (_ShouldMouseLeft && _ShouldMouseRight)
        {
            Console.WriteLine("Cannot move mouse left and right at the same time.");
        }
        else if (_ShouldMouseLeft)
        {
            Cursor.Position = new Point(Cursor.Position.X - mouseSens, Cursor.Position.Y);
        }
        else if (_ShouldMouseRight)
        {
            Cursor.Position = new Point(Cursor.Position.X + mouseSens, Cursor.Position.Y);
        }

        if (_ShouldLeftClick && _ShouldDoubleClick)
        {
            //NOTE: if someone wants to left click and double click at the same time, just double click
            Console.WriteLine("Attempted to double click and left click at the same time.  Default behavior is simply to left click.");
            doubleClick();
            _ShouldLeftClick = false;
            _ShouldDoubleClick = false;
        }
        else if (_ShouldDoubleClick)
        {
            doubleClick();
            _ShouldDoubleClick = false;
        }
        else if (_ShouldLeftClick)
        {
            leftClick();
            _ShouldLeftClick = false;
        }

        if (_ShouldRightClick)
        {
            rightClick();
            _ShouldRightClick = false;
        }
    }
}
