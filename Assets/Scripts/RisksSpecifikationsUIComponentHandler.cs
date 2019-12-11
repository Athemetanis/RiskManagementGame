using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RisksSpecifikationsUIComponentHandler : MonoBehaviour
{
    public TMP_InputField risk1Description;
    public TMP_InputField risk1ImpactAction;

    public TMP_InputField risk2Description;
    public TMP_InputField risk2ImpactAction;

    public TMP_InputField risk3Description;
    public TMP_InputField risk3ImpactAction;

    private bool initialized;

    private GameObject myPlayerDataObject;
    private RiskManager riskManager;

   // Start is called before the first frame update
    public void OnEnable()
    {
        initialized = false;
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        string gameID = myPlayerDataObject.GetComponent<PlayerManager>().GetGameID();
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        riskManager = myPlayerDataObject.GetComponent<RiskManager>();

        if(currentQuarter != 5)
        {
            risk1Description.text = riskManager.GetRisk1Description();
            risk1ImpactAction.text = riskManager.GetRisk1ImpactActions();
            risk2Description.text = riskManager.GetRisk2Description();
            risk2ImpactAction.text = riskManager.GetRisk2ImpactActions();
            risk3Description.text = riskManager.GetRisk3Description();
            risk3ImpactAction.text = riskManager.GetRisk3ImpactActions();

        }
        initialized = true;
    }

    //METHODS
    public void ChangeRisk1Description()
    {
        if (initialized)
        {
            riskManager.SetRisk1Description(risk1Description.text);
        }
    }
    public void ChangeRisk1ImpactAction()
    {
        if (initialized)
        {
            riskManager.SetRisk1ImpactAction(risk1ImpactAction.text);
        }
    }
    public void ChangeRisk2Description()
    {
        if (initialized)
        {
            riskManager.SetRisk2Description(risk2Description.text);
        }
    }
    public void ChangeRisk2ImpactAction()
    {
        if (initialized)
        {
            riskManager.SetRisk2ImpactAction(risk2ImpactAction.text);
        }
    }
    public void ChangeRisk3Description()
    {
        if (initialized)
        {
            riskManager.SetRisk3Description(risk3Description.text);
        }
    }
    public void ChangeRisk3ImpactAction()
    {
        if (initialized)
        {
            riskManager.SetRisk3ImpactAction(risk3ImpactAction.text);
        }
    }
}
