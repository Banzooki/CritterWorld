#region copyright
/*
* Copyright (c) 2008, Dion Kurczek
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
    //Sprite class - defines behavior of all TurboSprite sprite objects
    public abstract class Sprite
    {       
        //Public Properties

        //Static constructor populates the sin/cos lookup tables
        static Sprite()
        {
            for (int degree = 0; degree < 360; degree++)
            {
                _sin[degree] = (float)Math.Sin(DegToRad(degree));
                _cos[degree] = (float)Math.Cos(DegToRad(degree));
            }
        }

        //A random number generator anyone can use
        public static Random RND = new Random();

        //Utility function to quickly convert degrees to Radians
        public static float DegToRad(int degree)
        {
            return (float)((Math.PI / 180) * degree);
        }

        //Static properties return the Sin/Cos for specified degree values
        public static float Sin(int degree)
        {
            return _sin[degree];
        }
        public static float Cos(int degree)
        {
            return _cos[degree];
        }

        //Obtain a random color within start to end range
        public static Color ColorFromRange(Color startColor, Color endColor)
        {
            byte a = rndByte(startColor.A, endColor.A);
            byte r = rndByte(startColor.R, endColor.R);
            byte g = rndByte(startColor.G, endColor.G);
            byte b = rndByte(startColor.B, endColor.B);
            return Color.FromArgb(a, r, g, b);
        }

        //The "Shape" of the sprite represents its Width and Height as relative to its center
        public RectangleF Shape
        {
            get
            {
                return _shape;
            }
            set
            {
                _shape = value;
                ClickShape = value;
            }
        }

        //Clickshape determines the size of the sprite for purposes of registering a mouse click
        public RectangleF ClickShape
        {
            get
            {
                return _clickShape;
            }
            set
            {
                _clickShape = value;
            }
        }

        //Sprite's bounding rectangle, calculated based on size and position
        public RectangleF Bounds
        {
            get
            {
                _bounds.X = X + _shape.Left;
                _bounds.Width = _shape.Width;
                _bounds.Y = Y + _shape.Top;
                _bounds.Height = _shape.Height;
                return _bounds;
            }
        }

        //Bounding rectangle of clickable region
        public RectangleF ClickBounds
        {
            get
            {
                _clickBounds.X = X + _clickShape.Left - Surface.OffsetX;
                _clickBounds.Width = _clickShape.Width;
                _clickBounds.Y = Y + _clickShape.Top - Surface.OffsetY;
                _clickBounds.Height = _clickShape.Height;
                return _clickBounds;
            }
        }

        //Helper property, returns integer Width and Height
        public int Width
        {
            get
            {
                return (int)Shape.Width;
            }
        }
        public int Height
        {
            get
            {
                return (int)Shape.Height;
            }
        }

        //Helper properties, returns half of the Width and Height
        public int WidthHalf
        {
            get
            {
                return Width / 2;
            }
        }
        public int HeightHalf
        {
            get
            {
                return Height / 2;
            }
        }
        
        //Angle sprite is facing - values between 0 and 360 allowed - auto-conversion occurs
        public int FacingAngle
        {
            get
            {
                return _facingAngle;
            }
            set
            {
                _facingAngle = value;
                while (_facingAngle >= 360)
                    _facingAngle -= 360;
                while (_facingAngle < 0)
                    _facingAngle += 360;
            }
        }

        //The Sprite's associated SpriteEngine
        public SpriteEngine Engine
        {
            get
            {
                return _engine;
            }
        }

        //Expose the surface
        public SpriteSurface Surface
        {
            get
            {
                return _surface;
            }
        }

        //Is the sprite dead?
        public bool Dead
        {
            get
            {
                return _dead;
            }
        }

        //Sprite's position - integer and float types supported
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }
        public PointF PositionF
        {
            get
            {
                return new PointF(_x, _y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }
        public Point Position
        {
            get
            {
                return new Point((int)_x, (int)_y);
            }
            set
            {
                _x = value.X;
                _y = value.Y;
            }
        }

        //The sprite's spin (if any)
        public SpinType Spin
        {
            get
            {
                return _spin;
            }
            set
            {
                _spin = value;
            }
        }
        public int SpinSpeed
        {
            get
            {
                return _spinSpeed;
            }
            set
            {
                _spinSpeed = value;
            }
        }

        //Kill a sprite - it will be removed after next processing cycle
        public void Kill()
        {
            _dead = true;
        }

        //Lookup table to degree to radian conversion
        private static float[] _sin = new float[360];
        private static float[] _cos = new float[360];

        //Private Members        
        private int _facingAngle;
        private float _x;
        private float _y;
        private bool _dead;
        private RectangleF _shape = new RectangleF(-1, -1, -1, -1);
        private RectangleF _clickShape = new RectangleF(-1, -1, -1, -1);
        private RectangleF _bounds = new RectangleF();
        private RectangleF _clickBounds = new RectangleF();
        private SpinType _spin;
        private int _spinSpeed;

        //Internal "MoveData" object used by SpriteEngines to store movement info
        internal Object MovementData;

        //Internal Members
        internal SpriteEngine _engine;
        internal SpriteSurface _surface;  
       
        //Process the internal logic a sprite may require during each animation cycle
        internal void PreProcess()
        {
            switch (_spin)
            {
                case SpinType.Clockwise:
                    FacingAngle += SpinSpeed;
                    break;
                case SpinType.CounterClockwise:
                    FacingAngle -= SpinSpeed;
                    break;
            }
        }

        //Render the sprite on the SpriteSurface
        protected internal abstract void Render(Graphics g);

        //Perform any additional processing if required
        protected internal virtual void Process()
        {
        }

        //Get a random color byte value
        private static byte rndByte(byte b1, byte b2)
        {
            if (b1 > b2)
            {
                byte temp = b1;
                b1 = b2;
                b2 = temp;
            }
            byte diff = (byte)(b2 - b1);
            return (byte)(RND.Next(diff) + b1);
        }
    }

    //Direction of sprite's spin
    public enum SpinType { None, Clockwise, CounterClockwise };
}
