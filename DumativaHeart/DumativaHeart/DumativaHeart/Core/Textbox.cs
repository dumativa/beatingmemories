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
using System.Text;

namespace DumativaHeart.Core
{
    class Textbox
    {
        Objects.Objeto2D escurecida;
        TimeSpan lastTime, textSpeed;
        int width, slen;
        public Vector2 position;
        string text, drawString;
		bool active;

        SpriteFont font;
        SoundEffect tapSound, endSound;

        public Textbox(int width, Vector2 position, string text, int textSpeed, int textDuration)
        {
            font = Game1.Self.Content.Load<SpriteFont>("OCR");
            this.lastTime = Game1.currentTime;
            this.width = width;
            this.position = new Vector2(130, 140);
            this.text = text;
            this.slen = 1;
            drawString = "";
            this.textSpeed = TimeSpan.FromMilliseconds(textSpeed);
            //this.textDuration = TimeSpan.FromMilliseconds(textDuration);
            escurecida = new Objects.Objeto2D(Game1.Self.Content.Load<Texture2D>("escurecida"));
            escurecida.position.X = Game1.Self.Window.ClientBounds.Width / 2;
            escurecida.position.Y = Game1.Self.Window.ClientBounds.Height / 2;
            tapSound = Game1.Self.Content.Load<SoundEffect>("Sfx/Tap1");
            endSound = Game1.Self.Content.Load<SoundEffect>("Sfx/Tap2");
            active = true;
        }

        public void Update(GameTime gameTime)
        {
            //Console.WriteLine("drawstring: " + drawString); 
            if (slen >= text.Length)
                return;

            if (gameTime.TotalGameTime - lastTime > textSpeed)
            {
                slen = slen++;

                if (gameTime.ElapsedGameTime > textSpeed)
                {
                    slen++;

                }
                drawString = text.Substring(0, slen);

                lastTime = Game1.currentTime;

                if (slen >= text.Length)
                {
                    endSound.Play();
                    return;
                }
                    tapSound.Play();
                
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (active != false)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.Additive);
                spriteBatch.DrawString(font, WrapText(font, drawString, width), position, new Color(131,255,207) * 0.12f);
                spriteBatch.DrawString(font, WrapText(font, drawString, width), position, new Color(131, 255, 207) * 0.5f);
                spriteBatch.End();

                spriteBatch.Begin();
                spriteBatch.Draw(escurecida.textura, Vector2.Zero, Color.White * 0.4f);
                spriteBatch.End();
            }

        }

        public string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            if (text.Length == 0)
                return "";

            string[] words = text.Split(' ');

            StringBuilder sb = new StringBuilder();

            float lineWidth = 0f;

            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }
    }
}
