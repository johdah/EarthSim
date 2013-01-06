using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EarthSim.Utils;

namespace EarthSim.Entities.Abstract
{
    public abstract class AbstractSphereEntity : AbstractEntity
    {
        #region Fields

        private float Scale = 1.0f;
        protected VertexPositionNormalTexture[] vertices;
        protected int[] indices;

        protected Texture2D texture;
        protected HSL[,] heightData;
        protected float radius = 0f;
        protected int height;
        protected int width;

        #endregion

        public AbstractSphereEntity(Game game, float radius, Texture2D texture)
            : base(game)
        {
            this.radius = radius;
            this.texture = texture;
        }

        public void ConvertToSphere(float radius, float xMax, float yMax)
        {
            xMax -= 1;
            yMax -= 1;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 oldPosition = vertices[i].Position;
                //double ringradius = (float)((height + radius) * Math.Sin(row * Math.PI / yMax));
                float equatorRadius = radius * ((float)Math.Sin(oldPosition.Z * Math.PI / yMax)
                    + (float)Math.Sin(oldPosition.Y * Math.PI / yMax));

                Vector3 newPosition = new Vector3(
                        (float)Math.Cos((xMax - oldPosition.X) * Math.PI * 2.0f / xMax) * equatorRadius,
                        (float)Math.Cos(oldPosition.Y * Math.PI / yMax) * radius,
                        (float)Math.Sin((xMax - oldPosition.X) * Math.PI * 2.0f / xMax) * equatorRadius);
                
                vertices[i].Position = newPosition;
            }
        }

        public Vector3 GetGeoPosition(float latitude, float longitude, float elevation, float earthRadius)
        {
            earthRadius += elevation;

            float lat = latitude * (MathHelper.Pi / 180);
            float lon = longitude * (MathHelper.Pi / 180);

            float x = -earthRadius * (float)Math.Cos(lat) * (float)Math.Cos(lon);
            float y = earthRadius * (float)Math.Sin(lat);
            float z = earthRadius * (float)Math.Cos(lat) * (float)Math.Sin(lon);

            return Position = new Vector3(x, y, z);
        }

        protected void initializeIndices()
        {
            //6 vertices make up 2 triangles. Not decreasing Col because we want it to wrap around
            indices = new int[(height) * (width) * 6];

            int counter = 0;

            //Indices
            for (int row = 0; row < height - 1; row++)
            {
                for (int col = 0; col < width - 1; col++)
                {
                    int topLeft = (row * width) + col;
                    int topRight = (row * width) + col + 1;
                    int bottomLeft = ((row + 1) * width) + col;
                    int bottomRight = ((row + 1) * width) + col + 1;

                    indices[counter++] = topLeft;
                    indices[counter++] = topRight;
                    indices[counter++] = bottomLeft;

                    indices[counter++] = topRight;
                    indices[counter++] = bottomRight;
                    indices[counter++] = bottomLeft;
                }
            }
        }

        protected void initializeVertices()
        {
            vertices = new VertexPositionNormalTexture[(height * width)];
            int index = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vertices[index] = new VertexPositionNormalTexture(
                        new Vector3(col, row, -(float)heightData[col, row].H / 100f), 
                        new Vector3(), 
                        new Vector2());
                    vertices[index].TextureCoordinate.X = (float)col / width;
                    vertices[index].TextureCoordinate.Y = (float)row / height;
                    index++;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            World *= Matrix.CreateScale(Scale);
            Scale = 1.0f;
            base.Update(gameTime);
        }

        public void Draw(BasicEffect basicEffect)
        {
            /*basicEffect.World = World;
            basicEffect.Texture = heightMap;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            parent.GraphicsDevice.SetVertexBuffer(vb);
            parent.GraphicsDevice.Indices = ib;

            parent.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indices.Length, 0, indices.Length / 3);*/
        }
    }
}
