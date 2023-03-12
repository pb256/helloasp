using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private GameObject goStartButton;
    [SerializeField] private InputField inputField;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        var validId = inputField.text.Length > 0;
        goStartButton.SetActive(validId);
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
