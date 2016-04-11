using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Engine.Components;

namespace Engine.Objects
{
    public class Skeleton
    {
        public Hand LeftHand;
        public Hand RightHand;
        public Body Body;

        CameraSpacePoint cameraSpacePoint;
        ColorSpacePoint colorSpacePoint;

        public Skeleton()
        {
            LeftHand = new Hand();
            LeftHand.Type = JointType.HandLeft;
            RightHand = new Hand();
            RightHand.Type = JointType.HandRight;
        }

        public void Update(KinectSensor sensor)
        {
            LeftHand.Update();
            LeftHand.HandState = Body.HandLeftState;
            cameraSpacePoint = Body.Joints[LeftHand.Type].Position;
            colorSpacePoint = sensor.CoordinateMapper.MapCameraPointToColorSpace(cameraSpacePoint);
            LeftHand.Position = new Vector2(colorSpacePoint.X, colorSpacePoint.Y);


            RightHand.Update();
            RightHand.HandState = Body.HandRightState;
            cameraSpacePoint = Body.Joints[RightHand.Type].Position;
            colorSpacePoint = sensor.CoordinateMapper.MapCameraPointToColorSpace(cameraSpacePoint);
            RightHand.Position = new Vector2(colorSpacePoint.X, colorSpacePoint.Y);
        }
    }
}
