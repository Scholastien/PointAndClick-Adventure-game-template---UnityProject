using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame_QnA_Test
{
    public class Block : MonoBehaviour
    {
        public Vector2Int coord;
        public Vector2Int startingCoord;
        public Vector2Int value;
        public Tile tile;

        public void Init(Vector2Int startingCoord, Tile _tile, Texture2D filling, Texture2D outline, Shader shader, List<Materials> mats)
        {
            this.startingCoord = startingCoord;
            this.tile = _tile;
            coord = startingCoord;

            MeshRenderer mesh = GetComponent<MeshRenderer>();

            Material material = GetMaterialAndValue(mats);
            //GetComponent<MeshRenderer>().material = Resources.Load<Material>("Block"); // Create a block mat
            SetMaterial(mesh, filling, material);
        }

        private void SetMaterial(MeshRenderer mesh, Texture2D texture, Material material)
        {
            mesh.material = material;
            //mesh.material.shader = shader;
            mesh.material.mainTexture = texture;
        }

        private Material GetMaterialAndValue(List<Materials> mats)
        {
            int x = (int)Random.Range(0, mats.Count);
            int y = (int)Random.Range(0, mats[x].material.Count);

            value = new Vector2Int(x, y);

            return mats[x].material[y];
        }

        public bool CompareBlock(Block block)
        {
            if (value == block.value)
                return true;
            else
                return false;
        }
    }
}