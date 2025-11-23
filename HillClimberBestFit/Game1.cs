using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using System.Collections.Generic;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace HillClimberBestFit
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch sb;
        private MouseState previousMouseState;
        private List<PointCircle> points = new List<PointCircle>();
        private List<PointCircle> normPoints = new List<PointCircle>();
        int halfWidth;
        int halfHeight;
        int width;
        int height;
        Random random;
        Line line;
        Line changedLine;

        bool hasCreated = false;
        bool hasFinished = false;

        bool hasStarted = false;
        float minError = float.MaxValue;
        float currError = float.MaxValue;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            halfHeight = GraphicsDevice.Viewport.Height / 2;
            halfWidth = GraphicsDevice.Viewport.Width / 2;
            height = GraphicsDevice.Viewport.Height;
            width = GraphicsDevice.Viewport.Width;
            random = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState ms = Mouse.GetState();

            if (ms.LeftButton == ButtonState.Pressed)
            {
                points.Add(new PointCircle(new Vector2(ms.X, ms.Y), 2, sb));
                normPoints = NormalizeList(points);
                if (line != null)
                {
                    currError = MAECalc(normPoints, changedLine);
                    minError = currError;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !hasStarted)
            {
                hasStarted = true;
                if (hasStarted && !hasCreated)
                {
                    line = new Line(new Vector2(0, random.Next(0, height)),
                        (float)(random.NextDouble() * 4 - 2), sb, width, height);
                    changedLine = new Line(line);
                    hasCreated = true;
                }
                
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Back))
            {
                hasStarted = false;
                hasCreated = false;
                hasFinished = false;
                points.Clear();
                line = null;
                changedLine = null;
                minError = float.MaxValue;
                currError = float.MaxValue;
            }

            AnimateLineImprovement();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            sb.Begin();

            sb.DrawLine(0, halfHeight, width, halfHeight, Color.Black);
            sb.DrawLine(halfWidth, 0, halfWidth, height, Color.Black);

            foreach (PointCircle point in points)
            {
                point.Draw();
            }

            if (hasCreated || hasFinished)
            {
                line.Draw();
            }


            // TODO: Add your drawing code here
            sb.End();
            base.Draw(gameTime);
        }


        public float MAECalc(List<PointCircle> points, Line line)
        {
            float error = 0;

            foreach (PointCircle pc in points)
            {
                error += line.GetError(pc.point);
            }

            return error / points.Count;
        }

        public List<PointCircle> NormalizeList(List<PointCircle> points)
        {
            List<PointCircle> normPoints = new List<PointCircle>();
            Vector2 maxPoint = new Vector2(0, 0);
            Vector2 minPoint = new Vector2(int.MaxValue, int.MaxValue);
            foreach (PointCircle p in points)
            {
                if (p.point.X > maxPoint.X)
                    maxPoint.X = (int)p.point.X;
                if (p.point.Y > maxPoint.Y)
                    maxPoint.Y = (int)p.point.Y;
                if (p.point.X < minPoint.X)
                    minPoint.X = (int)p.point.X;
                if (p.point.Y < minPoint.Y)
                    minPoint.Y = (int)p.point.Y;
            }
            foreach (PointCircle pc in points)
            {
                Vector2 normPoint = new Vector2(
                    (int)((pc.point.X - minPoint.X) / (float)(maxPoint.X - minPoint.X) * width),
                    (int)((pc.point.Y - minPoint.Y) / (float)(maxPoint.Y - minPoint.Y) * height)
                );
                normPoints.Add(new PointCircle(normPoint, pc.radius, sb));
            }
            return normPoints;
        }


        public void AnimateLineImprovement()
        {

            if (hasCreated && currError > 0.01f)
            {
                for (int i = 0; i < 100; i++)
                {
                    //idk if this works well, but it did help a little so go me
                    float scale = MathF.Max(0.01f, minError / currError);

                    int choice = random.Next(0, 2);
                    float change = (float)(random.NextDouble() * 2 - 1);
                    float valChange = change * scale;
                    if (choice == 0)
                        changedLine.yIntercept.Y += valChange;
                    else
                        changedLine.slope += valChange;

                    currError = MAECalc(points, changedLine);

                    if (currError < minError)
                    {
                        minError = currError;
                        line = new Line(changedLine);
                    }
                    else
                    {
                        changedLine = new Line(line);
                    }
                }
            }
            else if (currError <= 0.01f)
            {
                hasCreated = false;
                hasStarted = false;
                hasFinished = true;
            }
        }
    }
}
