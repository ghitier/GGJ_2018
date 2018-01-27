using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRandomizer : MonoBehaviour {

    public Gender gender;
    public bool randomize;
    private List<BodyPartRandomizer> _parts = new List<BodyPartRandomizer>();

    private BodyPartRandomizer mouth;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        


        if(randomize)
        {
            gender = (Random.value > 0.5f) ? Gender.Female : Gender.Male;
        }

        _parts = new List<BodyPartRandomizer>(GetComponentsInChildren<BodyPartRandomizer>());
    }

    private void Start()
    {
        if(randomize)
        {
            RandomizeParts();
        }
    }

    public void RandomizeParts()
    {
        Color skinColor = NPCManager.Instance.GetRandomSkinColor();
        Color hairColor = NPCManager.Instance.GetRandomHairColor();

        foreach (var p in _parts)
        {
            if(p.type != PartType.None)
            {
                SpriteRenderer renderer = p.GetSpriteRenderer();
                renderer.sprite = NPCManager.Instance.GetRandomSprite(p.type, gender);

                switch(p.type)
                {
                    case PartType.Body:
                    case PartType.Head:
                    case PartType.Nose:
                        renderer.color = skinColor;
                        break;

                    case PartType.Hair:
                        renderer.color = hairColor;
                        break;

                    case PartType.Mouth:
                        mouth = p;
                        break;
                }
            }
            else
            {
                p.GetSpriteRenderer().color = skinColor;
            }
        }
    }

    public void SetEmotion(string emotion)
    {
        
    }
}
