using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnimator : MonoBehaviour
{
    private Text textUI;
    public List<string> textStates;
    private int stateIndex;
    public float stateDuration = 1.0f;
    public int delay = 0;
    public bool oneCharacterAtATime = false;
    private int characterIndex;

    void Start()
    {
        stateIndex = 0;
        characterIndex = 0;
        textUI = gameObject.GetComponent<Text>();

        if (oneCharacterAtATime)
            InvokeRepeating("UpdateCharacter", delay, 0.15f);
        else
            InvokeRepeating("UpdateText", delay, stateDuration);
    }

    private void UpdateText()
    {
        textUI.text = textStates[stateIndex];

        if (stateIndex == textStates.Count - 1)
            CancelInvoke();
        else
            stateIndex++;
    }

    private void UpdateCharacter()
    {
        if(stateIndex <= textStates.Count - 1)
        {
            if (characterIndex < textStates[stateIndex].Length - 1)
            {
                textUI.text += textStates[stateIndex][characterIndex];
                characterIndex++;
            }
            else
            {
                textUI.text = "";
                characterIndex = 0;
                stateIndex++;
            }
        }
        else
        {
            CancelInvoke();
            GUIManager.Instance.PromptText.enabled = true;
            GUIManager.Instance.playerNameInputField.Select();
            GUIManager.Instance.playerNameInputField.ActivateInputField();
        }       
    }
}
