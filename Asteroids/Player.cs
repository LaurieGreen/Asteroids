using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Asteroids
{
    public class Player
    {
        public Model CurrentTexture;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 VelocityAdd;
        public float Rotation;
        public Matrix[] Transforms;
        public Matrix shipTransformMatrix;
        public Matrix RotationMatrix;
        public bool isActive;

        public Player(Model currentTexture, Vector3 position, Vector3 velocity, Camera camera)
        {
            CurrentTexture = currentTexture;
            Position = position;
            Velocity = velocity;
            isActive = true;
            Transforms = camera.SetupEffectDefaults(CurrentTexture, camera);
        }

        public Vector3 getPosition()
        {
            return Position;
        }

        public void Update(KeyboardState state, Model bulletModel, Camera camera, float timeDelta, SoundEffectInstance engineInstance)
        {
            if (isActive == false)
            {
                Reset();
            }
            if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                Rotation += 0.075f;
            }
            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                Rotation -= 0.075f;
            }
            RotationMatrix = Matrix.CreateRotationZ(Rotation);

            Vector3 VelocityAdd = Vector3.Zero;
            VelocityAdd.X = (float)Math.Sin(Rotation);
            VelocityAdd.Y = -(float)Math.Cos(Rotation);
            VelocityAdd /= 100;

            if (state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
            {
                Velocity += VelocityAdd;
                if (engineInstance.State == SoundState.Stopped)
                {
                    engineInstance.Volume = 0.75f;
                    engineInstance.IsLooped = true;
                    engineInstance.Play();
                }
                else
                    engineInstance.Resume();
            }
            else
            {
                if (engineInstance.State == SoundState.Playing)
                {
                    engineInstance.Volume *= 0.75f;
                    if (engineInstance.State == SoundState.Playing && engineInstance.Volume < 0.05f)
                    {
                        engineInstance.Pause();
                        engineInstance.Volume = 0.75f;
                    }
                }

            }

            if (state.IsKeyDown(Keys.Space))
            {

            }
            
            Position -= Velocity;
            Velocity *= 0.99f;
            if (Position.X > GameConstants.PlayfieldSizeX)
                Position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Position.X < -GameConstants.PlayfieldSizeX)
                Position.X += 2 * GameConstants.PlayfieldSizeX;
            if (Position.Y > GameConstants.PlayfieldSizeY)
                Position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (Position.Y < -GameConstants.PlayfieldSizeY)
                Position.Y += 2 * GameConstants.PlayfieldSizeY;
        }

        public void Reset()
        {
            Position = Vector3.Zero;
            Velocity = Vector3.Zero;
            Rotation = 0.0f;
            isActive = true;
        }

        public void Draw(Camera camera)
        {
            shipTransformMatrix = RotationMatrix * Matrix.CreateTranslation(Position);
            camera.DrawModel(CurrentTexture, shipTransformMatrix, Transforms, camera);
        }
    }
}
