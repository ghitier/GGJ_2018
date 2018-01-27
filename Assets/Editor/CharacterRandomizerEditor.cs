using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CharacterRandomizer))]
public class CharacterRandomizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterRandomizer myTarget = (CharacterRandomizer)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Randomize"))
        {
            myTarget.RandomizeParts();
        }
    }
}
