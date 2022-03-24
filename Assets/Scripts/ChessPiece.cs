using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    [System.Serializable]
    public enum PieceType
    {
        Pawn = 0,
        Bishop = 1,
        Rook = 2,
        Knight = 3,
        Queen = 4,
        King = 5

    }
    public class ChessPiece : MonoBehaviour
    {
        public Material team0Material;
        public Material team1Material;

        private Vector3 localPosition;
        internal PieceType type;
        private int _team;
        internal int team
        {
            get { return _team; }
            set
            {
                GetComponent<MeshRenderer>().material = value == 0 ? team0Material : team1Material;
                if (value == 1)
                    transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
                _team = value;
            }
        }

        internal bool EverMoved;

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, localPosition, Time.deltaTime * 15);
        }

        internal virtual List<Vector2Int> GetAvailableTiles(ChessPiece[,] pieces,Vector2Int thisPos,int bordCountX,int bordCountY)
        {
            List<Vector2Int> r = new List<Vector2Int>();

            r.Add(new Vector2Int(thisPos.x,thisPos.y + 1));
            r.Add(new Vector2Int(thisPos.x,thisPos.y + 2));

            return r;
        }

        internal virtual SpecialMove GetSpecialMove(ChessPiece[,] pieces,Vector2Int thisPos,List<MoveDetail> moveDetails)
        {
            return SpecialMove.None;
        }

        protected virtual bool CheckBordRange(Vector2Int forCheck,int bordCountX,int bordCountY)
        {
            if (forCheck.x >= bordCountX || forCheck.x < 0 || forCheck.y >= bordCountY || forCheck.y < 0)
                return false;
            return true;
        }

        internal void MoveTo(Vector3 pos, bool force = false)
        {
            if (force)
                transform.localPosition = pos;
            localPosition = pos;
        }
    }
}