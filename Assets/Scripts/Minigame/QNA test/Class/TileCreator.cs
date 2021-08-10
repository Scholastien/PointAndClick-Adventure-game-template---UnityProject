using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_QnA_Test
{
    [System.Serializable]
    public class Tile
    {
        public Vector2Int coord;

        //  filler, outline
        Vector2 value;

        public Tile(Vector2Int _coord)
        {
            coord = _coord;
            

            value = new Vector2(0, 0);


        }

        public void SetTextureSize(Texture2D texture, int sizeX, int sizeY)
        {
            texture = new Texture2D(sizeX, sizeY);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply();
        }
    }

    public static class TileCreator
    {
        public static Tile[,] GetTiles( int blocksPerLine, int blocksPerColumn)
        {


            Tile[,] tiles = new Tile[blocksPerLine, blocksPerColumn];
            for (int y = 0; y < blocksPerColumn; y++)
            {
                for (int x = 0; x < blocksPerLine; x++)
                {
                    Vector2Int coord = new Vector2Int(x, y);

                    tiles[x,y] = new Tile(coord);
                }
            }



            return tiles;
        }
    }
}