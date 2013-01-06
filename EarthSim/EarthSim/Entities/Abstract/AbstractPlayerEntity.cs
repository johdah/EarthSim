using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
    }
}
