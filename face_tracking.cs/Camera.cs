using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace face_tracking.cs
{
    class Camera
    {
        private bool useMouse = true;
        private bool canUseMouse = true;
        private int mouseSens = 1;
        private float upLimit = 20;
        private float downLimit = -20;
        private float leftLimit = -50;
        private float rightLimit = 50;
        private float tiltLeftLimit = -30;
        private float tiltRightLimit = 30;

        public Camera(bool mode, int cfgSens, float cfgUp, float cfgDown, float cfgLeft, float cfgRight, float cfgTiltLeft, float cfgTiltRight)
        {
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

        public void OnHeadCenter(mouseDriven mouse)
        {
            if(useMouse)
            {
                mouse._ShouldMouseUp = false;
                mouse._ShouldMouseDown = false;
                mouse._ShouldMouseLeft = false;
                mouse._ShouldMouseRight = false;
            }
        }

        public void OnHeadTurnLeft(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if(useMouse)
            {
                mouse._ShouldMouseLeft = true;
            }
            else
            {
                Console.WriteLine("TAB FORWARD");
                selen.TabForward();
                //call faceplaywebdriver tabforward
            }
        }

        public void OnHeadTurnRight(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseRight = true;
            }
            else
            {
                Console.WriteLine("TAB BACK");
                selen.TabBackward();
                //call faceplaywebdriver tabbackward
            }
        }

        public void OnHeadTurnUp(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseUp = true;
            }
            else
            {
                Console.WriteLine("SCROLL UP");
                selen.ScrollUp();
                //call faceplaywebdriver SCROLL UP
            }
        }

        public void OnHeadTurnDown(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseDown = true;
            }
            else
            {
                Console.WriteLine("SCROLL Down");
                selen.ScrollDown();
                //call faceplaywebdriver SCROLL DOWN
            }
        }

        public void OnHeadLeft(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseLeft = true;
            }
            else
            {
                Console.WriteLine("TAB FORWARD");
                selen.TabForward();
                //call faceplaywebdriver tabforward
            }
        }

        public void OnHeadRight(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseRight = true;
            }
            else
            {
                Console.WriteLine("TAB BACK");
                selen.TabBackward();
                //call faceplaywebdriver tabbackward
            }
        }

        public void OnHeadUp(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseUp = true;
            }
            else
            {
                Console.WriteLine("SCROLL UP");
                selen.ScrollUp();
                //call faceplaywebdriver SCROLL UP
            }
        }

        public void OnHeadDown(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if (useMouse)
            {
                mouse._ShouldMouseDown = true;
            }
            else
            {
                Console.WriteLine("SCROLL Down");
                selen.ScrollDown();
                //call faceplaywebdriver SCROLL DOWN
            }
        }

        public void OnSmile(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if(useMouse)
            {
                mouse._ShouldLeftClick = true;
            }
            else
            {
                Console.WriteLine("FOCUS LOCATION BAR");
                selen.FocusLocationBar();
                //call faceplaywebdriver focuslocationbar
            }
        }

        public void OnSurprise(mouseDriven mouse, FaceplayWebdriver.Webdriver selen)
        {
            if(useMouse)
            {
                mouse._ShouldRightClick = true;
            }
            else
            {
                Console.WriteLine("FOCUS LOCATION BAR");
                selen.FocusLocationBar();
                //call faceplaywebdriver focuslocationbar
            }
        }

        public void Configure()
        {

        }
    }
}
