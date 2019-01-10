﻿#region copyright
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCG.TurboSprite
{
    // Animate a TextSprite
    public class TextTwitcher : IMover
    {
        private TextSprite _sprite;
        private Point originPosition;
        private int originSize;
        private long nextTwitchTime = 0;

        private Random rnd = new Random(Guid.NewGuid().GetHashCode());

        public int PositionTwitchRange { get; set; } = 10;

        public int SizeTwitchPercentage { get; set; } = 5;

        // Move the sprite, called by SpriteEngine's MoveSprite method
        public void MoveSprite(Sprite sprite)
        {
            if (_sprite == null)
            {
                _sprite = (TextSprite)sprite;
                originPosition = _sprite.Position;
                originSize = _sprite.Size;
            }
            long currentTimeInMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (currentTimeInMilliseconds > nextTwitchTime)
            {
                if (rnd.Next(10) == 1)
                {
                    int sizeTwitch = SizeTwitchPercentage * originSize / 100;
                    _sprite.Size = originSize + rnd.Next(-sizeTwitch, sizeTwitch);
                }
                else
                {
                    int x = originPosition.X + rnd.Next(-PositionTwitchRange, PositionTwitchRange);
                    int y = originPosition.Y + rnd.Next(-PositionTwitchRange, PositionTwitchRange);
                    _sprite.Position = new Point(x, y);
                }
                nextTwitchTime = rnd.Next(50) + currentTimeInMilliseconds;
            }
        }
    }
}
