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
    class Tutorial :SceneManagement.SceneBase
    {
        CenaManagement.CenaBase cena;

        List<string> listaStrings;

        static public Tutorial Self;

        Objeto2D fundo,avisoReading, avisoFollow,filtro;
        Textbox caixaTexto;
        Heart heart;
        Line line;
        SpriteFont font,fontDecisao;

        bool coracaoDesenhado,textoLido;

        int contadorNextTexto, fraseAtual,contadorTentativa;

        RenderTarget2D backRender;

        public override void start()
        {
            Self = this;
            listaStrings = new List<string>();
            listaStrings.Add("Hey... are you there?");
            listaStrings.Add("It's time to wake up, can you do that?");
            listaStrings.Add("I see, you can't do anything right now...");
            listaStrings.Add("I will help you then. Press 'A' to control the first beat of your heart.");
            listaStrings.Add("Nice! But that is not good enough to wake you up. Press 'S' or 'L' to control the second beat of your heart.");
            listaStrings.Add("Awesome! Now is the time. Press 'A' Then Press 'S' or 'L' alternately, in a heartbeat rhythm. That should do it.");
            listaStrings.Add("Perfect! I think it's working, soon you will be awake for good. I have one last thing to teach you. It's the most important thing you have to do now before you awake.");
            listaStrings.Add("Something is wrong with your body. If the beat is too fast, you will make tense and stressed choices... Try to stress your heart now.. But not too much ! Otherwise you will have a heart attack and well... die.");
            listaStrings.Add("Awesome! In the other hand, if the beat is too slow, you will make controlled and sensible choices... Now, Try to calm your heart down. But not too much ! Otherwise you will have a cardiac arrest and also die.");
            listaStrings.Add("Perfect! It seems a little tough right now, but you will get used to it. When you see 'Reading' on the screen, you can take your time to read the text. Press 'Enter' when you are done to go to the next step.  When you see 'Follow your heart' on the monitor, a decision has to be made. You will have to control your heartbeat.");
            listaStrings.Add("Great! You remained calm even with all those news. This is a good Sign!");
            listaStrings.Add("Wow! You're already stressed. Calm down my friend! There are a lot of things yet to come. This is just the beginning.");
            listaStrings.Add("You're awaking now. I can't help you anymore because of reasons you will not understand. Good Luck and stay alive!");

            backRender = new RenderTarget2D(Game1.Self.GraphicsDevice, Game1.GameRactangle.Width, Game1.GameRactangle.Height);

            SoundManager.StopMusic();

            avisoReading = new Objeto2D(Game1.Self.Content.Load<Texture2D>("reading"));
            avisoReading.position.X = 500;
            avisoReading.position.Y = 425;

            avisoFollow = new Objeto2D(Game1.Self.Content.Load<Texture2D>("follow_your_heart"));
            avisoFollow.position.X = 500;
            avisoFollow.position.Y = 425;

            fundo = new Objects.Objeto2D(Game1.Self.Content.Load<Texture2D>("fundoGame"));
            fundo.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            fundo.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            heart = new DumativaHeart.Objects.Heart();
            line = new Line();

            contadorTentativa = 600;
            contadorNextTexto = 240;
            fraseAtual = 0;

            textoLido = false;

            filtro = new Objeto2D(Game1.Self.Content.Load<Texture2D>("filtro vermelho"));
            //filtro.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            //filtro.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            caixaTexto = new Textbox(800, new Vector2(100, 200), listaStrings[fraseAtual], 10, 20);
            font = Game1.Self.Content.Load<SpriteFont>("OCR");
            fontDecisao = Game1.Self.Content.Load<SpriteFont>("fontDecisao");

        }

        public override void update(GameTime gameTime)
        {
            if (Controller.KeyPressed(Keys.Space))
                SceneManagement.SceneManager.setScene(new Scenes.PreviewScreen(), true);

            line.Update(gameTime);
            if (coracaoDesenhado)
            {
                heart.Update(gameTime);
            }
// Controle da pausa
            if (fraseAtual != 3 && fraseAtual != 4 && fraseAtual != 5 && fraseAtual != 7 && fraseAtual != 8 && fraseAtual != 9)
            {
                contadorNextTexto--;

                if (contadorNextTexto == 0)
                {
                    contadorNextTexto = 300;


                    if (fraseAtual == 11 || fraseAtual == 10)
                    {
                        fraseAtual = 12;
                        caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[12], 10, 20);
                    }

                    else if (fraseAtual == 12)
                    {
                        SceneManagement.SceneManager.setScene(new Scenes.PreviewScreen(), true);
                    }

                    else
                    {
                        fraseAtual++;
                        caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                    }


                }
            }
// O que acontece com cada frase que foge da pausa
            else
            {
                if (fraseAtual == 3)
                {
                    if (coracaoDesenhado == false)
                    {
                        heart.lastTimePressed = Game1.currentTime;
                    }

                    coracaoDesenhado = true;

                    heart.decayFactor = 0;
                    

                    if (Controller.KeyPressed(Keys.A))
                    {
                        fraseAtual++;
                        caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                    }
                }

                else if (fraseAtual == 4)
                {
                    if (Controller.KeyPressed(Keys.S) || (Controller.KeyPressed(Keys.L)))
                    {
                        fraseAtual++;
                        caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                    }
                }

                else if (fraseAtual == 5)
                {
                    if (heart.lastPattern == Heart.HeartPattern.Regular)
                    {
                        fraseAtual++;
                        heart.lastPattern = Heart.HeartPattern.None;
                        caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                    }

                }

                else if (fraseAtual == 7)
                {
                    heart.lastTimePressed = Game1.currentTime;

                    if (contadorTentativa == 0)
                    {
                        if (heart.heartSpeed > 100 && heart.heartSpeed < 150)
                        {
                            contadorTentativa = 900;
                            heart.decayFactor = 1;
                            fraseAtual++;
                            caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                        }

                        else if (heart.heartSpeed > 150 || heart.heartSpeed < 20)
                        {
                            CenaManagement.CenaBase.matarHeroi("tutorial");
                        }

                        else
                        {
                            contadorTentativa = 900;
                            caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                        }


                    }

                    else
                    {
                        heart.decayFactor = 0;
                        contadorTentativa--;
                    }


                }

                else if (fraseAtual == 8)
                {
                    heart.lastTimePressed = Game1.currentTime;
                    if (contadorTentativa == 0)
                    {

                        if (heart.heartSpeed > 20 && heart.heartSpeed < 70)
                        {
                            contadorTentativa = 420;
                            heart.decayFactor = 0;
                            fraseAtual++;
                            caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);

                        }

                        else if (heart.heartSpeed > 150 || heart.heartSpeed < 20)
                        {
                            CenaManagement.CenaBase.matarHeroi("tutorial");
                        }

                        else
                        {
                            contadorTentativa = 900;
                            caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                        }


                    }

                    else
                    {
                        //heart.decayFactor = 1;
                        contadorTentativa--;
                    }


                }

                else if (fraseAtual == 9)
                {
                    heart.lastTimePressed = Game1.currentTime;

                    if ((!heart.autoPilot) && (heart.decayFactor == 0))
                        heart.StartAutoPilot(gameTime);

                    if (Controller.KeyPressed(Keys.Enter) && heart.decayFactor == 0)
                    {
                        heart.decayFactor = 1;
                        heart.EndAutoPilot();
                        textoLido = true;
                       
                    }

                    if (heart.decayFactor == 1)
                    {
                        if (contadorTentativa == 0)
                        {
                            if (heart.heartSpeed > 100 && heart.heartSpeed < 150)
                            {
                                contadorTentativa = 600;
                                heart.decayFactor = 0;
                                fraseAtual = 11;
                                caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                            }

                            if (heart.heartSpeed > 20 && heart.heartSpeed < 100)
                            {
                                contadorTentativa = 600;
                                heart.decayFactor = 0;
                                fraseAtual = 10;
                                caixaTexto = new Textbox(800, new Vector2(100, 100), listaStrings[fraseAtual], 10, 20);
                            }

                            else if (heart.heartSpeed > 150 || heart.heartSpeed < 20)
                            {
                                CenaManagement.CenaBase.matarHeroi("tutorial");
                            }

                        }

                        else
                        {
                            heart.decayFactor = 1;
                            contadorTentativa--;
                        }
                    }

                }
            }



            caixaTexto.Update(gameTime);

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Game1.Self.GraphicsDevice.SetRenderTarget(backRender);
            Game1.Self.GraphicsDevice.Clear(Color.Black);
            
            spriteBatch.Begin();

            fundo.Draw(spriteBatch);

            if (coracaoDesenhado)
                heart.Draw(spriteBatch);

            spriteBatch.DrawString(font, "Press Space to Skip", new Vector2(694, 666), new Color(131, 255, 207) * 0.5f);

            if (fraseAtual == 9)
            {
                if (heart.decayFactor == 1)
                {
                    spriteBatch.DrawString(fontDecisao,caixaTexto.WrapText(fontDecisao, "Do you understand that?",800),new Vector2(130, 320), new Color(131, 255, 207) * 0.7f);

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
                    spriteBatch.Draw(Game1.Self.Content.Load<Texture2D>("filtro vermelho"),Vector2.Zero,Color.White*0.5f);
                    spriteBatch.End();
                    spriteBatch.Begin();
                }
                
                if (textoLido == false)
                {
                    avisoReading.Draw(spriteBatch);
                }
                else
                {
                    avisoFollow.Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            

            Game1.Self.GraphicsDevice.SetRenderTarget(null);
            Game1.Self.GraphicsDevice.Clear(Color.Black);

            line.Draw(spriteBatch, backRender);
            caixaTexto.Draw(spriteBatch);



        }

        public override void terminate()
        {
            line.lineList.Clear();
        }
    }
}
