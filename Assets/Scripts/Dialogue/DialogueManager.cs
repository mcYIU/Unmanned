using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    [Header("Load Global JSON")]
    [SerializeField] private TextAsset loadGlobalJSON;

    public static DialogueManager instance;
    private Story currentStory;
    public bool dialogueIsPlaying { get; set; }

    public DialogueVariables dialogueVariables;

    private bool isGameEnd = false;
    private bool isSleepAgain = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.Log("Found more than one Dialogue Manager in the scene");
        

        dialogueVariables = new DialogueVariables(loadGlobalJSON);
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];
        for (int index = 0; index < choices.Length; index++)
            choicesText[index] = choices[index].GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        CheckGameStatus();

        if (!dialogueIsPlaying)
            return;
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (!dialogueIsPlaying)
        {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);

            dialogueVariables.StartListening(currentStory);

            ContinueStory();
        }
        else return;
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        if (dialogueVariables != null)
            dialogueVariables.SaveVariables();

        dialogueVariables.StopListening(currentStory);
    }

    public void ContinueStory()
    {
        if (currentStory.canContinue && currentStory.currentChoices.Count == 0)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else if (!currentStory.canContinue)
            if (currentStory.currentChoices.Count > 0)
                return;
            else
                ExitDialogueMode();
        else
            return;
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        for (int index = 0; index < choices.Length; index++)
        {
            if (index < currentChoices.Count)
            {
                choices[index].SetActive(true);
                choicesText[index].text = currentChoices[index].text;
            }
            else
            {
                choices[index].SetActive(false);
                choicesText[index].text = "";
            }
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        dialogueText.text = currentStory.Continue();
        DisplayChoices();
    }

    private void CheckGameStatus()
    {
        isSleepAgain = ((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState("isSleepAgain")).value;
        isGameEnd = ((Ink.Runtime.BoolValue)DialogueManager.GetInstance().GetVariableState("isGameEnd")).value;

        if (isSleepAgain | isGameEnd)
        {
            ExitDialogueMode();
            LevelChanger.instance.DelayLoadScene("End");
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue ==  null)
            Debug.Log(variableValue + "Ink Variables Null!");
        return variableValue;
    }

}
