using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ScheduleManager : NetworkBehaviour
{
    public class SyncDictionaryIntString : SyncDictionary<int, string> { };

    //VARIABLES
    //--------------<contractID, scheduledFeature>-------------
    private Dictionary<string, ScheduledFeature> scheduledFeatures = new Dictionary<string, ScheduledFeature>();
    //--------------<order, contractID>
    private Dictionary<int, string> scheduleOrder = new Dictionary<int, string>();
    ///--------------contractID, developmentEndDay
    private Dictionary<string, int> scheduleDevelopmentEndDay = new Dictionary<string, int>();

    private ContractManager contractManager;
    private ScheduleUIHandler scheduleUIHandler;
    private HumanResourcesManager humanRecourcesManager;

    //GETTERS & SETTERS
    public Dictionary<string, ScheduledFeature> GetScheduledFeatures() { return scheduledFeatures; }
    public Dictionary<int, string> GetSchedule() { return scheduleOrder; }
    public void SetScheduleUIHandler(ScheduleUIHandler scheduleUIHandler) { this.scheduleUIHandler = scheduleUIHandler; }
    public Dictionary<string, int> GetScheduleDevelopmentEndDay() { return scheduleDevelopmentEndDay; }

    public int GetScheduledFeatureDevelopmentTime(string contractID) { return scheduledFeatures[contractID].GetDevelopmentTime(); }

    // Start is called before the first frame update
    void Start()
    {
        contractManager = this.gameObject.GetComponent<ContractManager>();
        humanRecourcesManager = this.gameObject.GetComponent<HumanResourcesManager>();
    }

    // Update is called once per frame
    public override void OnStartAuthority()
    {
        CmdSyncScheduledFeaturesOnClient();
    }

    [Command]
    public void CmdSyncScheduledFeaturesOnClient() //copying features form server to clients
    {
        foreach (ScheduledFeature sf in scheduledFeatures.Values)
        {
            sf.GetGraphPoints().ToArray();
            RpcCreateScheduledFeatureFromFeature(sf.GetOrder(), sf.GetContractID(), sf.GetProviderFirmID(), sf.GetContractState(), sf.GetFeature(), sf.GetGraphPoints().ToArray(), sf.GetGraphDays(), sf.GetDevelopmentTime(), sf.GetDeliveryTime());
        }
    }

    [ClientRpc]
    public void RpcCreateScheduledFeatureFromFeature(string order, string contractID, string providerFirmID, ContractState contractState, Feature feature, Vector3[] graphPointsArray, int[] graphDays, int developmentTime, int deliveryTime)
    {
        scheduledFeatures.Clear();
        scheduleOrder.Clear();
        scheduledFeatures.Add(contractID, new ScheduledFeature(order, contractID, providerFirmID, contractState, feature, graphPointsArray, graphDays, developmentTime, deliveryTime));
        if (order != "none")
        {
            scheduleOrder.Add(int.Parse(order), contractID);
        }
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateSchedeleListContent();
        }
        UpdateScheduleEndDevelopmentDay();



    }

    public void CreateScheduledFeature(string contractID, string providerID, ContractState contractState, Feature feature)  //called from contract manager on both server & client objets
    {
        if (scheduledFeatures.ContainsKey(contractID) == false)
        {
            ScheduledFeature scheduledFeature = new ScheduledFeature(contractID, providerID, contractState, feature);
            if (humanRecourcesManager != null)
            {
                GenerateGraph2DPoints(contractID, scheduledFeature, humanRecourcesManager.GetProgrammersCount(), humanRecourcesManager.GetUISPecialistsCount(), humanRecourcesManager.GetIntegrabilitySpecialistsCount());
            }
            else
            {
                Debug.LogError("human resource manager is null, graph points were not generated");
            }
            scheduledFeatures.Add(contractID, scheduledFeature);
        }
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
        }
        UpdateScheduleEndDevelopmentDay();

    }

    //vol8m zo serveru i z client
    [Server]
    public void DeleteScheduledFeatureServer(string contractID)
    {
        if (scheduledFeatures.ContainsKey(contractID) == true)
        {
            string deletedOrder = scheduledFeatures[contractID].GetOrder();

            //if (deletedOrder == "none")
            //{
            //scheduledFeatures.Remove(contractID);
            //}
            //else //change orders of other features, remove feature from schedule...
            // {
            scheduledFeatures[contractID].SetOrder("none");

            int count = scheduledFeatures.Count;
            if (deletedOrder == "none")
            {
                scheduledFeatures.Remove(contractID);
            }
            else
            {
                scheduledFeatures.Remove(contractID);
                scheduleOrder.Remove(int.Parse(deletedOrder));

                for (int i = int.Parse(deletedOrder) + 1; i <= scheduledFeatures.Count; i++)
                {
                    if (scheduleOrder.ContainsKey(i))
                    {

                        UpdateScheduledFeatureOrderServer(scheduleOrder[i], (i - 1).ToString());
                    }

                }
            }

            UpdateScheduleEndDevelopmentDay();
            if (scheduleUIHandler != null)
            {
                scheduleUIHandler.UpdateFeatureListContent();
            }
        }
    }

    [Client]
    public void DeleteScheduledFeatureClient(string contractID)
    {
        Debug.Log("mazem featuru ");
        if (scheduledFeatures.ContainsKey(contractID) == true)
        {
            string deletedOrder = scheduledFeatures[contractID].GetOrder();
                       
            //if (deletedOrder == "none")
            //{
            //scheduledFeatures.Remove(contractID);
            //}
            //else //change orders of other features, remove feature from schedule...
            // {
            //scheduledFeatures[contractID].SetOrder("none");
            
            int count = scheduledFeatures.Count;
            if (deletedOrder == "none")
            {  
                scheduledFeatures.Remove(contractID);
            }
            else
            {
                int deletedOrderInt = int.Parse(deletedOrder);
                Debug.Log("Menim ostatne featury kvoli zmazaniu jednej");
                
                scheduledFeatures.Remove(contractID);
                scheduleOrder.Remove(int.Parse(deletedOrder));

                for (int i = deletedOrderInt + 1; i <= count; i++)
                {
                    if (scheduleOrder.ContainsKey(i))
                    {
                        UpdateScheduledFeatureOrderClient(scheduleOrder[i], (i - 1).ToString());
                    }

                }
            }          

            UpdateScheduleEndDevelopmentDay();
            if (scheduleUIHandler != null)
            {
                scheduleUIHandler.UpdateFeatureListContent();
            }
        }
    }

    public void ChangeStateOfScheduledFeature(string contractID, ContractState contractstate)
    {
        if (scheduledFeatures[contractID].GetContractState() != contractstate)
        {
            scheduledFeatures[contractID].SetContractState(contractstate);
        }
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
        }

    }

    public void UpdateAllFeatureGraphs()
    {
        foreach(KeyValuePair<string, ScheduledFeature> scheduledFeature in scheduledFeatures)
        {
            GenerateGraph2DPoints(scheduledFeature.Key, scheduledFeature.Value, humanRecourcesManager.GetProgrammersCount(), humanRecourcesManager.GetUISPecialistsCount(), humanRecourcesManager.GetIntegrabilitySpecialistsCount());
        }
        if(scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
        }
    }

    public void GenerateGraph2DPoints(string contractID, ScheduledFeature scheduledFeature, int programmers, int uiSpecialists, int integrabilitySpecialists)
    {
        Feature feature = scheduledFeature.GetFeature();
        int averageDevelopmentTime = feature.timeCost;
        int functionality = feature.functionality;
        int userfriendliness = feature.userfriendliness;
        int integrability = feature.integrability;

        float unitComplexity = averageDevelopmentTime / (functionality + userfriendliness + integrability);
        float functionComplexity = unitComplexity * functionality;
        float uiComplexity = unitComplexity * userfriendliness;
        float integrationComplexity = unitComplexity * integrability;
        int functionDevelopmentTime = (int)System.Math.Round(functionComplexity / (programmers + uiSpecialists + integrabilitySpecialists), System.MidpointRounding.AwayFromZero);
        int uiDevelopmetTime = (int)System.Math.Round(uiComplexity / (programmers + uiSpecialists * 2 + integrabilitySpecialists), System.MidpointRounding.AwayFromZero);
        int integrabilityDevelopmentTime = (int)System.Math.Round(integrationComplexity / (programmers + uiSpecialists + integrabilitySpecialists * 2), System.MidpointRounding.AwayFromZero);

        int overallDevelopmentTime = functionDevelopmentTime + uiDevelopmetTime + integrabilityDevelopmentTime;

        int xAxisScale = 20;
        int minDevelopmentDays;
        int maxDevelopmentDays;
        List<Vector3> graphPoints = new List<Vector3>();
        int[] graphDays = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        minDevelopmentDays = (int)System.Math.Round(((float)overallDevelopmentTime * 0.8), System.MidpointRounding.AwayFromZero);
        maxDevelopmentDays = (int)System.Math.Round(((float)overallDevelopmentTime * 1.2), System.MidpointRounding.AwayFromZero);

        //Debug.Log("overallDevelopmentTime: " + overallDevelopmentTime);
        if (feature.difficulty == 1)   // y = ax+ b; a=  100/ dayrange; b = -1 * a * minimumdays;   y = percentage; x = day 
        {
            if (overallDevelopmentTime > 48)
            {
                graphPoints.Add(new Vector3(0, 0));
                double graphStep = (maxDevelopmentDays - minDevelopmentDays + 1) / 11;

                for (int i = 1; i <= 11; i++)
                {
                    graphDays[i - 1] = (int)System.Math.Round(minDevelopmentDays + (i - 1) * graphStep, System.MidpointRounding.AwayFromZero);
                    if (minDevelopmentDays + (i - 1) * graphStep < maxDevelopmentDays)
                    {
                        double x = minDevelopmentDays + (i - 1) * graphStep;
                        //     y =                                                         a * x + b             
                        double y = (100 / (double)(maxDevelopmentDays - minDevelopmentDays)) * x + ((100 / (double)(maxDevelopmentDays - minDevelopmentDays)) * (-1) * minDevelopmentDays);
                        graphPoints.Add(new Vector3(i * xAxisScale, (int)System.Math.Round(y)));
                    }
                    else
                    {
                        graphPoints.Add(new Vector3(i * xAxisScale, 100));
                    }
                }
            }
            else if(overallDevelopmentTime  < 0)
            {
                graphPoints.Add(new Vector3(0, 0));
                graphPoints.Add(new Vector3(11 * xAxisScale, 0));
                for (int i = 0; i <= 10; i++)
                {
                    graphDays[i] = 0;
                }
            }
            else if (overallDevelopmentTime == 1 || overallDevelopmentTime == 0)   //what should happen?  connstant development time? YES: 1 day
            {
                graphPoints.Add(new Vector3(0, 0));
                graphPoints.Add(new Vector3(1 * xAxisScale, 0));
                graphPoints.Add(new Vector3(1 * xAxisScale, 100));
                graphPoints.Add(new Vector3(11 * xAxisScale, 100));
                for (int i = 0; i <= 10; i++)
                {
                    graphDays[i] = i;
                }
            }
            else if (overallDevelopmentTime == 2)
            {
                graphPoints.Add(new Vector3(0, 0));
                graphPoints.Add(new Vector3(1 * xAxisScale, 0));
                graphPoints.Add(new Vector3(2 * xAxisScale, 100));
                graphPoints.Add(new Vector3(11 * xAxisScale, 100));
                for (int i = 0; i <= 10; i++)
                {
                    graphDays[i] = i;
                }
            }
            else if ((maxDevelopmentDays - minDevelopmentDays + 1) <= 11)
            {
                graphPoints.Add(new Vector3(0, 0));
                for (int i = 1; i <= 11; i++)
                {
                        graphDays[i - 1] = minDevelopmentDays + i - 1;
                        if (minDevelopmentDays + i - 1 < maxDevelopmentDays)
                        {
                            int x = minDevelopmentDays + i - 1;
                            //     y =                                                         a * x + b             
                            double y = (100 / (double)(maxDevelopmentDays - minDevelopmentDays)) * x + ((100 / (double)(maxDevelopmentDays - minDevelopmentDays)) * (-1) * minDevelopmentDays);
                            graphPoints.Add(new Vector3(i * xAxisScale, (int)System.Math.Round(y, System.MidpointRounding.AwayFromZero)));
                        }
                        else
                        {
                            graphPoints.Add(new Vector3(i * xAxisScale, 100));
                        }
                    }
                }
            else
            {
                graphPoints.Add(new Vector3(0, 0));
                int skipStep = -1;
                for (int i = 1; i <= 11; i++)
                {
                    skipStep = skipStep + 2; // computing points with step 2 
                    graphDays[i - 1] = minDevelopmentDays + skipStep - 1;

                    if (minDevelopmentDays + skipStep - 1 < maxDevelopmentDays)
                    { 
                            int x = minDevelopmentDays + skipStep - 1;
                            //     y =                                                         a * x + b             
                            double y = (100 / (double)(maxDevelopmentDays - minDevelopmentDays)) * x + ((100 / (double)(maxDevelopmentDays - minDevelopmentDays)) * (-1) * minDevelopmentDays);
                            graphPoints.Add(new Vector3(i * xAxisScale, (int)System.Math.Round(y, System.MidpointRounding.AwayFromZero)));
                    }
                    else
                    {
                            graphPoints.Add(new Vector3(i * xAxisScale, 100));
                    }
                }
            }
        }
        scheduledFeature.SetGraphDays(graphDays);
        scheduledFeature.SetGraphPoints(graphPoints.ToArray());
    }      

    public void UpdateScheduledFeatureOrder(string contractID, string newOrder)
    {
        string oldOrder = scheduledFeatures[contractID].GetOrder();
        if (oldOrder == newOrder)
        {
            return; //nic sa nemeni..
        }

        CmdUpdateScheduledFeatureOrder(contractID, newOrder);
    }
    [Command]
    public void CmdUpdateScheduledFeatureOrder(string contractID, string newOrder)
    {
        string oldOrder = scheduledFeatures[contractID].GetOrder();

        //poradie sa zmenilo

        //ak stare poradie bolo none tak nove poradie je cislo
        //ak sa nachadza v plane uz featura s mojim novym cislom odstanim ju z planu a nastavim jej none
        //potom iba pridam do planu moju novu featuru a aktualizujem jej cislo
        if (oldOrder == "none")
        {
            Debug.Log("PREBIEHA ZMENA PORADIA FEATURY " + contractID + " na: " + newOrder);
            if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
            {
                scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                scheduleOrder.Remove(int.Parse(newOrder));
            }
            scheduleOrder.Add(int.Parse(newOrder), contractID);
            scheduledFeatures[contractID].SetOrder((newOrder));
        }

        // feature uz bola zaradena do planu, musim ju z tade odstranit
        //ak jej nove poradie je none aktualizujem jej cislo
        //ak jej nove poradie je cislo pridam ju do planu s novym cislom a aktualizujem ju + odstanim z planu ak tam je featuru s mojim novym cislo a nastavim jej none
        if (oldOrder != "none")
        {
            if (newOrder == "none") //odstanim z planu a aktualizujem hodnotu featury
            {
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
            }
            else
            {
                if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
                {
                    scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                    scheduleOrder.Remove(int.Parse(newOrder));
                }
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
                scheduleOrder.Add(int.Parse(newOrder), contractID);
            }
        }
        RpcUpdateScheduledFeatureOrder(contractID, newOrder);
        UpdateScheduleEndDevelopmentDay();

    }
    [ClientRpc]
    public void RpcUpdateScheduledFeatureOrder(string contractID, string newOrder)
    {
        Debug.Log("PREBIEHA ZMENA PORADIA FEATURY " +  contractID + " na: " + newOrder);
        string oldOrder = scheduledFeatures[contractID].GetOrder();

        //poradie sa zmenilo

        //ak stare poradie bolo none tak nove poradie je cislo
        //ak sa nachadza v plane uz featura s mojim novym cislom odstanim ju z planu a nastavim jej none
        //potom iba pridam do planu moju novu featuru a aktualizujem jej cislo
        if (oldOrder == "none")
        {
            if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
            {
                scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                scheduleOrder.Remove(int.Parse(newOrder));
            }
            scheduleOrder.Add(int.Parse(newOrder), contractID);
            scheduledFeatures[contractID].SetOrder((newOrder));
        }

        // feature uz bola zaradena do planu, musim ju z tade odstranit
        //ak jej nove poradie je none aktualizujem jej cislo
        //ak jej nove poradie je cislo pridam ju do planu s novym cislom a aktualizujem ju + odstanim z planu ak tam je featuru s mojim novym cislo a nastavim jej none
        if (oldOrder != "none")
        {
            if (newOrder == "none") //odstanim z planu a aktualizujem hodnotu featury
            {
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
            }
            else  //new order is number 
            {
                if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
                {
                    scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                    scheduleOrder.Remove(int.Parse(newOrder));
                }
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
                scheduleOrder.Add(int.Parse(newOrder), contractID);
            }
        }
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent(); //changing input fields
            //
        }
        Debug.Log("POCET FEATURE KTORE SU V SCHEDULE JE TERAZ: " + scheduleOrder.Count);
        foreach(KeyValuePair<int, string> scheduledContract in scheduleOrder)
        {
            Debug.Log("Feature order: " + scheduledContract.Key + " , contractID " + scheduledContract.Value);
        }
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
            scheduleUIHandler.UpdateSchedeleListContent();
            UpdateScheduleEndDevelopmentDay();
        }

    }

    [Server]
    public void UpdateScheduledFeatureOrderServer(string contractID, string newOrder)
    {
        string oldOrder = scheduledFeatures[contractID].GetOrder();

        //poradie sa zmenilo

        //ak stare poradie bolo none tak nove poradie je cislo
        //ak sa nachadza v plane uz featura s mojim novym cislom odstanim ju z planu a nastavim jej none
        //potom iba pridam do planu moju novu featuru a aktualizujem jej cislo
        if (oldOrder == "none")
        {
            Debug.Log("PREBIEHA ZMENA PORADIA FEATURY " + contractID + " na: " + newOrder);
            if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
            {
                scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                scheduleOrder.Remove(int.Parse(newOrder));
            }
            scheduleOrder.Add(int.Parse(newOrder), contractID);
            scheduledFeatures[contractID].SetOrder((newOrder));
        }

        // feature uz bola zaradena do planu, musim ju z tade odstranit
        //ak jej nove poradie je none aktualizujem jej cislo
        //ak jej nove poradie je cislo pridam ju do planu s novym cislom a aktualizujem ju + odstanim z planu ak tam je featuru s mojim novym cislo a nastavim jej none
        if (oldOrder != "none")
        {
            Debug.Log("PREBIEHA ZMENA PORADIA FEATURY " + contractID + " na: " + newOrder);
            if (newOrder == "none") //odstanim z planu a aktualizujem hodnotu featury
            {
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
            }
            else
            {
                if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
                {
                    scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                    scheduleOrder.Remove(int.Parse(newOrder));
                }
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
                scheduleOrder.Add(int.Parse(newOrder), contractID);
            }
        }
        UpdateScheduleEndDevelopmentDay();
    }

    [Client]
    public void UpdateScheduledFeatureOrderClient(string contractID, string newOrder)
    {
        Debug.Log("Updatujem poradie featury na klientovi");
        string oldOrder = scheduledFeatures[contractID].GetOrder();

        //poradie sa zmenilo

        //ak stare poradie bolo none tak nove poradie je cislo
        //ak sa nachadza v plane uz featura s mojim novym cislom odstanim ju z planu a nastavim jej none
        //potom iba pridam do planu moju novu featuru a aktualizujem jej cislo
        if (oldOrder == "none")
        {
            Debug.Log("PREBIEHA ZMENA PORADIA FEATURY " + contractID + " na: " + newOrder);
            if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
            {
                scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                scheduleOrder.Remove(int.Parse(newOrder));
            }
            scheduleOrder.Add(int.Parse(newOrder), contractID);
            scheduledFeatures[contractID].SetOrder((newOrder));
        }

        // feature uz bola zaradena do planu, musim ju z tade odstranit
        //ak jej nove poradie je none aktualizujem jej cislo
        //ak jej nove poradie je cislo pridam ju do planu s novym cislom a aktualizujem ju + odstanim z planu ak tam je featuru s mojim novym cislo a nastavim jej none
        if (oldOrder != "none")
        {
            Debug.Log("PREBIEHA ZMENA PORADIA FEATURY " + contractID + " na: " + newOrder);
            if (newOrder == "none") //odstanim z planu a aktualizujem hodnotu featury
            {
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
            }
            else
            {
                if (scheduleOrder.ContainsKey(int.Parse(newOrder)))
                {
                    scheduledFeatures[scheduleOrder[int.Parse(newOrder)]].SetOrder("none");
                    scheduleOrder.Remove(int.Parse(newOrder));
                }
                scheduleOrder.Remove(int.Parse(oldOrder));
                scheduledFeatures[contractID].SetOrder((newOrder));
                scheduleOrder.Add(int.Parse(newOrder), contractID);
            }
        }
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
            scheduleUIHandler.UpdateSchedeleListContent();
        }
        UpdateScheduleEndDevelopmentDay();
    }

    
    public void UpdateScheduledFeatureDevelopmentTime(string contractID, int developmentTime)  
    {
        CmdUpdateScheduledFeatureDevelopmentTime(contractID, developmentTime);
    }

    [Command]
    public void CmdUpdateScheduledFeatureDevelopmentTime(string contractID, int developmentTime)
    {
        scheduledFeatures[contractID].SetDevelopmentTime(developmentTime);
        RpcUpdateScheduledFeatureDevelopmentTime(contractID, developmentTime);
        UpdateScheduleEndDevelopmentDay();
    }

    [ClientRpc]
    public void RpcUpdateScheduledFeatureDevelopmentTime(string contractID, int developmentTime)
    {
        scheduledFeatures[contractID].SetDevelopmentTime(developmentTime);
        if (scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateSchedeleListContent();
        }
        UpdateScheduleEndDevelopmentDay();
    }

    public void UpdateScheduleEndDevelopmentDay()
    {
        Debug.Log("updateujem schedule end development day");
        scheduleDevelopmentEndDay.Clear();
        int previousFeatureEnd = 0;
        for (int i = 1; i <= scheduledFeatures.Count; i++)
        {
            if (scheduleOrder.ContainsKey(i))
            {
                ScheduledFeature scheduledFeature =scheduledFeatures[scheduleOrder[i]];
                int developmentTimeOfFeature = scheduledFeature.GetDevelopmentTime();
                if (developmentTimeOfFeature == 0)
                {
                    return;
                }
                else
                {
                    scheduleDevelopmentEndDay.Add(scheduledFeature.GetContractID(), previousFeatureEnd + scheduledFeature.GetDevelopmentTime());
                    previousFeatureEnd = previousFeatureEnd + scheduledFeature.GetDevelopmentTime();
                }
               
            }
        }
        if(scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
            scheduleUIHandler.UpdateSchedeleListContent();
        }        
    }

    public void EvaluateTrueDevelopmentTime()
    {
        int previousFetureEnd = 0;
        for (int i = 1; i <= scheduledFeatures.Count; i++)
        {
            if (scheduleOrder.ContainsKey(i))
            {   
                Feature feature = scheduledFeatures[scheduleOrder[i]].GetFeature();
                string contractID = scheduledFeatures[scheduleOrder[i]].GetContractID();
                //contractManager.GetMyContracts()[contractID].SetStartDevelopmentDay(previousFetureEnd);
                int averageDevelopmentTime = feature.timeCost;
                int functionality = feature.functionality;
                int userfriendliness = feature.userfriendliness;
                int integrability = feature.integrability;

                float unitComplexity = averageDevelopmentTime / (functionality + userfriendliness + integrability);
                float functionComplexity = unitComplexity * functionality;
                float uiComplexity = unitComplexity * userfriendliness;
                float integrationComplexity = unitComplexity * integrability;

                int programmers = humanRecourcesManager.GetProgrammersCount();
                int uiSpecialists = humanRecourcesManager.GetUISPecialistsCount();
                int integrabilitySpecialists = humanRecourcesManager.GetIntegrabilitySpecialistsCount();

                int functionDevelopmentTime = (int)System.Math.Round(functionComplexity / (programmers + uiSpecialists + integrabilitySpecialists), System.MidpointRounding.AwayFromZero);
                int uiDevelopmetTime = (int)System.Math.Round(uiComplexity / (programmers + uiSpecialists * 2 + integrabilitySpecialists), System.MidpointRounding.AwayFromZero);
                int integrabilityDevelopmentTime = (int)System.Math.Round(integrationComplexity / (programmers + uiSpecialists + integrabilitySpecialists * 2), System.MidpointRounding.AwayFromZero);

                int overallDevelopmentTime = functionDevelopmentTime + uiDevelopmetTime + integrabilityDevelopmentTime;

                //int xAxisScale = 20;
                int minDevelopmentDays;
                int maxDevelopmentDays;

                int trueDevelopmentTime = 0;
                int truDevelopmentEnd = 0;                //List<Vector3> graphPoints = new List<Vector3>();
                ///int[] graphDays = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                minDevelopmentDays = (int)System.Math.Round(((float)overallDevelopmentTime * 0.8), System.MidpointRounding.AwayFromZero);
                maxDevelopmentDays = (int)System.Math.Round(((float)overallDevelopmentTime * 1.2), System.MidpointRounding.AwayFromZero);

                if (feature.difficulty == 1)   // y = ax+ b; a=  100/ dayrange; b = -1 * a * minimumdays;   y = percentage; x = day 
                {   
                    if(previousFetureEnd >= 60)  //development canceled
                    {
                        trueDevelopmentTime = 0;
                        truDevelopmentEnd = 0;
                    }
                    else if (overallDevelopmentTime < 0) //imposible development 
                    {
                        trueDevelopmentTime = 0;
                        truDevelopmentEnd = 0;
                    }
                    if (overallDevelopmentTime == 1 || overallDevelopmentTime == 0)  //very shor development time 
                    {
                        trueDevelopmentTime = 1;
                        truDevelopmentEnd = previousFetureEnd + trueDevelopmentTime;
                    }
                    else  
                    {
                        int percentage = Random.Range(1, 101);
                        int o = (maxDevelopmentDays - minDevelopmentDays) / 100;
                        int h = minDevelopmentDays;
                        trueDevelopmentTime = o * percentage + h;
                        truDevelopmentEnd = previousFetureEnd + trueDevelopmentTime;
                    }
                }
                Contract myContract = contractManager.GetMyContracts()[contractID];
                myContract.SetTrueDeliveryTime(truDevelopmentEnd);
                previousFetureEnd = truDevelopmentEnd;
            }            
        }
    }


    [Server]
    public void MoveToNextQuarter()
    {
        scheduledFeatures.Clear();
        scheduleOrder.Clear();
        scheduleDevelopmentEndDay.Clear();
        RpcScheduleClearance();
    }

    [ClientRpc]
    public void RpcScheduleClearance()
    {
        scheduledFeatures.Clear();
        scheduleOrder.Clear();
        scheduleDevelopmentEndDay.Clear();
        if(scheduleUIHandler != null)
        {
            scheduleUIHandler.UpdateFeatureListContent();
            scheduleUIHandler.UpdateSchedeleListContent();
        }
    }



}
