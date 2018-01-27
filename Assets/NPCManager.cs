using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartType
{
    None,
    Head,
    Eyes,
    Hair,
    Nose,
    MouthNormal,
    MouthSad,
    MouthHappy,
    Body,
    Cloth,
    Beard
}

public enum Gender
{
    Male,
    Female,
    Both
}

public class NPCManager : Singleton<NPCManager> {

    public Color[] skinColors = new Color[0];
    public Color[] hairColors = new Color[0];
    
    private Dictionary<PartType, List<Sprite>> maleSpritesDico = new Dictionary<PartType, List<Sprite>>();
    private Dictionary<PartType, List<Sprite>> femaleSpritesDico = new Dictionary<PartType, List<Sprite>>();

    private void Awake()
    {
        maleSpritesDico.Add(PartType.Eyes, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Eyes")));
        maleSpritesDico.Add(PartType.Hair, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Hair")));
        maleSpritesDico.Add(PartType.Nose, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Noses")));
        maleSpritesDico.Add(PartType.Head, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Head")));

        maleSpritesDico.Add(PartType.MouthNormal, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Mouth")));
        maleSpritesDico.Add(PartType.MouthSad, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/MouthSad")));
        maleSpritesDico.Add(PartType.MouthHappy, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/MouthHappy")));


        maleSpritesDico.Add(PartType.Body, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Body")));
        maleSpritesDico.Add(PartType.Cloth, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Cloth")));
        

        femaleSpritesDico.Add(PartType.Eyes, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Eyes")));
        femaleSpritesDico.Add(PartType.Hair, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Hair")));
        femaleSpritesDico.Add(PartType.Nose, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Noses")));
        femaleSpritesDico.Add(PartType.Head, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Head")));

        femaleSpritesDico.Add(PartType.MouthNormal, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Mouth")));
        femaleSpritesDico.Add(PartType.MouthSad, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/MouthSad")));
        femaleSpritesDico.Add(PartType.MouthHappy, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/MouthHappy")));

        femaleSpritesDico.Add(PartType.Body, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Body")));
        femaleSpritesDico.Add(PartType.Cloth, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Cloth")));
    }

    public Sprite GetRandomSprite(PartType partType, Gender gender)
    {
        Dictionary<PartType, List<Sprite>> refDico = new Dictionary<PartType, List<Sprite>>();

        switch(gender)
        {
            case Gender.Male:
                refDico = maleSpritesDico;
                break;

            case Gender.Female:
                refDico = femaleSpritesDico;
                break;

            case Gender.Both:
                //TODO
                break;
            
            default:
                Debug.LogWarning("Unknown gender. Wtf ?");
                return null;
                break;
        }

        List<Sprite> refList = refDico[partType];

        if(refList.Count > 0)
        {
            return refList[UnityEngine.Random.Range(0, refList.Count)];
        }
        else
        {
            return null;
        }
    }

    public Color GetRandomHairColor()
    {
        return hairColors[Random.Range(0, hairColors.Length)];
    }

    public Color GetRandomSkinColor()
    {
        return skinColors[Random.Range(0, skinColors.Length)];
    }
}
