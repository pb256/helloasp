using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AnimExplosion : ExplosionBase
{
    public SfxType sfx = SfxType.Explosion1;
    public float shakePower = 1f;
    public string animName = "Explode";

    protected Animator anim;
    protected float animationLength;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    protected IEnumerator CoCheckDestroy()
    {
        yield return Util.GetYieldSec(animationLength);

        while (!IsWaveAnimationEnd)
        {
            yield return null;
        }

        ReturnThis();
    }

    protected virtual void ReturnThis() { }

    public void Explode()
    {
        SoundManager.instance.PlaySound(sfx);

        Camera.main.GetComponent<CameraController>().Shake(shakePower);

        anim.Play(animName);

        var info = anim.GetCurrentAnimatorStateInfo(0);
        animationLength = info.length;
        StartCoroutine(CoCheckDestroy());

        wave.Init();
    }
}
