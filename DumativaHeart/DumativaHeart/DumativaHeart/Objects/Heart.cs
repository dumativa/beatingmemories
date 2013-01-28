using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using DumativaHeart.Core.Scenes;
using DumativaHeart.Core.SceneManagement;

namespace DumativaHeart.Objects
{
    class Heart
    {
        public static Heart Self;

        const float beatIncrease = 5f;

        public float heartSpeed = 80;
        public float decayFactor = 1f;

        float heartScale = 1f;
        float heartVolume
        {
            get
            {
                if (autoPilot)
                    return 0.33f;
                else
                    return 1f;
            }
        }

        public bool isDead = false;
        bool SystoleState = false;
        bool debuffState = false;
        bool autoFirst = false;

        public bool autoPilot = false;

        SoundEffect upBeat;
        SoundEffect downBeat;

        Texture2D heart;
        Texture2D splendidBar;

        Vector2 drawPosition;

        TimeSpan debuffDuration = TimeSpan.FromMilliseconds(1500);
        TimeSpan debuffStart = TimeSpan.Zero;
        TimeSpan timeDifference = TimeSpan.Zero;
        TimeSpan systoleStarted = TimeSpan.Zero;
        TimeSpan autoPilotCycle = TimeSpan.Zero;

        public TimeSpan lastTimePressed = TimeSpan.Zero;

        public enum HeartPattern { Regular, Dual_Systole, Only_Diastole, Only_Systole, None };

        public HeartPattern lastPattern;

        public Heart()
        {
            Self = this;
            lastTimePressed = Game1.currentTime;
            heart = Game1.Self.Content.Load<Texture2D>("Heart");
            splendidBar = Game1.Self.Content.Load<Texture2D>("heart_bar");
            upBeat = Game1.Self.Content.Load<SoundEffect>("upbeat");
            downBeat = Game1.Self.Content.Load<SoundEffect>("downbeat");
            drawPosition = new Vector2(500, 630);
        }

        public void Update(GameTime gameTime)
        {
            if (heartScale > 1f)
                heartScale -= 0.01f;
            if (heartScale < 1f)
                heartScale += 0.01f;

            if (autoPilot)
            {
                TimeSpan timeElapsed = (gameTime.TotalGameTime - autoPilotCycle);
                TimeSpan beatDuration = TimeSpan.FromSeconds(85 / (heartSpeed + 1));
                if ((timeElapsed.TotalMilliseconds > beatDuration.TotalMilliseconds * 0.75) && (!autoFirst))
                {
                    autoFirst = true;
                    heartScale += 0.1f;
                    upBeat.Play(heartVolume * 0.3f, 0f, 0f);
                }
                if (timeElapsed.TotalMilliseconds > beatDuration.TotalMilliseconds)
                {
                    autoFirst = false;
                    autoPilotCycle = gameTime.TotalGameTime;
                    heartScale -= 0.07f;
                    downBeat.Play(heartVolume, 0f, 0f);
                    SendPattern(HeartPattern.Regular, 500, gameTime);
                }
                return;
            }

            float timeFactor = (float)(10 * (gameTime.TotalGameTime.TotalSeconds - lastTimePressed.TotalSeconds));

            float decrease = (0.02f * (1 + timeFactor) + heartSpeed * 0.0028f) * decayFactor;
            heartSpeed -= decrease;

            if (heartSpeed < 20) heartSpeed = 19.9f;

            if (debuffState)
            {
                timeDifference = gameTime.TotalGameTime - debuffStart;
            }

            if ((heartSpeed < 20) || (heartSpeed > 150) || (isDead))
            {
                isDead = true;
                return;
            }

            if (!debuffState)
            {
                if (Core.Controller.KeyPressed(Keys.A) && !debuffState)
                    Systole(gameTime);

                if ((Core.Controller.KeyPressed(Keys.S)) || ((Core.Controller.KeyPressed(Keys.L))) && !debuffState)
                    Diastole(gameTime);

                if (Core.Controller.KeyPressed(Keys.Z))
                {
                    autoPilotCycle = gameTime.TotalGameTime;
                    autoPilot = true;
                }

            }
            else
            {
                if (timeDifference > debuffDuration)
                {
                    debuffStart = TimeSpan.Zero;
                    debuffState = false;
                    SystoleState = false;
                }
            }
            if (SystoleState)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - systoleStarted.TotalMilliseconds > 600)
                {
                    SystoleState = false;
                    debuffState = true;
                    debuffStart = gameTime.TotalGameTime;
                    SendPattern(HeartPattern.Only_Systole, (int)debuffDuration.TotalMilliseconds, gameTime);
                }
            }
        }

        public void Systole(GameTime gameTime)
        {
            if (!SystoleState)  //Normal Beat
            {
                systoleStarted = gameTime.TotalGameTime;
                SystoleState = true;
                heartSpeed += beatIncrease;
            }
            else    //Erratic Beat
            {
                SendPattern(HeartPattern.Dual_Systole, 600, gameTime);
                debuffState = true;
                debuffStart = gameTime.TotalGameTime;
                SystoleState = false;
            }
            heartScale += 0.1f;
            lastTimePressed = gameTime.TotalGameTime;
            upBeat.Play(heartVolume * 0.3f, 0f, 0f);
        }

        public void Diastole(GameTime gameTime)
        {
            if (SystoleState) //Normal Beat
            {
                SystoleState = false;
                heartSpeed += beatIncrease;

                SendPattern(HeartPattern.Regular, (int)(gameTime.TotalGameTime.TotalMilliseconds - systoleStarted.TotalMilliseconds), gameTime);

            }
            else    //Erratic Beat
            {
                SendPattern(HeartPattern.Only_Diastole, (int)debuffDuration.TotalMilliseconds/3, gameTime);
                SystoleState = false;
                debuffState = true;
                debuffStart = gameTime.TotalGameTime;
            }
            lastTimePressed = gameTime.TotalGameTime;
            downBeat.Play(heartVolume, 0f, 0f);
            heartScale -= 0.1f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(splendidBar, new Vector2(-5, 600), Color.White);

            drawPosition = Vector2.Lerp(drawPosition, new Vector2(116 + 788 * (heartSpeed-20) / 130, 630), 0.1f);

            Color drawColor = debuffState ? new Color(0.4f, 0.9f, 1f, 1f) : Color.White;
            drawColor = isDead ? new Color(0.1f, 0.1f, 1f, 1f) : drawColor;

            spriteBatch.Draw(heart, new Rectangle((int)drawPosition.X,
                                                  (int)drawPosition.Y,
                                                  (int)(heart.Width * heartScale),
                                                  (int)(heart.Height * heartScale)), null,
                                                  drawColor, 0f, new Vector2(heart.Width / 2, heart.Height / 2), SpriteEffects.None, 0);
        }

        public void SendPattern(HeartPattern pattern, int duration, GameTime gameTime)
        {
            Line.Self.SendPattern(pattern, duration, gameTime);
            lastPattern = pattern;
        }

        public void StartAutoPilot(GameTime gameTime)
        {
            autoPilot = true;
            autoPilotCycle = gameTime.TotalGameTime;
        }

        public void EndAutoPilot()
        {
            autoPilot = false;
            lastTimePressed = Game1.currentTime;
        }

    }
}
