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
        public int screenWidth;
        public int screenHeight;
        public Line(Vector2 yIntercept, float slope, SpriteBatch sb, int width, int height)
        {
            this.yIntercept = yIntercept;
            this.slope = slope;
            this.sb = sb;
            screenWidth = width;
            pointOne = new Vector2(width, slope * width + yIntercept.Y);
        }

        public Line(Line line)
        {
            this.yIntercept = line.yIntercept;
            this.slope = line.slope;
            this.sb = line.sb;
            this.screenWidth = line.screenWidth;
            pointOne = new Vector2(screenWidth, slope * screenWidth + yIntercept.Y);
        }


        public void Draw()
        {
            sb.DrawLine(yIntercept, pointOne, Color.Red);
        }
        public float GetError(Vector2 point)
        {
            Vector2 convert = (point); 
            float lineY = CalcY(convert.X);
            return Math.Abs(lineY - convert.Y);
        }
        public float CalcY(float X)
        {
            return (new Vector2(X,slope * X + yIntercept.Y)).Y;
        }


    }
}
