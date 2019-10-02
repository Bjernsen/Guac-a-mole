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
        int yVelocity;
        Rectangle hitBox;
        bool isActive = false;

        public Mole(Texture2D tex, Vector2 pos, int yVelocity, Texture2D knockoutTex)
        {
            this.tex = tex;
            this.pos = pos;
            this.yVelocity = yVelocity;
            this.knockoutTex = knockoutTex;

            currentTex = tex;

            hitBox = new Rectangle((int)pos.X, (int)pos.Y, 100, 100);
        }

        public void Update() // kan skicka med en speed och gameTime i update
        {
            pos.Y = pos.Y + yVelocity;
           
            hitBox.Y += yVelocity;

        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Activate()
        {
            
            isActive = true;
        }

        public void Deactivate()
        {

            
            isActive = false;
        }

        public void ChangeTexture()
        {
            if (currentTex == knockoutTex)
            {
                currentTex = tex;
            }
            else if (currentTex == tex)
            {
                currentTex = knockoutTex;
            }
            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(currentTex, pos, Color.White);
        }
    }
}
