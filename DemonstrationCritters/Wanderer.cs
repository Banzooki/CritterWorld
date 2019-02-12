﻿using CritterController;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public Wanderer(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {

        }

        public void Receive(string message, ConcurrentQueue<string> messagesToBody)
        {
            if (Debugging)
            {
                Console.WriteLine("Message from body for " + Name + ": " + message);
            }
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    messagesToBody.Enqueue("RANDOM_DESTINATION");
                    if (Debugging)
                    {
                        messagesToBody.Enqueue("DEBUG:1");
                    }
                    break;
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    messagesToBody.Enqueue("RANDOM_DESTINATION");
                    break;
                case "ERROR":
                    Console.WriteLine(message);
                    break;
            }
        }
    }
}
