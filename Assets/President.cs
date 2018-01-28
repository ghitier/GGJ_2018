using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PresidentExtensions;

public class President : Singleton<President> {

    public AudioClip applauseSound;

    public List<AudioClip> firstClips = new List<AudioClip>();
    public AudioClip snackClip;
    private bool _snackWasAdded = false;

    private AudioClip[] _speeches = new AudioClip[0];
    private AudioSource _source;

    public delegate void SpeechEvent(string speechSentence);
    public static event SpeechEvent OnSentenceSpoken;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _speeches = Resources.LoadAll<AudioClip>("SFX/discours");
    }

    private void Start()
    {
        StartCoroutine(Speech_Routine());
        SnackDoor.OnSnackDoorOpen += OnSnackOpen;
    }

    public void OnSnackOpen()
    {
        _snackWasAdded = true;
        SnackDoor.OnSnackDoorOpen -= OnSnackOpen;
    }

    private IEnumerator Speech_Routine()
    {
        List<AudioClip> todo = new List<AudioClip>(_speeches);
        List<AudioClip> done = new List<AudioClip>();

        while(true)
        {
            if(todo.Count == 0)
            {
                done.Shuffle();
                todo = done;
                done.Clear();
            }            

            if(_source.isPlaying)
            {
                yield return new WaitUntil(() => (_source.time / _source.clip.length) > 0.7f);

                if(OnSentenceSpoken != null)
                {
                    OnSentenceSpoken(_source.clip.name);
                }

                if(Random.value > 0.5f)
                {
                    Maestro.Instance.PlaySound(applauseSound, false);
                    yield return new WaitForSeconds(applauseSound.length + 1);
                }
                else
                {
                    yield return new WaitUntil(() => !_source.isPlaying);
                    yield return new WaitForSeconds(2.0f);
                }
            }

            yield return new WaitUntil(() => !_source.isPlaying);

            AudioClip selectee = todo[UnityEngine.Random.Range(0, todo.Count)];
            
            if (firstClips.Count > 0)
            {
                selectee = firstClips[0];
                firstClips.RemoveAt(0);
            }
            else if (_snackWasAdded)
            {
                selectee = snackClip;
                _snackWasAdded = false;
            }
            else
            {
                todo.Remove(selectee);
            }

            _source.clip = selectee;
            _source.Play();

            done.Add(selectee);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
