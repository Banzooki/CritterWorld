﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Forms;
using SCG.TurboSprite;

namespace CritterWorld
{
    public partial class Arena : Form
    {
        Random rnd = Sprite.RND;

        SpriteEngine spriteEngineDebug;

        private void surface_SpriteCollision(object sender, SpriteCollisionEventArgs e)
        {
            Critter critter1 = (Critter)e.Sprite1.Data;
            Critter critter2 = (Critter)e.Sprite2.Data;

            critter1.AssignRandomDestination(spriteEngineDebug);
            critter2.AssignRandomDestination(spriteEngineDebug);
        }

        private void surface_SpriteReachedDestination(object sender, SpriteEventArgs e)
        {
            Critter critter = (Critter)e.Sprite.Data;
            critter.AssignRandomDestination(spriteEngineDebug);
        }

        public Arena()
        {
            float critterCount = 30;

            InitializeComponent();

            spriteEngineDebug = new SpriteEngine(components);
            spriteEngineDebug.Surface = spriteSurfaceMain;
            spriteEngineDebug.DetectCollisionSelf = false;
            spriteEngineDebug.DetectCollisionTag = 50;

            spriteSurfaceMain.SpriteCollision += new System.EventHandler<SCG.TurboSprite.SpriteCollisionEventArgs>(this.surface_SpriteCollision);
            spriteEngineMain.SpriteReachedDestination += new System.EventHandler<SCG.TurboSprite.SpriteEventArgs>(this.surface_SpriteReachedDestination);

            CritterBody body = new CritterBody();
            PointF[][] frames = new PointF[2][];
            frames[0] = Critter.Scale(body.GetBody1(), 3);
            frames[1] = Critter.Scale(body.GetBody2(), 3);

            int startX = 30;
            int startY = 30;

            for (int i = 0; i < critterCount; i++)
            {
                Critter critter = new Critter(spriteEngineMain);
                critter.GetSprite().Color = Sprite.RandomColor(64);
                critter.GetSprite().LineWidth = 2;

                critter.GetSprite().Position = new Point(startX, startY);

                critter.AssignRandomDestination(spriteEngineDebug);

                startY += 30;
                if (startY >= spriteSurfaceMain.Height - 30) 
                {
                    startY = 30;
                    startX += 100;
                }
            }

            spriteSurfaceMain.Active = true;
            spriteSurfaceMain.WraparoundEdges = true;
        }
    }
}
