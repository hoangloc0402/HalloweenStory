using System;
using Microsoft.Xna.Framework;

namespace HalloweenStory
{
    class Camera
    {
        #region Fields
        const int LEFT_BOUND = 300;   //khoảng cách để camera theo bên trái
        const int RIGHT_BOUND = 900;  //khoảng cách để camera theo bên phải
        public Vector2 Position;  //vị trí của camera
        int LeftLimit; //giới hạn  bên trái camera
        int RightLimit; //giới hạn  bên phải camera
        #endregion
        #region Constructors
        public Camera(int leftLimit, int rightLimit)
        {
            LeftLimit = leftLimit;
            RightLimit = rightLimit - 1366;
        }
        #endregion
        #region Public Methods
        public void LookAt(Vector2 targetPosition)   //di chuyển theo vị trí của target
        {
            if (targetPosition.X < Position.X + LEFT_BOUND)
            {
                Position.X = targetPosition.X - LEFT_BOUND;
            }
            else if (targetPosition.X > Position.X + RIGHT_BOUND)
            {
                Position.X =  targetPosition.X - RIGHT_BOUND;
            }
            Position.X = MathHelper.Clamp(Position.X, LeftLimit, RightLimit);
        }
        public Matrix GetViewMatrix() //trả về ma trận dịch chuyển
        {
            return Matrix.CreateTranslation(new Vector3(-Position.X, 0, 0));
        }
        #endregion
    }
}
