using System;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public enum TileType
    {
        normall, Hower , highlight
    }
    public class Tile : MonoBehaviour
    {

        public MeshRenderer renderer;

        private TileType _type;
        internal TileType type
        {
            get { return _type; }
            set
            {
                switch (value)
                {
                    case TileType.normall:
                        renderer.material.color = new Color();
                        break;
                    case TileType.Hower:
                        renderer.material.color = Color.red;
                        break;
                    case TileType.highlight:
                        renderer.material.color = Color.blue;
                        break;
                    default:
                        break;
                }
                _type = value;
            }
        }

        public void SetInfo(Vector2Int index, float size)
        {
            transform.localPosition = new Vector3(index.x * size, 0, index.y * size);
        }

    }
}
