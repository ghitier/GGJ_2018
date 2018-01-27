using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeScreen : Singleton<FadeScreen> {

    private Animator _animator;

    public Text titleText;
    public Button button;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void FadeOut(string text, string buttonText, UnityAction action)
    {
        _animator.SetTrigger("FadeOut");
        SetNewTexts(text, buttonText, action);
    }

    public void FadeIn(string text, string buttonText, UnityAction action)
    {
        _animator.SetTrigger("FadeIn");
        SetNewTexts(text, buttonText, action);
    }

    public void Hide()
    {
        _animator.SetTrigger("Hidden");
    }

    private void SetNewTexts(string text, string buttonText, UnityAction action)
    {
        titleText.text = text;
        button.GetComponentInChildren<Text>().text = buttonText;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
}
