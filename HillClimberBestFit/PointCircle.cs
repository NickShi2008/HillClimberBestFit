using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HillClimberBestFit
{
    public class PointCircle
    {
        public Vector2 point;
        public int radius;
        public SpriteBatch sb;
        public CircleF circle;
        public PointCircle(Vector2 point, int radius, SpriteBatch sb)
        {
            this.sb = sb;
            this.point = point;
            this.radius = radius;
            circle = new CircleF(point, radius);
        }

        public void Draw()
        {
            sb.DrawCircle(circle, 32, Color.Black, 2);
        }
    }
}
