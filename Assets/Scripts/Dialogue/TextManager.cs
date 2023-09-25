using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Collections;

public class TextManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject textPanel;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private GameObject endBtn;
    [SerializeField] private TextMeshProUGUI nextBtnText;
    [SerializeField] private TextMeshProUGUI endBtnText;
    [SerializeField] private GameObject redBG;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset loadGlobalJSON;
    [SerializeField] private TextAsset textJSON;

    private static TextManager instance;
    private Story currentStory;

    public DialogueVariables dialogueVariables;

    private bool isGameEnd;
    private bool isSleepAgain;
    private bool isLastPage = false;
    private bool isCrashed = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        dialogueVariables = new DialogueVariables(loadGlobalJSON);

    }

    public static TextManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        isSleepAgain = ((Ink.Runtime.BoolValue)TextManager.GetInstance().GetVariableState("isSleepAgain")).value;
        isGameEnd = ((Ink.Runtime.BoolValue)TextManager.GetInstance().GetVariableState("isGameEnd")).value;

        if (isSleepAgain)
            redBG.SetActive(true);
        else redBG.SetActive(false);

        EnterTextMode();
    }
    
    private void Update()
    {
        
    }

    public void EnterTextMode()
    {
        currentStory = new Story(textJSON.text);
        nextBtn.SetActive(true);
        endBtn.SetActive(false);
        textPanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);

        ContinueStory();
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue)
            storyText.text = currentStory.Continue();

        CheckCrash(isCrashed);
        CheckLastPage(isLastPage);
    }

    private void CheckLastPage(bool isLast)
    {
        isLast = ((Ink.Runtime.BoolValue)TextManager.GetInstance().GetVariableState("isLastPage")).value;
        if (isLast)
        {
            nextBtn.SetActive(false);
            endBtn.SetActive(true);
        }
    }

    private void CheckCrash(bool isCrashing)
    {
        isCrashing = ((Ink.Runtime.BoolValue)TextManager.GetInstance().GetVariableState("isCrashing")).value;
        if (isCrashing)
        {
            SoundManager.instance.Crash();
            isCrashing = false;
        }

    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
            Debug.Log(variableValue + "Ink Variables Null!");
        return variableValue;
    }

}
