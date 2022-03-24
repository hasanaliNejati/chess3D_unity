using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Knight : ChessPiece
    {

        internal override List<Vector2Int> GetAvailableTiles(ChessPiece[,] pieces, Vector2Int thisPos, int bordCountX, int bordCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            void getTile(int x, int y)
            {

                Vector2Int v = thisPos + new Vector2Int(x, y);
                if (CheckBordRange(v, bordCountX, bordCountY))
                {
                    if (pieces[v.x, v.y] == null)
                    {
                        r.Add(v);
                    }
                    else
                    {
                        if (pieces[v.x, v.y].team != team)
                            r.Add(v);
                    }
                }
            }

            // +2+1
            getTile(2, 1);
            // +2-1
            getTile(2, -1);
            // -2+1
            getTile(-2, 1);
            // -2-1
            getTile(-2, -1);
            // +1+2
            getTile(1, 2);
            // -1+2
            getTile(-1, 2);
            // +1-2
            getTile(1, -2);
            // -1-2
            getTile(-1, -2);

            return r;
        }
    }
}