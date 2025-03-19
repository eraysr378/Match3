namespace Misc
{
    public enum CellType
    {
        Default,
    }
    public enum PieceType
    {
        None,
        ObstaclePiece,
        ActivatablePiece,
        SquareNormalPiece = NormalPieceType.Square,
        CircleNormalPiece =  NormalPieceType.Circle,
        TriangleNormalPiece =  NormalPieceType.Triangle,
    }

    public enum NormalPieceType
    {
        Square = 100,
        Circle = 101,
        Triangle = 102,
    }
}