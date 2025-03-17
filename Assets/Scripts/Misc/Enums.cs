namespace Misc
{
    public enum CellType
    {
        Default,
    }
    public enum PieceType
    {
        SquareNormalPiece = NormalPieceType.Square,
        CircleNormalPiece =  NormalPieceType.Circle,
        TriangleNormalPiece =  NormalPieceType.Triangle,
        ObstaclePiece,
        ActivatablePiece,
    }

    public enum NormalPieceType
    {
        Square = 100,
        Circle = 101,
        Triangle = 102,
    }
}