using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DumativaHeart.Core;
using Microsoft.Xna.Framework.Input;

namespace DumativaHeart.Objects
{
    class Line : Objeto2D
    {
        public List<Objeto2D> lineList;

        float idealPosition;

        Boolean patternEnabled, onlySystoleRandomsGenerated;
        int onlySystoleRandom1, onlySystoleRandom2, onlySystoleRandom3;
        TimeSpan patternDuration, patternStartTime, patternElapsedTime;
        Vector2 lastPosition;

        Random random;

        public static Line Self;

        Heart.HeartPattern heartPattern;

        #region GaussianBlur
        private const int BLUR_RADIUS = 2;
        public float blurAmount = 3.5f;

        private GaussianBlur gaussianBlur;

        private RenderTarget2D renderTarget1;
        private RenderTarget2D renderTarget2;
        private int renderTargetWidth;
        private int renderTargetHeight;
        
        public RenderTarget2D lineRenderTarget;
        private bool drawParticles;

        #endregion

        public Line()
            : base(Game1.Self.Content.Load<Texture2D>("particulaLinha"))
        {
            Self = this;
            lineList = new List<Objeto2D>();
            this.position = new Vector2(0, Game1.Self.Window.ClientBounds.Height / 6 * 4);
            this.position.X = 100;
            idealPosition = position.Y;

            random = new Random();

            #region GaussianBlur
            gaussianBlur = new GaussianBlur(Game1.Self);
            gaussianBlur.ComputeKernel(BLUR_RADIUS, blurAmount);

            InitRenderTargets(Game1.Self.GraphicsDevice);

            lineRenderTarget = new RenderTarget2D(Game1.Self.GraphicsDevice, Game1.GameRactangle.Width, Game1.GameRactangle.Height);

            #endregion
        }

        private void InitRenderTargets(GraphicsDevice graphicsDevice)
        {
            renderTargetWidth = graphicsDevice.PresentationParameters.BackBufferWidth / 2;
            renderTargetHeight = graphicsDevice.PresentationParameters.BackBufferHeight / 2;

            renderTarget1 = new RenderTarget2D(graphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            renderTarget2 = new RenderTarget2D(graphicsDevice,
                renderTargetWidth, renderTargetHeight, false,
                graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.None);

            gaussianBlur.ComputeOffsets(renderTargetWidth, renderTargetHeight);
        }

        public override void Update(GameTime gameTime)
        {
            verifyLinePosition();
            createLineParticles();
            updateParticles();
            verifyPattern(gameTime);
            

            base.Update(gameTime);
        }

        


        private void verifyLinePosition()
        {
            this.position.X += 3f;

            if (position.X >= Game1.GameRactangle.Width - 120)
            {
                position.X = 100;
            }
        }

        private void createLineParticles()
        {
            
            if (!patternEnabled || drawParticles)
            {
                createParticle(this.position);
            }
            
        }

        private void updateParticles()
        {
            for (int i = 0; i < lineList.Count; i++)
            {
                Objeto2D particle = lineList[i];

                particle.alpha = MathHelper.Lerp(particle.alpha, 0, 0.007f);

                if (particle.alpha <= 0.02f)
                {
                    lineList.Remove(particle);
                }
            }
        }

        public float moveTowards(float posInicial, float posFinal, int duration, GameTime gameTime)
        {
            float result;

            result = (float)((posFinal - posInicial) * (gameTime.ElapsedGameTime.TotalMilliseconds / duration));

            return result;
        }




        private void verifyPattern(GameTime gameTime)
        {
            if (patternEnabled)
            {
                switch(heartPattern)
                {
                    case Heart.HeartPattern.Regular:
                        #region RegularPattern
                        Boolean ultimo = false;


                        if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.1f)
                        { 
                            this.position.Y += moveTowards(idealPosition, idealPosition + 25, (int)(patternDuration.TotalMilliseconds * 0.1f), gameTime);
                            completeLine(this.position, ultimo);
                        }
                        else if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.40f)
                        {
                            this.position.Y += moveTowards(idealPosition + 25, idealPosition - 110, (int)(patternDuration.TotalMilliseconds * 0.3f), gameTime);
                            completeLine(this.position, ultimo);
                        }
                        else if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.70f)
                        {
                            this.position.Y += moveTowards(idealPosition - 110, idealPosition + 80, (int)(patternDuration.TotalMilliseconds * 0.3f), gameTime);
                            completeLine(this.position, ultimo);
                        }
                        else 
                        {
                            this.position.Y += moveTowards(idealPosition + 80, idealPosition, (int)(patternDuration.TotalMilliseconds * 0.3f), gameTime);
                    
                            ultimo = true;
                            completeLine(this.position, ultimo);
                        }


                        if ((this.position.Y <= idealPosition) && ultimo)
                        {
                            
                            this.position.Y = idealPosition;
                            completeLine(position, false);
                            patternEnabled = false;
                            heartPattern = Heart.HeartPattern.None;

                            createParticle(this.position);
                            return;
                        }
                        break;
                        #endregion

                    case Heart.HeartPattern.Only_Systole:
                        #region OnlySystole
                        if (!onlySystoleRandomsGenerated)
                        {
                            onlySystoleRandom1 = random.Next(-20, 20);
                            onlySystoleRandom2 = random.Next(-20, 20);
                            onlySystoleRandom3 = random.Next(-20, 20);
                            onlySystoleRandomsGenerated = true;
                        }



                        if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.15f)
                        {
                            this.position.Y += moveTowards(idealPosition, idealPosition - 60 + onlySystoleRandom1, (int)(patternDuration.TotalMilliseconds * 0.15f), gameTime);
                            completeLine(this.position, false);
                        }
                        else if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.3f)
                        {
                            this.position.Y += moveTowards(idealPosition - 60 + onlySystoleRandom1, idealPosition + onlySystoleRandom2, (int)(patternDuration.TotalMilliseconds * 0.15f), gameTime);
                            completeLine(this.position, false);
                        }
                        else if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.45f)
                        {
                            completeLine(position, false);
                            this.position.Y = onlySystoleRandom2 + idealPosition;
                            drawParticles = true;
                        }
                        else if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.6f)
                        {
                            drawParticles = false;
                            this.position.Y += moveTowards(onlySystoleRandom2 + idealPosition, idealPosition - 60 + onlySystoleRandom3, (int)(patternDuration.TotalMilliseconds * 0.15f), gameTime);
                            completeLine(this.position, false);
                        }
                        else if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds <= 0.75f)
                        {
                            this.position.Y += moveTowards(idealPosition - 60 + onlySystoleRandom3, idealPosition, (int)(patternDuration.TotalMilliseconds * 0.15f), gameTime);
                            completeLine(this.position, false);
                        }
                        else
                        {
                            drawParticles = true;
                        }

                       

                        if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds > 1)
                        {
                            this.position.Y = idealPosition;
                            //completeLine(position, false);
                            patternEnabled = false;
                            heartPattern = Heart.HeartPattern.None;
                            drawParticles = false;
                            
                        }

                        break;
                        #endregion

                    case Heart.HeartPattern.Dual_Systole:
                        #region DualSystole
                        patternElapsedTime += gameTime.ElapsedGameTime;

                        //drawParticles = true;
                        this.position.Y = idealPosition + -(float)(Math.Sin(MathHelper.ToRadians((float)patternElapsedTime.TotalMilliseconds / MathHelper.Pi)) * 70);
                        completeLine(this.position, false);

                        if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds > 0.5f)
                        {
                            if (this.position.Y > idealPosition)
                            {
                                this.position.Y = idealPosition;
                                //completeLine(position, false);
                                patternEnabled = false;
                                heartPattern = Heart.HeartPattern.None;
                                drawParticles = false;
                            }
                        }
                        break;
                        #endregion

                    case Heart.HeartPattern.Only_Diastole:
                        patternElapsedTime += gameTime.ElapsedGameTime;
                        this.position.Y = idealPosition + (float)(Math.Sin(MathHelper.ToRadians((float)patternElapsedTime.TotalMilliseconds / MathHelper.Pi)) * 70);
                        completeLine(this.position, false);

                        if ((gameTime.TotalGameTime.TotalMilliseconds - patternStartTime.TotalMilliseconds) / patternDuration.TotalMilliseconds > 1f)
                        {
                            if (this.position.Y < idealPosition)
                            {
                                this.position.Y = idealPosition;
                                //completeLine(position, false);
                                patternEnabled = false;
                                heartPattern = Heart.HeartPattern.None;
                                drawParticles = false;
                            }
                        }

                        break;
                }
                
            }
            else
            {
                this.position.Y = idealPosition;
            }

            lastPosition = this.position;  
            
        }

        public void createParticle(Vector2 position)
        {
            
            Objeto2D particle = new Objeto2D(this.textura);
            particle.position = position;
            if (!Heart.Self.isDead)
                particle.color = Color.LightGreen;
            else
                particle.color = Color.Red;
            particle.ScaleX = particle.ScaleY = 0.2f;
            particle.alpha = 0.7f;
            lineList.Add(particle);
        }

        private void completeLine(Vector2 position, Boolean ultimo)
        {
            if (lastPosition != null)
            {
                float distance = BringelUtils.Distance(position, lastPosition);
                int particlesNumber = (int)Math.Round(distance / 3);

                if (distance <= 500)
                {
                    for (int i = 1; i < particlesNumber + 1; i++)
                    {

                        if ((position.Y < idealPosition) && ultimo)
                        {
                            Console.WriteLine("oi");
                            break;
                        }

                        float posX = MathHelper.Lerp(position.X, lastPosition.X, (float)i / (particlesNumber));
                        float posY = MathHelper.Lerp(position.Y, lastPosition.Y, (float)i / (particlesNumber));

                        createParticle(new Vector2(posX, posY));


                    }
                }
            }
        }

        

        public void Draw(SpriteBatch spriteBatch, Texture2D backtexture)
        {
            
            #region GaussianBlur
            Game1.Self.GraphicsDevice.SetRenderTarget(lineRenderTarget);
            Game1.Self.GraphicsDevice.Clear(Color.Transparent);
            #endregion

            spriteBatch.Begin();
            
            spriteBatch.End();
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);


            

            foreach (Objeto2D particle in lineList)
            {
                particle.Draw(spriteBatch);
            }

            spriteBatch.End();
            
            
            #region GaussianBlur
            Game1.Self.GraphicsDevice.SetRenderTarget(null);

            Texture2D textura = gaussianBlur.PerformGaussianBlur(lineRenderTarget, renderTarget1, renderTarget2, spriteBatch);
            
            
            Game1.Self.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            spriteBatch.Draw(backtexture, Vector2.Zero, Color.White);
            spriteBatch.Draw(textura, Game1.GameRactangle, Color.White);
            spriteBatch.End();
            #endregion
            
        }

        public void SendPattern(Heart.HeartPattern heartPattern, int duration, GameTime gameTime)
        {
            this.heartPattern = heartPattern;
            patternEnabled = true;
            patternDuration = TimeSpan.FromMilliseconds(MathHelper.Clamp(duration, 100, 700));
            patternStartTime = gameTime.TotalGameTime;
            patternElapsedTime = TimeSpan.FromMilliseconds(0);
        }
    }

}
