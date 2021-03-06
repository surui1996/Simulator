﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotSimulator._3D
{
    class WheeledBox
    {
        //maybe also add height

        public Vector3 CenterPosition { get; set; }

        private Box3 box;
        private Cylinder frontLeft, frontRight, rearLeft, rearRight;

        //length, width, etc in pixels
        public WheeledBox(Texture2D boxTexture, Texture2D wheelSideTexture, Texture2D wheelCircumferenceTexture,
            float length, float width, float wheelRadius, Vector3 initialCenterPosition)
        {
            float wheelWidth = width / 10;

            float boxHeight = wheelRadius;

            box = new Box3(boxTexture, width, boxHeight, length, new Vector3(-width / 2, wheelRadius - (boxHeight / 2), -length / 2) + initialCenterPosition, 1);

            float plusX = width / 2;
            float minusX = -width / 2 - wheelWidth;

            frontLeft = new Cylinder(wheelCircumferenceTexture, wheelSideTexture, wheelRadius, wheelWidth,
                initialCenterPosition + new Vector3(plusX, wheelRadius, length / 2 - 1.2f * wheelRadius));
            frontRight = new Cylinder(wheelCircumferenceTexture, wheelSideTexture, wheelRadius, wheelWidth,
                initialCenterPosition + new Vector3(minusX, wheelRadius, length / 2 - 1.2f * wheelRadius));
            rearLeft = new Cylinder(wheelCircumferenceTexture, wheelSideTexture, wheelRadius, wheelWidth,
                initialCenterPosition + new Vector3(plusX, wheelRadius, -length / 2 + 1.2f * wheelRadius));
            rearRight = new Cylinder(wheelCircumferenceTexture, wheelSideTexture, wheelRadius, wheelWidth,
                initialCenterPosition + new Vector3(minusX, wheelRadius, -length / 2 + 1.2f * wheelRadius));
        }

        public void Draw(GraphicsDevice device, BasicEffect effect, float angleY = 0)
        {
            box.Draw(device, effect, angleY);
            frontLeft.Draw(device, effect, angleY);
            frontRight.Draw(device, effect, angleY);
            rearLeft.Draw(device, effect, angleY);
            rearRight.Draw(device, effect, angleY);
        }
    }
}
