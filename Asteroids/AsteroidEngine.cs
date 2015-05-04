using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class AsteroidEngine
    {
        public Asteroid[] asteroidList;
        Random random;
        Matrix[] asteroidTransforms;

        public AsteroidEngine(Model currentTexture, Camera camera)
        {
            asteroidList = new Asteroid[GameConstants.NumAsteroids];
            asteroidTransforms = SetupEffectDefaults(currentTexture, camera);
            random = new Random();
        }

        private Matrix[] SetupEffectDefaults(Model myModel, Camera camera)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = true;
                    effect.Projection = camera.Projection;
                    effect.View = camera.View;
                }
            }
            return absoluteTransforms;
        }


        public void ResetAsteroids(Model currentTexture, Camera camera)
        {
            float x;
            float y;
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (random.Next(2) == 0)
                {
                    x = (float)-GameConstants.PlayfieldSizeX;
                }
                else
                {
                    x = (float)GameConstants.PlayfieldSizeX;
                }
                y = (float)random.NextDouble() * GameConstants.PlayfieldSizeY;
                asteroidList[i] = new Asteroid(camera, currentTexture);
                asteroidList[i].Position = new Vector3(x,y,0);
                double angle = random.NextDouble() * 2 * Math.PI;
                asteroidList[i].Direction.X = -(float)Math.Sin(angle);
                asteroidList[i].Direction.Y = (float)Math.Cos(angle);
                asteroidList[i].Velocity = GameConstants.AsteroidMinSpeed + (float)random.NextDouble() * GameConstants.AsteroidMaxSpeed;
            }
        }

        public void Update(float timeDelta)
        {
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                asteroidList[i].Update(timeDelta);
            }
        }

        public void Draw(Camera camera)
        {
            for (int i = 0; i < GameConstants.NumAsteroids; i++)
            {
                if (asteroidList[i].isActive == true)
                {
                    asteroidList[i].Draw(camera, asteroidTransforms);
                }
            }
        }
    }
}
