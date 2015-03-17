/*******************************************************************************

INTEL CORPORATION PROPRIETARY INFORMATION
This software is supplied under the terms of a license agreement or nondisclosure
agreement with Intel Corporation and may not be copied or disclosed except in
accordance with the terms of that agreement
Copyright(c) 2013 Intel Corporation. All Rights Reserved.

*******************************************************************************/
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emotion_Detection
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            mouseDriven myMouse = new mouseDriven();
            Webdriver selen = new Webdriver("http://www.google.com");

            Camera cam = new Camera(myMouse, selen, true, 1, 10, -2, -20, 20, 0, 0);

            bool setup = true;

            cam.aTimer = new System.Timers.Timer(1);
            cam.aTimer.Elapsed += cam.OnTimedEvent;
            cam.aTimer.AutoReset = true;
            Console.WriteLine("The timer should fire every {0} milliseconds.",
                 cam.aTimer.Interval);
            cam.aTimer.Enabled = true;

            if (setup)    //for debugging/keyboard, set this to false before compiling
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                PXCMSession session;
                pxcmStatus sts = PXCMSession.CreateInstance(out session);
                if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    Application.Run(new MainForm(session));
                    session.Dispose();
                }
            }
        }
    }
}
