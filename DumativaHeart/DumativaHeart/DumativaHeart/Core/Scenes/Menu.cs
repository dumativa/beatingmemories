using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DumativaHeart.Core.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DumativaHeart.Objects;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace DumativaHeart.Core.Scenes
{
    class Menu:SceneBase
    {
        Objeto2D fundo,tracinho,escurecida;
        SpriteFont font;
        bool underlineVisible;
        int contadorMenu, contadorDrawUnderline;
        Vector2 posicao;

        public override void start()
        {
            
            SoundManager.SetMusic("title_song", true, false, false);

            posicao = new Vector2(285, 575);
            fundo = new Objeto2D(Game1.Self.Content.Load<Texture2D>("fundoMenu"));
            fundo.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            fundo.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            escurecida = new Objeto2D(Game1.Self.Content.Load<Texture2D>("escurecida"));
            escurecida.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            escurecida.position.Y = Game1.Self.Window.ClientBounds.Height / 2;


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
                    contadorMenu --;
                    SoundManager.PlaySoundEffect("button0"+ contadorMenu);               
                }
            }

            if (Controller.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                if (contadorMenu < 3)
                {         
                    contadorMenu++;
                    SoundManager.PlaySoundEffect("button0" + contadorMenu);      
                }
            }

            if (Controller.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                if (contadorMenu == 1)
                {
                    SceneManager.setScene(new Scenes.Tutorial(), true);
                    
                }

                if (contadorMenu == 2)
                {
                    SceneManager.setScene(new Scenes.Controls(), true);

                }

                if (contadorMenu == 3)
                {
                    Game1.Self.Exit();
                }
            }

            switch (contadorMenu)
            {
                case 1:
                    tracinho.position.X = 285;
                    tracinho.position.Y = 575;
                    break;

                case 2:
                    tracinho.position.X = 255;
                    tracinho.position.Y = 599;
                    break;

                case 3:
                    tracinho.position.X = 195;
                    tracinho.position.Y = 623;
                    break;
            }

            if (contadorDrawUnderline >= 20)
            {
                contadorDrawUnderline = 0;
            }
            
            else if (contadorDrawUnderline >= 10 && contadorDrawUnderline <20)
            {
                underlineVisible = false;
            }

            else if (contadorDrawUnderline <10 && contadorDrawUnderline >=0)
            {
                underlineVisible = true;
            }
            contadorDrawUnderline++;

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            fundo.Draw(spriteBatch); 
            
            if (underlineVisible == true)
            {
                tracinho.Draw(spriteBatch);
            }


            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.Draw(escurecida.textura, Vector2.Zero, Color.White*0.4f);
            spriteBatch.End();

        }

        public override void terminate()
        {

        }
    }
}
