using System;
using System.Collections.Generic;
using System.Text;
using static NavigationAlertLibrary.Enums;

namespace NavigationAlertLibrary
{
    public static class HelperMethods
    {
        public static Direction GetDirectionFromCordName(CardinalDirection cordName, CardinalDirection currentDirection)
        {
            CardinalDirection[] coordNames = { CardinalDirection.N, CardinalDirection.NE, CardinalDirection.E, CardinalDirection.SE,
                CardinalDirection.S, CardinalDirection.SW, CardinalDirection.W, CardinalDirection.NW };

            var currIndex = Array.IndexOf<CardinalDirection>(coordNames, currentDirection);
            var nextIndex = Array.IndexOf<CardinalDirection>(coordNames, cordName);

            if (nextIndex == (currIndex + 1) % 8)
                return Direction.slight_Right;
            else if (nextIndex == (currIndex + 2) % 8)
                return Direction.right;
            else if (currIndex == (nextIndex + 1) % 8)
                return Direction.slight_Left;
            else if (nextIndex == (nextIndex + 2) % 8)
                return Direction.left;

            return Direction.straight;
        }

        public static void Alert(string timestamp, string distance, Direction direction)
        {
            switch (direction)
            {
                case Direction.reached:
                    Console.WriteLine(timestamp + " : Reached Destination");
                    break;

                case Direction.straight:
                    Console.WriteLine(timestamp + " : Go straight for " + distance + " kilometers");
                    break;

                default:
                    Console.WriteLine(timestamp + " : Take a " + direction.ToString() + ".");
                    break;
            }
        }


        public static double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = DegTorad(lat2 - lat1);  // deg2rad below
            var dLon = DegTorad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(DegTorad(lat1)) * Math.Cos(DegTorad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }

        public static CardinalDirection GetDirectionFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {

            var radians = Math.Atan2((lon2 - lon1), (lat2 - lat1));

            var compassReading = radians * (180 / Math.PI);

            CardinalDirection[] coordNames = { CardinalDirection.N, CardinalDirection.NE, CardinalDirection.E, CardinalDirection.SE,
                CardinalDirection.S, CardinalDirection.SW, CardinalDirection.W, CardinalDirection.NW, CardinalDirection.N };

            var coordIndex = Math.Round(compassReading / 45);
            if (coordIndex < 0)
            {
                coordIndex = coordIndex + 8;
            };

            return coordNames[(int)coordIndex]; // returns the coordinate value
        }

        public static double DegTorad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
