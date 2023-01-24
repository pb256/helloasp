using UnityEngine;

public class IntroController : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        SoundManager.instance.SetSnapshotTo(AudioMixerSnapshotType.Highpass, 0);
        SoundManager.instance.PlaySound(BgmType.MainBGM);
    }

    public void OnClickGameStartButton()
    {
        anim.SetTrigger("gameStart");
        SoundManager.instance.PlaySound(SfxType.Start);
    }

    public void OnEndGameStartAnimation()
    {
        GameManager.instance.SceneGoto("InGame");
    }
}
