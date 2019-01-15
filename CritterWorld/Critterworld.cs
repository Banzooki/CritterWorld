﻿using SCG.TurboSprite;
using SCG.TurboSprite.SpriteMover;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace CritterWorld
{
    public partial class Critterworld : Form
    {
        // Level duration in seconds.
        const int levelDuration = 60 * 3; 

        private int tickCount = 0;
        private Level level;
        private Competition competition;

        private System.Timers.Timer fpsDisplayTimer;

        private Size oldSize;
        private Point oldLocation;
        private FormWindowState oldState;
        private FormBorderStyle oldStyle;

        private bool isFullScreen = false;

        private bool Fullscreen
        {
            get
            {
                return isFullScreen;
            }
            set
            {
                if (value)
                {
                    oldSize = Size;
                    oldState = WindowState;
                    oldStyle = FormBorderStyle;
                    oldLocation = Location;
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.None;
                    Bounds = Screen.PrimaryScreen.Bounds;
                    isFullScreen = true;
                }
                else
                {
                    Location = oldLocation;
                    WindowState = oldState;
                    FormBorderStyle = oldStyle;
                    Size = oldSize;
                    isFullScreen = false;
                }
            }
        }

        private static string tickLine = ".....";

        private String TickShow()
        {
            tickCount = (tickCount + 1) % tickLine.Length;
            return " " + tickLine.Substring(tickCount) + " " + tickLine.Substring(tickLine.Length - tickCount) + ".";
        }

        private void Shutdown()
        {
            LevelTimerStop();
            arena.Shutdown();
            if (level != null)
            {
                level.Shutdown();
                level = null;
            }
            if (competition != null)
            {
                competition.Shutdown();
                competition = null;
            }
        }

        private void StartOneLevel()
        {
            Shutdown();
            LevelTimerStart();
            level = new Level(arena, (Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png"), new Point(457, 440));
            level.Launch();
        }

        private void NextLevel()
        {
            if (competition != null)
            {
                LevelTimerStart();
                competition.NextLevel();
            }
            else if (level != null)
            {
                StartOneLevel();
            }
        }

        private void ExitApplication()
        {
            LevelTimerStop();
            fpsDisplayTimer.Stop();
            Shutdown();
            Thread.Sleep(500);
            Application.Exit();
        }

        private void MenuStart_Click(object sender, EventArgs e)
        {
            StartOneLevel();
        }

        private void MenuCompetionStart_Click(object sender, EventArgs e)
        {
            Shutdown();
            LevelTimerStart();
            competition = new Competition(arena);
            competition.Finished += (sndr, ev) => DisplayGameOver();
            competition.FinishedLevel += (sndr, ev) => LevelTimerStart();
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background00.png"), new Point(345, 186)));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background01.png"), new Point(319, 247)));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background02.png"), new Point(532, 32)));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background03.png"), new Point(504, 269)));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background04.png"), new Point(183, 279)));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background05.png"), new Point(457, 440)));
            competition.Add(new Level((Bitmap)Image.FromFile("Resources/TerrainMasks/Background06.png"), new Point(280, 360)));
            competition.Launch();
        }

        private void MenuFullScreen_Click(object sender, EventArgs e)
        {
            Fullscreen = !Fullscreen;
            menuFullScreen.Checked = Fullscreen;
        }

        private void MenuNextLevel_Click(object sender, EventArgs e)
        {
            NextLevel();
        }

        private void MenuStop_Click(object sender, EventArgs e)
        {
            DisplayGameOver();
        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void DisplayGameOver()
        {
            Shutdown();
            LevelTimerStop();
            TextSprite splashText = new TextSprite("GAME OVER", "Arial", 1, FontStyle.Regular);
            splashText.Mover = new TextGrower(1, 100, 3);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2);
            System.Timers.Timer gameOverTimer = new System.Timers.Timer();
            gameOverTimer.AutoReset = false;
            gameOverTimer.Interval = 5000;
            gameOverTimer.Elapsed += (sender, e) => DisplaySplash();
            gameOverTimer.Start();
            arena.Launch();
        }

        private void DisplayCritterworldText()
        {
            Sprite splashText = new TextSprite("CritterWorld", "Arial", 150, FontStyle.Regular);
            arena.AddSprite(splashText);
            splashText.Position = new Point(arena.Width / 2, arena.Height / 2 - 100);
            TextTwitcher splashTextTwitcher = new TextTwitcher
            {
                PositionTwitchRange = 2,
                SizeTwitchPercentage = 0
            };
            splashText.Mover = splashTextTwitcher;
        }

        private void DisplayVersion()
        {
            TextSprite splashTextVersion = new TextSprite("2", "Arial", 250, FontStyle.Bold);
            arena.AddSprite(splashTextVersion);
            splashTextVersion.Position = new Point(arena.Width / 2, arena.Height / 2 + 150);
            splashTextVersion.Color = Color.Green;
            TextTwitcher splashTextVersionTwitcher = new TextTwitcher
            {
                PositionTwitchRange = 3,
                SizeTwitchPercentage = 50
            };
            splashTextVersion.Mover = splashTextVersionTwitcher;
        }

        private void DisplayWanderingCritter()
        {
            PolygonSprite wanderer = new PolygonSprite((new CritterBody()).GetBody());
            wanderer.Color = Sprite.RandomColor(127);
            wanderer.Processors += sprite =>
            {
                TargetMover spriteMover = (TargetMover)sprite.Mover;
                if (spriteMover == null || (spriteMover.SpeedX == 0 && spriteMover.SpeedY == 0))
                {
                    return;
                }
                double theta = Sprite.RadToDeg((float)Math.Atan2(spriteMover.SpeedY, spriteMover.SpeedX));
                spriteMover.TargetFacingAngle = (int)theta + 90;
            };
            int margin = 20;
            int speed = 4;
            int moveCount = 0;
            Route route = new Route(wanderer);
            route.SpriteMoved += (sender, spriteEvent) =>
            {
                if (moveCount-- == 0)
                {
                    wanderer.IncrementFrame();
                    moveCount = 5 - Math.Min(5, speed);
                }
            };
            route.Add(margin, margin, speed);
            route.Add(arena.Width - margin, margin, speed);
            route.Add(arena.Width - margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height - margin, speed);
            route.Add(arena.Width - margin, arena.Height - margin, speed);
            route.Add(arena.Width - margin, arena.Height / 2 - margin, speed);
            route.Add(margin, arena.Height / 2 - margin, speed);
            route.Repeat = true;
            arena.AddSprite(wanderer);
            route.Start();
        }

        private void DisplaySplash()
        {
            Shutdown();
            LevelTimerStop();
            DisplayCritterworldText();
            DisplayVersion();
            DisplayWanderingCritter();
            arena.Launch();
        }

        private System.Timers.Timer levelTimer = null;
        private int countDown;

        private void Tick()
        {
            countDown--;
            if (countDown <= 0)
            {
                NextLevel();
            }
            else
            {
                Invoke(new Action(() => levelTimeoutProgress.Value = countDown * 100 / levelDuration));
            }
        }

        private void LevelTimerStart()
        {
            if (levelTimer == null)
            {
                levelTimer = new System.Timers.Timer();
                levelTimer.Interval = 1000;
                levelTimer.AutoReset = true;
                levelTimer.Elapsed += (sender, e) => Tick();
            }
            levelTimer.Stop();
            levelTimer.Start();
            countDown = levelDuration;
            Invoke(new Action(() => levelTimeoutProgress.Value = 100));
        }

        private void LevelTimerStop()
        {
            if (levelTimer != null)
            {
                levelTimer.Stop();
                Invoke(new Action(() => levelTimeoutProgress.Value = 0));
            }
        }
   
        // Use explicit layout to get around issues with HiDPI displays.
        private void ForceLayout()
        {
            levelTimeoutProgress.Width = (int)(Width * 0.75);

            int heightOfDisplayArea = ClientRectangle.Height - statusStrip.Height - menuStrip.Height;

            textLog.Bounds = new Rectangle(0, arena.Bottom, arena.Width, heightOfDisplayArea - arena.Height);

            int widthToRightOfArena = ClientRectangle.Width - arena.Width;
            panelScore.Bounds = new Rectangle(arena.Right, menuStrip.Height, widthToRightOfArena / 2, heightOfDisplayArea);

            labelLeaderboard.Location = new Point(panelScore.Right, menuStrip.Height);

            dataGridViewLeaderboard.Bounds = new Rectangle(panelScore.Right, labelLeaderboard.Bottom, widthToRightOfArena / 2, (heightOfDisplayArea - labelLeaderboard.Bounds.Height) / 2);

            labelWaiting.Location = new Point(panelScore.Right, dataGridViewLeaderboard.Bottom);

            dataGridViewWaiting.Bounds = new Rectangle(panelScore.Right, labelWaiting.Bottom, ClientRectangle.Width - arena.Width - panelScore.Width, heightOfDisplayArea - labelWaiting.Height - dataGridViewLeaderboard.Height - labelLeaderboard.Height);
        }

        // Use explicit layout to get around issues with HiDPI displays.
        private void ForceInitialLayout()
        {
            arena.Width = 1024;
            arena.Height = 768;

            Location = new Point(20, 20);
            Width = Screen.PrimaryScreen.Bounds.Width - 50;
            Height = Screen.PrimaryScreen.Bounds.Height - 50;

            ForceLayout();
        }

        private void Critterworld_Resize(object sender, EventArgs e)
        {
            ForceLayout();
        }

        public Critterworld()
        {
            InitializeComponent();

            ForceInitialLayout();

            menuFullScreen.Checked = Fullscreen;
            menuFullScreen.ImageScaling = ToolStripItemImageScaling.None;       // fix alignment problem on HiDPI displays

            FormClosing += (sender, e) => ExitApplication();

            labelVersion.Text = Version.VersionName;

            DisplaySplash();

            fpsDisplayTimer = new System.Timers.Timer();
            fpsDisplayTimer.Interval = 250;
            fpsDisplayTimer.Elapsed += (sender, e) => Invoke(new Action(() => labelFPS.Text = arena.ActualFPS + " FPS" + TickShow()));
            fpsDisplayTimer.Start();
        }
    }
}
