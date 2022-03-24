using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
    public class ChessBord : MonoBehaviour
    {
        [Header("Art")]
        [SerializeField] private Tile tileObject;
        [SerializeField] private float deathOffset = 0.6f;


        [Header("Pices")]
        [SerializeField] private ChessPiece[] chessPieceObjects;

        [Header("GUI")]
        [SerializeField] private GUIGameplay ui;


        // LOGIC
        private Tile[,] tiles;
        private const int bordCountX = 8;
        private const int bordCountY = 8;
        private const float size = 1;
        private Camera correntCamera;
        private Vector2Int howerTile;
        private ChessPiece[,] pieces;
        private Vector2Int selectedPiece = -Vector2Int.one;
        private List<ChessPiece> deathsTeam0 = new List<ChessPiece>();
        private List<ChessPiece> deathsTeam1 = new List<ChessPiece>();
        private List<Vector2Int> availableTile = new List<Vector2Int>();
        private bool team0Turn = true;
        private SpecialMove specialMove;
        private Vector2Int pawnForPromotion;
        private List<MoveDetail> moveDetails = new List<MoveDetail>();
        private bool pauseGame = false;


        private void Awake()
        {
            GenerateTiles();
            GenerateAllPieces();
            correntCamera = Camera.main;

        }

        private void Update()
        {
            if (pauseGame)
                return;

            RaycastHit hit;
            Ray ray = correntCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100, LayerMask.NameToLayer("Tile")))
            {
                Tile t = null;
                if (t = hit.transform.GetComponent<Tile>())
                {
                    if (t.type == TileType.normall
                        || t.type == TileType.highlight)
                    {
                        Hower(t);
                    }
                }
                else
                {
                    notHower();
                }

                //click
                if (Input.GetMouseButtonDown(0))
                {
                    if (howerTile != -Vector2Int.one && selectedPiece == -Vector2Int.one && pieces[howerTile.x, howerTile.y] != null)
                    {
                        //you are turn?
                        if ((pieces[howerTile.x, howerTile.y].team == 0 && team0Turn) || (pieces[howerTile.x, howerTile.y].team == 1 && !team0Turn))
                        {
                            //select piece
                            SelectPiece(howerTile);
                        }

                    }
                    if (selectedPiece != -Vector2Int.one && selectedPiece != howerTile)
                    {
                        //move piece
                        if (!MovePiece(selectedPiece, howerTile))
                            if (pieces[howerTile.x, howerTile.y] != null && ((pieces[howerTile.x, howerTile.y].team == 0 && team0Turn) || (pieces[howerTile.x, howerTile.y].team == 1 && !team0Turn)))
                            {
                                SelectPiece(howerTile);
                            }
                    }
                }
            }
            else
            {
                notHower();
            }

        }

        //generate tile
        private void GenerateTiles()
        {
            tiles = new Tile[bordCountX, bordCountY];
            for (int x = 0; x < bordCountX; x++)
                for (int y = 0; y < bordCountY; y++)
                {
                    //print(y.ToString() + "  " + x.ToString());
                    tiles[x, y] = GenerateTile(x, y, size);
                }
        }
        private Tile GenerateTile(int x, int y, float size)
        {
            var tile = Instantiate(tileObject, transform);
            tile.SetInfo(new Vector2Int(x, y), size);
            tile.gameObject.name = string.Format("x : {0} - y : {1}", x, y);
            return tile;
        }

        //hower
        private void Hower(Tile tile)
        {
            for (int x = 0; x < bordCountX; x++)
                for (int y = 0; y < bordCountY; y++)
                {
                    if (tiles[x, y] == tile)
                    {
                        notHower();
                        tiles[x, y].type = TileType.Hower;
                        howerTile = new Vector2Int(x, y);
                    }
                }

        }
        void notHower()
        {
            if (howerTile == -Vector2Int.one)
                return;
            tiles[howerTile.x, howerTile.y].type = CheckHighlight(howerTile) ? TileType.highlight : TileType.normall;
            howerTile = -Vector2Int.one;
        }

        //generate piece
        private void GenerateAllPieces()
        {
            pieces = new ChessPiece[bordCountX, bordCountY];
            //team 0
            GenerateSinglePieces(0, 0, 0, PieceType.Rook);
            GenerateSinglePieces(1, 0, 0, PieceType.Knight);
            GenerateSinglePieces(2, 0, 0, PieceType.Bishop);
            GenerateSinglePieces(3, 0, 0, PieceType.Queen);
            GenerateSinglePieces(4, 0, 0, PieceType.King);
            GenerateSinglePieces(5, 0, 0, PieceType.Bishop);
            GenerateSinglePieces(6, 0, 0, PieceType.Knight);
            GenerateSinglePieces(7, 0, 0, PieceType.Rook);
            for (int i = 0; i < bordCountX; i++)
                GenerateSinglePieces(i, 1, 0, PieceType.Pawn);

            //team 1
            GenerateSinglePieces(0, 7, 1, PieceType.Rook);
            GenerateSinglePieces(1, 7, 1, PieceType.Knight);
            GenerateSinglePieces(2, 7, 1, PieceType.Bishop);
            GenerateSinglePieces(3, 7, 1, PieceType.Queen);
            GenerateSinglePieces(4, 7, 1, PieceType.King);
            GenerateSinglePieces(5, 7, 1, PieceType.Bishop);
            GenerateSinglePieces(6, 7, 1, PieceType.Knight);
            GenerateSinglePieces(7, 7, 1, PieceType.Rook);
            for (int i = 0; i < bordCountX; i++)
                GenerateSinglePieces(i, 6, 1, PieceType.Pawn);
        }
        private void GenerateSinglePieces(int x, int y, int team, PieceType type)
        {
            ChessPiece piece = Instantiate(chessPieceObjects[(int)type], transform);
            piece.MoveTo(new Vector3(x * size, 0, y * size), true);
            piece.type = type;
            piece.team = team;
            pieces[x, y] = piece;
        }

        //selection
        private void SelectPiece(Vector2Int selectedPos)
        {
            selectedPiece = selectedPos;
            var _availableTiles = pieces[selectedPiece.x, selectedPiece.y].GetAvailableTiles(pieces, selectedPiece, bordCountX, bordCountY);
            HighlightTiles(PreventMate(_availableTiles,selectedPos));
            CheckSpecialMove(selectedPos);


        }
        private void UnSelectedPiece()
        {
            ClearHighlights();
            selectedPiece = -Vector2Int.one;
        }

        //special move
        private void CheckSpecialMove(Vector2Int selectedPos)
        {
            specialMove = pieces[selectedPos.x, selectedPos.y].GetSpecialMove(pieces, selectedPos, moveDetails);

            var list = new List<Vector2Int>();
            switch (specialMove)
            {
                case SpecialMove.None:
                    break;
                case SpecialMove.EnPassant:
                    var lastMove = moveDetails[moveDetails.Count - 1];
                    Vector2Int pos = new Vector2Int(lastMove.to.x, (lastMove.to.y + lastMove.from.y) / 2);
                    list.Add(pos);
                    break;
                case SpecialMove.Castling:

                    //right
                    if (pieces[selectedPos.x + 1, selectedPos.y] == null
                        && pieces[selectedPos.x + 2, selectedPos.y] == null
                        && pieces[7, selectedPos.y] != null && !pieces[7, selectedPos.y].EverMoved)
                    {
                        list.Add(new Vector2Int(selectedPos.x + 2, selectedPos.y));
                    }
                    //left
                    if (pieces[selectedPos.x - 1, selectedPos.y] == null
                        && pieces[selectedPos.x - 2, selectedPos.y] == null
                        && pieces[selectedPos.x - 3, selectedPos.y] == null
                        && pieces[0, selectedPos.y] != null && !pieces[0, selectedPos.y].EverMoved)
                    {
                        list.Add(new Vector2Int(selectedPos.x - 2, selectedPos.y));
                    }
                    break;
                case SpecialMove.PawnPromotion:
                    // none highlight
                    break;
                default:
                    break;
            }
            HighlightTiles(list);
        }

        //highlighting
        private void HighlightTiles(List<Vector2Int> availableTile)
        {
            this.availableTile.AddRange(availableTile);
            foreach (var piece in availableTile)
                tiles[piece.x, piece.y].type = TileType.highlight;
        }
        private void ClearHighlights()
        {
            foreach (var piece in availableTile)
                tiles[piece.x, piece.y].type = TileType.normall;
            availableTile.Clear();
        }
        private bool CheckHighlight(Vector2Int forCheck)
        {
            foreach (var tile in availableTile)
                if (tile.x == forCheck.x && tile.y == forCheck.y)
                    return true;
            return false;
        }

        //move
        private bool MovePiece(Vector2Int from, Vector2Int to)
        {
            var moveDetail = new MoveDetail(from, to);

            bool r = false;
            if (CheckHighlight(to))
            {
                //functions
                void deathPiece(ChessPiece piece, Vector2Int pos)
                {
                    if (piece.team == 1)
                    {
                        if (piece.type == PieceType.King)
                            Victory(0);
                        piece.MoveTo(new Vector3(-1 * size, 0, (bordCountY * size) - (deathsTeam1.Count * deathOffset)));
                        deathsTeam1.Add(piece);
                    }
                    else
                    {
                        if (piece.type == PieceType.King)
                            Victory(1);
                        piece.MoveTo(new Vector3(bordCountX * size, 0, -1 + (deathsTeam0.Count * deathOffset)));
                        deathsTeam0.Add(piece);
                    }
                    moveDetail.addDeath(piece, pos);
                    if (pieces[pos.x, pos.y] == piece)
                        pieces[pos.x, pos.y] = null;
                }



                ChessPiece fromPiece = pieces[from.x, from.y];
                ChessPiece toPiece = pieces[to.x, to.y];
                if (toPiece == null)
                {
                    fromPiece.MoveTo(new Vector3(to.x * size, 0, to.y * size));
                    pieces[to.x, to.y] = fromPiece;
                    pieces[from.x, from.y] = null;
                    r = true;
                }
                else
                {
                    if (fromPiece.team != toPiece.team)
                    {
                        deathPiece(toPiece, to);
                        fromPiece.MoveTo(new Vector3(to.x * size, 0, to.y * size));
                        pieces[to.x, to.y] = fromPiece;
                        pieces[from.x, from.y] = null;

                        r = true;
                    }
                }



                if (r)
                {
                    //special move
                    switch (specialMove)
                    {
                        case SpecialMove.None:
                            break;
                        case SpecialMove.EnPassant:
                            var lastMove = moveDetails[moveDetails.Count - 1];
                            Vector2Int pos = new Vector2Int(lastMove.to.x, (lastMove.to.y + lastMove.from.y) / 2);
                            if (to == pos)
                                deathPiece(pieces[to.x, from.y], new Vector2Int(to.x, from.y));
                            break;
                        case SpecialMove.Castling:
                            //right
                            if ((to.x - from.x) == 2)
                            {
                                pieces[7, to.y].MoveTo(new Vector3(5 * size, 0, to.y));
                            }
                            if ((to.x - from.x) == -2)
                            {
                                pieces[0, to.y].MoveTo(new Vector3(3 * size, 0, to.y));
                            }
                            //left
                            break;
                        case SpecialMove.PawnPromotion:
                            pauseGame = true;
                            ui.ShowPawnPromotionMenu();
                            pawnForPromotion = to;
                            break;
                        default:
                            break;
                    }

                    team0Turn = !team0Turn;
                    fromPiece.EverMoved = true;
                    //keep detail
                    moveDetails.Add(moveDetail);

                    if (CheckMate(fromPiece.team))
                        Victory(fromPiece.team);
                }
            }
            UnSelectedPiece();




            return r;
        }

        //promotion
        public void PromotionTo(int type)
        {
            if (pawnForPromotion == -Vector2Int.one)
                return;
            pauseGame = false;
            ChessPiece piece = pieces[pawnForPromotion.x, pawnForPromotion.y];
            GenerateSinglePieces(pawnForPromotion.x, pawnForPromotion.y, piece.team, (PieceType)type);
            piece.gameObject.SetActive(false);

            pawnForPromotion = -Vector2Int.one;
        }

        //Mate
        private List<Vector2Int> PreventMate(List<Vector2Int> available, Vector2Int from)
        {
            List<Vector2Int> output = new List<Vector2Int>();
            output.AddRange(available);

            //find king
            Vector2Int kingPos = -Vector2Int.one;
            for (int x = 0; x < bordCountX; x++)
                for (int y = 0; y < bordCountY; y++)
                    if (pieces[x, y] != null && pieces[x, y].type == PieceType.King && pieces[x, y].team == pieces[from.x, from.y].team)
                        kingPos = new Vector2Int(x, y);



            //check avalable move
            foreach (var item in available) {

                //clon pieces list
                ChessPiece[,] _pieces = new ChessPiece[bordCountX,bordCountY];
                for (int x = 0; x < bordCountX; x++)
                    for (int y = 0; y < bordCountY; y++)
                        _pieces[x, y] = pieces[x, y];

                _pieces[item.x, item.y] = _pieces[from.x, from.y];
                _pieces[from.x, from.y] = null;

                //update king pos
                if (_pieces[item.x, item.y].type == PieceType.King)
                    kingPos = item;

                //ckeck all pieces moves
                for (int x = 0; x < bordCountX; x++)
                    for (int y = 0; y < bordCountY; y++)
                    {
                        if (_pieces[x, y] != null && _pieces[x, y].team != _pieces[item.x, item.y].team)
                        {
                            foreach (Vector2Int v in _pieces[x, y].GetAvailableTiles(_pieces, new Vector2Int(x, y), bordCountX, bordCountY))
                            {
                                if (kingPos == v) 
                                    output.Remove(item);
                                continue;
                            }
                        }
                    }
            }

            return output;
        }
        private bool CheckMate(int movedTeam)
        {
            for (int x = 0; x < bordCountX; x++)
                for (int y = 0; y < bordCountY; y++)
                    if(pieces[x,y] != null && movedTeam != pieces[x, y].team)
                        if (PreventMate(pieces[x, y].GetAvailableTiles(pieces, new Vector2Int(x, y), bordCountX, bordCountY),new Vector2Int(x,y)).Count > 0)
                            return false;

            return true;
        }
        //end game
        private void Victory(int team)
        {
            string massage = "";

            if (team == 0)
                massage = "Victory whait";
            else
                massage = "Victory black";

            ui.EndGame(massage);
        }
    }

    public enum SpecialMove
    {
        None = 0,
        EnPassant = 1,
        Castling = 2,
        PawnPromotion = 3
    }
    public class MoveDetail
    {
        public MoveDetail(Vector2Int from, Vector2Int to)
        {
            this.from = from;
            this.to = to;
        }
        public void addDeath(ChessPiece deathed, Vector2Int deathPos)
        {
            this.deathedPiece = deathed;
            this.deathPos = deathPos;
        }
        public Vector2Int from;
        public Vector2Int to;
        public ChessPiece deathedPiece;
        public Vector2Int deathPos;
    }
}