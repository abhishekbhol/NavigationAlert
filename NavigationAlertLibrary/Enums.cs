using System;
using System.Collections.Generic;
using System.Text;

namespace NavigationAlertLibrary
{
    public class Enums
    {
        public enum CardinalDirection
        {
            N,
            NE,
            E,
            SE,
            S,
            SW,
            W,
            NW
        }

        public enum Direction
        {
            straight,
            left,
            right,
            slight_Right,
            slight_Left,
            reached
        }
    }
}
