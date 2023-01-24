using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    public enum CreatePositionType
    {
        Random,
        RandomFromCenter,
        RandomFromPlayer,
        Spiral,
        SpiralInverse,
        CircleFromCenter,
        CircleFromPlayer,
        NextPlayerPosition,
    }

    [System.Serializable]
    public struct EnemyWaveData
    {
        public EnemyType enemy;
        public int count;
        [Space(5)]
        public CreatePositionType posType;
        public float range;
        [Space(5)]
        public float duration;
        public float delay;
    }

    public EnemyWaveData[] waves;
}
