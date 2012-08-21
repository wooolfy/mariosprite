using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GayMarioGame
{

    //xbox has screen resolution of 800 x 480
    class Level
    {
        private Microsoft.Xna.Framework.Game game;
        private int[,] levelmap;
        private List<Tile> tiles;
        private Texture2D leveltiles;
        private int mapwidth;
        private int mapheight;
        private int tilewidth;
        private int tileheight;
        private GraphicsDevice graphicsDevice;

        public Level(Microsoft.Xna.Framework.Game game, GraphicsDevice graphicsDevice)
        {
            this.game = game;
            this.graphicsDevice = graphicsDevice;
            tiles = new List<Tile>();
            //game.Content.RootDirectory = "Content";
        }

        public void scroll(bool direction, int value)
        {
            foreach (var lala in tiles)
            {
                if (direction)
                    lala.posx+= value;
                else
                    lala.posx-=value;
            }
        }
        public List<Tile> getTiles()
        {
            return this.tiles;
        }

        public void scrollypos(int posy)
        {
            foreach (var lala in tiles)
            {
                lala.posy -= posy;
            }
        }

        //TODO:check if tile is visible
        public void LoadMap(string levelname)
        {

            ReadLevel level = new ReadLevel(game);
            level.SetInpFile(levelname);
            level.Parse();
            this.levelmap = level.getMap();
            this.mapwidth = level.mapwidth;
            this.mapheight = level.mapheight;
            this.tilewidth = level.tilewidth;
            this.tileheight = level.tileheight;
            this.leveltiles = game.Content.Load<Texture2D>("mariotiles2fach");//@level.tilesname);

            for (int j = 0; j < this.mapheight; j++)
                for (int i = 0; i < this.mapwidth; i++)
                {
                    if (levelmap[i, j] != 0)
                    {
                        Tile tile = new Tile(i, j, mapwidth, mapheight, levelmap[i, j]-1, tilewidth, tileheight, false);
                        tiles.Add(tile);
                    }
                }

        }
        public void drawMap(SpriteBatch spritebatch)
        {
            foreach(var tile in tiles)
                {
                    tile.tilePosRect = new Rectangle(tile.posx, tile.posy, tilewidth, tileheight);

                    spritebatch.Draw(
                    leveltiles,
                    tile.tilePosRect,
                    tile.rectangleInPicture,
                    Color.White
                    );
                }
        }
    }
    class Tile
    {
        public int posx, posy;
        public Rectangle rectangleInPicture;
        public Rectangle tilePosRect;
        bool visible;
        public int tilewidth;
        public int tileheight;

        public Tile(int posx, int posy, int mapwidth, int mapheight, int tileint,int tilewidth, int tileheight, bool visible)
        {
            this.tileheight = tileheight;
            this.tilewidth = tilewidth;
            int z=0;
            this.posx = posx * 32;
            this.posy = posy * 32;
            for (int j=0; j<480/32; j++)
                for (int i = 0; i < 704/32; i++)
                {
                    if (z == tileint)
                    {
                        rectangleInPicture = new Rectangle(i * tilewidth, j * tileheight, tilewidth, tileheight);
                        this.visible = true;
                        tilePosRect = new Rectangle(i * tilewidth, j * tileheight, tilewidth, tileheight);
                    }
                    z++;
                }
        }
        public void setVisible(bool visible)
        {
            this.visible = visible;
        }
 
    }

    class ReadLevel
    {

        private String jsonInput;
        private int[] intlevel;
        public int mapwidth { get; set; }
        public int mapheight { get; set; }
        public int tilewidth { get; set; }
        public int tileheight { get; set; }
        public string tilesname { get; set; }
        private int[,] levelarray;
        Microsoft.Xna.Framework.Game game;

        public ReadLevel(Microsoft.Xna.Framework.Game game)
        {
            this.game = game;
        }
        public void Parse()
        {
            var obj = (JObject)JsonConvert.DeserializeObject(this.jsonInput);

            foreach (var item in obj["layers"])
            {
                String[] stringlevel = item["data"].ToString().Split(',');
                intlevel = new int[stringlevel.Length + 1];
                int i = 0;
                foreach (string bl in stringlevel)
                {
                    intlevel[i] = int.Parse(RemoveSpecialCharacters(bl));
                    i++;
                }
                mapwidth = int.Parse(RemoveSpecialCharacters(item["width"].ToString()));
                mapheight = int.Parse(RemoveSpecialCharacters(item["height"].ToString()));
            }
            foreach (var item in obj["tilesets"])
            {
                this.tilesname = RemoveSpecialCharactersImageName(item["image"].ToString());
            }

            tilewidth = int.Parse(RemoveSpecialCharacters(obj["tilewidth"].ToString()));
            tileheight = int.Parse(RemoveSpecialCharacters(obj["tileheight"].ToString()));
            int j = 0;
            levelarray = new int[mapwidth, mapheight];
            for (int y = 0; y < mapheight; y++)
                for (int x = 0; x < mapwidth; x++)
                {
                    levelarray[x, y] = intlevel[j];
                    j++;
                }
            //foreach(var lev in levelarray)
            //    Debug.WriteLine(lev.ToString());
        }
        public void SetInpFile(String file)
        {
            //List<string> lines = new List<string>();
            //using (StreamReader reader = new StreamReader(game.Content.Load<Stream>(@"Leveltry.txt")))
            //{
            //    //game.Content.RootDirectory = "Content";
            //    this.jsonInput = reader.ReadToEnd();
            //    //Debug.WriteLine(jsonInput);
            //}
            this.jsonInput = game.Content.Load<String>(@"Leveltry");
        }
        public static string RemoveSpecialCharacters(string str)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9'))
                    sb.Append(str[i]);
            }

            return sb.ToString();
        }

        public static string RemoveSpecialCharactersImageName(string str)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] >= '0' && str[i] <= '9' || str[i] >= 'a' && str[i] <= 'z' || str[i] >= 'A' && str[i] <= 'Z')  || str[i] == '.')//|| str[i] == '.' && str[i-1] != '.')
                    sb.Append(str[i]);
                else { 
                    sb = new StringBuilder(); 
                }
            }

            return sb.ToString();
        }

        public int[,] getMap()
        {
            return this.levelarray;
        }
    }
}
