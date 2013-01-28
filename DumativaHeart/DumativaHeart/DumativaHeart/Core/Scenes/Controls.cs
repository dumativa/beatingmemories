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

namespace DumativaHeart.Core.Scenes
{
    class Controls : SceneManagement.SceneBase
    {
        Objects.Objeto2D fundoControl;
        Objects.Heart heart;
        Objects.Line line;
        RenderTarget2D backRender;

        public override void start()
        {
            backRender = new RenderTarget2D(Game1.Self.GraphicsDevice, Game1.GameRactangle.Width, Game1.GameRactangle.Height);
            fundoControl = new Objects.Objeto2D(Game1.Self.Content.Load<Texture2D>("fundoControl"));
            fundoControl.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            fundoControl.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            line = new Objects.Line();

            heart = new Objects.Heart();
            heart.decayFactor = 0.6f;
        }

        public override void update(GameTime gameTime)
        {
            line.Update(gameTime);
            if (Controller.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                SceneManagement.SceneManager.setScene(new Scenes.Menu(), true);
            }

            heart.Update(gameTime);

        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Game1.Self.GraphicsDevice.SetRenderTarget(backRender);
            Game1.Self.GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            fundoControl.Draw(spriteBatch);
            heart.Draw(spriteBatch);
            spriteBatch.End();

            Game1.Self.GraphicsDevice.SetRenderTarget(null);
            Game1.Self.GraphicsDevice.Clear(Color.Black);

            line.Draw(spriteBatch, backRender);

            
        }

        public override void terminate()
        {
            line.lineList.Clear();
        }
    }
}
