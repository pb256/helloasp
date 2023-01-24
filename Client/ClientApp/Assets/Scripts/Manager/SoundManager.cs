using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;
using DG.Tweening;

public sealed class SoundManager : Singleton<SoundManager>
{
    private class Sfx : IEquatable<Sfx>
    {
        public SfxType type;
        public float volume;

        public Sfx(SfxType type, float volume)
        {
            this.type = type;
            this.volume = volume;
        }
        public bool Equals(Sfx other)
        {
            return type == other.type;
        }
    }

    public AudioMixer mixer;
    public AudioMixerSnapshot[] snapshotList;

    public AudioClip[] sfxList, bgmList;

    private AudioSource audioSfx, audioBgm;

    private Queue<Sfx> sfxQ = new();

    protected override void Awake()
    {
        base.Awake();

        audioSfx = gameObject.AddComponent<AudioSource>();
        audioBgm = gameObject.AddComponent<AudioSource>();

        audioSfx.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioBgm.outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];

        sfxQ.Clear();
    }

    private void Start()
    {
        audioSfx.Stop();
    }

    public void PlaySound(SfxType snd, float volume = 1f)
    {
        Sfx sfx = new Sfx(snd, volume);
        if (!sfxQ.Contains(sfx))
        {
            sfxQ.Enqueue(new Sfx(snd, volume));
        }
    }

    public void PlaySound(BgmType snd, float volume = 1f)
    {
        if (audioBgm.isPlaying && audioBgm.clip == bgmList[(int)snd])
        {
            return;
        }

        audioBgm.Stop();
        audioBgm.clip = bgmList[(int)snd];
        audioBgm.volume = volume;
        audioBgm.loop = true;
        audioBgm.Play();
    }

    public void SetSnapshotTo(AudioMixerSnapshotType type, float duration = 0.6f)
    {
        snapshotList[(int)type].TransitionTo(duration);
    }

    public void PauseAll()
    {
        audioSfx.Pause();
        audioBgm.Pause();
    }

    public void ResumeAll()
    {
        audioSfx.UnPause();
        audioBgm.UnPause();
    }

    private void LateUpdate()
    {
        while (sfxQ.Count > 0)
        {
            Sfx sfx = sfxQ.Dequeue();
            audioSfx.PlayOneShot(sfxList[(int)sfx.type], sfx.volume);
        }
    }

    public void SetBgmPitchTo(float pitch, float duration = 0.5f)
    {
        audioBgm.DOKill();
        audioBgm.DOPitch(pitch, duration).SetEase(Ease.Linear);
    }
}
