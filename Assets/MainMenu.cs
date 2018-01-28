using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    
	// Use this for initialization
    void Start()
    {

    }

	public void Play()
    {
        Application.LoadLevel("S_Game");
    }

    public void Quit()
    {
        if (Application.isEditor)
            EditorApplication.isPlaying = false;
        else
            Application.Quit();
    }
}
