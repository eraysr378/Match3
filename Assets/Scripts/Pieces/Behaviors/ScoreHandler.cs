using Managers;
using UnityEngine;

namespace Pieces.Behaviors
{
    public class ScoreHandler : MonoBehaviour
    {
        public void ReportScore(Piece piece)
        {
            EventManager.OnPieceScored.Invoke(piece);
        }
    }
}