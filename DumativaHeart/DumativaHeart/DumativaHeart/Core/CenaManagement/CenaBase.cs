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

namespace DumativaHeart.Core.CenaManagement
{
    class CenaBase
    {
        int indexCena, contadorTempo;

        Objects.Objeto2D fundoGame,avisoReading,avisoFollow;

        Textbox caixaTexto;
        SpriteFont fontDecisao;
        bool decisaoDesenhada;
        DumativaHeart.Objects.Heart heart;

        SpriteFont font;

        public Line line;

        RenderTarget2D backRender;

        public void start(int indexCena)
        {
            this.indexCena = indexCena;
            fundoGame = new Objects.Objeto2D(Game1.Self.Content.Load<Texture2D>("fundoGame"));
            fundoGame.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            fundoGame.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            heart = new DumativaHeart.Objects.Heart();

            avisoReading = new Objeto2D(Game1.Self.Content.Load<Texture2D>("reading"));
            avisoReading.position.X = 500;
            avisoReading.position.Y = 425;

            avisoFollow = new Objeto2D(Game1.Self.Content.Load<Texture2D>("follow_your_heart"));
            avisoFollow.position.X = 500;
            avisoFollow.position.Y = 425;

            contadorTempo = 600;
            Save.lerXml(indexCena);
            caixaTexto = new Textbox(800, new Vector2(100, 200), Save.textoAtual, 10, 20);
            fontDecisao = Game1.Self.Content.Load<SpriteFont>("fontDecisao");
            heart.autoPilot = true;

            font = Game1.Self.Content.Load<SpriteFont>("OCR");
            line = new Line();

            backRender = new RenderTarget2D(Game1.Self.GraphicsDevice, Game1.GameRactangle.Width, Game1.GameRactangle.Height);
        }

        public void Update(GameTime gameTime)
        {
            heart.Update(gameTime);

            if (Controller.KeyPressed(Keys.Enter) && heart.autoPilot == true)
            {
                heart.EndAutoPilot();
            }

            if (heart.autoPilot == false)
            {
                contadorTempo--;
                decisaoDesenhada = true;

                if (contadorTempo == 0)
                {
                    if (indexCena != 47 && indexCena != 48)
                    {
                        checkConsequence();
                        heart.StartAutoPilot(gameTime);
                        contadorTempo = 900;
                        decisaoDesenhada = false;
                    }

                    else
                    {
                        SceneManagement.SceneManager.setScene(new Scenes.Menu(), true);

                    }
                }
            }
            if ((heart.heartSpeed < 20) || (heart.heartSpeed > 150))
            CenaBase.matarHeroi("");

            line.Update(gameTime);
            caixaTexto.Update(gameTime);
        }

        public void checkConsequence()
        {
            if (indexCena != 47 && indexCena != 48)
            {
                if (heart.heartSpeed > 20 && heart.heartSpeed < 70)
                {
                    indexCena = Save.hardBeatConsequence;
                }

                else if (heart.heartSpeed > 70 && heart.heartSpeed < 100)
                {
                    indexCena = Save.mediumBeatConsequence;
                }

                else if (heart.heartSpeed > 100 && heart.heartSpeed < 150)
                {
                    indexCena = Save.weakBeatConsequence;
                }

                trocarTela(indexCena);
            }

            else
            {
                trocarTela(indexCena);
            }
        }

        private void trocarTela(int indexCena)
        {
            if (indexCena != 47 && indexCena != 48)
            {
                contadorTempo = 600;
                Save.lerXml(indexCena);

                caixaTexto = new Textbox(800, new Vector2(100, 200), Save.textoAtual, 10, 20);
                heart.autoPilot = true;
            }

            else
            {
                contadorTempo = 600;
                caixaTexto = new Textbox(800, new Vector2(100, 200), Save.textoAtual, 10, 20);
                //SceneManagement.SceneManager.setScene(new Scenes.Menu(), true);

            }
        }

        public void terminate()
        {

        }
          
        public static void matarHeroi(string cenaOrigem)
        {
            if (cenaOrigem == "tutorial")
            {
                SceneManagement.SceneManager.setScene(new Scenes.Tutorial(), true);
            }

            else
            {
                SceneManagement.SceneManager.setScene(new Scenes.GameOver(), true);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Game1.Self.GraphicsDevice.SetRenderTarget(backRender);
            Game1.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            fundoGame.Draw(spriteBatch);
            heart.Draw(spriteBatch);
            spriteBatch.End();

                if (decisaoDesenhada)
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                    spriteBatch.Draw(Game1.Self.Content.Load<Texture2D>("filtro vermelho"), Vector2.Zero, Color.White * 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin();
                    avisoFollow.Draw(spriteBatch);
                    spriteBatch.DrawString(fontDecisao, caixaTexto.WrapText(fontDecisao, Save.textoDecisao, 700), new Vector2(130, 330), new Color(131, 255, 207) * 0.7f);
                    spriteBatch.End();
                }

            else
            {
                spriteBatch.Begin();
                avisoReading.Draw(spriteBatch);
                spriteBatch.End();
            } 

            Game1.Self.GraphicsDevice.SetRenderTarget(null);
            Game1.Self.GraphicsDevice.Clear(Color.Black);

            line.Draw(spriteBatch, backRender);
            caixaTexto.Draw(spriteBatch);
        }

    }
}
