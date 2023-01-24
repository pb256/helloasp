using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    private static Dictionary<float, YieldInstruction> mYieldDic = new();

    public static YieldInstruction GetYieldSec(float sec)
    {
        if (!mYieldDic.ContainsKey(sec))
        {
            var wfs = new WaitForSeconds(sec);
            mYieldDic.Add(sec, wfs);
        }
        return mYieldDic[sec];
    }

    public static bool RandomBool()
    {
        return Random.Range(0, 2) == 0;
    }

    public static bool RandomChance(int chance)
    {
        return Random.Range(0, chance) == 0;
    }

    public static int Choose(params int[] args)
    {
        var randomIndex = Random.Range(0, args.Length);
        return args[randomIndex];
    }

    public static Vector2 GetVector(float deg)
    {
        Vector2 vec = Quaternion.Euler(0, 0, deg) * Vector3.right;

        return vec;
    }
}
