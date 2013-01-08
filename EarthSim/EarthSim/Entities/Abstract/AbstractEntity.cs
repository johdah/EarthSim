using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EarthSim.Entities.Abstract
{
    public abstract class AbstractEntity
    {
        protected List<AbstractEntity> _children;
        protected Game game;
        protected Vector3 Position;
        protected Quaternion Rotation;
        protected Matrix World;
        protected float Scale;

        public AbstractEntity(Game game)
        {
            this.game = game;
            this._children = new List<AbstractEntity>();
        }

        public Vector3 GetPosition()
        {
            return Position;
        }

        public Quaternion GetRotation()
        {
            return Rotation;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
