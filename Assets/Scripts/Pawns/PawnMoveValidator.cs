﻿using UnityEngine;

public class PawnMoveValidator : MonoBehaviour
{
    private TileGetter tileGetter;
    private GameObject pawn;
    private GameObject targetTile;
    private GameObject pawnToCapture;
    private TileIndex targetTileIndex;
    private TileIndex currentTileIndex;
    private TileIndex positionDifferenceInIndex;

    private void Awake()
    {
        tileGetter = GetComponent<TileGetter>();
    }

    public bool IsValidMove(GameObject pawnToCheck, GameObject targetTileToCheck)
    {
        SetValues(pawnToCheck, targetTileToCheck);
        if (!IsMoveDiagonal() || IsTileOccupied(targetTileIndex))
            return false;
        if (!IsPawnKing())
            return positionDifferenceInIndex.Row == GetPawnRowMoveDirection();
        else
            return IsPathCollidingWithOtherPawns();
    }

    private void SetValues(GameObject pawnToCheck, GameObject targetTileToCheck)
    {
        pawn = pawnToCheck;
        targetTile = targetTileToCheck;
        targetTileIndex = targetTile.GetComponent<TileProperties>().GetTileIndex();
        currentTileIndex = pawn.GetComponent<IPawnProperties>().GetTileIndex();
        positionDifferenceInIndex = targetTileIndex - currentTileIndex;
    }

    private bool IsMoveDiagonal()
    {
        return Mathf.Abs(positionDifferenceInIndex.Column) == Mathf.Abs(positionDifferenceInIndex.Row);
    }

    private bool IsPawnKing()
    {
        return pawn.GetComponent<IPawnProperties>().IsKing;
    }

    private int GetPawnRowMoveDirection()
    {
        var pawnProperties = pawn.GetComponent<IPawnProperties>();
        return pawnProperties.PawnColor == PawnColor.White ? 1 : -1;
    }

    private bool IsPathCollidingWithOtherPawns()
    {
        var moveDirectionInIndex = GetDiagonalMoveDirectionInIndex();
        for (var checkedTileIndex = currentTileIndex + moveDirectionInIndex;
            checkedTileIndex != targetTileIndex;
            checkedTileIndex += moveDirectionInIndex)
            if (IsTileOccupied(checkedTileIndex))
                return false;

        return true;
    }

    private TileIndex GetDiagonalMoveDirectionInIndex()
    {
        //Move direction means TileIndex with both values equal to +-1.
        return new TileIndex(positionDifferenceInIndex.Column / Mathf.Abs(positionDifferenceInIndex.Column),
            positionDifferenceInIndex.Row / Mathf.Abs(positionDifferenceInIndex.Row));
    }

    private bool IsTileOccupied(TileIndex tileIndex)
    {
        return tileGetter.GetTile(tileIndex).GetComponent<TileProperties>().IsOccupied();
    }

    public bool IsCapturingMove(GameObject pawnToCheck, GameObject targetTileToCheck)
    {
        SetValues(pawnToCheck, targetTileToCheck);
        if (!IsMoveDiagonal() || IsTileOccupied(targetTileIndex))
            return false;
        return IsCapturePositionChangeValid() && IsOpponentsPawnOnOneBeforeTargetTile();
    }

    private bool IsCapturePositionChangeValid()
    {
        return (!IsPawnKing() && Mathf.Abs(positionDifferenceInIndex.Row) == 2) ||
               (IsPawnKing() && Mathf.Abs(positionDifferenceInIndex.Row) >= 2);
    }

    private bool IsOpponentsPawnOnOneBeforeTargetTile()
    {
        var pawnBeforeTargetTile = PawnBeforeTargetTile();
        if (pawnBeforeTargetTile == 0)
            return false;
        var potentialPawnToCapture = GetPotentialPawnToCapture(pawnBeforeTargetTile);
        if (!IsPawnDifferentColorThanLastClickedPawn(potentialPawnToCapture))
            return false;
        pawnToCapture = potentialPawnToCapture;
        return true;
    }

    private int PawnBeforeTargetTile()
    {
        
        var moveDirectionInIndex = GetDiagonalMoveDirectionInIndex();
        if (!IsPawnKing())
        {
            for (var checkedTileIndex = currentTileIndex + moveDirectionInIndex;
            checkedTileIndex != targetTileIndex;
            checkedTileIndex += moveDirectionInIndex)
                if (IsTileOccupied(checkedTileIndex) && checkedTileIndex != targetTileIndex - moveDirectionInIndex)
                    return 0;

            return IsTileOccupied(targetTileIndex - moveDirectionInIndex) ? 1 : 0;
        }
        else
        {
            if (IsTileOccupied(targetTileIndex))
            {
                return 0;
            }

            var occupiedCount = 0;
            var pawnBefore = 0;
            for (var checkedTileIndex = currentTileIndex + moveDirectionInIndex;
                    checkedTileIndex != targetTileIndex;
                    checkedTileIndex += moveDirectionInIndex)
            {
                pawnBefore += occupiedCount == 1 ? 1 : 0;
                if (IsTileOccupied(checkedTileIndex))
                    occupiedCount++;
            }

            if (occupiedCount == 1)
            {
                return pawnBefore + 1;
            }

            return 0;
        }
    }

    private GameObject GetPotentialPawnToCapture(int pawnBeforeTargetTile)
    {
        var moveDirectionInIndex = GetDiagonalMoveDirectionInIndex();
        var potentialTileIndex = targetTileIndex;
        for (var i = 0; i < pawnBeforeTargetTile; i++)
            potentialTileIndex -= moveDirectionInIndex;

        return tileGetter.GetTile(potentialTileIndex).GetComponent<TileProperties>().GetPawn();
    }

    private bool IsPawnDifferentColorThanLastClickedPawn(GameObject pawnToCheck)
    {
        return pawnToCheck.GetComponent<IPawnProperties>().PawnColor !=
               pawn.GetComponent<IPawnProperties>().PawnColor;
    }

    public GameObject GetPawnToCapture()
    {
        return pawnToCapture;
    }
}