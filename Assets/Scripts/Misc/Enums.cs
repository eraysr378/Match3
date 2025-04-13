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
        BombPiece,
        RocketPiece,
        RainbowPiece,


        
        
        SquareNormalPiece = NormalPieceType.Square,
        CircleNormalPiece =  NormalPieceType.Circle,
        TriangleNormalPiece =  NormalPieceType.Triangle,
        HexagonNormalPiece = NormalPieceType.Hexagon

    }

    public enum NormalPieceType
    {
        Square = 100,
        Circle = 101,
        Triangle = 102,
        Hexagon = 103,
    }

    public enum CombinationType
    {
        RocketBombCombination,
        BombBombCombination,
        RocketRocketCombination,
        RainbowRocketCombination,
        RainbowBombCombination,
        RainbowRainbowCombination,
        
    }
    public enum ParticleType
    {
        None,
        Activation,
        Explosion,
        SquareExplosion,
        TriangleExplosion,
        CircleExplosion,
        BombExplosion,
        BombBombExplosion,
        HexagonExplosion,
        
        
    }

}