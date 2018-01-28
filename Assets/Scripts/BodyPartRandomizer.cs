using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartRandomizer : MonoBehaviour {

    public PartType type;
    public BodyPartRandomizer relative;

    public SpriteRenderer GetSpriteRenderer()
    {
        return GetComponent<SpriteRenderer>();
    }
}
