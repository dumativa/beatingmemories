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
using DumativaHeart.Objects;

namespace DumativaHeart.Core.Scenes
{
    class PreviewScreen : SceneManagement.SceneBase
    {
        Objeto2D fundoGameOver, tracinho;
        SpriteFont font;

        int contadorMenu, contadorDrawUnderline;
        Vector2 posicao;

        TimeSpan startTime;
        TimeSpan delay;

        public override void start()
        {
            delay = TimeSpan.FromSeconds(5d);

            //SoundManager.SetMusic("GameOverSong", true, false, false);
            
            fundoGameOver = new Objeto2D(Game1.Self.Content.Load<Texture2D>("fundoGame"));
            fundoGameOver.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            fundoGameOver.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            contadorDrawUnderline = 0;
            contadorMenu = 1;

            startTime = Game1.currentTime;

            font = Game1.Self.Content.Load<SpriteFont>("OCR");
        }

        public override void update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - startTime > delay)
            {
                SceneManagement.SceneManager.setScene(new Scenes.Level(), true);
            }

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            fundoGameOver.Draw(spriteBatch);
            
            spriteBatch.DrawString(font, "This is only a preview version of the game.", new Vector2(240, 360), new Color(131, 255, 207) * 0.5f);

            spriteBatch.End();
        }

        public override void terminate()
        {

        }
    }
}
