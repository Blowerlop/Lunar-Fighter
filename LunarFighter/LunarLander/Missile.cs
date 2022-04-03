using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarFighter
{
    class Missile
    {
        public bool Tir = false;
        public Vector2 Position;
        public Vector2 VelocityTir;


        public void Update()
        {
            Position += VelocityTir;
        }
    }
}
