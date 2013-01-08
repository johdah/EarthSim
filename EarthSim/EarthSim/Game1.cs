using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using EarthSim.Components;
using EarthSim.Entities.Concrete;
using EarthSim.Entities;

namespace EarthSim
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private DebugComponent _debug;
        private InputComponent _input;

        private Camera camera;
        private BasicEffect effect;
        private Matrix projection;
        private Matrix view;
        private Matrix world;

        private EarthEntity earthEntity;
        private OceanEntity oceanEntity;
        private SkyEntity skyEntity;
        private TankEntity tankEntity;


        private SimplePlane simplePlane;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.IsFullScreen = false;
            graphics.PreferMultiSampling = true;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferredBackBufferWidth = 800;
            graphics.SynchronizeWithVerticalRetrace = true;

            _debug = new DebugComponent(this);
            Components.Add(_debug);
            Services.AddService(typeof(DebugComponent), _debug);

            _input = new InputComponent(this);
            Components.Add(_input);
            Services.AddService(typeof(InputComponent), _input);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            InitializeCamera();

            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rs;

            base.Initialize();
        }

        private void InitializeCamera()
        {
            /*float aspectRatio = GraphicsDevice.DisplayMode.AspectRatio;

            Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                aspectRatio,
                0.1f,
                1000.0f,
                out projection);

            cameraPosition = new Vector3(0f, 0f, 15f);
            cameraTarget = new Vector3(0f, 0f, 0f);
            cameraUpVector = Vector3.Up;

            Matrix.CreateLookAt(
               ref cameraPosition,
               ref cameraTarget,
               ref cameraUpVector,
               out view);*/
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            effect = new BasicEffect(GraphicsDevice);
            camera = new Camera(GraphicsDevice);

            earthEntity = new EarthEntity(this, 5f, this.Content.Load<Texture2D>("Entities/earthTexture"));
            oceanEntity = new OceanEntity(this, 20f, this.Content.Load<Texture2D>("Entities/oceanTexture"));
            //skyEntity = new SkyEntity(this, 5f, this.Content.Load<Texture2D>("Entities/skyTexture"));
            tankEntity = new TankEntity(this, this.Content.Load<Model>("Entities/Tank/tank"), earthEntity);

            simplePlane = new SimplePlane(GraphicsDevice, Vector3.Zero, Quaternion.Identity, 1);

            world = Matrix.Identity;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _input.Update();

            earthEntity.Update(gameTime);
            oceanEntity.Update(gameTime);
            tankEntity.Update(gameTime);

            //camera.Update(earthEntity);

            _debug.cameraPos = camera.Position;
            _debug.playerPos = tankEntity.GetPosition();
            //_debug.playerDir = tank.GetDirection();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            world = Matrix.Identity;
            effect.Projection = camera.ViewProjectionMatrix;
            effect.View = camera.ViewMatrix;
            effect.World = world;

            earthEntity.Draw(effect);
            oceanEntity.Draw(effect);
            //skyEntity.Draw(effect);
            tankEntity.Draw(ref world, ref view, ref projection, ref effect);

            simplePlane.Draw(ref effect, ref world);

            base.Draw(gameTime);
        }
    }
}
