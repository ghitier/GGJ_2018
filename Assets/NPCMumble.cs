using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMumble : MonoBehaviour {

    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(Mumble_Routine());
    }

    private IEnumerator Mumble_Routine()
    {
        while(true)
        {

            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 50f));


            if (!_source.isPlaying || _source.clip == null)
            {
                AudioClip newMumble = Maestro.Instance.GetRandomMumble();
                _source.clip = newMumble;
                _source.Play();
            }

            
        }
    }
}
