﻿using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemonstrationCritters
{
    public class Wanderer : ICritterController
    {
        private readonly bool Debugging = false;

        public string Name { get; set; }

        public Send Responder { get; set; }

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        private void Log(string msg)
        {
            if (Debugging)
            {
                Console.WriteLine(Name + ":" + msg);
            }
        }

        private void Send(string message)
        {
            Responder.Invoke(message);
        }

        public Wanderer(string name)
        {
            Debugging = false;

            Name = name;
        }

        public void LaunchUI()
        {
            // TODO - need to provide this.
        }

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    Send("RANDOM_DESTINATION");
                    if (Debugging)
                    {
                        Send("DEBUG:1");
                    }
                    break;
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    Send("RANDOM_DESTINATION");
                    break;
                case "ERROR":
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
