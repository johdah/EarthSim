using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EarthSim.Entities.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EarthSim.Entities.Concrete
{
    public class SkyEntity : AbstractSphereEntity
    {
        public SkyEntity(Game game, float radius, Texture2D texture)
            : base(game, radius, texture)
        { }
    }
}
