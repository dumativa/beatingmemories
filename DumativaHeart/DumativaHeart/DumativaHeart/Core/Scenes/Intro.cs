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
using DumativaHeart.Core.SceneManagement;
using DumativaHeart.Objects;



namespace DumativaHeart.Core.Scenes
{
    class Intro : SceneBase
    {
        Video video;
        VideoPlayer videoPlayer;
        Objeto2D layer;

        public override void start()
        {
            video = Game1.Self.Content.Load<Video>("VideoIntro");
            videoPlayer = new VideoPlayer();
            videoPlayer.Play(video);
            layer = new Objeto2D(videoPlayer.GetTexture());
            layer.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            layer.position.Y = Game1.Self.Window.ClientBounds.Height / 2;

            if (SoundManager.isEffectsMuted)
                videoPlayer.Volume = 0;
        }

        public override void update(GameTime gameTime)
        {
            layer.textura = videoPlayer.GetTexture();
            if (videoPlayer.State == MediaState.Stopped) 
            {
                layer.alpha = MathHelper.Lerp(layer.alpha, 0, 0.03f);
                if (layer.alpha < 0.01f)
                {
                    SceneManager.setScene(new Menu(), false);
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            layer.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void terminate()
        {
            
        }
    }
}
