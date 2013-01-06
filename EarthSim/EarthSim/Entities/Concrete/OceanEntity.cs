using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EarthSim.Entities.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EarthSim.Utils;

namespace EarthSim.Entities.Concrete
{
    public class OceanEntity : AbstractSphereEntity
    {
        public OceanEntity(Game game, float radius, Texture2D texture)
            : base(game, radius, texture)
        {
            LoadHeightMap(texture);

            // Initialize
            initializeVertices();
            initializeIndices();
            FixNormals();
            //ConvertToSphere(radius, width, height);
            updateData();
        }

        private void LoadHeightMap(Texture2D heightMap)
        {
            height = heightMap.Height;
            width = heightMap.Width;

            Color[] heightMapColorInfo = new Color[width * height];
            heightMap.GetData(heightMapColorInfo);
            heightData = new HSL[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    heightData[x, y] = RGB2HSL.ToHSL(heightMapColorInfo[x + y * width]);
                }
            }
        }
    }
}
