namespace Misc
{
    public enum CellType
    {
        Default,
        Red,
        None,
    }
    public enum PieceType
    {
        None,

        ObstaclePiece,
        BombPiece,
        RocketPiece,
        RainbowPiece,
        WoodBox,

        
        
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
        Stars,
        
        
    }
    public enum SwapReactionPriority
    {
        NormalPiece = 10,
        ActivatablePiece = 5,
        PassivePiece = 0
    }
    public enum PieceOperation
    {
        None,
        Swapping,
        Filling,
        Falling,
        Activating,
        WaitingForActivation
        
    }
    public enum GoalType
    {
        Square,
        Circle,
        Triangle,
        Hexagon,
        WoodBox,
        Glass
    }

    public enum CellOverlayType
    {
        None,
        Glass
    }

    public enum MatchShape
    {
        Line,
        T,
        L
    }
}