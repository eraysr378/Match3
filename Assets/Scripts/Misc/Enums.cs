namespace Misc
{
    public enum CellType
    {
        SquareNormalCell = NormalCellType.Square,
        CircleNormalCell =  NormalCellType.Circle,
        TriangleNormalCell =  NormalCellType.Triangle,
        ObstacleCell,
        ActivatableCell,
    }

    public enum NormalCellType
    {
        Square = 100,
        Circle = 101,
        Triangle = 102,
    }
}