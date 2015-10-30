using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Components;
using FarseerPhysics;
using System.Diagnostics;

namespace Engine
{
    interface ICamera2Dp
    {
        bool IsInView(Vector2 position, Texture2D texture);
        void JumpToTarget(Vector2 Position);
        void SetTarget(Vector2 Position);

        Vector2 Origin { get; }
        Vector2 Position { get; }
        Vector2 ScreenCenter { get; }

        Matrix View { get; }
        
        float Rotation { get; }
        float Scale { get; set; }
        float Speed { get; set; }
    }

    public class Camera2Dp : ICamera2Dp
    {
        #region Properties

        /// <summary>
        /// Position of the camera
        /// </summary>
        private Vector2 currentPosition;
        public Vector2 Position
        {
            get { return ConvertUnits.ToDisplayUnits(currentPosition); }
            set { currentPosition = ConvertUnits.ToSimUnits(value); }
        }

        /// <summary>
        /// Rotation of the camera
        /// </summary>
        private float rotation = 0;
        public float Rotation
        {
            get { return rotation; }
            //set { rotation = value; }
        }

        /// <summary>
        /// Scale of the camera (Zoom)
        /// </summary>
        private float scale = 1;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// The calculated view used to send to SpriteBatch
        /// </summary>
        private Matrix view;
        public Matrix View
        {
            get { return view; }
        }

        /// <summary>
        /// Center of the screen (Does not account for scale)
        /// </summary>
        private Vector2 screenCenter;
        public Vector2 ScreenCenter
        {
            get { return screenCenter; }
        }

        /// <summary>
        /// Center of the screen (Do accound for scale)
        /// </summary>
        private Vector2 origin;
        public Vector2 Origin
        {
            get { return origin; }
        }

        private float speed = 5;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        #endregion

        private Vector2 targetPosition;
        private Vector2 viewPort;
        private Matrix projection;

     
        public Camera2Dp(Game game, int viewPortWidth, int viewPortHeight)
        {   
            this.viewPort = new Vector2(ConvertUnits.ToSimUnits(viewPortWidth), ConvertUnits.ToSimUnits(viewPortHeight));
            this.screenCenter = new Vector2(viewPort.X / 2, viewPort.Y / 2);
            this.origin = screenCenter / ConvertUnits.ToSimUnits(scale);
            this.projection = Matrix.CreateOrthographicOffCenter(0, ConvertUnits.ToSimUnits(viewPortWidth), ConvertUnits.ToSimUnits(viewPortHeight), 0, 0, 1);

            game.Services.AddService(typeof(ICamera2Dp), this);
        }


        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (scale < 0.5f)
                scale = 0.5f;
            if (scale > 1.5f)
                scale = 1.5f;

            this.origin = screenCenter / scale;

            this.currentPosition = new Vector2(MathHelper.Lerp(this.currentPosition.X, this.targetPosition.X, this.speed * elapsedTime), MathHelper.Lerp(this.currentPosition.Y, this.targetPosition.Y, this.speed * elapsedTime));
 
            this.view = Matrix.Identity *
                             Matrix.CreateTranslation(-currentPosition.X,-currentPosition.Y, 0) *
                             Matrix.CreateRotationZ(rotation) *
                             Matrix.CreateTranslation(origin.X, origin.Y, 0) *
                             Matrix.CreateScale(new Vector3(ConvertUnits.ToDisplayUnits(scale), ConvertUnits.ToDisplayUnits(scale), 0));
        }

        /// <summary>
        /// Set the position the camera will move to.
        /// Movement is based on the speed value
        /// </summary>
        /// <param name="position"></param>
        public void SetTarget(Vector2 position)
        {
            targetPosition = ConvertUnits.ToSimUnits(position);
        }

        /// <summary>
        /// Jumps directly to a position regardless of speed
        /// </summary>
        /// <param name="position"></param>
        public void JumpToTarget(Vector2 position)
        {
            Position = position;
            targetPosition = ConvertUnits.ToSimUnits(position);
        }

        /// <summary>
        /// Checks if an object is in the cameras view.
        /// </summary>
        /// <param name="position">The position of the object to check</param>
        /// <param name="texture">The texture of the object</param>
        /// <returns></returns>
        public bool IsInView(Vector2 position, Texture2D texture)
        {
            if ((position.X + texture.Width) < (currentPosition.X - origin.X) || (position.X - texture.Width) > (currentPosition.X + origin.X))
            {
                return false;
            }
            if ((position.Y + texture.Height) < (currentPosition.Y - origin.Y) || (position.Y - texture.Height) > (currentPosition.Y + origin.Y))
            {
                return false; 
            }

            return true;
        }
    }
}
