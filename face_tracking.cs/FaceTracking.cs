using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace face_tracking.cs
{
    class FaceTracking
    {
        private MainForm form;
        private bool disconnected = false;

        public FaceTracking(MainForm form)
        {
            this.form = form;
        }

        private bool DisplayDeviceConnection(bool state)
        {
            Camera.pitch = 0;
            Camera.roll = 0;
            Camera.yaw = 0;
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

        private void DisplayLocation(PXCMFaceAnalysis ft)
        {
            for (uint i=0;;i++) {
                int fid; ulong ts;
                if (ft.QueryFace(i, out fid, out ts) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                /* Retrieve face location data */
                PXCMFaceAnalysis.Detection ftd = ft.DynamicCast<PXCMFaceAnalysis.Detection>(PXCMFaceAnalysis.Detection.CUID);
                PXCMFaceAnalysis.Detection.Data ddata;
                if (ftd.QueryData(fid, out ddata) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                    form.DrawLocation(ddata);

                /* Retrieve face landmark data */
                PXCMFaceAnalysis.Landmark ftl = ft.DynamicCast<PXCMFaceAnalysis.Landmark>(PXCMFaceAnalysis.Landmark.CUID);
                PXCMFaceAnalysis.Landmark.ProfileInfo pinfo;
                ftl.QueryProfile(out pinfo);
                PXCMFaceAnalysis.Landmark.LandmarkData[] ldata = new PXCMFaceAnalysis.Landmark.LandmarkData[(int)(pinfo.labels&PXCMFaceAnalysis.Landmark.Label.LABEL_SIZE_MASK)];
                if (ftl.QueryLandmarkData(fid, pinfo.labels, ldata) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                   form.DrawLandmark(ldata);
                PXCMFaceAnalysis.Landmark.PoseData pose;
                ftl.QueryLandmarkData(fid, PXCMFaceAnalysis.Landmark.Label.LABEL_NOSE_TIP, ldata);
                ftl.QueryPoseData(fid, out pose);
                if(fid > 0)
                {
                    Camera.pitch = pose.pitch;
                    Camera.roll = pose.roll;
                    Camera.yaw = pose.yaw;
                }
                else
                {
                    Camera.pitch = 0;
                    Camera.roll = 0;
                    Camera.yaw = 0;
                }
            }
        }

        /* Derive MyUtilMPipeline from UtilMPipeline to override the Emotion configuration */
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
            public override void OnFaceLandmarkSetup(ref PXCMFaceAnalysis.Landmark.ProfileInfo finfo)
            {
                PXCMFaceAnalysis.Landmark ftl=QueryFace().DynamicCast<PXCMFaceAnalysis.Landmark>(PXCMFaceAnalysis.Landmark.CUID);
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
                pp = new MyUtilMPipeline(form.GetCheckedLandmarkProfile(), form.GetFileName(), true);
                pp.QueryCapture().SetFilter(form.GetCheckedDevice());
            }
            else if (form.GetPlaybackState())
            {
                pp = new MyUtilMPipeline(form.GetCheckedLandmarkProfile(), form.GetFileName(), false);
            }
            else
            {
                pp = new MyUtilMPipeline(form.GetCheckedLandmarkProfile());
                pp.QueryCapture().SetFilter(form.GetCheckedDevice());
            }

            /* Set Module */
            pp.EnableFaceLocation(form.GetCheckedModule());
            pp.EnableFaceLandmark(form.GetCheckedModule());

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
                        PXCMFaceAnalysis ft = pp.QueryFace();
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
            if (sts)
            {
                form.UpdateStatus("Stopped");
                Camera.pitch = 0;
                Camera.roll = 0;
                Camera.yaw = 0;
            }
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

            PXCMFaceAnalysis face;
            sts = session.CreateImpl<PXCMFaceAnalysis>(ref desc, PXCMFaceAnalysis.CUID, out face);
            if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.UpdateStatus("Failed to create the face module");
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
                PXCMFaceAnalysis.ProfileInfo pinfo;
                sts = face.QueryProfile(i, out pinfo);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                sts = capture.LocateStreams(ref pinfo.inputs);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;
                sts = face.SetProfile(ref pinfo);
                if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR) break;
            }
            if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                form.UpdateStatus("Failed to pair the face module with I/O");
                capture.Dispose();
                face.Dispose();
                session.Dispose();
                return;
            }

            /* Set detection configuration */
            PXCMFaceAnalysis.Detection faced=face.DynamicCast<PXCMFaceAnalysis.Detection>(PXCMFaceAnalysis.Detection.CUID);
            PXCMFaceAnalysis.Detection.ProfileInfo pinfo1;
            faced.QueryProfile(0,out pinfo1);
            faced.SetProfile(ref pinfo1);

            /* Set landmark configuration */
            PXCMFaceAnalysis.Landmark facel =face.DynamicCast<PXCMFaceAnalysis.Landmark>(PXCMFaceAnalysis.Landmark.CUID);
            PXCMFaceAnalysis.Landmark.ProfileInfo pinfo2;
            facel.QueryProfile(form.GetCheckedLandmarkProfile(), out pinfo2);
            facel.SetProfile(ref pinfo2);

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

                sts = face.ProcessImageAsync(images, out sps[1]);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMScheduler.SyncPoint.SynchronizeEx(sps);
                sts=sps[0].Synchronize();
                if (DisplayDeviceConnection(sts==pxcmStatus.PXCM_STATUS_DEVICE_LOST)) continue;
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                /* Display Results */
                DisplayPicture(capture.QueryImage(images,PXCMImage.ImageType.IMAGE_TYPE_COLOR));
                DisplayLocation(face);
                form.UpdatePanel();
            }
            PXCMImage.Dispose(images);
            PXCMScheduler.SyncPoint.Dispose(sps);

            capture.Dispose();
            face.Dispose();
            session.Dispose();
            Camera.pitch = 0;
            Camera.roll = 0;
            Camera.yaw = 0;
            form.UpdateStatus("Stopped");
        }
    }
}
