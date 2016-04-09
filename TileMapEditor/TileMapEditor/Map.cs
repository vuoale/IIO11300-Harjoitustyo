﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileMapEditor
{
    public class Map
    {
        private int _mapRows;
        private int _mapColumns;
        private List<Tile> _mapTiles;
        private Tile[,] _mapTileArr;

        public Map(int rows, int columns, int tileWidth, int tileHeight)
        {
            _mapRows = rows;
            _mapColumns = columns;
            _mapTiles = new List<Tile>();
            _mapTileArr = new Tile[rows, columns];
            setEmptyTiles(tileWidth, tileHeight);        
        }

        public Map(string filename, TileSet tileset)
        {
            loadMap(filename, tileset);
        }

        public int MapRows
        {
            get { return _mapRows; }
            set { _mapRows = value; }
        }

        public int MapColumns
        {
            get { return _mapColumns; }
            set { _mapColumns = value; }
        }

        public List<Tile> MapTiles
        {
            get { return _mapTiles; }
        }    

        private void setEmptyTiles(int tileWidth, int tileHeight)
        {
            for (int r = 0; r < _mapRows; r++)
            {
                for (int c = 0; c < _mapColumns; c++)
                {
                    Tile tile = new Tile(tileWidth, tileHeight);
                    _mapTiles.Add(tile);
                    _mapTileArr[r, c] = tile;
                }
            }
        } 

        public void saveMap(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int r = 0; r < _mapRows; r++)
                {
                    for (int c = 0; c < _mapColumns; c++)
                    {
                        writer.Write(_mapTileArr[r,c].TileNumber);
                        if (c < _mapColumns - 1)
                            writer.Write(",");
                    }
                    writer.WriteLine(); //rivinvaihto
                }
            }           
        }

        public void loadMap(string filename, TileSet tileSet)
        {
            string[] lines = File.ReadAllLines(filename);
            _mapRows = lines.Length;
            _mapColumns = lines[0].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
            _mapTiles = new List<Tile>();
            _mapTileArr = new Tile[_mapRows, _mapColumns];

            using (StreamReader reader = new StreamReader(filename))
            {
                int r = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    for (int c = 0; c < values.Length; c++)
                    {
                        Tile setTile = tileSet.getTileByNumber(int.Parse(values[c]));
                        Tile mapTile = new Tile(setTile.TileSetBitmap, setTile.RenderRect, setTile.TileNumber);
                        _mapTiles.Add(mapTile);
                        _mapTileArr[r, c] = mapTile;
                    }
                    r++;
                }
            }
        }
    }
}