﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMumble : MonoBehaviour {

    private AudioSource _source;

    private float _oriVolume;
    private float _oriRange;

    private string _currentEmotion = "neutral";
    public string Emotion
    {
        get
        {
            return _currentEmotion;
        }
        set
        {
            if(value != "neutral")
            {
                StopAllCoroutines();
                StartCoroutine(Mumble_Routine());
            }

            _currentEmotion = value;
        }
    }

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _oriRange = _source.maxDistance;
        _oriVolume = _source.volume;
    }

    private void Start()
    {
        StartCoroutine(Mumble_Routine());
    }

    private void LateUpdate()
    {
        _source.maxDistance = _oriRange * CameraManager._zoomLevel;
        _source.volume = _oriVolume + 0.13f * (1 - CameraManager._zoomLevel);

    }

    private IEnumerator Mumble_Routine()
    {
        yield return null;

        while(true)
        {
            if(_currentEmotion != "neutral")
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 20f));
            }
            
            if (!_source.isPlaying || _source.clip == null)
            {
                AudioClip newMumble = Maestro.Instance.GetRandomMumble(_currentEmotion);
                _source.clip = newMumble;
                _source.Play();
            }
        }
    }
}
