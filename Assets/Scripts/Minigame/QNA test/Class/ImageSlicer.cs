using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_QnA_Test
{
    // Slice a texture into multiple blocks
    public static class ImageSlicer
    {
        public static Texture2D[,] GetSlices(Texture2D image, int blocksPerLine, int blocksPerColumn)
        {
            //int imageSize = Mathf.Min(image.width, image.height); // get the image size
            int blockSizeX = image.width / blocksPerLine; // block is a fraction of that size
            int blockSizeY = image.height / blocksPerColumn; // block is a fraction of that size

            Debug.Log("Block size X : " + blockSizeX);
            Debug.Log("Block size Y : " + blockSizeY);

            Texture2D[,] blocks = new Texture2D[blocksPerLine, blocksPerColumn];  // 2D array with the dimension of the board

            for (int y = 0; y < blocksPerColumn; y++)
            {
                for (int x = 0; x < blocksPerLine; x++)
                {
                    Texture2D block = new Texture2D(blockSizeX, blockSizeY); // create a new texture with the size of a block

                    block.wrapMode = TextureWrapMode.Clamp; // Clamp texture, so it doesn't rescale like a moron

                    block.SetPixels(image.GetPixels(x * blockSizeX, y * blockSizeY, blockSizeX, blockSizeY)); // Move the texture to the coord of the tile

                    block.Apply();

                    blocks[x, y] = block;

                }
            }

            return blocks;
        }
    }
}