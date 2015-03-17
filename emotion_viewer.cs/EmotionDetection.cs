using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emotion_Detection
{
    
    class EmotionDetection
    {

        private MainForm form;
        private bool disconnected = false;

        public EmotionDetection(MainForm form)
        {
            this.form = form;
        }

        private bool DisplayDeviceConnection(bool state)
        {
            if (state)
            {
                if (!disconnected) form.UpdateStatus("Device Disconnected");
                disconnected = true;
            }
            else
            {
                if (disconnected) form.UpdateStatus("Device Reconnected");
                disconnected = false;
            }
            return disconnected;
        }

        private void DisplayPicture(PXCMImage image)
        {
            PXCMImage.ImageData data;
            if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.ColorFormat.COLOR_FORMAT_RGB32, out data) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.DisplayBitmap(data.ToBitmap(image.info.width, image.info.height));
                image.ReleaseAccess(ref data);
            }
        }

        private void DisplayLocation(PXCMEmotion ft)
        {
            uint numFaces = ft.QueryNumFaces();
            for (uint i=0; i<numFaces;i++) {
                /* Retrieve emotionDet location data */
                PXCMEmotion.EmotionData[] arrData = new PXCMEmotion.EmotionData[form.NUM_EMOTIONS];
                if(ft.QueryAllEmotionData(i, arrData) >= pxcmStatus.PXCM_STATUS_NO_ERROR){
                    form.DrawLocation(arrData);
                    Console.WriteLine("x: " + arrData[0].rectangle.x + " " + "y: " + arrData[0].rectangle.y);
                    Camera.x = arrData[0].rectangle.x;
                    Camera.y = arrData[0].rectangle.y;
                }
            }
        }

        /* Derive MyUtilMPipeline from UtilMPipeline to override the landmark configuration */
        class MyUtilMPipeline : UtilMPipeline
        {
            private uint profileIndex;
            public MyUtilMPipeline(uint pidx, string file, bool record)
                : base(file, record)
            {
                profileIndex = pidx;
            }
            public MyUtilMPipeline(uint pidx)
                : base()
            {
                profileIndex = pidx;
            }
            public override void OnEmotionSetup(ref PXCMEmotion.ProfileInfo finfo)
            {
                PXCMEmotion ftl = QueryEmotion();
                ftl.QueryProfile(profileIndex, out finfo);
            }
        }

        public void SimplePipeline()
        {
            bool sts = true;
            MyUtilMPipeline pp = null;
            disconnected = false;

            /* Set Source & Landmark Profile Index */
            if (form.GetRecordState())
            {
                pp = new MyUtilMPipeline(0, form.GetFileName(), true);
                pp.QueryCapture().SetFilter(form.GetCheckedDevice());
            }
            else if (form.GetPlaybackState())
            {
                pp = new MyUtilMPipeline(0, form.GetFileName(), false);
            }
            else
            {
                pp = new MyUtilMPipeline(0);
                pp.QueryCapture().SetFilter(form.GetCheckedDevice());
            }

            /* Set Module */
            pp.EnableEmotion(form.GetCheckedModule());

            /* Initialization */
            form.UpdateStatus("Init Started");
            if (pp.Init())
            {
                form.UpdateStatus("Streaming");

                while (!form.stop)
                {
                    if (!pp.AcquireFrame(true)) break;
                    if (!DisplayDeviceConnection(pp.IsDisconnected()))
                    {
                        /* Display Results */
                        PXCMEmotion ft = pp.QueryEmotion();
                        DisplayPicture(pp.QueryImage(PXCMImage.ImageType.IMAGE_TYPE_COLOR));
                        DisplayLocation(ft);
                            
                        form.UpdatePanel();
                    }
                    pp.ReleaseFrame();
                }
            }
            else
            {
                form.UpdateStatus("Init Failed");
                sts = false;
            }

            pp.Close();
            pp.Dispose();
            if (sts) form.UpdateStatus("Stopped");
        }

        public void AdvancedPipeline()
        {
            PXCMSession session;
            pxcmStatus sts = PXCMSession.CreateInstance(out session);
            if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.UpdateStatus("Failed to create an SDK session");
                return;
            }

            /* Set Module */
            PXCMSession.ImplDesc desc = new PXCMSession.ImplDesc();
            desc.friendlyName.set(form.GetCheckedModule());

            PXCMEmotion emotionDet;
            sts = session.CreateImpl<PXCMEmotion>(ref desc, PXCMEmotion.CUID, out emotionDet);
            if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.UpdateStatus("Failed to create the emotionDet module");
                session.Dispose();
                return;
            }

            UtilMCapture capture = null;
            if (form.GetRecordState())
            {
                capture = new UtilMCaptureFile(session, form.GetFileName(), true);
                capture.SetFilter(form.GetCheckedDevice());
            }
            else if (form.GetPlaybackState())
            {
                capture = new UtilMCaptureFile(session, form.GetFileName(), false);
            }
            else
            {
                capture = new UtilMCapture(session);
                capture.SetFilter(form.GetCheckedDevice());
            }

            form.UpdateStatus("Pair moudle with I/O");
            for (uint i = 0; ; i++)
            {
                PXCMEmotion.ProfileInfo pinfo;
                sts = emotionDet.QueryProfile(i, out pinfo);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                sts = capture.LocateStreams(ref pinfo.inputs);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;
                sts = emotionDet.SetProfile(ref pinfo);
                if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR) break;
            }
            if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.UpdateStatus("Failed to pair the emotionDet module with I/O");
                capture.Dispose();
                emotionDet.Dispose();
                session.Dispose();
                return;
            }

            form.UpdateStatus("Streaming");
            PXCMImage[] images = new PXCMImage[PXCMCapture.VideoStream.STREAM_LIMIT];
            PXCMScheduler.SyncPoint[] sps = new PXCMScheduler.SyncPoint[2];
            while (!form.stop)
            {
                PXCMImage.Dispose(images);
                PXCMScheduler.SyncPoint.Dispose(sps);
                sts = capture.ReadStreamAsync(images, out sps[0]);
                if (DisplayDeviceConnection(sts == pxcmStatus.PXCM_STATUS_DEVICE_LOST)) continue;
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                sts = emotionDet.ProcessImageAsync(images, out sps[1]);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMScheduler.SyncPoint.SynchronizeEx(sps);
                sts=sps[0].Synchronize();
                if (DisplayDeviceConnection(sts==pxcmStatus.PXCM_STATUS_DEVICE_LOST)) continue;
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                /* Display Results */
                DisplayPicture(capture.QueryImage(images,PXCMImage.ImageType.IMAGE_TYPE_COLOR));
                DisplayLocation(emotionDet);
                form.UpdatePanel();
            }
            PXCMImage.Dispose(images);
            PXCMScheduler.SyncPoint.Dispose(sps);

            capture.Dispose();
            emotionDet.Dispose();
            session.Dispose();
            form.UpdateStatus("Stopped");
        }
    }
}
