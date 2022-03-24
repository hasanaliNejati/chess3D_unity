using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class Pawn : ChessPiece
    {
        internal override List<Vector2Int> GetAvailableTiles(ChessPiece[,] pieces, Vector2Int thisPos, int bordCountX, int bordCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            int direction = team == 0 ? 1 : -1;

            //move forward
            Vector2Int forward = thisPos + new Vector2Int(0, direction);
            if (CheckBordRange(forward, bordCountX, bordCountY) && pieces[forward.x, forward.y] == null)
            {
                r.Add(forward);

                //first move 
                //team 0
                if ((thisPos.y == 1 && team == 0))
                {
                    Vector2Int v = thisPos + new Vector2Int(0, (direction * 2));
                    if (pieces[v.x, v.y] == null)
                    {
                        r.Add(v);
                    }
                }
                //team 1
                if ((thisPos.y == 6 && team == 1))
                {
                    Vector2Int v = thisPos + new Vector2Int(0, (direction * 2));
                    if (pieces[v.x, v.y] == null)
                    {
                        r.Add(v);
                    }
                }
            }


            //kill
            //right
            Vector2Int right = thisPos + new Vector2Int(1, direction);
            if (CheckBordRange(right, bordCountX, bordCountY) && pieces[right.x, right.y] != null && pieces[right.x, right.y].team != team)
                r.Add(right);
            //left
            Vector2Int left = thisPos + new Vector2Int(-1, direction);
            if (CheckBordRange(left, bordCountX, bordCountY) && pieces[left.x, left.y] != null && pieces[left.x, left.y].team != team)
                r.Add(left);


            return r;
        }

        internal override SpecialMove GetSpecialMove(ChessPiece[,] pieces, Vector2Int thisPos, List<MoveDetail> moveDetails)
        {
            //first move
            if (moveDetails.Count == 0)
                return SpecialMove.None;

            var lastMove = moveDetails[moveDetails.Count - 1];
            var lastMovePiece = pieces[lastMove.to.x, lastMove.to.y];
            if(lastMovePiece.type == PieceType.Pawn)
            {
                if(Mathf.Abs(lastMove.to.y - lastMove.from.y) == 2)
                {
                    if(Mathf.Abs(lastMove.to.x - thisPos.x) == 1)
                    {
                        if(thisPos.y == lastMove.to.y)
                        return SpecialMove.EnPassant;
                    }
                }
            }

            if ((team == 0 && thisPos.y == 6) || (team == 1 && thisPos.y == 1))
                return SpecialMove.PawnPromotion;

            return SpecialMove.None;
        }
    }
}