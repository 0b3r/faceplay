using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace face_tracking.cs
{
    class Camera
    {
        public static Single pitch = 0;
        public static Single roll = 0;
        public static Single yaw = 0;
        public static Single x = 50;
        public static Single y = 50;

        private bool useMouse = true;
        private bool canUseMouse = true;
        private bool headCentered = true;
        private int mouseSens = 1;
        private float upLimit = 20;
        private float downLimit = -20;
        private float leftLimit = -50;
        private float rightLimit = 50;
        private float tiltLeftLimit = -30;
        private float tiltRightLimit = 30;
        public System.Timers.Timer aTimer;
        mouseDriven mouse;
        Webdriver selenium;

        public Camera(mouseDriven mou, Webdriver selen, bool mode, int cfgSens, float cfgUp, float cfgDown, float cfgLeft, float cfgRight, float cfgTiltLeft, float cfgTiltRight)
        {
            mouse = mou;
            selenium = selen;
            this.Configure();
            Console.WriteLine("Camera constructor");
            useMouse = mode;
            if(!canUseMouse)
            {
                useMouse = false;
            }
            mouseSens = cfgSens;
            upLimit = cfgUp;
            downLimit = cfgDown;
            leftLimit = cfgLeft;
            rightLimit = cfgRight;
            tiltLeftLimit = cfgTiltLeft;
            tiltRightLimit = cfgTiltRight;

        }

        public void ChangeMode()
        {
            if (canUseMouse)
            {
                useMouse = !useMouse;
            }
            else
            {
                useMouse = false;
            }

        }

        public void OnHeadCenter()
        {
            if(useMouse)
            {
                mouse._ShouldMouseUp = false;
                mouse._ShouldMouseDown = false;
                mouse._ShouldMouseLeft = false;
                mouse._ShouldMouseRight = false;
            }
            else
            {
                headCentered = true;
            }
        }

        public void OnHeadTurnLeft()
        {
            if(useMouse)
            {
                mouse._ShouldMouseLeft = true;
                mouse.MoveMouseLeft();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("TAB FORWARD");
                    selenium.TabForward();
                    //call faceplaywebdriver tabforward
                    headCentered = false;
                }

            }
        }

        public void OnHeadTurnRight()
        {
            if (useMouse)
            {
                mouse._ShouldMouseRight = true;
                mouse.MoveMouseRight();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("TAB BACK");
                    selenium.TabBackward();
                    //call faceplaywebdriver tabbackward
                    headCentered = false;
                }
            }
        }

        public void OnHeadTurnUp()
        {
            if (useMouse)
            {
                mouse._ShouldMouseUp = true;
                mouse.MoveMouseUp();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("SCROLL UP");
                    selenium.ScrollUp();
                    //call faceplaywebdriver SCROLL UP
                    headCentered = false;
                }
            }
        }

        public void OnHeadTurnDown()
        {
            if (useMouse)
            {
                mouse._ShouldMouseDown = true;
                mouse.MoveMouseDown();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("SCROLL Down");
                    selenium.ScrollDown();
                    //call faceplaywebdriver SCROLL DOWN
                    headCentered = false;
                }
            }
        }

        public void OnHeadLeft()
        {
            if (useMouse)
            {
                mouse._ShouldMouseLeft = true;
                mouse.MoveMouseLeft();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("TAB FORWARD");
                    selenium.TabForward();
                    //call faceplaywebdriver tabforward
                    headCentered = false;
                }
            }
        }

        public void OnHeadRight()
        {
            if (useMouse)
            {
                mouse._ShouldMouseRight = true;
                mouse.MoveMouseRight();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("TAB BACK");
                    selenium.TabBackward();
                    //call faceplaywebdriver tabbackward
                    headCentered = false;
                }
            }
        }

        public void OnHeadUp()
        {
            if (useMouse)
            {
                mouse._ShouldMouseUp = true;
                mouse.MoveMouseUp();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("SCROLL UP");
                    selenium.ScrollUp();
                    //call faceplaywebdriver SCROLL UP
                    headCentered = false;
                }
            }
        }

        public void OnHeadDown()
        {
            if (useMouse)
            {
                mouse._ShouldMouseDown = true;
                mouse.MoveMouseDown();
            }
            else
            {
                if(headCentered)
                {
                    Console.WriteLine("SCROLL Down");
                    selenium.ScrollDown();
                    //call faceplaywebdriver SCROLL DOWN
                    headCentered = false;
                }
            }
        }

        public void OnSmile()
        {
            if(useMouse)
            {
                mouse._ShouldLeftClick = true;
            }
            else
            {
                Console.WriteLine("FOCUS LOCATION BAR");
                selenium.FocusLocationBar();
                //call faceplaywebdriver focuslocationbar
                headCentered = false;
            }
        }

        public void OnSurprise()
        {
            if(useMouse)
            {
                mouse._ShouldRightClick = true;
            }
            else
            {
                Console.WriteLine("FOCUS LOCATION BAR");
                selenium.FocusLocationBar();
                //call faceplaywebdriver focuslocationbar
                headCentered = false;
            }
        }

        public void Configure()
        {

        }

        public void exitProgram()
        {
            return;
        }

        public void checkInputs()
        {
            if (/*InputSimulator.IsKeyDown(VirtualKeyCode.UP) || */y <= 45)
            {
                OnHeadUp();
            }
            else
            {
                OnHeadCenter();
            }

            if (/*InputSimulator.IsKeyDown(VirtualKeyCode.DOWN) || */y >= 65)
            {
                OnHeadDown();
            }
            else
            {
                OnHeadCenter();
            }

            if (/*InputSimulator.IsKeyDown(VirtualKeyCode.LEFT) || */x >= 70)
            {
                OnHeadLeft();
            }
            else
            {
                OnHeadCenter();
            }

            if (/*InputSimulator.IsKeyDown(VirtualKeyCode.RIGHT) || */x <= 30)
            {
                OnHeadRight();
            }
            else
            {
                OnHeadCenter();
            }
            /*if (InputSimulator.IsKeyDown(VirtualKeyCode.VK_1))
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
            }*/

        }

        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

            // Console.WriteLine("PITCH: " + mouseDriven.pitch + " YAW: " + mouseDriven.yaw + " ROLL: " + mouseDriven.roll);

            checkInputs();
            Console.WriteLine("checkInputs");
            /*if (_ShouldMouseDown && _ShouldMouseUp)
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
            }*/
        }
    }
}
