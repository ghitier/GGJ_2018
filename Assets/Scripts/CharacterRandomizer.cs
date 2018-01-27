using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRandomizer : MonoBehaviour {

    private const int BAD_ANIM_COUNT = 1;
    private const int HAPPY_ANIM_COUNT = 2;
    private const int NEUTRAL_ANIM_COUNT = 2;

    public Gender gender;
    public bool randomize;
    public List<string> disapprovingThings = new List<string>();

    private List<BodyPartRandomizer> _parts = new List<BodyPartRandomizer>();

    private BodyPartRandomizer mouth;
    private Animator _animator;
    private NPCMumble _mumble;

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

        President.OnSentenceSpoken += ReactToSentence;
    }

    public void ReactToSentence(string sentence)
    {
        StartCoroutine(React_Routine(sentence));
    }

    private IEnumerator React_Routine(string sentence)
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));

        bool dislike = false;
        foreach (var s in disapprovingThings)
        {
            if (sentence.Contains(s))
            {
                dislike = true;
            }
        }

        if (!dislike && Random.value < 0.20f)
        {
            dislike = true;
        }

        SetEmotion(dislike ? "bad" : (Random.value > 0.5f) ? "happy" : "neutral");
    }
    
    public void SetEmotion(string emotion)
    {
        switch(emotion)
        {
            case "bad":
                _animator.SetTrigger("Bad_" + Random.Range(1, BAD_ANIM_COUNT + 1).ToString());
                mouth.GetSpriteRenderer().sprite = NPCManager.Instance.GetRandomSprite(PartType.MouthSad, gender);
                break;

            case "happy":
                mouth.GetSpriteRenderer().sprite = NPCManager.Instance.GetRandomSprite(PartType.MouthHappy, gender);
                _animator.SetTrigger("Joy_" + Random.Range(1, HAPPY_ANIM_COUNT + 1).ToString());
                break;

            case "neutral":
                mouth.GetSpriteRenderer().sprite = NPCManager.Instance.GetRandomSprite(PartType.MouthNormal, gender);
                _animator.SetTrigger("Idle_" + Random.Range(1, NEUTRAL_ANIM_COUNT + 1).ToString())  ;
                break;

            default:
                Debug.LogWarning("Unknown emotion : " + emotion);
                return;
        }

        _mumble.Emotion = emotion;
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

                    case PartType.MouthNormal:
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

    public string StringifyCharacter()
    {
        string res = "";
        foreach(var p in _parts)
        {
            res += p.GetSpriteRenderer().sprite.name;
            res += "|";
        }
        return res;
    }

    public bool CompareCharacter(CharacterRandomizer otherChar)
    {
        return StringifyCharacter() == otherChar.StringifyCharacter();
    }

    public void SetMumble(NPCMumble m)
    {
        _mumble = m;
    }
}
