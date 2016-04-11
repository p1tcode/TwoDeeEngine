using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Engine.Objects
{
    public class Hand
    {
        public JointType Type { get; set; }
        public HandState HandState { get; set; }
        public Vector2 Position { get; set; }

        HandState previousState;

        public bool IsClosed
        {
            get
            {
                if ((HandState == HandState.Closed) && (previousState == HandState.Closed))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsClosing
        {
            get
            {
                if ((HandState == HandState.Closed) && ((previousState == HandState.Open) || (previousState == HandState.Lasso)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Update()
        {
            previousState = HandState;
        }
    }
}
