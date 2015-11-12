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
        KinectSensor kinect;
        ColorFrameReader colorFrameReader;
        FrameDescription desc;
        byte[] colorPixels;


        Texture2D videoTexture;
        public Texture2D VideoStream
        {
            get { return videoTexture; }
        }

        public KinectManager(Game game)
        {
            this.kinect = KinectSensor.GetDefault();

            this.colorFrameReader = kinect.ColorFrameSource.OpenReader();
            colorFrameReader.FrameArrived += Reader_ColorFrameArrived;

            desc = kinect.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba);
            
            colorPixels = new byte[desc.Width * desc.Height * desc.BytesPerPixel];

            this.kinect.Open();

            videoTexture = new Texture2D(game.GraphicsDevice, desc.Width, desc.Height);
        }

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


        public void UnloadContent()
        {
            colorFrameReader.Dispose();
            colorFrameReader = null;

            this.kinect.Close();
            kinect = null;
        }

        public void Update()
        {

        }

    }
}
