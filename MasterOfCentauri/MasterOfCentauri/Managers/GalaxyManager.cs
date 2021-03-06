﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MasterOfCentauri.Util;

namespace MasterOfCentauri.Managers
{
    class GalaxyManager
    {

        private MersenneRandom _rand = new MersenneRandom((uint)DateTime.Now.Ticks);
        private readonly ContentController _content;
        private readonly GameContentManager _gameContentManager;


        public GalaxyManager(IServiceProvider services)
        {
            _content = (ContentController)services.GetService(typeof(ContentController));
            _gameContentManager = (GameContentManager)services.GetService(typeof(GameContentManager));
        }

        public Model.Galaxy GenerateSpiralGalaxy(Model.GalaxySetup galaxySetup)
        {
            Model.Galaxy gal = new Model.Galaxy();
            gal.Height = galaxySetup.Height;
            gal.Width = galaxySetup.Width;
            gal.StarsCount = galaxySetup.Stars;
            gal.Sectors = new List<Model.GalaxySector>();

            //Generate sectors
            int xCounter, yCounter;
            xCounter = 0;
            yCounter = 0;
            for (int i = 0; i < 16; i++)
            {
                Model.GalaxySector sec = new Model.GalaxySector();
                sec.X = (galaxySetup.Width / 4) * xCounter;
                sec.Y = (galaxySetup.Height / 4) * yCounter;
                sec.Width = (galaxySetup.Width / 4);
                sec.Height = (galaxySetup.Height / 4);
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
            double arm_angle = 2.0 * Math.PI / galaxySetup.Arms;
            double arm_spread = 0.7 * Math.PI / galaxySetup.Arms ;
            double arm_length = 1.5 * Math.PI;
            double center = 0.25;
            double x, y;
            int j, attempts;

            for (j = 0, attempts = 0; j < galaxySetup.Stars && attempts < Model.Constants.MaxAttemptsToPlaceStar; ++j, ++attempts)
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
                    double arm = _rand.Next(0, galaxySetup.Arms ) * arm_angle;
                    double angle = _rand.NextDouble(0.0, arm_spread);

                    x = radius * Math.Cos(arm_offset + arm + angle + radius * arm_length);
                    y = radius * Math.Sin(arm_offset + arm + angle + radius * arm_length);
                }

                x = (x + 1) * galaxySetup.Width  / 2.0;
                y = (y + 1) * galaxySetup.Height / 2.0;

                if (x < 0 || galaxySetup.Width <= x || y < 0 || galaxySetup.Height  <= y)
                    continue;


                Model.GalaxySector sec = gal.Sectors.Find(c => (c.X < x && (c.X + c.Width > x) && c.Y < y && (c.Y + c.Height > y)));
                int halfDistance = Model.Constants.MinDistanceBetweenStars / 2;
                if (x > sec.X + halfDistance && (x < (sec.X + sec.Width) - halfDistance) && (y > sec.Y + halfDistance) && (y < (sec.Y + sec.Height) - halfDistance))
                {
                    double lowest_dist = GetDistanceToClosestStar(sec, (int)x, (int)y);

                    //if the stars is to close we try again.
                    if (lowest_dist < Model.Constants.MinDistanceBetweenStars && attempts < Model.Constants.MaxAttemptsToPlaceStar - 1)
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
                Model.StarType starType = _gameContentManager.GetRandomStarType();
                string starTexture = starType.TextureName;
                sec.Stars.Add(new Model.Star { Position = new Vector2((int)x,(int)y), BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, Model.Constants.STAR_WIDTH , Model.Constants.STAR_WIDTH), StarTexture = starTexture });
                attempts = 0;
            }
            return gal;
        }


        public Model.Galaxy GenerateIrregularGalaxy(Model.GalaxySetup galaxySetup)
        {
            Model.Galaxy gal = new Model.Galaxy();
            gal.Height = galaxySetup.Height;
            gal.Width = galaxySetup.Width ;
            gal.StarsCount = galaxySetup.Stars;
            gal.Decorations = new List<Model.GalaxyDecoration>();
            gal.Sectors = new List<Model.GalaxySector>();

            //Generate sectors
            int xCounter, yCounter;
            xCounter = 0;
            yCounter = 0;
            for (int j = 0; j < 16; j++)
            {
                Model.GalaxySector sec = new Model.GalaxySector();
                sec.X = (galaxySetup.Width / 4) * xCounter;
                sec.Y = (galaxySetup.Height  / 4) * yCounter;
                sec.Width = (galaxySetup.Width / 4);
                sec.Height = (galaxySetup.Height / 4);
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


            int i, attempts;
            for (i = 0, attempts = 0; i < galaxySetup.Stars  && attempts < Model.Constants.MaxAttemptsToPlaceStar; ++i, ++attempts)
            {
                double x = _rand.NextDouble() * galaxySetup.Width ;
                double y = _rand.NextDouble() * galaxySetup.Height ;

                Model.GalaxySector sec = gal.Sectors.Find(c => (c.X < x && (c.X + c.Width > x) && c.Y < y && (c.Y + c.Height > y)));
                int halfDistance = Model.Constants.MinDistanceBetweenStars / 2;

                if (x > sec.X + halfDistance && (x < (sec.X + sec.Width) - halfDistance) && (y > sec.Y + halfDistance) && (y < (sec.Y + sec.Height) - halfDistance))
                {
                    double lowest_dist = GetDistanceToClosestStar(sec, (int)x, (int)y);

                    //if the stars is to close we try again.
                    if (lowest_dist < Model.Constants.MinDistanceBetweenStars && attempts < Model.Constants.MaxAttemptsToPlaceStar - 1)
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
                Model.StarType starType = _gameContentManager.GetRandomStarType();
                Model.Star starToAdd = new Model.Star();
                starToAdd.Name = _gameContentManager.GetRandomStarName();
                starToAdd.Position = new Vector2((int)x, (int)y);
                starToAdd.StarTexture = starType.TextureName;
                starToAdd.BoundingBox = new Microsoft.Xna.Framework.Rectangle((int)x, (int)y, Model.Constants.STAR_WIDTH, Model.Constants.STAR_WIDTH);
                sec.Stars.Add(starToAdd);
            }

            //Add Decoration
            for (int j = 0; j < 30; j++)
            {
                Model.GalaxyDecoration decor = new Model.GalaxyDecoration();
                double x = _rand.NextDouble() * galaxySetup.Width ;
                double y = _rand.NextDouble() * galaxySetup.Height ;
                decor.Position = new Vector2((int)x, (int)y);
                decor.Width = 256;
                decor.Height = 256;
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
                    return Model.Constants.MinDistanceBetweenStars + 1;


                Vector2 src, dest;
                src = new Vector2(x, y);
                dest =sec.Stars[0].Position;
                double lowest_dist = Vector2.Distance(src, dest);
                double distance;

                for (i = 1; i < sec.Stars.Count; i++)
                {
                    dest = sec.Stars[i].Position;
                    distance = Vector2.Distance(src, dest);
                    if (lowest_dist > distance)
                        lowest_dist = distance;
                }

                return (int)lowest_dist;
            }
            else
            {
                return Model.Constants.MinDistanceBetweenStars + 1;
            }
        }

    }
}
