using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Singleton<Maestro> {

    public AudioClip[] allClips = new AudioClip[0];
    
    public AudioSource oneShotSource;

    public GameObject mumblePrefab;

    private AudioClip[] _mumbles;
    private string[] _mumbleNames;

    private int _index = 0;
    public float crossfadeTimer = 1f;
    public AudioSource[] crossfadeSources = new AudioSource[0];

    private AudioSource NextAvailableSource
    {
        get
        {
            int nextIndex = (_index + 1) % crossfadeSources.Length;
            return crossfadeSources[nextIndex];
        }
    }

    public bool IsCrossfadePlaying
    {
        get
        {
            foreach(var s in crossfadeSources)
            {
                if (s.isPlaying)
                {
                    return true;
                }
            }

            return false;
        }
    }

    private void Awake()
    {
        _mumbles = Resources.LoadAll<AudioClip>("SFX/mumbles");
        _mumbleNames = new string[_mumbles.Length];

        for (int i = 0; i < _mumbles.Length; i++)
        {
            _mumbleNames[i] = _mumbles[i].name;
        }
    }

    private void Start()
    {
        PopulateMumbles();
    }

    private void Update()
    {
        Vector3 newPos;
        if(CameraManager.Instance.CameraToTheatreRaycast(out newPos))
        {
            transform.position = newPos;
        }
    }

    public void PlaySound(AudioClip _clip, bool loop = true)
    {
        NextAvailableSource.clip = _clip;
        NextAvailableSource.loop = loop;
        StartCoroutine(Crossfade_Routine(loop));
        BumpIndex();
    }

    public void PlayClipOnce(AudioClip _clip, bool prioritize = false)
    {
        if(!oneShotSource.isPlaying || prioritize)
        {
            oneShotSource.PlayOneShot(_clip);
        }
    }

    private void BumpIndex()
    {
        _index = (_index + 1) % crossfadeSources.Length;
    }
    
    private void PopulateMumbles()
    {
        CharacterRandomizer[] allCharacters = Object.FindObjectsOfType<CharacterRandomizer>();

        foreach (var c in allCharacters)
        {
            GameObject newMumble = Instantiate(mumblePrefab) as GameObject;
            newMumble.transform.SetParent(c.transform, false);
            newMumble.transform.localPosition = Vector3.zero;

            c.SetMumble(newMumble.GetComponent<NPCMumble>());
        }
    }

    public AudioClip GetRandomMumble()
    {
        return _mumbles[UnityEngine.Random.Range(0, _mumbles.Length)];
    }

    public AudioClip GetRandomMumble(string emotion)
    {
        List<int> _selectees = new List<int>();
        for (int i = 0; i < _mumbleNames.Length; i++)
        {
            if(_mumbleNames[i].Contains(emotion))
            {
                _selectees.Add(i);
            }
        }

        return _mumbles[_selectees[UnityEngine.Random.Range(0, _selectees.Count)]];
    }

    private IEnumerator Crossfade_Routine(bool loop = true)
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
        
        if(!loop)
        {
            yield return new WaitUntil(() => (nextSource.time / nextSource.clip.length) > 0.9f);

            float startVolume = nextSource.volume;
            start = Time.time;
            float remaining = nextSource.time - nextSource.clip.length;
            
            while (nextSource.isPlaying)
            {
                float duration = start - Time.time;
                nextSource.volume = Mathf.Lerp(startVolume, 0, duration / remaining);
            }
        }        
    }

    public void PlayRandomSound()
    {
        PlaySound(allClips[UnityEngine.Random.Range(0, allClips.Length)]);
    }
}
