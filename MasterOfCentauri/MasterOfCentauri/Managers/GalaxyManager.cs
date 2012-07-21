using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MasterOfOrion.Classes.Misc;
using Microsoft.Xna.Framework;

namespace MasterOfCentauri.Managers
{
    class GalaxyManager
    {

        private MersenneRandom _rand = new MersenneRandom((uint)DateTime.Now.Ticks);
        private readonly ContentController _content;
        //Constants
        const int STAR_WIDTH = 128; //This is the stars width including halo in worldunits
        const int MinDistanceBetweenStars = STAR_WIDTH * 2; //in worldunits so that stars aren't placed to close
        const int MaxAttemptsToPlaceStar = 100;

        public GalaxyManager(IServiceProvider services)
        {
            _content = (ContentController)services.GetService(typeof(ContentController));
        }

        public Model.Galaxy GenerateSpiralGalaxy(int arms, int width, int height, int numStars)
        {
            width = width;
            height = height;
            Model.Galaxy gal = new Model.Galaxy();
            gal.Height = height;
            gal.Width = width;
            gal.StarsCount = numStars;
            gal.Stars = new List<Model.Star>();
            List<Model.GalaxySector> sectors = new List<Model.GalaxySector>();
 
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
                sectors.Add(sec);
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

                if (x < width)
                {
                    string test = "test";
                }
                if (x < 0 || width <= x || y < 0 || width <= y)
                    continue;


                Model.GalaxySector sec = sectors.Find(c => (c.X < x && (c.X + c.Width > x) && c.Y < y && (c.Y + c.Height > y)));
                int halfDistance = MinDistanceBetweenStars / 2;
                if (x > sec.X + halfDistance && (x < (sec.X + sec.Width) - halfDistance) && (y > sec.Y + halfDistance) && (y < (sec.Y + sec.Height) - halfDistance))
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
                string starTexture = _rand.NextDouble() > 0.5 ? "StarDisk_133" : "StarDisk_136";
                sec.Stars.Add(new Model.Star { X = (int)x, Y = (int)y, BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, STAR_WIDTH, STAR_WIDTH), StarTexture = starTexture });
                gal.Stars.Add(new Model.Star { X = (int)x, Y = (int)y, BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, STAR_WIDTH, STAR_WIDTH), StarTexture = starTexture });
                attempts = 0;
            }
            SortStarListAfterTexture(gal);
            return gal;
        }


        public Model.Galaxy GenerateIrregularGalaxy(int numStars, int width)
        {

            width = width;
            Model.Galaxy gal = new Model.Galaxy();
            gal.Height = width;
            gal.Width = width;
            gal.StarsCount = numStars;
            gal.Stars = new List<Model.Star>();
            gal.Decorations = new List<Model.GalaxyDecoration>();
            List<Model.GalaxySector> sectors = new List<Model.GalaxySector>();

            //Generate sectors
            int xCounter, yCounter;
            xCounter = 0;
            yCounter = 0;
            for (int j = 0; j < 16; j++)
            {
                Model.GalaxySector sec = new Model.GalaxySector();
                sec.X = (width / 4) * xCounter;
                sec.Y = (width / 4) * yCounter;
                sec.Width = (width / 4);
                sec.Height = (width / 4);
                sec.Stars = new List<Model.Star>();
                sec.BoundingBox = new Microsoft.Xna.Framework.Rectangle(sec.X, sec.Y, sec.Width, sec.Height);

                xCounter++;
                if (xCounter >= 4)
                {
                    xCounter = 0;
                    yCounter++;
                }
                sectors.Add(sec);
            }


            int i, attempts;
            for (i = 0, attempts = 0; i < numStars && attempts < MaxAttemptsToPlaceStar; ++i, ++attempts)
            {
                double x = _rand.NextDouble() * width;
                double y = _rand.NextDouble() * width;

                Model.GalaxySector sec = sectors.Find(c => (c.X < x && (c.X + c.Width > x) && c.Y < y && (c.Y + c.Height > y)));
                int halfDistance = MinDistanceBetweenStars / 2;

                if (x > sec.X + halfDistance && (x < (sec.X + sec.Width) - halfDistance) && (y > sec.Y + halfDistance) && (y < (sec.Y + sec.Height) - halfDistance))
                {
                    double lowest_dist = GetDistanceToClosestStar(sec, (int)x, (int)y);

                    //if the stars is to close we try again.
                    if (lowest_dist < MinDistanceBetweenStars && attempts < MaxAttemptsToPlaceStar - 1)
                    {
                        --i;
                        continue;
                    }
                }
                else
                {
                    --i;
                    continue;
                }

                //Create Star
                attempts = 0;
                string starTexture = _rand.NextDouble() > 0.5 ? "neutron01" : "orange01";
                sec.Stars.Add(new Model.Star { X = (int)x, Y = (int)y, BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, STAR_WIDTH, STAR_WIDTH), StarTexture = starTexture });
                gal.Stars.Add(new Model.Star { X = (int)x, Y = (int)y, BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, STAR_WIDTH, STAR_WIDTH), StarTexture = starTexture });
            }
            SortStarListAfterTexture(gal);

            //Add Decoration
            for (int j = 0; j < 10; j++)
            {
                Model.GalaxyDecoration decor = new Model.GalaxyDecoration();
                double x = _rand.NextDouble() * width;
                double y = _rand.NextDouble() * width;
                decor.Position = new Vector2((int)x, (int)y);
                decor.Width = 2000;
                decor.Height = 2000;
                decor.Rotation = 0;
                decor.TextureName = "gaseous01";
                gal.Decorations.Add(decor);
            }
            return gal;
        }

        public Model.Galaxy GenerateGalaxyFromDensityMap(string densityMap, int width, int height, int numStars)
        {

            Model.Galaxy gal = new Model.Galaxy();
            gal.Height = width;
            gal.Width = width;
            gal.StarsCount = numStars;
            List<Model.GalaxySector> sectors = new List<Model.GalaxySector>();
            //Generate sectors
            int xCounter, yCounter;
            xCounter = 0;
            yCounter = 0;
            for (int j = 0; j < 16; j++)
            {
                Model.GalaxySector sec = new Model.GalaxySector();
                sec.X = (width / 4) * xCounter;
                sec.Y = (width / 4) * yCounter;
                sec.Width = (width / 4);
                sec.Height = (width / 4);
                sec.Stars = new List<Model.Star>();
                sec.BoundingBox = new Microsoft.Xna.Framework.Rectangle(sec.X, sec.Y, sec.Width, sec.Height);

                xCounter++;
                if (xCounter >= 4)
                {
                    xCounter = 0;
                    yCounter++;
                }
                sectors.Add(sec);
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


                Vector2 src, dest;
                src = new Vector2(x, y);
                dest = new Vector2(sec.Stars[0].X, sec.Stars[0].Y);
                double lowest_dist = Vector2.Distance(src, dest);
                double distance;

                for (i = 1; i < sec.Stars.Count; i++)
                {
                    dest.X = sec.Stars[i].X;
                    dest.Y = sec.Stars[i].Y;
                    distance = Vector2.Distance(src, dest);
                    if (lowest_dist > distance)
                        lowest_dist = distance;
                }

                return (int)lowest_dist;
            }
            else
            {
                return MinDistanceBetweenStars + 1;
            }
        }

        public void SortStarListAfterTexture(Model.Galaxy gal)
        {
            Dictionary<String, List<Model.Star>> _sorted = new Dictionary<string, List<Model.Star>>();
            foreach (Model.Star star in gal.Stars)
            {
                Util.TextureAtlas atlas = _content.getStarAtlasFromTextureName(star.StarTexture);
                if (!_sorted.ContainsKey(atlas.Name))
                {
                    _sorted[atlas.Name] = new List<Model.Star>();
                }
                _sorted[atlas.Name].Add(star);
            }
            gal.Stars.Clear();
            foreach (string key in _sorted.Keys)
            {
                foreach (Model.Star star in _sorted[key])
                {
                    gal.Stars.Add(star);
                }
            }
        }
    }
}
