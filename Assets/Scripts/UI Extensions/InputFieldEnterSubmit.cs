using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// Credit Vicente Russo  
/// Sourced from - https://bitbucket.org/ddreaper/unity-ui-extensions/issues/23/returnkeytriggersbutton


/// <summary>
/// Usage: Add this component to the input and add the function to execute to the EnterSubmit event of this script
/// </summary>
[RequireComponent(typeof(InputField))]
public class InputFieldEnterSubmit : MonoBehaviour
{
    [System.Serializable]
    public class EnterSubmitEvent : UnityEvent<string>
    {

    }

    public EnterSubmitEvent EnterSubmit;
    public bool defocusInput = true;
    private InputField _input;

    void Awake()
    {
        _input = GetComponent<InputField>();
        _input.onEndEdit.AddListener(OnEndEdit);
    }

    public void OnEndEdit(string txt)
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
            return;
        EnterSubmit.Invoke(txt);
        if (defocusInput)
        {
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
