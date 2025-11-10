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
    public class Line
    {
        public Vector2 yIntercept;
        public float slope;
        public SpriteBatch sb;
        public Vector2 pointOne;
        public Vector2 pointTwo;
        public int screenWidth;
        public Line(Vector2 yIntercept, float slope, SpriteBatch sb, int width)
        {
            this.yIntercept = yIntercept;
            this.slope = slope;
            this.sb = sb;
            screenWidth = width;
            pointOne = new Vector2(width, slope * width + yIntercept.Y);
            pointTwo = new Vector2(0, yIntercept.Y);
        }

        public Line(Line line)
        {
            this.yIntercept = line.yIntercept;
            this.slope = line.slope;
            this.sb = line.sb;
            this.screenWidth = line.screenWidth;
            pointOne = new Vector2(screenWidth, slope * screenWidth + yIntercept.Y);
            pointTwo = new Vector2(0, yIntercept.Y);
        }

        public void Draw()
        {
            sb.DrawLine(pointTwo, pointOne, Color.Red);
            //sb.DrawLine()
        }
        public float GetError(float x, float y)
        {
            float lineY = CalcY(x);
            return Math.Abs(lineY - y);
        }
        public float CalcY(float X)
        {
            return slope * X + yIntercept.Y;
        }


    }
}
