using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Objects;

namespace Engine.Components
{
    public class KinectManager
    {
        KinectSensor kinectSensor;
        public KinectSensor KinectSensor
        {
            get { return kinectSensor; }
        }

        ColorFrameReader colorFrameReader;
        BodyFrameReader bodyFrameReader;
     

        FrameDescription desc;
        byte[] colorPixels;

        Body[] bodies;
        Skeleton[] skeletons;
        public Skeleton[] Skeletons
        {
            get { return skeletons; }
        }

        Texture2D videoTexture;
        public Texture2D VideoStream
        {
            get { return videoTexture; }
        }

        public KinectManager(Game game)
        {
            this.kinectSensor = KinectSensor.GetDefault();

            // Video Stream initialization
            this.colorFrameReader = kinectSensor.ColorFrameSource.OpenReader();
            colorFrameReader.FrameArrived += Reader_ColorFrameArrived;

            desc = kinectSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            
            colorPixels = new byte[desc.Width * desc.Height * desc.BytesPerPixel];

            // Bodyframe initialization
            skeletons = new Skeleton[kinectSensor.BodyFrameSource.BodyCount];
            for (int i = 0; i < Skeletons.Length; i++)
            {
                skeletons[i] = new Skeleton();
            }
            bodies = new Body[kinectSensor.BodyFrameSource.BodyCount];

            bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            bodyFrameReader.FrameArrived += BodyFrameReader_FrameArrived;

            this.kinectSensor.Open();

            videoTexture = new Texture2D(game.GraphicsDevice, desc.Width, desc.Height);
        }

        


        /// <summary>
        /// Updates video stream when kinect is ready.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reader_ColorFrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    colorFrame.CopyConvertedFrameDataToArray(colorPixels, ColorImageFormat.Rgba);
                    videoTexture.SetData(colorPixels);
                }
            }
        }


        private void BodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    Body body = bodies[i];

                    Console.WriteLine(String.Format("Body{0}: {1}", i.ToString(), body.IsTracked.ToString()));
                    if (body.IsTracked)
                    {
                        skeletons[i].Body = body;

                        skeletons[i].Update(kinectSensor);
                    }
                }
            }
        }



        public void UnloadContent()
        {
            if (colorFrameReader != null)
            {
                colorFrameReader.Dispose();
                colorFrameReader = null;
            }

            if (bodyFrameReader != null)
            {
                bodyFrameReader.Dispose();
                bodyFrameReader = null;
            }

            if (kinectSensor != null)
            {
                this.kinectSensor.Close();
                kinectSensor = null;
            }   
        }
    }
}
