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



namespace DumativaHeart.Core.SceneManagement
{

    
    
    public static class SceneManager 
    {
        //private static PauseScreen pauseScreen = new PauseScreen();
        public static SceneBase current;
        private static bool isGameLevel = false;
        public static bool isPaused = false;
        private static bool justPaused = false;

        public static void setScene(SceneBase scene, bool isGameLevel)
        {
            SceneManager.isGameLevel = isGameLevel;
          
            if (current != null)
                current.terminate();

            current = scene;
        

            if (current != null)
                current.start();
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
                //Se não está pausado, atualize o jogo e confira se o jogo foi pausado; Se não, atualize a tela de pause
                if (!isPaused)
                {
                    current.update(gameTime);
                    //isPaused = justPaused = Controller.KeyPressed(Keys.Escape) && isGameLevel;
                }
                //else
                //{
                //    pauseScreen.update(gameTime);
                //}

                return true;
            }

        }

        public static bool draw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            //Se o jogo acabou de ser pausado, prepare a tela de pause
            if (justPaused)
            {
                //pauseScreen.preparePause(spriteBatch, graphicsDevice);
                justPaused = false;
            }

            
            if (current == null)
            {
                return false;
            }
            else
            {
                //if (isPaused)
                //    pauseScreen.draw(spriteBatch);
                //else
                    current.draw(spriteBatch);

                return true;
            }
        }

        //Função usada pelo pauseScreen para tirar uma foto do jogo
        public static void justDraw(SpriteBatch spriteBatch)
        {
            current.draw(spriteBatch);
            

        }
    }
}
