using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Singleton<Maestro> {

    public AudioClip[] allClips = new AudioClip[0];
    
    public AudioSource oneShotSource;

    public float crossfadeTimer = 1f;
    public AudioSource[] crossfadeSources = new AudioSource[0];
    private int _index = 0;

    private AudioSource NextAvailableSource
    {
        get
        {
            int nextIndex = (_index + 1) % crossfadeSources.Length;
            return crossfadeSources[nextIndex];
        }
    }

    private void Awake()
    {
    }

    public void PlaySound(AudioClip _clip)
    {
        NextAvailableSource.clip = _clip;
        StartCoroutine(Crossfade_Routine());
        BumpIndex();
    }

    public void PlayClipOnce(AudioClip _clip)
    {
        oneShotSource.PlayOneShot(_clip);
    }

    private void BumpIndex()
    {
        _index = (_index + 1) % crossfadeSources.Length;
    }

    private IEnumerator Crossfade_Routine()
    {
        float start = Time.time;
        AudioSource currentSource = crossfadeSources[_index];
        AudioSource nextSource = NextAvailableSource;

        currentSource.volume = 1;
        nextSource.volume = 0;

        if(currentSource.clip != null && !currentSource.isPlaying)
        {
            currentSource.Play();
        }

        if(nextSource.clip != null && !nextSource.isPlaying)
        {
            nextSource.Play();
        }

        while (Time.time - start < crossfadeTimer)
        {
            float duration = Time.time - start;
            currentSource.volume = Mathf.Lerp(1, 0, duration / crossfadeTimer);
            nextSource.volume = Mathf.Lerp(0, 1, duration / crossfadeTimer);

            yield return null;
        }

        currentSource.Stop();
    }

    public void PlayRandomSound()
    {
        PlaySound(allClips[UnityEngine.Random.Range(0, allClips.Length)]);
    }
}
