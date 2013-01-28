using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DumativaHeart.Core.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DumativaHeart.Objects;
using Microsoft.Xna.Framework.Media;
using DumativaHeart.Core.CenaManagement;

namespace DumativaHeart.Core.Scenes
{
    class Level : SceneBase
    {
        
        public static Level Self;
        public static bool firstPlay;
        public Heart heart;
        public CenaBase cena;
        
        public override void start()
        {
            SoundManager.StopMusic();

            SoundManager.FadeInVelocity = 1f;
            SoundManager.SetMusic("GameplaySong1", true, false, false);
            
            cena = new CenaManagement.CenaBase();
            Self = this;
            
            heart = new Heart();

            CenaManagement.CenaManager.setScene(cena, 0, true);
        }

        public override void update(GameTime gameTime)
        {
            //
            cena.Update(gameTime);
        }
        //Forti tem pinto pequeno
        public override void draw(SpriteBatch spriteBatch)
        {

            cena.Draw(spriteBatch);
            //

        }

        public override void terminate()
        {
            cena.line.lineList.Clear();
        }
    }
}
