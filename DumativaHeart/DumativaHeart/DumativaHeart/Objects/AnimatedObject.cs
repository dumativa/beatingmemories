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

namespace DumativaHeart.Objects
{
    class AnimatedObject : Objeto2D
    {

        public Dictionary<String, AnimationStructure> animations;

        public AnimationStructure animation;
        public int frame;

        public TimeSpan animationTime;

        public struct AnimationStructure
        {
            public int frameWidth;
            public int frameHeight;
            public int frameCount;
            public int framesPerSecond;
            public int X;
            public int Y;
        }

        public AnimatedObject(Texture2D textura, int Width, int Height)
            : base(textura)
        {
            animations = new Dictionary<String, AnimationStructure>();

            sourceRectangle = new Rectangle(0, 0, Width, Height);
            VetorDeOrigem = new Vector2 (sourceRectangle.Width / 2, sourceRectangle.Height / 2);


            AnimationStructure animationDefault;
            animationDefault = new AnimationStructure();
            animationDefault.frameWidth = Width;
            animationDefault.frameHeight = Height;
            animationDefault.X = 0;
            animationDefault.Y = 0;
            animationDefault.framesPerSecond = 100;
            animationDefault.frameCount = 1;
            this.AddAnimation("default", animationDefault);

            animationTime = new TimeSpan();

            this.ChangeAnimation("default");


        }


        public override void Update(GameTime gameTime)
        {
            animationTime = animationTime.Add(gameTime.ElapsedGameTime);

            frame = (int)(animationTime.TotalSeconds * animation.framesPerSecond) % animation.frameCount;

            base.Update(gameTime);
        }


        public void AddAnimation(String name, AnimationStructure animation)
        {
            animations.Add(name, animation);
        }

        public void ChangeAnimation(String name)
        {
            animation = animations[name];
        }

        public override void CalcularRetangulo()
        {
            collisionBounds = new Rectangle(
                (int)(this.position.X),
                (int)(this.position.Y),
                (int)(sourceRectangle.Width * ScaleX),
                (int)(sourceRectangle.Height * ScaleY)
                );
        
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            calcularCorte();
                

            base.Draw(spriteBatch);
            
        }

        protected virtual void calcularCorte()
        {
            sourceRectangle = new Rectangle(
                       animation.X + frame * animation.frameWidth,
                       animation.Y,
                       animation.frameWidth,
                       animation.frameHeight);
        }

        

        
    }
}
