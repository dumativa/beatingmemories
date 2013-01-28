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
    class GameOver : SceneManagement.SceneBase
    {
        Objects.Objeto2D fundoGameOver, tracinho;
        SpriteFont font;
        bool underlineVisible;

        int contadorMenu, contadorDrawUnderline;
        Vector2 posicao;
        public override void start()
        {
            SoundManager.SetMusic("GameOverSong", true, false, false);
            posicao = new Vector2(285, 590);
            fundoGameOver = new Objects.Objeto2D(Game1.Self.Content.Load<Texture2D>("fundoGameOver"));
            fundoGameOver.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            fundoGameOver.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            tracinho = new Objeto2D(Game1.Self.Content.Load<Texture2D>("tracinho"));
            tracinho.position = posicao;
            contadorDrawUnderline = 0;
            contadorMenu = 1;

            font = Game1.Self.Content.Load<SpriteFont>("OCR");
        }

        public override void update(GameTime gameTime)
        {
            if (Controller.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                if (contadorMenu > 1)
                {
                    contadorMenu--;
                }
            }

            if (Controller.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if (contadorMenu < 2)
                {
                    contadorMenu++;
                }
            }

            if (Controller.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                if (contadorMenu == 1)
                {
                    SceneManagement.SceneManager.setScene(new Scenes.Level(), true);
                }

                if (contadorMenu == 2)
                {
                    SceneManagement.SceneManager.setScene(new Scenes.Menu(), true);
                }
            }

            switch (contadorMenu)
            {
                case 1:
                    tracinho.position.X = 262;
                    tracinho.position.Y = 585;
                    break;

                case 2:
                    tracinho.position.X = 322;
                    tracinho.position.Y = 618;
                    break;

            }

            if (contadorDrawUnderline >= 20)
            {
                contadorDrawUnderline = 0;
            }

            else if (contadorDrawUnderline >= 10 && contadorDrawUnderline < 20)
            {
                underlineVisible = false;
            }

            else if (contadorDrawUnderline < 10 && contadorDrawUnderline >= 0)
            {
                underlineVisible = true;
            }
            contadorDrawUnderline++;

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            fundoGameOver.Draw(spriteBatch);

                if (underlineVisible == true)
                {
                    tracinho.Draw(spriteBatch);
                }

            spriteBatch.End();
        }

        public override void terminate()
        {

        }
    }
}
