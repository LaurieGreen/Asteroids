using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asteroids
{
    static class GameConstants
    {
        public const float PlayfieldSizeX = 22.5f;
        public const float PlayfieldSizeY = 12.5f;

        public const int NumAsteroids = 10;
        public const float AsteroidMinSpeed = 0.5f;
        public const float AsteroidMaxSpeed = 2.0f;
        public const float AsteroidSpeedAdjustment = 5.0f;

        public const float AsteroidBoundingSphereScale = 0.95f;  
        public const float ShipBoundingSphereScale = 0.5f;  

        public const int NumBullets = 30;
        public const float BulletSpeedAdjustment = 5.0f;
    }
}
