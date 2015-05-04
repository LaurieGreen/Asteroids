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

namespace Asteroids
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Camera camera;
        AsteroidEngine roids;
        Texture2D background;
        Model playerModel;
        Model roidModel;
        Model bulletModel;
        ParticleEngine particleEngine;
        BulletEngine bulletEngine;
        List<Model> textures;
        KeyboardState state;
        KeyboardState lastState;
        SoundEffect engineSound;
        SoundEffectInstance engineInstance;
        SoundEffect laserSound;
        SoundEffect explosionSound;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

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
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            lastState = Keyboard.GetState();

            textures = new List<Model>();

            background = Content.Load<Texture2D>("background");
            playerModel = Content.Load<Model>("models/ship");
            roidModel = Content.Load<Model>("models/roid");
            bulletModel = Content.Load<Model>("particles/circle");
            textures.Add(Content.Load<Model>("particles/circle"));

            engineSound = Content.Load<SoundEffect>("sound/engine_2");
            laserSound = Content.Load<SoundEffect>("sound/tx0_fire1");
            explosionSound = Content.Load<SoundEffect>("sound/explosion3");

            engineInstance = engineSound.CreateInstance();

            camera = new Camera(Vector3.Zero, graphics.GraphicsDevice.Viewport.AspectRatio, MathHelper.ToRadians(90.0f));
            player = new Player(playerModel, Vector3.Zero, Vector3.Zero, camera);            
            roids = new AsteroidEngine(roidModel, camera);
            particleEngine = new ParticleEngine(textures);
            bulletEngine = new BulletEngine(bulletModel, camera);

            roids.ResetAsteroids(roidModel, camera);
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.Update(state, bulletModel, camera, timeDelta, engineInstance);
            roids.Update(timeDelta);
            particleEngine.Update(player.Position + (-0.725f*player.RotationMatrix.Up), player.Velocity*0.5f,state, camera);
            bulletEngine.Update(state, lastState, player.RotationMatrix.Up, GameConstants.BulletSpeedAdjustment, player.Position + (0.725f * player.RotationMatrix.Up), camera, timeDelta, laserSound);
            CheckCollisions();
            lastState = state;
            base.Update(gameTime);
        }

        public void CheckCollisions()
        {
            //bullet VS asteroid collision check
            for (int i = 0; i < roids.asteroidList.Count(); i++)
            {
                if (roids.asteroidList[i].isActive)
                {
                    BoundingSphere asteroidSphere = new BoundingSphere(roids.asteroidList[i].Position, roidModel.Meshes[0].BoundingSphere.Radius * GameConstants.AsteroidBoundingSphereScale);
                    for (int j = 0; j < bulletEngine.bullets.Count; j++)
                    {
                        if (bulletEngine.bullets[j].isActive)
                        {
                            BoundingSphere bulletSphere = new BoundingSphere(bulletEngine.bullets[j].Position, bulletModel.Meshes[0].BoundingSphere.Radius);
                            if (asteroidSphere.Intersects(bulletSphere))
                            {
                                //soundExplosion2.Play();
                                roids.asteroidList[i].isActive = false;
                                bulletEngine.bullets[j].TTL = -1;
                            }
                        }
                    }
                }
            }

            //ship VS asteroid collision check
            BoundingSphere shipSphere = new BoundingSphere(player.Position, player.CurrentTexture.Meshes[0].BoundingSphere.Radius * GameConstants.ShipBoundingSphereScale);
            for (int i = 0; i < roids.asteroidList.Count(); i++)
            {
                if (roids.asteroidList[i].isActive == true)
                {
                    BoundingSphere b = new BoundingSphere(roids.asteroidList[i].Position, roidModel.Meshes[0].BoundingSphere.Radius * GameConstants.AsteroidBoundingSphereScale);
                    if (b.Intersects(shipSphere))
                    {
                        //blow up ship
                        player.isActive = false;
                        explosionSound.Play(0.1f, 0, 0);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 800, 480), Color.White);
            spriteBatch.End();

            player.Draw(camera);
            roids.Draw(camera);
            particleEngine.Draw(camera);
            bulletEngine.Draw(camera);

            base.Draw(gameTime);
        }
    }
}
