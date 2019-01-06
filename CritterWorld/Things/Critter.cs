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

namespace CritterWorld
{
    class Critter : PolygonSprite
    {
        public const int maxThinkTimeMilliseconds = 1000;
        public const int maxThinkTimeOverrunViolations = 5;

        public int thinkTimeOverrunViolations = 0;
        public long thinkCount = 0;
        public long totalThinkTime = 0;

        private int moveCount = 0;

        private Thread thinkThread = null;
        private bool stopped = true;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public void ClearDestination()
        {
            AssignDestination((int)X, (int)Y);
        }

        public void AssignDestination(int destX, int destY)
        {
            TargetMover mover = (TargetMover)Mover;
            if (mover == null)
            {
                return;
            }
            mover.Speed = rnd.Next(10) + 1;
            mover.Target = new Point(destX, destY);
            mover.StopAtTarget = true;
        }

        public void AssignRandomDestination()
        {
            int destX = rnd.Next(Surface.Width);
            int destY = rnd.Next(Surface.Height);
            AssignDestination(destX, destY);
        }

        // Bounce back to position before most recent move. 
        // Invoke after a collision to prevent "embedding" or slowly 
        // creeping through obstacles when a collision is detected.
        public void Bounceback()
        {
            ((TargetMover)Mover)?.Bounceback();
        }

        public long TotalThinkTime
        {
            get
            {
                return totalThinkTime;
            }
        }

        public long ThinkCount
        {
            get
            {
                return thinkCount;
            }
        }

        public double AverageThinkTime
        {
            get
            {
                if (ThinkCount == 0)
                {
                    return double.NaN;
                }
                else
                {
                    return (double)TotalThinkTime / (double)ThinkCount;
                }
            }
        }

        protected internal void Think(Random random)
        {
            // Do things here.
            int rand = random.Next(0, 250);
            if (rand == 1)
            {
                Sprite shockwave = new ShockWaveSprite(5, 20, 10, Color.DarkBlue, Color.LightBlue);
                shockwave.Position = Position;
                shockwave.Mover = new SlaveMover(this);
                Engine.AddSprite(shockwave);
            }
        }

        public Critter(int startX, int startY, int scale) : base((new CritterBody()).GetBody(scale))
        {
            LineWidth = 1;
            Color = Sprite.RandomColor(127);
            Position = new Point(startX, startY);
            FacingAngle = 90;

            Processors += sprite =>
            {
                TargetMover spriteMover = (TargetMover)Mover;
                if (spriteMover == null || (spriteMover.SpeedX == 0 && spriteMover.SpeedY == 0))
                {
                    return;
                }
                double theta = Sprite.RadToDeg((float)Math.Atan2(spriteMover.SpeedY, spriteMover.SpeedX));
                spriteMover.TargetFacingAngle = (int)theta + 90;
            };
        }

        public void SmokeAndStop(Color startColor, Color endColor)
        {
            Mover = null;
            Shutdown();
            ParticleFountainSprite smoke = new ParticleFountainSprite(20, Color.Black, Color.Brown, 1, 10, 10);
            smoke.Position = Position;
            Engine.AddSprite(smoke);
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

        public void Startup()
        {
            if (thinkThread != null)
            {
                return;
            }

            TargetMover spriteMover = new TargetMover();
            spriteMover.SpriteReachedTarget += (sender, spriteEvent) => AssignRandomDestination();
            spriteMover.SpriteMoved += (sender, spriteEvent) =>
            {
                if (moveCount-- == 0)
                {
                    IncrementFrame();
                    moveCount = 5 - Math.Min(5, (int)spriteMover.Speed);
                }
            };
            Mover = spriteMover;

            thinkThread = new Thread(() =>
            {
                stopped = false;
                Stopwatch stopwatch = new Stopwatch();
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                while (!Surface.IsDisposed && !Surface.Disposing && !Dead && !stopped)
                {
                    if (Surface.Active)
                    {
                        try
                        {
                            stopwatch.Reset();
                            stopwatch.Start();
                            Think(rnd);
                            stopwatch.Stop();
                            long elapsed = stopwatch.ElapsedMilliseconds;
                            if (elapsed > 1000)
                            {
                                if (thinkTimeOverrunViolations >= maxThinkTimeOverrunViolations)
                                {
                                    Console.WriteLine("You were warned " + thinkTimeOverrunViolations + " times about thinking for too long. Now you may not think again.");
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Warning #" + (++thinkTimeOverrunViolations) + " you have exceeded the maximum think time of " + maxThinkTimeMilliseconds + " by " + (elapsed - maxThinkTimeMilliseconds) + " milliseconds.");
                                }
                            }
                            totalThinkTime += elapsed;
                            thinkCount++;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Critter halted due to exception whilst thinking: " + e);
                            SmokeAndStop(Color.Aquamarine, Color.Blue);
                            break;
                        }
                    }
                    Thread.Sleep(5);
                }
                thinkThread = null;
            });
            thinkThread.Start();

            Surface.Disposed += (e, evt) => thinkThread?.Abort();
            Died += (e, evt) => thinkThread?.Abort();

            AssignRandomDestination();
        }

        public void Shutdown()
        {
            stopped = true;
        }
    }
}
