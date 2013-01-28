using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DumativaHeart.Core.CenaManagement
{
    class CenaManager
    {
        private static CenaBase current;
        private static bool isCenaAtual;
        
        public static void setScene(CenaBase cena, int indexCena, bool isCenaAtual)
        {
            CenaManager.isCenaAtual = isCenaAtual;

            if (current != null)
                current.terminate();

            current = cena;

            if (current != null)
                current.start(indexCena);
        }

        public static bool update(GameTime gameTime)
        {

            //Se a cena não existir, pare.
            if (current == null)
            {
                return false;
            }
            //Se a cena existir
            else
            {

                return true;
            }

        }


        public static bool draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {

            if (current == null)
            {
                return false;
            }
            else
            {
                current.Draw(spriteBatch);

                return true;
            }
        }
    }
}
