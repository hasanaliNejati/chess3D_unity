using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class King : ChessPiece
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

            // x+1
            getTile(1, 0);
            // x-1
            getTile(-1, 0);
            // y+1
            getTile(0, 1);
            // y-1
            getTile(0, -1);

            // +1+1
            getTile(1, 1);
            // +1-1
            getTile(1, -1);
            // -1+1
            getTile(-1, 1);
            // -1-1
            getTile(-1, -1);

            return r;
        }

        internal override SpecialMove GetSpecialMove(ChessPiece[,] pieces, Vector2Int thisPos, List<MoveDetail> moveDetails)
        {
            if (!EverMoved)
            {
                //right
                if (pieces[thisPos.x + 1, thisPos.y] == null
                    && pieces[thisPos.x + 2, thisPos.y] == null
                    && pieces[7,thisPos.y] != null && !pieces[7 ,thisPos.y].EverMoved)
                {
                    return SpecialMove.Castling;
                }
                //left
                if (pieces[thisPos.x - 1, thisPos.y] == null
                    && pieces[thisPos.x - 2, thisPos.y] == null
                    && pieces[thisPos.x - 3, thisPos.y] == null
                    && pieces[0, thisPos.y] != null && !pieces[0, thisPos.y].EverMoved)
                {
                    return SpecialMove.Castling;
                }
            }

            return SpecialMove.None;
        }
    }
}