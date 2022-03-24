using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Bishop : ChessPiece
    {

        internal override List<Vector2Int> GetAvailableTiles(ChessPiece[,] pieces, Vector2Int thisPos, int bordCountX, int bordCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            void getLine(int x, int y)
            {
                for (int i = 1; i < bordCountX; i++)
                {
                    Vector2Int v = thisPos + new Vector2Int(i * x, i * y);
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
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // +1+1
            getLine(1, 1);
            // +1-1
            getLine(1, -1);
            // -1+1
            getLine(-1, 1);
            // -1-1
            getLine(-1, -1);

            return r;
        }

    }
}