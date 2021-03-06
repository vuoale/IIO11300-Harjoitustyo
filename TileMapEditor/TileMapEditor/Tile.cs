﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TileMapEditor
{
    public class Tile : INotifyPropertyChanged
    {
        private BitmapSource _tileSetBitmap;
        private Int32Rect _renderRect;
        private CroppedBitmap _tileSprite;
        private int _tileNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        public Tile(int tileWidth, int tileHeight)
        {
            /* Konstruktori luo tyhjän tiilen */
            int stride = tileWidth / 8;
            byte[] pixels = new byte[tileHeight * stride];

            List<Color> colors = new List<Color>();
            Color color = Color.FromArgb(10, 255, 255, 255);
            colors.Add(color);
            BitmapPalette myPalette = new BitmapPalette(colors);

            BitmapSource empty = BitmapSource.Create(
                tileWidth,
                tileHeight,
                96,
                96,
                PixelFormats.Indexed1,
                myPalette,
                pixels,
                stride);

            _tileSetBitmap = empty;
            _renderRect = new Int32Rect();
            _tileSprite = new CroppedBitmap(_tileSetBitmap, _renderRect);
            _tileNumber = -1;
        }

        public Tile(BitmapSource tileSetBitmap, Int32Rect renderRect, int tileNumber)
        {
            /* Konstruktori luo tiilen tilesetistä */
            _tileSetBitmap = tileSetBitmap;
            _renderRect = renderRect;
            _tileSprite = new CroppedBitmap(_tileSetBitmap, _renderRect);
            _tileNumber = tileNumber;
        }

        public BitmapSource TileSetBitmap
        {
            get { return _tileSetBitmap; }
        }

        public Int32Rect RenderRect
        {
            get { return _renderRect; }
        }

        public CroppedBitmap TileSprite
        {
            get { return _tileSprite; }
        }

        public int TileNumber
        {
            get { return _tileNumber; }
        }

        public void setData(BitmapSource tileSetBitmap, Int32Rect renderRect, int tileNumber)
        {
            /* Asettaa kaikki tiilen tiedot kerralla ja lähettää viestin bindatulle kohteelle muuttuneista tiedoista INotifyPropertyChanged interfacen avulla */
            _tileSetBitmap = tileSetBitmap;
            _renderRect = renderRect;
            _tileSprite = new CroppedBitmap(_tileSetBitmap, _renderRect);
            _tileNumber = tileNumber;
            Notify("TileSetBitmap");
            Notify("RenderRect");
            Notify("TileSprite");
            Notify("TileNumber");
        }

        void Notify(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
