﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SCG.TurboSprite;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.CodeDom.Compiler;
using System.CodeDom;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace CritterWorld
{
    public class Critter : PolygonSprite
    {
        public const float movementEnergyConsumptionFactor = 250;  // the higher this is, the less movement consumes energy
        public const float eatingAddsEnergy = 50.0F;       // each piece of food adds this much energy; maximum 100
        public const float eatingAddsHealth = 10.0F;
        public const float fightingDeductsHealth = 0.5F;
        public const float bumpingTerrainDeductsHealth = 0.25F;

        public int Number { get; private set; }

        public string Name { get; set; } = GetRandomName();
        public string Author { get; set; } = "Critterworld";

        public string NameAndAuthor { get { return Name + " by " + Author; } }

        public int EscapedCount { get; private set; }
        public int BombedCount { get; private set; }
        public int CrashedCount { get; private set; }
        public int TerminatedCount { get; private set; }
        public int StarvedCount { get; private set; }
        public int FatallyInjuredCount { get; private set; }

        public int OverallScore { get; private set; }
        public int CurrentScore { get; private set; }

        public float Energy { get; private set; }
        public float Health { get; private set; }

        public bool IsEscaped { get; private set; }

        public string DeadReason { get; private set; } = null;
        public bool IsDead { get { return DeadReason != null; } }

        public BlockingCollection<string> MessagesFromBody { get; } = new BlockingCollection<string>();
        public BlockingCollection<string> MessagesToBody { get; } = new BlockingCollection<string>();

        private int moveCount = 0;

        private bool stopped = true;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        private TextSprite numberPlate = null;
        private int numberPlateIncrement = 1;

        private static void CritterProcessor(Sprite sprite)
        {
            if (sprite is Critter critter)
            {
                if (sprite.Mover is TargetMover mover && (mover.SpeedX != 0 || mover.SpeedY != 0))
                {
                    mover.TargetFacingAngle = (int)GetAngle(mover.SpeedX, mover.SpeedY) + 90;
                }

                if (critter.numberPlate != null)
                {
                    if (critter.stopped)
                    {
                        critter.numberPlate.Color = Color.LightGray;
                        critter.numberPlate.FillColor = Color.LightGray;
                        critter.numberPlate.Alpha = 255;
                    }
                    else
                    {
                        critter.numberPlate.Position = critter.Position;
                        critter.numberPlate.Alpha += (byte)critter.numberPlateIncrement;
                        if (critter.numberPlate.Alpha == 255)
                        {
                            critter.numberPlateIncrement = -1;
                        }
                        else if (critter.numberPlate.Alpha == 0)
                        {
                            critter.numberPlateIncrement = 1;
                        }
                    }
                }
            }
        }

        internal static string GetRandomName()
        {
            string[] consonants = { "b", "c", "d", "f", "ff", "g", "gh", "h", "j", "l", "m", "l", "n", "p", "ph", "q", "r", "s", "th", "tt", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "y", "ee", "ea", "io", "oi", "ae" };
            string name = "";
            int len = rnd.Next(2, 5);
            while (len-- > 0)
            {
                name += consonants[rnd.Next(consonants.Length)];
                name += vowels[rnd.Next(vowels.Length)];
            }
            return name[0].ToString().ToUpper() + name.Substring(1);
        }

        internal Critter(int critterNumber) : base(new CritterBody().GetBody(1))
        {
            Number = critterNumber;

            LineWidth = 1;
            Color = Sprite.RandomColor(127);

            Reset();

            Processors += CritterProcessor;
        }

        internal void Reset()
        {
            numberPlate = null;
            FacingAngle = 90;
            CurrentScore = 0;
            Health = 100;
            Energy = 100;
            IsEscaped = false;
            DeadReason = null;
            Dead = false;
        }

        private void Log(String message, Exception exception = null)
        {
            LogEntry newLogEntry = new LogEntry(Number, Name, Author, message, exception);
            Critterworld.Log(newLogEntry);
        }

        internal void Escaped()
        {
            Log("escaped");
            EscapedCount++;
            OverallScore += CurrentScore;
            Kill();
            IsEscaped = true;
        }

        internal void Scored()
        {
            Log("scored");
            CurrentScore++;
        }

        internal void Ate()
        {
            Log("ate");
            if (Energy + eatingAddsEnergy > 100)
            {
                Energy = 100;
            }
            else
            {
                Energy += eatingAddsEnergy;
            }
            if (Health + eatingAddsHealth > 100)
            {
                Health = 100;
            }
            else
            {
                Health += eatingAddsHealth;
            }
        }

        internal void ConsumeEnergy(float consumption)
        {
            if (Energy - consumption <= 0)
            {
                Energy = 0;
                Starved();
                Shutdown();
            }
            else
            {
                Energy -= consumption;
            }
        }

        internal void FightWith(string opponent)
        {
            if (Health - fightingDeductsHealth <= 0)
            {
                Health = 0;
                FatallyInjured();
                Shutdown();
            }
            else
            {
                Health -= fightingDeductsHealth;
            }
        }

        internal void Bump()
        {
            if (Health - bumpingTerrainDeductsHealth <= 0)
            {
                Health = 0;
                FatallyInjured();
                Shutdown();
            }
            else
            {
                Health -= bumpingTerrainDeductsHealth;
            }
        }

        internal void FatallyInjured()
        {
            FatallyInjuredCount++;
            DeadReason = "fatally injured";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Starved()
        {
            StarvedCount++;
            DeadReason = "starved";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Bombed()
        {
            BombedCount++;
            DeadReason = "bombed";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Crashed()
        {
            CrashedCount++;
            DeadReason = "crashed";
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void Terminated(string reason)
        {
            TerminatedCount++;
            DeadReason = "terminated for " + reason;
            Log(DeadReason);
            Health = 0;
            Energy = 0;
        }

        internal void ShowShockwave()
        {
            Sound.PlayArc();
            Sprite shockwave = new ShockWaveSprite(5, 20, 50, Color.DarkBlue, Color.LightBlue);
            shockwave.Position = Position;
            shockwave.Mover = new SlaveMover(this);
            Engine?.AddSprite(shockwave);
        }

        internal void ClearDestination()
        {
            AssignDestination((int)X, (int)Y);
        }

        internal void AssignDestination(int destX, int destY)
        {
            if (Mover is TargetMover mover)
            {
                mover.Speed = rnd.Next(10) + 1;
                mover.Target = new Point(destX, destY);
                mover.StopAtTarget = true;
            }
        }

        internal void AssignRandomDestination()
        {
            int destX = rnd.Next(Surface.Width);
            int destY = rnd.Next(Surface.Height);
            AssignDestination(destX, destY);
        }

        // Bounce back to position before most recent move. 
        // Invoke after a collision to prevent "embedding" or slowly 
        // creeping through obstacles when a collision is detected.
        internal void Bounceback()
        {
            if (Mover is TargetMover mover)
            {
                mover.Bounceback();
            }
        }

        // Something has crashed, burned out or blown up. Stop thinking, moving, or doing anything except
        // emit smoke for a while.
        internal void StopAndSmoke(Color startColor, Color endColor)
        {
            Mover = new NullMover();
            Shutdown();
            ParticleFountainSprite smoke = new ParticleFountainSprite(20, startColor, endColor, 1, 10, 10)
            {
                Position = Position
            };
            Engine?.AddSprite(smoke);
            System.Timers.Timer smokeTimer = new System.Timers.Timer
            {
                Interval = 1000,
                AutoReset = true
            };
            smokeTimer.Elapsed += (sender2, e2) =>
            {
                if (smoke.EndDiameter >= 2)
                {
                    smoke.EndDiameter -= 1;
                    smoke.Radius -= 1;
                }
                else
                {
                    smoke.Kill();
                    smokeTimer.Stop();
                }
            };
            smokeTimer.Start();
        }

        internal void Crash()
        {
            Crashed();
            Sound.PlayCrash();
            StopAndSmoke(Color.DarkBlue, Color.LightBlue);
            Log("Crashed due to exception in user code.");
        }

        // Create a number plate for this Critter at a given position
        private TextSprite CreateNumberPlate()
        {
            return new TextSprite(Number.ToString(), "Arial", 14, FontStyle.Regular)
            {
                Position = Position,
                IsFilled = true,
                Color = Color.White,
                FillColor = Color.White,
                Alpha = 200
            };
        }

        // Attach a number plate to this Critter.
        private void AttachNumberPlate()
        {
            if (numberPlate != null)
            {
                return;
            }
            numberPlate = CreateNumberPlate();
            numberPlate.Mover = new SlaveMover(this);
            Engine?.AddSprite(numberPlate);
        }

        private static void MoveHandler(object sender, SpriteMoveEventArgs mover)
        {
            if (mover.Sprite is Critter critter)
            {
                critter.ConsumeEnergy(mover.Distance * mover.Speed / movementEnergyConsumptionFactor);
                if (critter.moveCount-- == 0)
                {
                    critter.IncrementFrame();
                    critter.moveCount = 5 - Math.Min(5, (int)mover.Speed);
                }
            }
        }

        // Launch this Critter.
        internal void Launch()
        {
            Reset();

            AttachNumberPlate();

            TargetMover spriteMover = new TargetMover();
            spriteMover.SpriteReachedTarget += (sender, spriteEvent) => AssignRandomDestination();
            spriteMover.SpriteMoved += MoveHandler;
            Mover = spriteMover;

            stopped = false;

            Log("launched");

            AssignRandomDestination();
        }

        // Shut down this Critter.
        internal void Shutdown()
        {
            if (stopped)
            {
                return;
            }

            Mover = new NullMover();
            stopped = true;

            Log("shutdown");
        }

        // True if this critter is stopped or dead
        internal bool Stopped
        {
            get
            {
                return Dead || stopped;
            }
        }

        public override void Kill()
        {
            Shutdown();
            if (numberPlate != null)
            {
                numberPlate.Kill();
                numberPlate = null;
            }
            base.Kill();
        }
    }
}
