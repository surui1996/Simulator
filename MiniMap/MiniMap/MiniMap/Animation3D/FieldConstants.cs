﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniMap.Animation3D
{
    class FieldConstants
    {
        public const float CAMERA_RATIO = 640f / 480f;

        //in feet
        public const float WIDTH = 24f + (8f / 12f);
        public const float HEIGHT = 54f;
        public const float RATIOZX = HEIGHT / WIDTH;
        public const float PICTURE_RATIO = 961f / 351f;
        public const float HIGHGOAL_HEIGHT = 3f + (1f / 12f);
        private const float high_goal_bottom_above_carpet = 6f + (10.75f / 12f);
        public const float HEIGHT_ABOVE_CARPET = high_goal_bottom_above_carpet + HIGHGOAL_HEIGHT;
        public const float RATIOYX = WIDTH / HEIGHT_ABOVE_CARPET;
        //public static float LOWGOAL_HEIGHT_ABOVE_CARPET = 7f / 12f; // 7 in.
        //public static float LOWGOAL_HEIGHT = 2f + 4f / 12f; //2 ft. 4 in.
        public const float LOWGOAL_WIDTH = 2f + 5f / 12f; //2 ft. 5 in.

        public const float FOOT_IN_METERS = 0.3048f;
        public const float HEIGHT_IN_METERS = HEIGHT * FOOT_IN_METERS;
        public const float WIDTH_IN_METERS = WIDTH * FOOT_IN_METERS;

        public const float DYNAMIC_HEIGHT_ABOVE_CARPET = 5f + (8f / 12f);  //5 ft. 8 in.
        public const float DYNAMIC_WIDTH = 1f + (11.5f / 12f); //1ft. 11.5 in.
        public const float DYNAMIC_HEIGHT = 4f / 12f; // 4 in.
        public const float STATIC_HEIGHT_ABOVE_CARPET = 3f + (1.5f / 12f); //3 ft. 1 ½ in.
        public const float STATIC_WIDTH = 4f / 12f; // 4 in.
        public const float STATIC_HEIGHT = 2f + (8f / 12f); // 2 ft. 8 in.
        public const float STATIC_BLACK_STRIPES_WIDTH = 2f / 12f; //2 in.

        //1foot = C pixels
        public const float C = 10f;
        public const float PIXELS_IN_ONE_METER = FieldConstants.C / FieldConstants.FOOT_IN_METERS;



        
    }
}
