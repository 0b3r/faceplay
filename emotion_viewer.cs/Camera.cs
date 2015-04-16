    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Timers;

namespace Emotion_Detection
{
        class Camera
        {
            public static Single x = 0;
            public static Single y = 0;
            public static int timer = 1;
            public static float stopX = 0f;
            public static float stopY = 0f;

            public static Single centerX = 0f;
            public static Single centerY = 0f;
            public int iterationsX = 0;
            public int iterationsY = 0;
            public int currentTicks = 0;
            public int configureTicks = 800;

            public static bool shouldStopConfig = false;
            public static bool shouldSmile = false;
            public static bool shouldSurprise = false;
            public static bool shouldContempt = false;
            public static bool shouldConfigure = false;
            public static bool updateMode = false;
            public static bool nearMode = true;

            private bool isSmiling = false;
            private bool isSurprise = false;
            private bool isContempt = false;
            public static bool shouldNeutral = true;
            public static bool configureMode = false;
            public static bool stopped = false;

            private bool locked = false;

            private bool useMouse = true;
            private bool headCentered = true;
            public static int mouseSens = 1;

            public static float upNear = 10;
            public static float upFar = 20;
            public static float rightNear = 10;
            public static float rightFar = 20;

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
                mouseSens = cfgSens;
                if(!mode)
                {
                    timer = 100;
                }
            }

            public void ChangeMode()
            {
                useMouse = !useMouse;
                if (useMouse)
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
                    /*mouse._ShouldMouseUp = false;
                    mouse._ShouldMouseDown = false;
                    mouse._ShouldMouseLeft = false;
                    mouse._ShouldMouseRight = false;*/
                }
                else
                {
                    headCentered = true;
                }
            }

            public void OnHeadLeft()
            {
                if (useMouse)
                {
                    //mouse._ShouldMouseLeft = true;
                    mouse.MoveMouseLeft();
                }
                else
                {
                    if(headCentered)
                    {
                        selenium.TabBackward();
                        //call faceplaywebdriver tabforward
                        headCentered = false;
                    }
                }
            }

            public void OnHeadRight()
            {
                if (useMouse)
                {
                    //mouse._ShouldMouseRight = true;
                    mouse.MoveMouseRight();
                }
                else
                {
                    if(headCentered)
                    {
                        selenium.TabForward();
                        //call faceplaywebdriver tabbackward
                        headCentered = false;
                    }
                }
            }

            public void OnHeadUp()
            {
                if (useMouse)
                {
                    //mouse._ShouldMouseUp = true;
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
                    //mouse._ShouldMouseDown = true;
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

            public void OnContempt()
            {
                if (useMouse)
                {
                    mouse.rightClick();
                }
                else
                {
                    selenium.FocusLocationBar();
                    //call faceplaywebdriver focuslocationbar
                    headCentered = false;
                }
                isContempt = true;
                shouldNeutral = false;
            }

            public void OnSmile()
            {
                if(useMouse)
                {
                    mouse.leftClick();
                }
                else
                {
                    selenium.Enter();
                    //call faceplaywebdriver focuslocationbar
                    headCentered = false;
                }
                isSmiling = true;
                shouldNeutral = false;
            }

            public void OnSurprise()
            {
                locked = !locked;
               //mouse.middleClickDown();
               isSurprise = true; 
               shouldNeutral = false;
            }
            
            public void OnNeutral()
            {
                isSmiling = false;
                isSurprise = false;
                isContempt = false;
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
                    currentTicks += 5;
                }

                centerX = changeAverage(x, centerX, ref iterationsX);
                centerY = changeAverage(y, centerY, ref iterationsY);
            }

            public void checkInputs()
            {
                if (shouldSurprise)
                {
                    if (!isSurprise)
                    {
                        OnSurprise();
                    }
                }

                if (shouldNeutral)
                {
                    OnNeutral();
                }

                if(locked)
                {
                    return;
                }

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

                if (shouldContempt)
                {
                    Console.WriteLine("CONTEMPT");
                    if (!isContempt)
                    {
                        OnContempt();
                    }
                }

                if(shouldSmile)
                {
                    if(!isSmiling)
                    {
                        OnSmile();
                    }
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
                    EmotionDetection.form.UpdateStatus("Calibrating: " + Math.Truncate(1.0*currentTicks/configureTicks * 100) + "%");
                    if(currentTicks >= configureTicks || shouldStopConfig)
                    {
                        if(nearMode)
                        {
                            upLimit = centerY - upNear;
                            downLimit = centerY + upNear;
                            leftLimit = centerX + rightNear;
                            rightLimit = centerX - rightNear;
                        }
                        else
                        {
                            upLimit = centerY - upFar;
                            downLimit = centerY + upFar;
                            leftLimit = centerX + rightFar;
                            rightLimit = centerX - rightFar;
                        }
                        stopX = leftLimit - 1;
                        stopY = upLimit + 1;
                        x = centerX;
                        y = centerY;
                        configureMode = false;
                        EmotionDetection.form.UpdateStatus("Streaming");
                        if (shouldStopConfig)
                        {
                            EmotionDetection.form.UpdateStatus("Stopped");
                            shouldStopConfig = false;
                        }
                    }
                    else if(currentTicks <= 200)
                    {
                        currentTicks++;
                        return;
                    }
                    else
                    {
                        Configure();
                        return;
                    }
                }
                if(updateMode)
                {
                    if (nearMode)
                    {
                        upLimit = centerY - upNear;
                        downLimit = centerY + upNear;
                        leftLimit = centerX + rightNear;
                        rightLimit = centerX - rightNear;
                    }
                    else
                    {
                        upLimit = centerY - upFar;
                        downLimit = centerY + upFar;
                        leftLimit = centerX + rightFar;
                        rightLimit = centerX - rightFar;
                    }
                    updateMode = false;
                }
                if(stopped)
                {
                    return;
                }
                checkInputs();
            }
        }
    }
