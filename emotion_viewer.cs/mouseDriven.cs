using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Timers;
using System.IO;
using System.Runtime.InteropServices;

public class mouseDriven
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    //PITCH IS UP AND DOWN
    //ROLL IS TILT SIDEWAYS
    //YAW IS TURN LEFT/RIGHT
    /*public static Single pitch = 0;
    public static Single roll = 0;
    public static Single yaw = 0;
    public static Single prevPitch = 0;
    public static Single prevRoll = 0;
    public static Single prevYaw = 0;
    public static bool fid = true;

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
    public System.Timers.Timer aTimer;*/
    public int mouseSens = 1;

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;
    private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    private const int MOUSEEVENTF_RIGHTUP = 0x10;
    private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    private const int MOUSEEVENTF_MIDDLEUP = 0x0040;

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

    public void middleClickDown()
    {
        mouse_event(MOUSEEVENTF_MIDDLEDOWN, Cursor.Position.X, Cursor.Position.Y, 0, 0);
    }

    public void middleClickUp()
    {
        mouse_event(MOUSEEVENTF_MIDDLEUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
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

    public void exitProgram()
    {
       // InputSimulator.SimulateKeyDown(VirtualKeyCode.RETURN);
    }
    
    public void MoveMouseUp()
    {
        Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - mouseSens);
    }

    public void MoveMouseDown()
    {
        Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + mouseSens);
    }

    public void MoveMouseLeft()
    {
        Cursor.Position = new Point(Cursor.Position.X - mouseSens, Cursor.Position.Y);
    }

    public void MoveMouseRight()
    {
        Cursor.Position = new Point(Cursor.Position.X + mouseSens, Cursor.Position.Y);
    }
}
