using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterOfOrion.Classes.Misc;

namespace MasterOfCentauri.Managers
{
    class GalaxyManager
    {

        private MersenneRandom _rand = new MersenneRandom((uint)DateTime.Now.Ticks);

        //Constants
        const int STAR_WIDTH = 20; //This is the stars width including halo in worldunits
        const int MinDistanceBetweenStars = 35; //in worldunits so that stars aren't placed to close
        const int MaxAttemptsToPlaceStar = 100;

        public GalaxyManager()
        {

        }

        public Model.Galaxy GenerateSpiralGalaxy(int arms, int width, int height, int numStars)
        {
            Model.Galaxy gal = new Model.Galaxy();
            gal.Height = height;
            gal.Width = width;
            gal.StarsCount = numStars;
            gal.Sectors = new List<Model.GalaxySector>(); 
 
            //Generate sectors
            int xCounter, yCounter;
            xCounter = 0;
            yCounter = 0;
            for (int i = 0; i < 16; i++)
            {   
                Model.GalaxySector sec = new Model.GalaxySector();
                sec.X = (width / 4) * xCounter;
                sec.Y = (height / 4) * yCounter;
                sec.Width = (width / 4);
                sec.Height = (height / 4);
                sec.Stars = new List<Model.Star>();
                sec.BoundingBox = new Microsoft.Xna.Framework.Rectangle(sec.X, sec.Y, sec.Width, sec.Height);

                xCounter++;
                if (xCounter >= 4)
                {
                    xCounter = 0;
                    yCounter++;
                }
                gal.Sectors.Add(sec);
            }

            //Add stars
            //Begin generation, should run as a background thread, and display a loading screen
            double arm_offset = _rand.NextDouble(0.0, 2.0 * Math.PI);
            double arm_angle = 2.0 * Math.PI / arms;
            double arm_spread = 0.7 * Math.PI / arms;
            double arm_length = 1.5 * Math.PI;
            double center = 0.25;
            double x, y;
            int j, attempts;

            for (j = 0, attempts = 0; j < numStars && attempts < MaxAttemptsToPlaceStar; ++j, ++attempts)
            {
                double radius = _rand.NextDouble(0.0, 1.0);
                if (radius < center)
                {
                    double angle = _rand.NextDouble(0.0, 2.0 * Math.PI);
                    x = radius * Math.Cos(arm_offset + angle);
                    y = radius * Math.Sin(arm_offset + angle);
                }
                else
                {
                    double arm = _rand.Next(0, arms) * arm_angle;
                    double angle = _rand.NextDouble(0.0, arm_spread);

                    x = radius * Math.Cos(arm_offset + arm + angle + radius * arm_length);
                    y = radius * Math.Sin(arm_offset + arm + angle + radius * arm_length);
                }

                x = (x + 1) * width / 2.0;
                y = (y + 1) * width / 2.0;

                if (x < 0 || width <= x || y < 0 || width <= y)
                    continue;
                attempts = 0;

                Model.GalaxySector sec = gal.Sectors.Find(c => (c.X < x && (c.X + c.Width > x) && c.Y < y && (c.Y + c.Height > y)));
                if (x > sec.X + MinDistanceBetweenStars && (x < (sec.X + sec.Width) - MinDistanceBetweenStars) && (y > sec.Y + MinDistanceBetweenStars) && (y < (sec.Y + sec.Height) - MinDistanceBetweenStars))
                {
                    double lowest_dist = GetDistanceToClosestStar(sec, (int)x, (int)y);

                    //if the stars is to close we try again.
                    if (lowest_dist < MinDistanceBetweenStars && attempts < MaxAttemptsToPlaceStar - 1)
                    {
                        --j;
                        continue;
                    }
                }
                else
                {
                     --j;
                        continue;
                }

               
                //Create Star

                sec.Stars.Add(new Model.Star { X = (int)x, Y = (int)y , BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, STAR_WIDTH, STAR_WIDTH)});

            }

            return gal;
        }

        public int GetDistanceToClosestStar(Model.GalaxySector sec, int x, int y)
        {
            
            if (sec != null)
            {
                int i;
                //If this is the first star to be placed it will have no distance to other stars
                if (sec.Stars.Count <= 0)
                    return MinDistanceBetweenStars + 1;



                int lowest_dist = (sec.Stars[0].X - x) * (sec.Stars[0].X - x)
                + (sec.Stars[0].Y - y) * (sec.Stars[0].Y - y), distance = 0;

                for (i = 1; i < sec.Stars.Count; i++)
                {
                    distance = (sec.Stars[i].X - x) * (sec.Stars[i].X - x)
                    + (sec.Stars[i].Y - y) * (sec.Stars[i].Y - y);
                    if (lowest_dist > distance)
                        lowest_dist = distance;
                }

                return lowest_dist;
            }
            else
            {
                return MinDistanceBetweenStars + 1;
            }
        }
    }
}
