﻿/*******************************************************************************

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

namespace face_tracking.cs
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool setup = true;
            mouseDriven myMouse = new mouseDriven();
            myMouse.aTimer = new System.Timers.Timer(1);
            myMouse.aTimer.Elapsed += myMouse.OnTimedEvent;
            myMouse.aTimer.AutoReset = true;
            Console.WriteLine("The timer should fire every {0} milliseconds.",
                 myMouse.aTimer.Interval);
            myMouse.aTimer.Enabled = true;

            if(setup)    //for debugging/keyboard, set this to false before compiling
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