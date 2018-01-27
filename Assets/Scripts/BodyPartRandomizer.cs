using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BodyPart
{
    public string name;
    public Sprite sprite;
    public Gender gender = Gender.Neutral;
}

public class BodyPartRandomizer : MonoBehaviour {
    [SerializeField]
    public List<BodyPart> sprites = new List<BodyPart>();
    private SpriteRenderer renderer;

    void Awake()
    {
        
        renderer = GetComponent<SpriteRenderer>();
    }

    public void SelectRandom()
    {
        Debug.Log("Randomizing " + renderer.name + " sprite...");
        Sprite sprite = sprites[Random.Range(1, sprites.Count) - 1].sprite;
        renderer.sprite = sprite;
    }
}
