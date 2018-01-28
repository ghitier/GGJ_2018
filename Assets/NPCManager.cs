using PresidentExtensions;
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
    public Color[] clothColors = new Color[0];

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
        maleSpritesDico.Add(PartType.Beard, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Male/Beard")));
        
        // -----------------------------------------------------------------------------------------------------

        femaleSpritesDico.Add(PartType.Eyes, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Eyes")));
        femaleSpritesDico.Add(PartType.Hair, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Hair")));
        femaleSpritesDico.Add(PartType.Nose, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Noses")));
        femaleSpritesDico.Add(PartType.Head, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Head")));

        femaleSpritesDico.Add(PartType.MouthNormal, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Mouth")));
        femaleSpritesDico.Add(PartType.MouthSad, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/MouthSad")));
        femaleSpritesDico.Add(PartType.MouthHappy, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/MouthHappy")));

        femaleSpritesDico.Add(PartType.Body, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Body")));
        femaleSpritesDico.Add(PartType.Cloth, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Cloth")));
        femaleSpritesDico.Add(PartType.Beard, new List<Sprite>(Resources.LoadAll<Sprite>("Textures/Characters/Female/Beard")));
    }

    private Dictionary<PartType, List<Sprite>> GetAppropriateDictionary(Gender gender)
    {
        Dictionary<PartType, List<Sprite>> refDico = new Dictionary<PartType, List<Sprite>>();

        switch (gender)
        {
            case Gender.Male:
                return maleSpritesDico;

            case Gender.Female:
                return femaleSpritesDico;
        }
        return null;
    }

    public Sprite GetRandomSprite(BodyPartRandomizer part, Gender gender)
    {
        if(part.type == PartType.Cloth && part.relative != null)
        {
            Dictionary<PartType, List<Sprite>> refDico = GetAppropriateDictionary(gender);
            List<Sprite> refList = refDico[part.type];

            string relativeName = part.relative.GetSpriteRenderer().sprite.name;
            char last = relativeName[relativeName.Length - 1];

            char target = '0';
            switch(last)
            {
                case '1':
                    target = '1';
                    break;

                case '0':
                    target = '2';
                    break;

                case '2':
                    target = '3';
                    break;
            }

            refList.Shuffle();
            foreach(var s in refList)
            {
                if(s.name[s.name.Length - 3] == target)
                {
                    return s;
                }
            }

            return null;
        }
        else
        {
            return GetRandomSprite(part.type, gender);
        }
    }

    public Sprite GetRandomSprite(PartType partType, Gender gender)
    {
        Dictionary<PartType, List<Sprite>> refDico = GetAppropriateDictionary(gender);
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

    public Color GetRandomClothColor()
    {
        return clothColors[Random.Range(0, clothColors.Length)];
    }
}
