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

    public static float _zoomLevel = 1f;

    public bool controlled = true;
    public AudioClip shotSound;
    public AudioClip akShotSound;
    public AudioClip zoomSound;
    public AudioClip panicSound;
    public GameObject lastHitObject = null;
    public GameObject assassin = null;

    private float ZoomLevel
    {
        get
        {
            return _zoomLevel;
        }
        set
        {
            float newZoomLevel = Mathf.Clamp(value, MIN_ZOOM, 1f);

            _zoomLevel = newZoomLevel;
            _camera.orthographicSize = _oriFOV * _zoomLevel;
        }
    }

	private void Awake()
    {   
        Cursor.visible = true;

        _oriPos = transform.position;

        _camera = GetComponent<Camera>();
        _oriFOV = _camera.orthographicSize;
    }

    private void Start()
    {
        transform.position = _oriPos;

        SetUpGame();

        TimeManager.OnTimerEnd += OnTimerEnd;
    }


    private void Update () {
        if(controlled)
        {
            float mouseDelta_X = Input.GetAxis("Mouse X") + Input.GetAxis("Horizontal");
            float mouseDelta_Y = Input.GetAxis("Mouse Y") + Input.GetAxis("Vertical");

            Vector3 newCamPos = transform.position;
            Vector3 oldPos = newCamPos;
            newCamPos.x += mouseDelta_X * _zoomLevel * SCROLL_SPEED * Time.deltaTime;
            newCamPos.y += mouseDelta_Y * _zoomLevel * SCROLL_SPEED * Time.deltaTime;
            transform.position = newCamPos;

            Vector3 hitPos;
            if(!CameraToTheatreRaycast(out hitPos))
            {
                transform.position = oldPos;
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                SetNewZoom(ZoomLevel - Mathf.Clamp(Input.GetAxis("Mouse ScrollWheel") * 10, -1, 1) * ZOOM_STEP);
            }

            GameObject currentHitObject = TargetedCharacter();
            lastHitObject = currentHitObject ?? lastHitObject;

            if (Input.GetMouseButtonDown(0))
            {
                Maestro.Instance.PlayClipOnce(shotSound, true);
                Maestro.Instance.PlaySound(panicSound);
                StartCoroutine(ShotMovement_Routine());
                EndGame();
            }

            
        }        
    }

    public void OnTimerEnd()
    {
        TimeManager.OnTimerEnd -= OnTimerEnd;

        Cursor.visible = true;
        controlled = false;

        Maestro.Instance.PlayClipOnce(akShotSound);
        Maestro.Instance.PlaySound(panicSound);

        FadeScreen.Instance.FadeOut("Vous n'avez pas pu empêcher l'assassinat de notre vénérable président !", "Oh, noooon !", delegate
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("S_Menu");
        });
    }

    private void SetUpGame()
    {
        Cursor.visible = true;
        controlled = false;

        FadeScreen.Instance.FadeIn("Trouvez l'assassin avant qu'il ne tue notre estimé président !", "Ok !",
        delegate 
        {
            Cursor.visible = false;
            controlled = true;
            FadeScreen.Instance.Hide();
        });
    }

    private void EndGame()
    {
        Cursor.visible = true;
        controlled = false;

        GameObject targetObject = TargetedCharacter();

        if (targetObject != null)
        {
            CharacterRandomizer shotChar = targetObject.GetComponentInParent<CharacterRandomizer>();
            
            if(shotChar != null)
            {
                shotChar.bloodObject.SetActive(true);
            }
        }

        if(targetObject == assassin)
        {
            FadeScreen.Instance.FadeOut("Vous avez prévenu l'assassinat de notre estimé président !", "Youpi !", delegate
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("S_Menu");
            });
        }
        else if(targetObject != null)
        {
            FadeScreen.Instance.FadeOut("Vous n'avez pas trouvé la bonne personne, l'assassin s'est enfui..", "Oooh mince !", delegate
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("S_Menu");
            });
        }
        else
        {
            FadeScreen.Instance.FadeOut("La balle n'a eu aucun effet, l'assassin s'est enfui..", "Oooh mince !", delegate
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("S_Menu");
            });
        }

    }
    
    private void SetNewZoom(float zoomTarget)
    {
        StopAllCoroutines();
        StartCoroutine(Zoom_Routine(zoomTarget));
    }

    public bool CameraToTheatreRaycast(out Vector3 hitPoint)
    {
        Ray charles = new Ray(transform.position, Vector3.forward);
        Debug.DrawRay(charles.origin, charles.direction * 1000, Color.blue, 0.1f);

        int layerCollide = 1 << LayerMask.NameToLayer("Background");

        RaycastHit hit;

        if (Physics.Raycast(charles, out hit, 1000.0f, layerCollide))
        {
            hitPoint = hit.point;
            return true;
        }
        else
        {
            hitPoint = Vector3.zero;
            return false;
        }
    }

    private GameObject TargetedCharacter()
    {
        Ray charles = new Ray(transform.position, Vector3.forward);

        int layerCollide = 1 << LayerMask.NameToLayer("NPC");

        RaycastHit hit;

        if (Physics.Raycast(charles, out hit, 1000.0f, layerCollide))
        {
            return hit.collider.gameObject;
        }
        else return null;
    }

    private IEnumerator Zoom_Routine(float zoomTarget)
    {
        zoomTarget = Mathf.Clamp(zoomTarget, MIN_ZOOM, 1f);

        float start = Time.time;
        float startZoom = ZoomLevel;

        if(zoomTarget != startZoom)
        {
            Maestro.Instance.PlayClipOnce(zoomSound);
        }

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
