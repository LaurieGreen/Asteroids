using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Asteroid
    {
        public Model CurrentTexture;
        public Vector3 Position;
        public Vector3 Direction;
        public float Velocity;
        public Matrix TransformMatrix;
        public bool isActive;

        public Asteroid(Camera camera, Model currentTexture)
        {
            CurrentTexture = currentTexture;
            isActive = true;
        }

        public void Update(float delta)
        {
            Position += Direction * Velocity * GameConstants.AsteroidSpeedAdjustment * delta;
            if (Position.X > GameConstants.PlayfieldSizeX)
                Position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Position.X < -GameConstants.PlayfieldSizeX)
                Position.X += 2 * GameConstants.PlayfieldSizeX;
            if (Position.Y > GameConstants.PlayfieldSizeY)
                Position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (Position.Y < -GameConstants.PlayfieldSizeY)
                Position.Y += 2 * GameConstants.PlayfieldSizeY;
        }

        public void Draw(Camera camera, Matrix[] asteroidTransforms)
        {
            TransformMatrix = Matrix.CreateTranslation(Position);
            camera.DrawModel(CurrentTexture, TransformMatrix, asteroidTransforms, camera);
        }
    }
}
