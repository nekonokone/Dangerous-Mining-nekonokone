using UnityEngine;

public class GameResultManager : MonoBehaviour
{
    public static bool timeOver = false;
    public static int collectedNorms = 0;
    public static bool isDead = false;

    public static int normTarget = 6;
    public static int totalNorms = 7;

    public enum ResultRank { F, B, A, S }

    public static bool IsClear()
    {
        return !timeOver && !isDead && collectedNorms >= normTarget;
    }
    public static ResultRank GetRank()
    {
        if (isDead || timeOver) return ResultRank.F;
        if (collectedNorms < normTarget) return ResultRank.B;

        if (collectedNorms >= totalNorms) return ResultRank.S;
        if (collectedNorms == normTarget) return ResultRank.A;
        return ResultRank.A;                                  
    }

    public static void Reset(int? normTargetOverride = null, int? totalNormsOverride = null)
    {
        timeOver = false;
        isDead = false;
        collectedNorms = 0;

        if (normTargetOverride.HasValue) normTarget = normTargetOverride.Value;
        if (totalNormsOverride.HasValue) totalNorms = totalNormsOverride.Value;
    }
}