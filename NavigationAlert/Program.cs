using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationAlert
{
    // This program has some assumptions -
    // 1. The first co-ordinates are start co-ordinates and start direction is East
    // 2. The person can not go back , he can go only to straight, right , left, slight right, slight left
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var filePath = "..\\..\\TestFiles\\input1.csv";
                var latlongList = GetFileData(filePath);
                var speed = 10; // In km/hr

                AlertPath(latlongList, speed);
                Console.ReadKey();
            }
            catch(Exception e)
            {
                Console.WriteLine("Not able to Alert Navigation. Exception : " + e.StackTrace);
            }
        }

        private static void AlertPath(List<LatitudeLongitude> latlongList, double speed)
        {
            var startTime = DateTime.Now;
            var currentDirection = CardinalDirection.E;

            for (int i=0; i< latlongList.Count - 1; i++)
            {
                var dist = GetDistanceFromLatLonInKm(latlongList[i].latitude, latlongList[i].longitude,
                    latlongList[i + 1].latitude, latlongList[i + 1].longitude);
                //Console.WriteLine("Distance : " + dist.ToString());

                var cordName = GetDirectionFromLatLonInKm(latlongList[i].latitude, latlongList[i].longitude,
                    latlongList[i + 1].latitude, latlongList[i + 1].longitude);
                Console.WriteLine("Direction : " + cordName);

                var timeNeeded = dist / speed;

                startTime = startTime.AddMinutes(timeNeeded);

                var direction = GetDirectionFromCordName(cordName, currentDirection);

                currentDirection = cordName;

                Alert(startTime.ToString("HH:mm:ss tt"), dist.ToString("0.00") , direction);
            }
        }

        private static Direction GetDirectionFromCordName(CardinalDirection cordName, CardinalDirection currentDirection)
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
            else if (currIndex == (nextIndex + 2) % 8)
                return Direction.left;

            return Direction.straight;
        }

        private static void Alert(string timestamp, string distance, Direction direction)
        {
            switch(direction)
            {
                case Direction.reached:
                    Console.WriteLine(timestamp +" : Reached Destination");
                    break;

                case Direction.straight:
                    Console.WriteLine(timestamp + " : Go straight for " + distance + " kilometers");
                    break;

                default:
                    Console.WriteLine(timestamp + " : Take a " + direction.ToString() + ".");
                    break;
            }
        }

        private static List<LatitudeLongitude> GetFileData(string filePath)
        {
            var fileData = File.ReadAllText(filePath).Replace("\r\n", ";").Split(';');

            var latlongList = new List<LatitudeLongitude>();

            foreach (var data in fileData)
            {
                if (String.IsNullOrEmpty(data))
                    continue;

                var latlong = new LatitudeLongitude();

                double lat;
                Double.TryParse(data.Split(',')[0], out lat);
                latlong.latitude = lat;

                double longitude;
                Double.TryParse(data.Split(',')[1], out longitude);
                latlong.longitude = longitude;

                latlongList.Add(latlong);
            }

            return latlongList;
        }

        private static double GetDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
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

        private static CardinalDirection GetDirectionFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
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

        private static double DegTorad(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }

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

    public class LatitudeLongitude
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
