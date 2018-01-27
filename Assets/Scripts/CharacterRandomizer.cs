using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gender
{
    Male,
    Female,
    Neutral
}

public class CharacterRandomizer : MonoBehaviour {

    public Gender gender;
    public bool randomize;
    private List<BodyPartRandomizer> _parts = new List<BodyPartRandomizer>();

    private void Awake()
    {
        _parts = new List<BodyPartRandomizer>(GetComponentsInChildren<BodyPartRandomizer>());
    }

    private void Start()
    {
        if (randomize == true)
        {
            _parts.ForEach(part => part.SelectRandom());
        }
    }
}
