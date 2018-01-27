using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager> {
    
    private const float SCROLL_SPEED = 10.0F;
    private const float ZOOM_SPEED = 1.0F;
    private const float MIN_ZOOM = 0.1F;
    private const float ZOOM_STEP = 0.3F;

    private Vector3 _oriPos;
    private Camera _camera;
    private float _oriFOV;
    private float _zoomLevel = 1f;

    public bool controlled = true;
    public AudioClip shotSound;

    private float ZoomLevel
    {
        get
        {
            return _zoomLevel;
        }
        set
        {
            _zoomLevel = Mathf.Clamp(value, MIN_ZOOM, 1f);
            _camera.orthographicSize = _oriFOV * _zoomLevel;
        }
    }

	private void Awake()
    {
        Cursor.visible = false;

        _oriPos = transform.position;

        _camera = GetComponent<Camera>();
        _oriFOV = _camera.orthographicSize;
    }
    
    private void Update () {
        if(controlled)
        {
            float mouseDelta_X = Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal");
            float mouseDelta_Y = Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical");

            Vector3 newCamPos = transform.position;
            newCamPos.x += mouseDelta_X * _zoomLevel * SCROLL_SPEED * Time.deltaTime;
            newCamPos.y += mouseDelta_Y * _zoomLevel * SCROLL_SPEED * Time.deltaTime;
            transform.position = newCamPos;

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                SetNewZoom(ZoomLevel - Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * 10, -1, 1) * ZOOM_STEP);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Maestro.Instance.PlayClipOnce(shotSound);
                StartCoroutine(ShotMovement_Routine());
                Cursor.visible = true;
                controlled = false;
            }
        }        
    }
    
    private void SetNewZoom(float zoomTarget)
    {
        StopAllCoroutines();
        StartCoroutine(Zoom_Routine(zoomTarget));
    }

    private IEnumerator Zoom_Routine(float zoomTarget)
    {
        zoomTarget = Mathf.Clamp(zoomTarget, MIN_ZOOM, 1f);

        float start = Time.time;
        float startZoom = ZoomLevel;

        while(ZoomLevel != zoomTarget)
        {
            float duration = Time.time - start;
            ZoomLevel = Mathfx.Hermite(ZoomLevel, zoomTarget, duration / ZOOM_SPEED);
            yield return null;
        }
    }

    private IEnumerator ShotMovement_Routine()
    {
        float shotDuration = 0.15f;

        Vector3 oriPos = transform.position;
        Vector3 targetPos = oriPos;
        targetPos.y += 0.5f;

        float start = Time.time;
        while(Time.time - start < shotDuration / 2)
        {
            float duration = Time.time - start;
            transform.position = Vector3.Lerp(oriPos, targetPos, duration / (shotDuration / 2));
            yield return null;
        }

        start = Time.time;
        while (Time.time - start < shotDuration / 2)
        {
            float duration = Time.time - start;
            transform.position = Vector3.Lerp(targetPos, oriPos, duration / (shotDuration / 2));
            yield return null;
        }
    }
}
