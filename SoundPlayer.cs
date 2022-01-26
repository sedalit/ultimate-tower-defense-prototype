using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundPlayer: SingletoneBase<SoundPlayer>
{
    [SerializeField] private Sounds m_Sounds;
    [SerializeField] private AudioClip m_DefaultMusic;
    private AudioSource m_AudioSource;
    private new void Awake()
    {
        base.Awake();
        m_AudioSource = GetComponent<AudioSource>();
        Instance.m_AudioSource.clip = m_DefaultMusic;
        Instance.m_AudioSource.Play();
    }

    public void Play(Sound sound)
    {
        m_AudioSource.PlayOneShot(m_Sounds[sound]);
    }
}