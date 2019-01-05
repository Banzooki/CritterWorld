#region copyright
/*
* Copyright (c) 2008, Dion Kurczek
* Modifications copyright (c) 2018, Dave Voorhis
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither the name of the <organization> nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY DION KURCZEK ``AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL DION KURCZEK BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace SCG.TurboSprite
{
    public class ParticleExplosionSprite : Sprite
    {
        private List<Particle> _particles = new List<Particle>();
        private int _lifeSpan;

        private static Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public ParticleExplosionSprite(int particles, Color startColor, Color endColor, int startDiam, int endDiam, int lifeSpan)
        {
            _lifeSpan = lifeSpan;
            while (particles > 0)
            {
                Particle p = new Particle
                {
                    Color = Sprite.RandomColorFromRange(startColor, endColor),
                    DirectionX = rnd.NextDouble() * 4 - 2,
                    DirectionY = rnd.NextDouble() * 4 - 2,
                    Diameter = rnd.Next(endDiam - startDiam) + startDiam
                };
                _particles.Add(p);
                particles--;
            }
            Shape = new RectangleF(1, 1, 1, 1);
            Processors += sprite =>
            {
                _lifeSpan--;
                if (_lifeSpan <= 0)
                {
                    Kill();
                }
            };
        }

        // Render the sprite
        protected internal override void Render(Graphics graphics)
        {
            foreach (Particle particle in _particles)
            {
                int x = (int)(X - Surface.OffsetX + particle.X - particle.Diameter / 2);
                int y = (int)(Y - Surface.OffsetY + particle.Y - particle.Diameter / 2);
                particle.X += particle.DirectionX;
                particle.Y += particle.DirectionY;
                using (Brush brush = new SolidBrush(particle.Color))
                {
                    graphics.FillEllipse(brush, x, y, particle.Diameter * 2, particle.Diameter * 2);
                }
            }
        }
    }
}
