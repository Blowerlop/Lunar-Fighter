using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarFighter
{
    public class Lander
    {
        public Vector2 position { get; set; } = Vector2.Zero;
        public Vector2 velocity { get; set; } = Vector2.Zero;
        public float angle { get; set; } = 270;
        public bool engineOn { get; set; } = false;
        public float speed { get; set; } = 0.05f;
        private float speedMax = 2f;
        public Texture2D img { get; set; }
        public Texture2D imgEngine { get; set; }

        public void update()
        {
            velocity += new Vector2(0, 0.005f);

            if (Math.Abs(velocity.X) >= speedMax)
            {
                velocity = new Vector2((velocity.X < 0 ? 0 - speedMax : speedMax), velocity.Y);
            }
            if (Math.Abs(velocity.Y) >= speedMax)
            {
                velocity = new Vector2(velocity.X, (velocity.Y < 0 ? 0 - speedMax : speedMax));
            }

            position += velocity;
        }
    }
}
