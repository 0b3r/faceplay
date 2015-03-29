    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

namespace Emotion_Detection
{
        class Camera
        {
            public static Single pitch = 0;
            public static Single roll = 0;
            public static Single yaw = 0;
            public static Single x = 0;
            public static Single y = 0;
            public static int timer = 1;
            public static float stopX = 0f;
            public static float stopY = 0f;

            public Single centerX = 0f;
            public Single centerY = 0f;
            public int iterationsX = 0;
            public int iterationsY = 0;
            public int currentTicks = 0;
            public int configureTicks = 500;


            public static bool shouldSmile = false;
            public static bool shouldSurprise = false;
            public static bool shouldConfigure = false;

            private bool isSmiling = false;
            private bool isSurprise = false;
            public static bool shouldNeutral = true;
            public bool configureMode = false;

            private bool useMouse = true;
            private bool canUseMouse = true;
            private bool headCentered = true;
            private int mouseSens = 1;
            private float upLimit = 20;
            private float downLimit = -20;
            private float leftLimit = -50;
            private float rightLimit = 50;


            public System.Timers.Timer aTimer;
            mouseDriven mouse;
            Webdriver selenium;

            public Camera(mouseDriven mou, Webdriver selen, bool mode, int cfgSens)
            {
                mouse = mou;
                selenium = selen;
                //this.Configure();
                Console.WriteLine("Camera constructor");
                useMouse = mode;
                if(!canUseMouse)
                {
                    useMouse = false;
                }
                mouseSens = cfgSens;
                if(!mode)
                {
                    timer = 100;
                }
            }

            public void ChangeMode()
            {
                if (canUseMouse)
                {
                    useMouse = !useMouse;
                    if(useMouse)
                    {
                        timer = 1;
                        aTimer = new System.Timers.Timer(timer);
                        aTimer.Elapsed += OnTimedEvent;
                        aTimer.AutoReset = true;
                        Console.WriteLine("The timer should fire every {0} milliseconds.",
                             aTimer.Interval);
                        aTimer.Enabled = true;
                    }
                    else
                    {
                        timer = 100;
                        aTimer = new System.Timers.Timer(timer);
                        aTimer.Elapsed += OnTimedEvent;
                        aTimer.AutoReset = true;
                        Console.WriteLine("The timer should fire every {0} milliseconds.",
                             aTimer.Interval);
                        aTimer.Enabled = true;
                    }
                }
                else
                {
                    useMouse = false;
                    timer = 100;
                    aTimer = new System.Timers.Timer(timer);
                }

            }

            //This will also increment the iterations so that the iterations don't need to be updated outside of the function
            public Single changeAverage(Single nextPoint, Single previousAverage, ref int iterations)
            {
                Single sum = nextPoint + (previousAverage * iterations);
                iterations++;
                return sum / iterations;
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
                    mouse.leftClick();
                }
                else
                {
                    selenium.FocusLocationBar();
                    //call faceplaywebdriver focuslocationbar
                    headCentered = false;
                }
                isSmiling = true;
                shouldNeutral = false;
            }

            public void OnSurprise()
            {
                if(useMouse)
                {
                    mouse.rightClick();
                }
                else
                {
                    selenium.FocusLocationBar();
                    //call faceplaywebdriver focuslocationbar
                    headCentered = false;
                }
                isSurprise = true;
                shouldNeutral = false;
            }

            public void OnNeutral()
            {
                isSmiling = false;
                isSurprise = false;
            }

            public void OnConfigure()
            {
                centerX = 0;
                centerY = 0;
                iterationsX = 0;
                iterationsY = 0;
                currentTicks = 0;
                configureMode = true;
            }

            public void Configure()
            {
                //May want to make ticker static in Program and increment using that variable rather than this hardcode
                if(useMouse)
                {
                    currentTicks++;
                }
                else
                {
                    currentTicks += 10;
                }

                centerX = changeAverage(x, centerX, ref iterationsX);
                centerY = changeAverage(y, centerY, ref iterationsY);
            }

            public void exitProgram()
            {
                return;
            }

            public void checkInputs()
            {
                if (y <= upLimit)
                {
                    OnHeadUp();
                }

                if (y >= downLimit)
                {
                    OnHeadDown();
                }

                if (x >= leftLimit)
                {
                    OnHeadLeft();
                }

                if (x <= rightLimit)
                {
                    OnHeadRight();
                }
                
                if(x < leftLimit && x > rightLimit && y > upLimit && y < downLimit)
                {
                    OnHeadCenter();
                }

                if(shouldSmile)
                {
                    if(!isSmiling)
                    {
                        OnSmile();
                    }
                }

                if(shouldSurprise)
                {
                    if(!isSurprise)
                    {
                        OnSurprise();
                    }
                }

                if(shouldNeutral)
                {
                    OnNeutral();
                }
            }

            public void OnTimedEvent(Object source, ElapsedEventArgs e)
            {
                // Console.WriteLine("PITCH: " + mouseDriven.pitch + " YAW: " + mouseDriven.yaw + " ROLL: " + mouseDriven.roll);
                if(shouldConfigure)
                {
                    shouldConfigure = false;
                    OnConfigure();
                }
                if(configureMode)
                {
                    Console.WriteLine(currentTicks);
                    if(currentTicks >= configureTicks)
                    {
                        upLimit = centerY - 20;
                        downLimit = centerY + 20;
                        leftLimit = centerX + 20;
                        rightLimit = centerX - 20;
                        stopX = leftLimit - 1;
                        stopY = upLimit + 1;
                        configureMode = false;
                        EmotionDetection.form.UpdateStatus("Streaming");
                    }
                    else
                    {
                        Configure();
                        return;
                    }
                }
                checkInputs();
            }
        }
    }
