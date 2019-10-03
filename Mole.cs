using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Whack_a_mole
{
    class Mole
    {
        Texture2D tex;
        Texture2D knockoutTex;
        Texture2D currentTex;
        Vector2 pos;
        public Vector2 ogPos;
        public Vector2 highestPoint;
        int yVelocity;
        const int popDistance = 150;
        double timeSpentHigh;
        const double timeAllowedHigh = 1;
        public Rectangle hitBox;
        bool isActive = false;

        public Mole(Texture2D tex, Vector2 pos, int yVelocity, Texture2D knockoutTex)
        {
            this.tex = tex;
            this.pos = pos;
            this.yVelocity = yVelocity;
            this.knockoutTex = knockoutTex;

            currentTex = tex;
            ogPos = pos;
            
            highestPoint = new Vector2((ogPos.X - popDistance), (ogPos.Y - popDistance));

            hitBox = new Rectangle((int)pos.X + tex.Width / 2, (int)pos.Y + tex.Height / 6, 100, 150);
        }

        public void Update(GameTime gameTime) // kan skicka med en speed och gameTime i update
        {
                        
            pos.Y -= yVelocity;
            hitBox.Y -= yVelocity;
            
            if (pos.Y <= highestPoint.Y)
            {
                yVelocity = 0;
                timeSpentHigh += gameTime.ElapsedGameTime.TotalSeconds;
                if (timeSpentHigh >= timeAllowedHigh)
                {
                    yVelocity = -5;
                    timeSpentHigh = 0;
                    isActive = false;
                    // lives--;
                }
            }
            if (pos.Y >= ogPos.Y && isActive == false)
            {
                yVelocity = 0;
                pos = ogPos;
                SetTexture(tex);
                
            }
           /* if (pos.Y > ogPos.Y - popDistance && isActive == false)
            {
                pos.Y += 10;
                hitBox.Y += 10;
            } */
            
            
        }

        public void ResetPosition()
        {
            pos = ogPos;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Activate(int velocity)
        {
            yVelocity = velocity;
            isActive = true;
        }

        public void Deactivate()
        {
            
            isActive = false;
        }

        public void SetTexture(Texture2D tex)
        {
            currentTex = tex;
        }

        public void SetVelocity(int v)
        {
            yVelocity = v;
        }

        public Vector2 GetPos()
        {
            return pos;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(currentTex, pos, Color.White);
        }
    }
}
