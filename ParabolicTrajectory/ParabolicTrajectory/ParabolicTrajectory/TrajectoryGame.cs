﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ParabolicTrajectory
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TrajectoryGame : Microsoft.Xna.Framework.Game
    {
        #region Graphics Variabels
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D ball2D;
        Texture2D arrow2D;
        Texture2D ground2D;
        #endregion

        #region Constants
        const int GROUND_LEN = 225;
        const float X_POS = 50f, Y_POS = 400f, DEFAULT_VELOCITY = 10f, DEFAULT_ANGLE = 45f, DT = 0.005f, SCALE = 0.1f;
        Color DEFAULT_COLOR = Color.CornflowerBlue;
        #endregion

        #region Updating Variables
        float velocity = DEFAULT_VELOCITY, angle = DEFAULT_ANGLE;
        bool spaceClicked = false, finishedShot = false;
        float timeFromShot;
        int positionIndex;
        #endregion

        #region Game Objects
        Graph graph;
        Ball ball, ball2, dragBall;
        Camera camera;
        List<Vector3> simpleTrajectory; bool trajectoryIsNull = true;
        #endregion

        public TrajectoryGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera = new Camera();
            graph = new Graph(GraphicsDevice, camera);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("font");
            ball2D = Content.Load<Texture2D>("transparent ball");
            arrow2D = Content.Load<Texture2D>("transparent arrow");
            ground2D = Content.Load<Texture2D>("ground");

            ball = new Ball(new Vector2(X_POS, Y_POS), DEFAULT_VELOCITY, DEFAULT_ANGLE, ball2D);
            ball.CalculateTrajectoryWithNoDrag();
            ball2 = new Ball(new Vector2(X_POS, Y_POS), DEFAULT_VELOCITY, DEFAULT_ANGLE, ball2D);
            ball2.CalculateTrajectoryWithNoDrag2();
            dragBall = new Ball(new Vector2(X_POS, Y_POS), DEFAULT_VELOCITY, DEFAULT_ANGLE, ball2D);
            dragBall.CalculateTrajectoryWithDrag();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape)) // Allows the game to exit
                this.Exit();
            else if (state.IsKeyDown(Keys.W))
                velocity += 0.5f;
            else if (state.IsKeyDown(Keys.S))
                velocity -= 0.5f;
            else if (state.IsKeyDown(Keys.D))
                angle -= 0.05f;
            else if (state.IsKeyDown(Keys.A))
                angle += 0.05f;
            else if (state.IsKeyDown(Keys.Z))
                camera.ZoomIn();
            else if (state.IsKeyDown(Keys.X))
                camera.ZoomOut();
            else if (state.IsKeyDown(Keys.Up))
                camera.Translate(Keys.Up);
            else if (state.IsKeyDown(Keys.Down))
                camera.Translate(Keys.Down);
            else if (state.IsKeyDown(Keys.Left))
                camera.Translate(Keys.Left);
            else if (state.IsKeyDown(Keys.Right))
                camera.Translate(Keys.Right);
            else if (state.IsKeyDown(Keys.Enter))
            {
                //Trajectory 2 seems to be more of what i wanted...
                simpleTrajectory = Ball.GetSimpleTrajectory2(dragBall.MaximumPoint, dragBall.InitialVelocityMagnitude,
                    dragBall.InitialAngle, new Vector2(X_POS, Y_POS));
                trajectoryIsNull = false;
            }

            if (state.IsKeyDown(Keys.Space) && !spaceClicked)
            {
                spaceClicked = true;
                timeFromShot = 0;
                positionIndex = 0;
                ball.InitialAngle = angle;
                ball.InitialVelocityMagnitude = velocity;
                ball.CalculateTrajectoryWithNoDrag();

                ball2.InitialAngle = angle;
                ball2.InitialVelocityMagnitude = velocity;
                ball2.CalculateTrajectoryWithNoDrag2();

                dragBall.InitialAngle = angle;
                dragBall.InitialVelocityMagnitude = velocity;
                dragBall.CalculateTrajectoryWithDrag();
            }
            else if (!state.IsKeyDown(Keys.Space) && spaceClicked)
            {
                spaceClicked = false;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(DEFAULT_COLOR);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, camera.Transform());
            DrawShootingParameters();
            DrawMovingBall(gameTime.ElapsedGameTime.Milliseconds / 1000f);
            DrawGround();
            spriteBatch.End();

            graph.Draw(ball.Get3DPositions(), Color.Black);
            graph.Draw(ball2.Get3DPositions(), Color.Red);
            graph.Draw(dragBall.Get3DPositions(), Color.Green);
            graph.DrawLine(ball.MaximumPoint, new Vector2(ball.MaximumPoint.X, Y_POS), Color.Black);
            graph.DrawLine(ball2.MaximumPoint, new Vector2(ball2.MaximumPoint.X, Y_POS), Color.Red);
            graph.DrawLine(dragBall.MaximumPoint, new Vector2(dragBall.MaximumPoint.X, Y_POS), Color.Green);
            if (!trajectoryIsNull)
            {
                graph.Draw(simpleTrajectory, Color.GreenYellow);
                Vector2 p = Ball.CalculateMaximumPoint(Ball.To2D(simpleTrajectory));
                graph.DrawLine(p, new Vector2(p.X, Y_POS), Color.GreenYellow);
            }

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draws Into the screen the velocity and angle udating variables
        /// </summary>
        private void DrawShootingParameters()
        {
            spriteBatch.DrawString(font, "velocity: " + Math.Round(velocity).ToString() + "m/s", new Vector2(600, 30), Color.Red);
            spriteBatch.DrawString(font, "angle: " + angle.ToString(), new Vector2(600, 60), Color.Red);
            spriteBatch.Draw(arrow2D, new Vector2(X_POS, Y_POS), null, Color.CornflowerBlue, -MathHelper.ToRadians(angle),
                new Vector2(0f, 55f), new Vector2(SCALE * 0.8f * velocity / DEFAULT_VELOCITY, SCALE), SpriteEffects.None, 0);
        }

        /// <summary>
        /// Draws Into the screen the moving ball according to the time
        /// <param name="ellapsedSeconds"> seconds elapsed since space was clicked </param>
        /// </summary>
        private void DrawMovingBall(float ellapsedSeconds)
        {
            if (!finishedShot)
            {
                positionIndex = (int)(timeFromShot / DT);
                finishedShot = ball.DrawBall(positionIndex, spriteBatch) || ball2.DrawBall(positionIndex, spriteBatch)
                    || dragBall.DrawBall(positionIndex, spriteBatch);


                timeFromShot += ellapsedSeconds;
            }
            else
                finishedShot = ball.DrawBall(positionIndex, spriteBatch) || ball2.DrawBall(positionIndex, spriteBatch)
                    || dragBall.DrawBall(positionIndex, spriteBatch);
        }

        /// <summary>
        /// Draws horizontal ground with the ground2D texture
        /// </summary>
        private void DrawGround()
        {
            float len = ((float)GROUND_LEN * SCALE);
            for (int i = 0; i <= (int)((float)(graphics.PreferredBackBufferWidth) / len); i++)
                spriteBatch.Draw(ground2D, new Vector2(i * len, Y_POS), null, Color.White, 0f, new Vector2(0, 0),
                    SCALE, SpriteEffects.None, 0);
        }
    }
}
