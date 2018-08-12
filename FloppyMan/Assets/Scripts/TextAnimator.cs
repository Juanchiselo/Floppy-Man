using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimator : MonoBehaviour
{
    private Text textUI;
    public List<string> textStates;
    private int stateIndex;

    void Start()
    {
        stateIndex = 0;
        textUI = gameObject.GetComponent<Text>();
        InvokeRepeating("UpdateText", 0, 1.0f);
    }

    private void UpdateText()
    {
        textUI.text = textStates[stateIndex % textStates.Count];

        if (stateIndex == textStates.Count - 1)
            CancelInvoke();
        else
            stateIndex++;
    }
}
