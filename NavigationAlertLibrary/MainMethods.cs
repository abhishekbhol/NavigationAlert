using System;
using System.Collections.Generic;
using System.IO;
using static NavigationAlertLibrary.Enums;

namespace NavigationAlertLibrary
{
    public class MainMethods
    {
        /// <summary>
        /// This method is the main method which takes csv file path as input and speed of the delivery speed.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="speed"></param>
        public static void AlertPath(String filePath, double speed)
        {
            var startTime = DateTime.Now;
            var currentDirection = CardinalDirection.E;

            List<LatitudeLongitude> latlongList = GetFileData(filePath);

            for (int i = 0; i < latlongList?.Count - 1; i++)
            {
                var dist = HelperMethods.GetDistanceFromLatLonInKm(latlongList[i].latitude, latlongList[i].longitude,
                    latlongList[i + 1].latitude, latlongList[i + 1].longitude);
                //Console.WriteLine("Distance : " + dist.ToString());

                var cordName = HelperMethods.GetDirectionFromLatLonInKm(latlongList[i].latitude, latlongList[i].longitude,
                    latlongList[i + 1].latitude, latlongList[i + 1].longitude);
                //Console.WriteLine("Direction : " + cordName);

                var timeNeeded = dist / speed;

                startTime = startTime.AddMinutes(timeNeeded);

                var direction = HelperMethods.GetDirectionFromCordName(cordName, currentDirection);

                currentDirection = cordName;

                HelperMethods.Alert(startTime.ToString("HH:mm:ss tt"), dist.ToString("0.00"), direction);
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

    }
}
