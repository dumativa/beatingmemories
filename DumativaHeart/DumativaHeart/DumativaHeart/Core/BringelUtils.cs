using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DumativaHeart
{
    static class BringelUtils
    {

        public static float Distance(Vector2 point1, Vector2 point2)
        {
            return (point1 - point2).Length();
        }



        public static Texture2D Pixelate(Texture2D textura)
        {
            //VAI LÁ APRENDER SHADDER (código ruim e lento e puta merda)
            Color[] data = new Color[textura.Width * textura.Height];
            Color[,] data2D = new Color[textura.Width, textura.Height];

            textura.GetData<Color>(data);

            for (int x = 0; x < textura.Width; x++)
                for (int y = 0; y < textura.Height; y++)
                    data2D[x, y] = data[x + y * textura.Width];


            for (int y = 0; y < textura.Height - 4; y++)
            {
                for (int x = 0; x < textura.Width - 4; x++)
                {
                    //RED
                    data2D[x + 0, y + 0].R = data2D[x + 1, y + 0].R = data2D[x + 2, y + 0].R = data2D[x + 3, y + 0].R =
                    data2D[x + 0, y + 1].R = data2D[x + 1, y + 1].R = data2D[x + 2, y + 1].R = data2D[x + 3, y + 1].R =
                    data2D[x + 0, y + 2].R = data2D[x + 1, y + 2].R = data2D[x + 2, y + 2].R = data2D[x + 3, y + 2].R =
                    data2D[x + 0, y + 3].R = data2D[x + 1, y + 3].R = data2D[x + 2, y + 3].R = data2D[x + 3, y + 3].R =
                    (byte)
                    ((
                    data2D[x + 0, y + 0].R + data2D[x + 1, y + 0].R + data2D[x + 2, y + 0].R + data2D[x + 3, y + 0].R +
                    data2D[x + 0, y + 1].R + data2D[x + 1, y + 1].R + data2D[x + 2, y + 1].R + data2D[x + 3, y + 1].R +
                    data2D[x + 0, y + 2].R + data2D[x + 1, y + 2].R + data2D[x + 2, y + 2].R + data2D[x + 3, y + 2].R +
                    data2D[x + 0, y + 3].R + data2D[x + 1, y + 3].R + data2D[x + 2, y + 3].R + data2D[x + 3, y + 3].R) / 16);


                    //GREEN
                    data2D[x + 0, y + 0].G = data2D[x + 1, y + 0].G = data2D[x + 2, y + 0].G = data2D[x + 3, y + 0].G =
                    data2D[x + 0, y + 1].G = data2D[x + 1, y + 1].G = data2D[x + 2, y + 1].G = data2D[x + 3, y + 1].G =
                    data2D[x + 0, y + 2].G = data2D[x + 1, y + 2].G = data2D[x + 2, y + 2].G = data2D[x + 3, y + 2].G =
                    data2D[x + 0, y + 3].G = data2D[x + 1, y + 3].G = data2D[x + 2, y + 3].G = data2D[x + 3, y + 3].G =
                    (byte)
                    ((
                    data2D[x + 0, y + 0].G + data2D[x + 1, y + 0].G + data2D[x + 2, y + 0].G + data2D[x + 3, y + 0].G +
                    data2D[x + 0, y + 1].G + data2D[x + 1, y + 1].G + data2D[x + 2, y + 1].G + data2D[x + 3, y + 1].G +
                    data2D[x + 0, y + 2].G + data2D[x + 1, y + 2].G + data2D[x + 2, y + 2].G + data2D[x + 3, y + 2].G +
                    data2D[x + 0, y + 3].G + data2D[x + 1, y + 3].G + data2D[x + 2, y + 3].G + data2D[x + 3, y + 3].G) / 16);


                    //BLUE
                    data2D[x + 0, y + 0].B = data2D[x + 1, y + 0].B = data2D[x + 2, y + 0].B = data2D[x + 3, y + 0].B =
                    data2D[x + 0, y + 1].B = data2D[x + 1, y + 1].B = data2D[x + 2, y + 1].B = data2D[x + 3, y + 1].B =
                    data2D[x + 0, y + 2].B = data2D[x + 1, y + 2].B = data2D[x + 2, y + 2].B = data2D[x + 3, y + 2].B =
                    data2D[x + 0, y + 3].B = data2D[x + 1, y + 3].B = data2D[x + 2, y + 3].B = data2D[x + 3, y + 3].B =
                    (byte)
                    ((
                    data2D[x + 0, y + 0].B + data2D[x + 1, y + 0].B + data2D[x + 2, y + 0].B + data2D[x + 3, y + 0].B +
                    data2D[x + 0, y + 1].B + data2D[x + 1, y + 1].B + data2D[x + 2, y + 1].B + data2D[x + 3, y + 1].B +
                    data2D[x + 0, y + 2].B + data2D[x + 1, y + 2].B + data2D[x + 2, y + 2].B + data2D[x + 3, y + 2].B +
                    data2D[x + 0, y + 3].B + data2D[x + 1, y + 3].B + data2D[x + 2, y + 3].B + data2D[x + 3, y + 3].B) / 16);



                    x += 3;

                }
                y += 3;
            }

            Color[] finalData = new Color[textura.Width * textura.Height];

            for (int x = 0; x < textura.Width; x++)
            {
                for (int y = 0; y < textura.Height; y++)
                {

                    finalData[x + y * textura.Width] = data2D[x % textura.Width, y % textura.Height];

                }
            }

            Texture2D texturaFinal = new Texture2D(Game1.Self.GraphicsDevice, textura.Width, textura.Height);
            texturaFinal.SetData<Color>(finalData);

            return texturaFinal;
        }

        public static int DivRoundUp(int dividend, int divisor)
        {
            //if (divisor == 0 ) throw ...
            //if (divisor == -1 && dividend == Int32.MinValue) throw ...
            int roundedTowardsZeroQuotient = dividend / divisor;
            bool dividedEvenly = (dividend % divisor) == 0;
            if (dividedEvenly)
                return roundedTowardsZeroQuotient;

            // At this point we know that divisor was not zero 
            // (because we would have thrown) and we know that 
            // dividend was not zero (because there would have been no remainder)
            // Therefore both are non-zero.  Either they are of the same sign, 
            // or opposite signs. If they're of opposite sign then we rounded 
            // UP towards zero so we're done. If they're of the same sign then 
            // we rounded DOWN towards zero, so we need to add one.

            bool wasRoundedDown = ((divisor > 0) == (dividend > 0));
            if (wasRoundedDown)
                return roundedTowardsZeroQuotient + 1;
            else
                return roundedTowardsZeroQuotient;
        }

        public static float moveTowards(float posInicial, float posFinal, int duration, GameTime gameTime)
        {
            float result;

            result = (float)((posFinal - posInicial) * (gameTime.ElapsedGameTime.TotalMilliseconds / duration));

            return result;
        }
        
        
        public static double getAngle(Vector2 initialVector, Vector2 destinationVector)
        {
            return Math.Atan2((double)(initialVector.Y - destinationVector.Y), (double)(initialVector.X - destinationVector.X));
        }
    }
}
