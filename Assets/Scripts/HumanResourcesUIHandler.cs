using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HumanResourcesUIHandler : MonoBehaviour
{   
    //VARIABLES
    public TextMeshProUGUI programersCurrentCountText;
    public TextMeshProUGUI userInterfaceSpecialistsCurrentCountText;
    public TextMeshProUGUI integrabilitySpecialistsCurrentCountText;

 
    public TextMeshProUGUI programersAvailableCountText;
    public TextMeshProUGUI userInterfaceSpecialistsAvailableCountText;
    public TextMeshProUGUI integrabilitySpecialistsAvialableCountText;

    public Button addProgrammersButton;
    public Button addUIpecialistButton;
    public Button addIntegrabilitySpecialistButton;

    public Button substractProgrammerButton;
    public Button substractUISpecialistButton;
    public Button substractIntegrabilityButton;


    public TMP_InputField hireProgrammersCountIF;
    public TMP_InputField hireUISpecialistsCountIF;
    public TMP_InputField hireIntegrabilitySpecialistsCountIF;

    public Slider programmerSalarySlider;
    public Slider uiSpecialistSalarySlider;
    public Slider integrabilitySpecialistSalarySlider;

    public TextMeshProUGUI programmerSalaryText;
    public TextMeshProUGUI uiSpecialistSalaryText;
    public TextMeshProUGUI integrabilitySpecialistsSalaryText;

    private bool initialized;
    private GameObject myPlayerDataObject;
    private HumanResourcesManager humanResourcesManager;

    //GETTERS & SETTERS
    public void SetProgramersCurrentCountText(string programersCurrentCountText) { this.programersCurrentCountText.text = programersCurrentCountText; }
    public void SetUserInterfaceSpecialistsCurrentCountText(string userInterfaceSpecialistsCurrentCountText) { this.userInterfaceSpecialistsCurrentCountText.text = userInterfaceSpecialistsCurrentCountText; }
    public void SetIntegrabilitySpecialistsCurrentCountText(string integrabilitySpecialistsCurrentCountText) { this.integrabilitySpecialistsCurrentCountText.text = integrabilitySpecialistsCurrentCountText; }

    public void SetprogramersAvailableCountText(string programersAvailableCountText) { this.programersAvailableCountText.text = programersAvailableCountText; }
    //public void SetSpecialistsAvailableCountText(string specialistsAvailableCountText) { this.specialistsAvailableCountText.text = specialistsAvailableCountText; }

    public void SetIntegrabilitySpecialistsAvailableCountText(string integrabilitySpecialistsAvailavbleCountText) { this.integrabilitySpecialistsAvialableCountText.text = integrabilitySpecialistsAvailavbleCountText; }
    public void SetUISpecialistsAvailableCountText(string userInterfaceSpecialistsAvailableCountText) { this.userInterfaceSpecialistsAvailableCountText.text = userInterfaceSpecialistsAvailableCountText; }

    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();

        humanResourcesManager = myPlayerDataObject.GetComponent<HumanResourcesManager>();
        humanResourcesManager.SetHumanResourcesUIHandler(this);
        UpdateAllElements();
    }
    
    public void AdditonOfOne(TMP_InputField inputfield)
    {
        int value = int.Parse(inputfield.text) + 1;
        inputfield.text = value.ToString();
    }
    public void SubstractionOfOne(TMP_InputField inputfield)
    {
        int value = int.Parse(inputfield.text) - 1;
        if(value < 0)
        {
            inputfield.text = "0";
        }
        else
        {
            inputfield.text = value.ToString();
        }
    }

    public void AddProgramer() { humanResourcesManager.AddProgramer(); }
    public void AddUISpecialist() { humanResourcesManager.AddUISpecialist(); }
    public void AddIntegrabilitySpecialist() { humanResourcesManager.AddIntegrabilitySpecialist(); }
    public void SubstractProgrammer() { humanResourcesManager.SubstractProgrammer(); }
    public void SubstractUISpecialist() { humanResourcesManager.SubstractUISpecialist(); }
    public void SubstarctIntegrability() { humanResourcesManager.SubstarctIntegrabilitySpecialist(); }

    public void HireProgrammersNextQuarter()
    {   
        if(Int32.Parse(hireProgrammersCountIF.text) > 10)
        {
            humanResourcesManager.HireProgrammersNextQuarter(10);
        }
        else
        {
            humanResourcesManager.HireProgrammersNextQuarter(Int32.Parse(hireProgrammersCountIF.text));
        }   
    }

    public void HireUISPecialistsNextQuarter()
    {

        if (Int32.Parse(hireUISpecialistsCountIF.text) > 10)
        {
            humanResourcesManager.HireUISPecialistsNextQuarter(10);
        }
        else
        {
            humanResourcesManager.HireUISPecialistsNextQuarter(Int32.Parse(hireUISpecialistsCountIF.text));
        }

    }
    public void HireIntegrabilitySpecialistsNextQuarter()
    {
        if (Int32.Parse(hireIntegrabilitySpecialistsCountIF.text) > 10)
        {
            humanResourcesManager.HireIntegrabilitySpecialistsNextQuarter(10);
        }
        else
        {

            humanResourcesManager.HireIntegrabilitySpecialistsNextQuarter(Int32.Parse(hireIntegrabilitySpecialistsCountIF.text));
        }
    }

    public void ChangeProgrammmerSalary() //triggered by UI
    {
        if (initialized)
        {
            humanResourcesManager.ChangeProgrammmerSalaryMonth((int)programmerSalarySlider.value);
            programmerSalaryText.text = programmerSalarySlider.value.ToString();
        }
    }
    public void ChangeUISpecialistSalary()
    {
        if (initialized)
        {
            humanResourcesManager.ChangeUISpecialistSalaryMonth((int)uiSpecialistSalarySlider.value);
            uiSpecialistSalaryText.text = uiSpecialistSalarySlider.value.ToString();
        }
    }
    public void ChangeIntegrabilitySpecialistSalary()
    {
        if (initialized)
        {
            humanResourcesManager.ChangeIntegrabilitySpecialistSalaryMonth((int)integrabilitySpecialistSalarySlider.value);
            integrabilitySpecialistsSalaryText.text = integrabilitySpecialistSalarySlider.value.ToString();
        }
    }

    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateAllElements()
    {
        initialized = false;
        programmerSalarySlider.value = humanResourcesManager.GetProgrammerSalaryPerMonth();
        uiSpecialistSalarySlider.value = humanResourcesManager.GetUISpecialistSalaryPerMonth();
        integrabilitySpecialistSalarySlider.value = humanResourcesManager.GetIntegrabilitySpecialistSalaryPerMonth();
        
        UpdateProgramersCurrentCountText(humanResourcesManager.GetProgrammersCount());
        UpdateUserInterfaceSpecialistsCurrentCountText(humanResourcesManager.GetUISPecialistsCount());
        UpdateIntegrabilitySpecialistsCurrentCountText(humanResourcesManager.GetIntegrabilitySpecialistsCount());
        UpdateProgramersAvailableCountText(humanResourcesManager.GetProgrammersAvailableCount());
        UpdateIntegrabilitySpecialistsAvailableCountText(humanResourcesManager.GetIntegrabilitySpecialistsAvailableCount());
        UpdateUISpecialistsAvailableCountText(humanResourcesManager.GetUISpecialsitsAvailableCount());
        UpdateProgrammerSalarySlider(humanResourcesManager.GetProgrammerSalaryPerMonth());
        UpdateUISpecialistSalarySlider(humanResourcesManager.GetUISpecialistSalaryPerMonth());
        UpdateIntegrabilitySpecialistSalarySlider(humanResourcesManager.GetIntegrabilitySpecialistSalaryPerMonth());

        UpdateHireProgrammersCount(0);
        UpdateHireUISpecialistsCount(0);
        UpdateHireIntegrabilitySpecialistsCount(0);
        initialized = true;
    }

    public void UpdateProgramersCurrentCountText(int programmersCurrentCount)
    {
        programersCurrentCountText.text = programmersCurrentCount.ToString();
        if (programmersCurrentCount == 0)
        {
            DisableSubstractProgrammerButton();
        }
        else
        {
            EnableSubstractProgrammerButton();
        }
    }
    public void UpdateUserInterfaceSpecialistsCurrentCountText(int userInterfaceSpecialistsCurrentCount)
    {
        userInterfaceSpecialistsCurrentCountText.text = userInterfaceSpecialistsCurrentCount.ToString();
        if (userInterfaceSpecialistsCurrentCount == 0)
        {
            DisableSubstractUISpecialistButton();
        }
        else 
        {
            EnableSubstractUISpecialistButton();
        }
    }
    public void UpdateIntegrabilitySpecialistsCurrentCountText(int integrabilitySpecialistsCurrentCount)
    {
        integrabilitySpecialistsCurrentCountText.text = integrabilitySpecialistsCurrentCount.ToString();
        if (integrabilitySpecialistsCurrentCount == 0)
        {
            DisableSubstractIntegrabilitySpecialistButton();
        }
        else 
        {
            EnableSubstractIntegrabilitySpecialistButton();
        }
    }
    public void UpdateProgramersAvailableCountText(int programmersAvailableCount)
    {
        programersAvailableCountText.text = programmersAvailableCount.ToString();
        if (programmersAvailableCount == 0)
        {
            DisableAddProgrammerButton();
        }
        else
        {
            EnableAddProgrammerButton();
        }
    }
    public void UpdateUISpecialistsAvailableCountText(int uiSpecialistsAvailableCount)
    {
        userInterfaceSpecialistsAvailableCountText.text = uiSpecialistsAvailableCount.ToString();
        if(uiSpecialistsAvailableCount == 0)
        {
            DisableAddUISpecialistButton();
        }
        else
        {
            EnableAddUISpecialistButton();
        }
    }
    public void UpdateIntegrabilitySpecialistsAvailableCountText(int integrabilitySpecialistsAvailableCount)
    {
        integrabilitySpecialistsAvialableCountText.text = integrabilitySpecialistsAvailableCount.ToString();
        if(integrabilitySpecialistsAvailableCount == 0)
        {
            DisableAddIntegrabilitySpecialistButton();
        }
        else
        {
            EnableAddIntegrabilitySpecialistButton();
        }
    }

    public void UpdateHireProgrammersCount(int hireProgrammersCount) { this.hireProgrammersCountIF.text = hireProgrammersCount.ToString(); }
    public void UpdateHireUISpecialistsCount(int hireUISpecialistsCount) { this.hireUISpecialistsCountIF.text = hireUISpecialistsCount.ToString(); }
    public void UpdateHireIntegrabilitySpecialistsCount(int hireIntegrabilitySpecialistsCount) { this.hireIntegrabilitySpecialistsCountIF.text = hireIntegrabilitySpecialistsCount.ToString(); }

    public void UpdateProgrammerSalarySlider(int programmerSalary)
    {
        this.programmerSalarySlider.value = programmerSalary;
        programmerSalaryText.text = programmerSalary.ToString();

    }
    public void UpdateUISpecialistSalarySlider(int uiSpecialistSalary)
    {
        this.uiSpecialistSalarySlider.value = uiSpecialistSalary;
        uiSpecialistSalaryText.text = uiSpecialistSalary.ToString();
    }
    public void UpdateIntegrabilitySpecialistSalarySlider(int integrabilitySpecialistSalary)
    {
        this.integrabilitySpecialistSalarySlider.value = integrabilitySpecialistSalary;
        integrabilitySpecialistsSalaryText.text = integrabilitySpecialistSalary.ToString();
    }

    public void DisableAddProgrammerButton()
    {
        addProgrammersButton.interactable = false;
    }
    public void DisableSubstractProgrammerButton()
    {
        substractProgrammerButton.interactable = false;
    }


    public void DisableAddUISpecialistButton()
    {
        addUIpecialistButton.interactable = false;
    }
    public void DisableAddIntegrabilitySpecialistButton()
    {
        addIntegrabilitySpecialistButton.interactable = false;
    }

    public void DisableSubstractUISpecialistButton()
    {
        substractUISpecialistButton.interactable = false;
    }
    public void DisableSubstractIntegrabilitySpecialistButton()
    {
        substractIntegrabilityButton.interactable = false;
    }
    public void EnableAddUISpecialistButton()
    {
        addUIpecialistButton.interactable = true;
    }
    public void EnableAddIntegrabilitySpecialistButton()
    {
        addIntegrabilitySpecialistButton.interactable = true;
    }

    public void EnableSubstractUISpecialistButton()
    {
        substractUISpecialistButton.interactable = true;
    }
    public void EnableSubstractIntegrabilitySpecialistButton()
    {
        substractIntegrabilityButton.interactable = true;
    }


    public void EnableAddProgrammerButton()
    {
        addProgrammersButton.interactable = true;
    }
    public void EnableSubstractProgrammerButton()
    {
        substractProgrammerButton.interactable = true;
    }

    public void DisableEditation()
    {
        addProgrammersButton.interactable = false;
        addUIpecialistButton.interactable = false;
        addIntegrabilitySpecialistButton.interactable = false;

        substractProgrammerButton.interactable = false;
        substractUISpecialistButton.interactable = false;
        substractIntegrabilityButton.interactable = false;

        hireProgrammersCountIF.interactable = false;
        hireUISpecialistsCountIF.interactable = false;
        hireIntegrabilitySpecialistsCountIF.interactable = false;

        programmerSalarySlider.interactable = false;
        uiSpecialistSalarySlider.interactable = false;
        integrabilitySpecialistSalarySlider.interactable = false;
    }

}
