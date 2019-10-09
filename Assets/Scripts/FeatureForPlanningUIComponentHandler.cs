using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeatureForPlanningUIComponentHandler : MonoBehaviour
{
    public Dropdown orderDropdown;
    public Text contractIDText;
    public Text partnersNameText;
    public Text contractStateText;
    public Text featureIDText;
    public Text functionalityValueText;
    public Text userInterfaceValueText;
    public Text integrabilityValueText;
    public LineRenderer graphLineRenderer;
    public InputField developmentTimeIF;
    public GameObject graphContainer;

    public Text point1Text;
    public Text point2Text;
    public Text point3Text;
    public Text point4Text;
    public Text point5Text;
    public Text point6Text;
    public Text point7Text;
    public Text point8Text;
    public Text point9Text;
    public Text point10Text;
    public Text point11Text;

    private bool initialized;
    
       
    private  ScheduleUIHandler scheduleUIHandler;
    //GETTERS & SETTERS
    public void SetContracIDText(string contractID) { contractIDText.text = contractID; }
    public void SetParnersNameText(string firmName) { partnersNameText.text = firmName; }
    public void SetContractState(ContractState contractState) { contractStateText.text = contractState.ToString(); }
    public void SetFeatureIDText(string featureID) { featureIDText.text = featureID; }
    public void SetFunctionalityValueText(int functionality) { this.functionalityValueText.text = functionality.ToString(); }
    public void SetUIValueText(int userfriendliness) { userInterfaceValueText.text = userfriendliness.ToString(); }
    public void SetIntegrabilityValueText(int integrability) { integrabilityValueText.text = integrability.ToString(); }
    public void SetDevelopmentTimeIF(int developmentTime) { developmentTimeIF.text = developmentTime.ToString(); }
    public void SetScheduleUIHandler(ScheduleUIHandler scheduleUIHandler) { this.scheduleUIHandler = scheduleUIHandler; }
    
    //METHODS

    public void SetUpFeatureForPlanning(int dropdownOptionsCount, string order, string contractID, string partnersName, ContractState contractState, string featureID, int functionality, int userfriendliness, int integrability, List<Vector3> graphPoints, int[] graphDays, int developmentTime)
    {
        initialized = false;
        GenerateDropdownOptions(dropdownOptionsCount);
        SetUIOrderDropdown(order);
        SetContracIDText(contractID);
        SetParnersNameText(partnersName);
        SetContractState(contractState);
        SetFeatureIDText(featureID);
        SetFunctionalityValueText(functionality);
        SetUIValueText(userfriendliness);
        SetIntegrabilityValueText(integrability);
        GenerateGraph(graphPoints);
        SetGraphTexts(graphDays);
        SetDevelopmentTimeIF(developmentTime);
        initialized = true;
    }

    public void GenerateDropdownOptions(int dropdownOptionsCount)
    {   
        List<string> options  = new List<string>();
        options.Add("none");  //position 0 
        
        for(int i = 1; i <= dropdownOptionsCount; i++)
        {
            options.Add(i.ToString());
        }
        orderDropdown.AddOptions(options);
    }

    public void UpdateDropdownOptions(int count)
    {
        Debug.Log("updating dropdown options");
        orderDropdown.ClearOptions();
        GenerateDropdownOptions(count);
    }
    
    public void SetUIOrderDropdown(string order)
    {   
        if(order == "none")
        {
            orderDropdown.value = 0;
        }
        else
        {
            orderDropdown.value = int.Parse(order);
        }

    }
    
   /* public void GenerateGraph(List<Vector3> graphPoints)
    {
        graphLineRenderer.positionCount = graphPoints.Count;
        
        graphLineRenderer.SetPositions(graphPoints.ToArray());
    }*/

    public void GenerateGraph(List<Vector3> graphpoints)
    {
        Vector2 lastPoint = new Vector2(0,0);
        for(int i = 0; i < graphpoints.Count; i++)
        {   
            Vector2 currentPoint = graphpoints[i];
            if (lastPoint != new Vector2(0, 0))
            {
                
                GameObject connectingLine = new GameObject("line", typeof(Image));
                connectingLine.transform.SetParent(graphContainer.transform, false);
                RectTransform connectingLineRT = connectingLine.gameObject.GetComponent<RectTransform>();
                Vector2 direction = (currentPoint - lastPoint).normalized;
                float distance = Vector2.Distance(lastPoint, currentPoint);

                connectingLineRT.anchorMin = new Vector2(0, 0);
                connectingLineRT.anchorMax = new Vector2(0, 0);
                connectingLineRT.sizeDelta = new Vector2(distance, 3f);
                connectingLineRT.anchoredPosition = lastPoint + direction * distance * 0.5f;
                connectingLineRT.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            }
            lastPoint = currentPoint;
        }
    }





    public void SetGraphTexts(int[] graphDays)
    {
        point1Text.text = graphDays[0].ToString();
        point2Text.text = graphDays[1].ToString();
        point3Text.text = graphDays[2].ToString();
        point4Text.text = graphDays[3].ToString();
        point5Text.text = graphDays[4].ToString();
        point6Text.text = graphDays[5].ToString();
        point7Text.text = graphDays[6].ToString();
        point8Text.text = graphDays[7].ToString();
        point9Text.text = graphDays[8].ToString();
        point10Text.text = graphDays[9].ToString();
        point11Text.text = graphDays[10].ToString();

    }

    public void UpdateDevelopmentTime() //trigered when InputFieldChanged
    {   
        if(int.Parse(developmentTimeIF.text) > 60)
        {
            developmentTimeIF.text = "60";
        }
        scheduleUIHandler.UpdateDevelopmentTime(contractIDText.text, int.Parse(developmentTimeIF.text));
    }

    public void UpdateOrder()
    {
        if (initialized)
        {
            //Debug.LogError("updating order on: " + orderDropdown.value);
           // Debug.LogError(contractIDText.text);
            if (orderDropdown.value == 0)
            {
                scheduleUIHandler.UpdateFeatureOrder(contractIDText.text, "none");

            }
            else
            {
                scheduleUIHandler.UpdateFeatureOrder(contractIDText.text, orderDropdown.value.ToString());
            }
        }
        
    }



}
