using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public static AudioManager instance;

    private int bgmIndex;

    private void Awake() => instance = this;

    private void Update()
    {
        if (!bgm[bgmIndex].isPlaying)
            PlayRandomBGM();
    }

    public void PlaySFX(int index)
    {
        sfx[index].pitch = Random.Range(0.85f, 1.15f);
        sfx[index].Play();
    }

    private void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length); 
        PlayBGM(bgmIndex);
    }

    public void StopBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }
    private void PlayBGM(int index)
    {
        StopBGM();
        bgm[index].Play();
    }


}
