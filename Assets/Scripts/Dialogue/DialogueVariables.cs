using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    private Story globalVariablesStory;
    private const string saveVariablesKey = "INK_VARIABLES";

    public DialogueVariables(TextAsset loadGlobalJSON)
    {
        ///create the story
        globalVariablesStory = new Story(loadGlobalJSON.text);
        if(PlayerPrefs.HasKey(saveVariablesKey))
        {
            string jsonState = PlayerPrefs.GetString(saveVariablesKey);
            globalVariablesStory.state.LoadJson(jsonState);
        }
        ///initialize the dictionary of variables
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
        }
    }

    public void SaveVariables()
    {
        if(globalVariablesStory != null)
        {
            VariablesToStory(globalVariablesStory);
            PlayerPrefs.SetString(saveVariablesKey,globalVariablesStory.state.ToJson());
        }
    }

    public void ClearVariables()
    {
        
        PlayerPrefs.DeleteAll();
    }

    public void StartListening(Story story)
    {
        ///set the varibles global should be done before the listener
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

   private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        ///only maintain variables initialized from the global ink file
        if (variables.ContainsKey(name))
        {
            variables.Remove(name);
            variables.Add(name, value);
        }            
    }

    private void VariablesToStory(Story story)
    {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables)
            story.variablesState.SetGlobal(variable.Key, variable.Value);
    } 
}
