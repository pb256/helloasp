using UnityEngine;

public class WaveTest : WaveBase
{
    protected override void OnWaveStart() => 
        mWaveScale = Vector3.one;
}
