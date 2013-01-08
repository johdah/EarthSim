using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EarthSim.Components.Input;

namespace EarthSim.Entities.Abstract
{
    public abstract class AbstractPlayerEntity : AbstractEntity
    {
        protected float xRotation;
        protected float zRotation;
        protected bool rotate;
        protected float geoElevation;
        protected float geoLongitude = 0.0f;
        protected float geoLatitude = 0.0f;
        protected float speed = 0.5f;

        private float Direction = 0.0f;

        public AbstractPlayerEntity(Game game)
            : base(game)
        { }

        protected void GetRotationForModel(out float pitch, out float roll, out float yaw, float latitude, float longitude)
        {
            yaw = 0;
            pitch = Direction;
            roll = MathHelper.PiOver2;
            roll += -MathHelper.Pi * 2 * (latitude / 360);
            yaw = MathHelper.Pi * 2 * (longitude / 360);
        }

        public void performAction(ActionType action, float elapsedTime)
        {
            switch (action)
            {
                case ActionType.Left:
                    this.geoLongitude -= elapsedTime * speed;
                    if (geoLongitude <= 0) geoLongitude = 360 - geoLongitude;
                    break;
                case ActionType.Right:
                    this.geoLongitude += elapsedTime * speed;
                    this.geoLongitude = this.geoLongitude % 360;
                    break;
                case ActionType.Down:
                    this.geoLatitude += elapsedTime * speed;
                    this.geoLatitude = this.geoLatitude % 360;
                    break;
                case ActionType.Up:
                    this.geoLatitude -= elapsedTime * speed;
                    if (geoLatitude <= 0) geoLatitude = 360 - geoLatitude;
                    break;
                case ActionType.IncreaseSpeed:
                    this.speed += 0.1f;
                    break;
                case ActionType.DecreaseSpeed:
                    this.speed -= 0.1f;
                    break;
            }
        }

        public abstract void Draw(Matrix world, Matrix view, Matrix projection, BasicEffect effect);
    }
}
