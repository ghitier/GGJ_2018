using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackDoor : MonoBehaviour {

    private Animator _animator;
    private bool _open = false;

    public float timeFactor = 0.1f;

    public delegate void SnackEvent();
    public static event SnackEvent OnSnackDoorOpen;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!_open && TimeManager.ElapsedTime >= (TimeManager.Instance.totalTime * timeFactor))
        {
            _animator.SetTrigger("Open");
            _open = true;

            if(OnSnackDoorOpen != null)
            {
                OnSnackDoorOpen();
            }
        }
    }

}
