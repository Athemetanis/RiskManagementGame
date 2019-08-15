using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanResourcesUIHandler : MonoBehaviour
{   
    //VARIABLES
    public Text programersCurrentCountText;
    public Text userInterfaceSpecialistsCurrentCountText;
    public Text integrabilitySpecialistsCurrentCountText;

    public Text programersAvailableCountText;
    public Text specialistsAvailableCountText;

    public Text qualityAssuranceSpecialistsCountText;

    public Button addProgrammersButton;
    public Button addUIpecialistButton;
    public Button addIntegrabilitySpecialistButton;

    public Button substractProgrammerButton;
    public Button substractUISpecialistButton;
    public Button substractIntegrabilityButton;

    public InputField hireProgrammersCountIF;
    public InputField hireUISpecialistsCountIF;
    public InputField hireIntegrabilitySpecialistsCountIF;
    public InputField hireGASpecialistsCountIF;

    public Slider programmerSalarySlider;
    public Slider uiSpecialistSalarySlider;
    public Slider integrabilitySpecialistSalarySlider;
    public Slider qaSpecilistSalarySlider;

    private GameObject myPlayerDataObject;
    private HumanResourcesManager humanResourcesManager;

    //GETTERS & SETTERS
    public void SetProgramersCurrentCountText(string programersCurrentCountText) { this.programersCurrentCountText.text = programersCurrentCountText; }
    public void SetUserInterfaceSpecialistsCurrentCountText(string userInterfaceSpecialistsCurrentCountText) { this.userInterfaceSpecialistsCurrentCountText.text = userInterfaceSpecialistsCurrentCountText; }
    public void SetIntegrabilitySpecialistsCurrentCountText(string integrabilitySpecialistsCurrentCountText) { this.integrabilitySpecialistsCurrentCountText.text = integrabilitySpecialistsCurrentCountText; }

    public void SetprogramersAvailableCountText(string programersAvailableCountText) { this.programersAvailableCountText.text = programersAvailableCountText; }
    public void SetSpecialistsAvailableCountText(string specialistsAvailableCountText) { this.specialistsAvailableCountText.text = specialistsAvailableCountText; }

    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        humanResourcesManager = myPlayerDataObject.GetComponent<HumanResourcesManager>();
        humanResourcesManager.SetHumanResourcesUIHandler(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdditonOfOne(InputField inputfield)
    {
        int value = int.Parse(inputfield.text) + 1;
        inputfield.text = value.ToString();
    }
    public void SubstractionOfOne(InputField inputfield)
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

    public void HireProgrammersNextQuarter(InputField hireProgrammers)
    {
        humanResourcesManager.HireProgrammersNextQuarter(Int32.Parse(hireProgrammers.text));
        
    }
    public void HireUISPecialistsNextQuarter(InputField hireUISpecialists)
    {
        humanResourcesManager.HireUISPecialistsNextQuarter(Int32.Parse(hireUISpecialists.text));
    }
    public void HireIntegrabilitySpecialistsNextQuarter(InputField hireIntegrabilitySpecialists)
    {
        humanResourcesManager.HireIntegrabilitySpecialistsNextQuarter(Int32.Parse(hireIntegrabilitySpecialists.text));
    }

    public void ChangeProgrammmerSalary(Slider programmerSalary) { humanResourcesManager.ChangeProgrammmerSalary((int)programmerSalary.value); }
    public void ChangeUISpecialistSalary(Slider uiSpecilistSalary) { humanResourcesManager.ChangeUISpecialistSalary((int)uiSpecialistSalarySlider.value); }
    public void ChangeIntegrabilitySpecialistSalary(Slider integrabilitySpecialistSalary) { humanResourcesManager.ChangeIntegrabilitySpecialistSalary((int) integrabilitySpecialistSalary.value); }

    //METHODS FOR UPDATING UI ELEMENTS
    public void UpdateProgramersCurrentCountText(int programmersCurrentCount)
    {
        programersCurrentCountText.text = programmersCurrentCount.ToString();
    }
    public void UpdateUserInterfaceSpecialistsCurrentCountText(int userInterfaceCurrentCount)
    {
        userInterfaceSpecialistsCurrentCountText.text = userInterfaceCurrentCount.ToString();
    }
    public void UpdateIntegrabilitySpecialistsCurrentCountText(int integrabilitySpecialistsCurrentCount)
    {
        integrabilitySpecialistsCurrentCountText.text = integrabilitySpecialistsCurrentCount.ToString();
    }
    public void UpdateProgramersAvailableCountText(int programmsersAvailableCount)
    {
        programersAvailableCountText.text = programmsersAvailableCount.ToString();
    }
    public void UpdateSpecialistsAvailableCountText(int specialistsAvailableCount)
    {
        specialistsAvailableCountText.text = specialistsAvailableCount.ToString();
    }

    public void UpdateHireProgrammersCount(int hireProgrammersCount) { this.hireProgrammersCountIF.text = hireProgrammersCount.ToString(); }
    public void UpdateHireUISpecialistsCount(int hireUISPecialistsCount) { this.hireUISpecialistsCountIF.text = hireUISpecialistsCountIF.ToString(); }
    public void UpdateHireIntegrabilitySpecialistsCount(int hireIntegrabilitySpecialistsCount) { this.hireIntegrabilitySpecialistsCountIF.text = hireIntegrabilitySpecialistsCount.ToString(); }

    public void UpdateProgrammerSalarySlider(int programmerSalary) { this.programmerSalarySlider.value = programmerSalary; }
    public void UpdateUISpecialistSalarySlider(int uiSpecialistSalary) { this.uiSpecialistSalarySlider.value = uiSpecialistSalary;  }
    public void UpdateIntegrabilitySpecialistSalarySlider(int integrabilitySpecialistSalary) { this.integrabilitySpecialistSalarySlider.value = integrabilitySpecialistSalary; }

    public void DisableAddProgrammerButton()
    {
        addProgrammersButton.interactable = false;
    }
    public void DisableSubstractProgrammerButton()
    {
        substractProgrammerButton.interactable = false;
    }
    public void DiasbleAddSpecialistButtons()
    {
        addUIpecialistButton.interactable = false;
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

    public void EnableAddProgrammerButton()
    {
        addProgrammersButton.interactable = true;
    }
    public void EnableSubstractProgrammerButton()
    {
        substractProgrammerButton.interactable = true;
    }
    public void EnasbleAddSpecialistButtons()
    {
        addUIpecialistButton.interactable = true;
        addIntegrabilitySpecialistButton.interactable = true;
    }
    public void EnableSubstractSpecialistButton()
    {
        substractUISpecialistButton.interactable = true;
        substractIntegrabilityButton.interactable = true;
    }


}
