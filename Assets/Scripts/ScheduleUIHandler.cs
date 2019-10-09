using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduleUIHandler : MonoBehaviour
{
    public GameObject featureUIComponentForPlanningPrefab;
    public GameObject featureListContent;

    public GameObject scheduledFeatureUIComponentPrefab;
    public GameObject scheduleListContent;



    private GameObject myPlayerDataObject;
    private ScheduleManager scheduleManager;


    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        scheduleManager = myPlayerDataObject.GetComponent<ScheduleManager>();
        scheduleManager.SetScheduleUIHandler(this);
        UpdateFeatureListContent();
    }



    public void UpdateFeatureListContent()
    {   
        foreach(Transform child in featureListContent.transform)
        {GameObject.Destroy(child.gameObject);}

        foreach(ScheduledFeature scheduledFeature in scheduleManager.GetScheduledFeatures().Values)
        {
            GameObject featureUIComponentForPlanning = Instantiate(featureUIComponentForPlanningPrefab);
            featureUIComponentForPlanning.transform.SetParent(featureListContent.transform, false);
            FeatureForPlanningUIComponentHandler featureForPlanningUIComponentHandler = featureUIComponentForPlanning.GetComponent<FeatureForPlanningUIComponentHandler>();
            featureForPlanningUIComponentHandler.SetScheduleUIHandler(this);
            featureForPlanningUIComponentHandler.SetUpFeatureForPlanning(scheduleManager.GetScheduledFeatures().Values.Count, scheduledFeature.GetOrder(), scheduledFeature.GetContractID(), scheduledFeature.GetProviderFirmID(), scheduledFeature.GetContractState(), scheduledFeature.GetFeature().nameID, scheduledFeature.GetFeature().functionality, scheduledFeature.GetFeature().userfriendliness, scheduledFeature.GetFeature().integrability, scheduledFeature.GetGraphPoints(),scheduledFeature.GetGraphDays(), scheduledFeature.GetDevelopmentTime());
        }
        UpdateSchedeleListContent();
    }

    public void UpdateSchedeleListContent()
    {
        foreach (Transform child in scheduleListContent.transform)
        { GameObject.Destroy(child.gameObject); }

        Debug.Log("schedule contains " + scheduleManager.GetSchedule().Count + " features");
        int previousFeatureEnd = 0;
        for(int i = 1; i <= scheduleManager.GetScheduledFeatures().Count; i++)
        { 
            if (scheduleManager.GetSchedule().ContainsKey(i))
            {
                ScheduledFeature scheduledFeature = scheduleManager.GetScheduledFeatures()[scheduleManager.GetSchedule()[i]];
                int developmentTimeOfFeature = scheduledFeature.GetDevelopmentTime();
                int endDevelopmentTimeOfFeature = previousFeatureEnd + developmentTimeOfFeature;
                if (developmentTimeOfFeature == 0)
                {
                    return;
                }
                if(developmentTimeOfFeature > 60)
                {
                    return;
                }
                if(previousFeatureEnd > 60)
                {
                    return;
                }
                if (endDevelopmentTimeOfFeature > 60)
                {
                    GameObject scheduledFeatureUIComponent = Instantiate(scheduledFeatureUIComponentPrefab);
                    scheduledFeatureUIComponent.transform.SetParent(scheduleListContent.transform, false);
                    ScheduluedFeatureUIComponentHandler scheduledFeatureUIComponentHandler = scheduledFeatureUIComponent.GetComponent<ScheduluedFeatureUIComponentHandler>();
                    RectTransform developmentTimeVisualRepresenatation = scheduledFeatureUIComponentHandler.GetDevelopmentTimeRectangleRT();
                    scheduledFeatureUIComponentHandler.GetFeatureImage().color = Color.red;
                    scheduledFeatureUIComponentHandler.GetFeatureLabel().text = scheduledFeature.GetContractID() + '\n' + scheduledFeature.GetFeature().nameID;
                    developmentTimeVisualRepresenatation.anchoredPosition = new Vector2(previousFeatureEnd * 10, 0);
                    developmentTimeVisualRepresenatation.sizeDelta = new Vector2(600 - (previousFeatureEnd * 10), 0);
                    previousFeatureEnd = endDevelopmentTimeOfFeature;

                }
                else
                {
                    GameObject scheduledFeatureUIComponent = Instantiate(scheduledFeatureUIComponentPrefab);
                    scheduledFeatureUIComponent.transform.SetParent(scheduleListContent.transform, false);
                    ScheduluedFeatureUIComponentHandler scheduledFeatureUIComponentHandler = scheduledFeatureUIComponent.GetComponent<ScheduluedFeatureUIComponentHandler>();
                    RectTransform developmentTimeVisualRepresenatation = scheduledFeatureUIComponentHandler.GetDevelopmentTimeRectangleRT();
                    scheduledFeatureUIComponentHandler.GetFeatureImage().color = Color.green;
                    scheduledFeatureUIComponentHandler.GetFeatureLabel().text = scheduledFeature.GetContractID() + '\n' + scheduledFeature.GetFeature().nameID;
                    developmentTimeVisualRepresenatation.anchoredPosition = new Vector2(previousFeatureEnd * 10, 0);
                    developmentTimeVisualRepresenatation.sizeDelta = new Vector2(developmentTimeOfFeature * 10, 0);
                    previousFeatureEnd = endDevelopmentTimeOfFeature;
                }
                
            }
        }
    }

    public void UpdateDevelopmentTime(string contractID, int developmentTime)
    {   
        scheduleManager.UpdateScheduledFeatureDevelopmentTime(contractID, developmentTime);
    }

    public void UpdateFeatureOrder(string contractID, string newOrder)
    {
        scheduleManager.UpdateScheduledFeatureOrder(contractID, newOrder);
    }

}
