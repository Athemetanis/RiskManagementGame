using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncDictionaryStringInt : SyncDictionary<string, int> { };
public class SyncDictionaryStringBool : SyncDictionary<string, bool> { };
public class SyncDictionaryStringColor : SyncDictionary<string, Color> { };
public class SyncListString : SyncList<string> { }

public class RiskManager : NetworkBehaviour    ///max 32 sync variables....
{
    //STORED VALUES FOR Q0, Q1, Q2, Q3, Q4, on corresponding indexes: 0 1 2 3 4
    private SyncListString risk1DescriptionQuarters = new SyncListString() {};
    private SyncListString risk1ImpactActionQuarters = new SyncListString() {};
    private SyncListString risk2DescriptionQuarters = new SyncListString() {};
    private SyncListString risk2ImpactActionQuarters = new SyncListString() {};
    private SyncListString risk3DescriptionQuarters = new SyncListString() {};
    private SyncListString risk3ImpactActionQuarters = new SyncListString() {};
    //--MATRIX Q3
    private SyncListString risksQ3 = new SyncListString();
    private SyncDictionaryStringInt riskLikelihoodQ3 = new SyncDictionaryStringInt();
    private SyncDictionaryStringInt riskImpactQ3 = new SyncDictionaryStringInt();
    private SyncDictionaryStringBool riskMonitorQ3 = new SyncDictionaryStringBool();
    private SyncDictionaryStringColor riskColorQ3 = new SyncDictionaryStringColor();
    //--MATRIX Q4
    private SyncListString risksQ4 = new SyncListString();
    private SyncDictionaryStringInt riskLikelihoodQ4 = new SyncDictionaryStringInt();
    private SyncDictionaryStringInt riskImpactQ4 = new SyncDictionaryStringInt();
    private SyncDictionaryStringBool riskMonitorQ4 = new SyncDictionaryStringBool();
    private SyncDictionaryStringColor riskColorQ4 = new SyncDictionaryStringColor();

    //CURRENT VALUES
    // index: 0 = name/description, 1 = imapact & actions;
    [SyncVar]
    private string risk1Description;
    [SyncVar]
    private string risk1ImpactAction;
    [SyncVar]
    private string risk2Description;
    [SyncVar]
    private string risk2ImpactAction;
    [SyncVar]
    private string risk3Description;
    [SyncVar]
    private string risk3ImpactAction;

    //MATRIX 
    private SyncListString risks = new SyncListString();
    //----------------------riskID, value----------------------
    private SyncDictionaryStringInt riskLikelihood = new SyncDictionaryStringInt();
    private SyncDictionaryStringInt riskImpact = new SyncDictionaryStringInt();
    private SyncDictionaryStringBool riskMonitor = new SyncDictionaryStringBool();
    private SyncDictionaryStringColor riskColor = new SyncDictionaryStringColor();


    private string gameID;
    private int currentQuarter;

    //REFERENCES
    private RiskUIHandler riskUIHandler;

    //GETTERS & SETTERS
    public void SetRiskUIHandler(RiskUIHandler riskUIHandler) { this.riskUIHandler = riskUIHandler; }
    public List<string> GetRisks() { return new List<string>(risks); }
    public void SetRisk1Description(string description) { risk1Description = description; }
    public void SetRisk2Description(string description) { risk2Description = description; }
    public void SetRisk3Description(string description) { risk3Description = description; }
    public void SetRisk1ImpactAction(string impactAction) { risk1ImpactAction = impactAction; }
    public void SetRisk2ImpactAction(string impactAction) { risk2ImpactAction = impactAction; }
    public void SetRisk3ImpactAction(string impactAction) { risk3ImpactAction = impactAction; }
    public string GetRisk1Description() { return risk1Description; }
    public string GetRisk2Description() { return risk2Description; }
    public string GetRisk3Description() { return risk3Description; }
    public string GetRisk1ImpactActions() { return risk1ImpactAction; }
    public string GetRisk2ImpactActions() { return risk2ImpactAction; }
    public string GetRisk3ImpactActions() { return risk3ImpactAction; }

    public string GetRisk1DescriptionQ(int quarter) { return risk1DescriptionQuarters[quarter]; }
    public string GetRisk2DescriptionQ(int quarter) { return risk2DescriptionQuarters[quarter]; }
    public string GetRisk3DescriptionQ(int quarter) { return risk3DescriptionQuarters[quarter]; }
    public string GetRisk1ImpactActionsQ(int quarter) { return risk1ImpactActionQuarters[quarter]; }
    public string GetRisk2ImpactActionsQ(int quarter) { return risk2ImpactActionQuarters[quarter]; }
    public string GetRisk3ImpactActionsQ(int quarter) { return risk3ImpactActionQuarters[quarter]; }




    public override void OnStartServer()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if(risk1DescriptionQuarters.Count == 0)
        {
            SetDefeaultValues();
        }
        LoadQuarterData(currentQuarter);
    }

    public override void OnStartClient()
    {
        gameID = this.gameObject.GetComponent<PlayerData>().GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        // syncDict is already populated with anything the server set up
        // but we can subscribe to the callback in case it is updated later on
        riskLikelihood.Callback += OnLikelihoodChange;
        riskImpact.Callback += OnImpactChange;
        riskColor.Callback += OnColorChange;
        risks.Callback += OnRisksChange;
    }

    [Server]
    public void SetDefeaultValues()
    {
        risk1DescriptionQuarters.Insert(0, "");
        risk1ImpactActionQuarters.Insert(0, "");
        risk2DescriptionQuarters.Insert(0, "");
        risk2ImpactActionQuarters.Insert(0, "");
        risk3DescriptionQuarters.Insert(0, "");
        risk3ImpactActionQuarters.Insert(0, "");
    }
    
    //METHODS
    [Server]
    public void LoadQuarterData(int quarter)
    {   
        if (quarter == 4)
        {
            if (risksQ3.Count > 0 && risks.Count == 0) // player did not join already running game in Q4  so there are some data to be loaded from Q3
            {
                foreach (string risk in risksQ3)
                {
                    risks.Add(risk);
                }
                foreach (KeyValuePair<string, int> likelihood in riskLikelihoodQ3)
                {
                    riskLikelihood.Add(likelihood);
                }
                foreach (KeyValuePair<string, int> impact in riskImpactQ3)
                {
                    riskImpact.Add(impact);
                }
                foreach (KeyValuePair<string, bool> monitor in riskMonitorQ3)
                {
                    riskMonitor.Add(monitor);
                }
                foreach (KeyValuePair<string, Color> color in riskColorQ3)
                {
                    riskColor.Add(color);
                }
            }
            if(risk1DescriptionQuarters.Count != 4)
            {
               for(int i  = risk1DescriptionQuarters.Count + 1; i < quarter; i++)
                {
                    risk1DescriptionQuarters.Insert(i, risk1DescriptionQuarters[i - 1]);
                    risk2DescriptionQuarters.Insert(i, risk2DescriptionQuarters[i - 1]);
                    risk3DescriptionQuarters.Insert(i, risk3DescriptionQuarters[i - 1]);
                    risk1ImpactActionQuarters.Insert(i, risk1ImpactActionQuarters[i - 1]);
                    risk2ImpactActionQuarters.Insert(i, risk2ImpactActionQuarters[i - 1]);
                    risk3ImpactActionQuarters.Insert(i, risk3ImpactActionQuarters[i - 1]);
                }
            }
            risk1Description = risk1DescriptionQuarters[quarter - 1];
            risk2Description = risk2DescriptionQuarters[quarter - 1];
            risk3Description = risk3DescriptionQuarters[quarter - 1];
            risk1ImpactAction = risk1ImpactActionQuarters[quarter - 1];
            risk2ImpactAction = risk2ImpactActionQuarters[quarter - 1];
            risk3ImpactAction = risk3ImpactActionQuarters[quarter - 1];
        }
        else
        risk1Description = "";
        risk1ImpactAction = "";
        risk2Description = "";
        risk2ImpactAction = "";
        risk3Description = "";
        risk3ImpactAction = "";
    }

    public bool ContainsRisk(string riskID)
    {
        if (risks.Contains(riskID))
        {
            return true;
        }
        else return false;
    }

    public Color GetColor(string riskID)
    {
        return riskColor[riskID];
    }
    public int GetLikelihood(string riskID)
    {
        return riskLikelihood[riskID];
    }
    public int GetImpact(string riskID)
    {
        return riskImpact[riskID];
    }
    public bool GetMonitor(string riskID)
    {
        return riskMonitor[riskID];
    }

    public void AddRisk(string riskID, int likelihood, int impact, bool monitor, Color color)
    {
        CmdAddRisk(riskID, likelihood, impact, monitor, color);
    }
    [Command]
    public void CmdAddRisk(string riskID, int likelihood, int impact, bool monitor, Color color)
    {
        riskLikelihood.Add(riskID, likelihood);
        riskImpact.Add(riskID, impact);
        riskMonitor.Add(riskID, monitor);
        riskColor.Add(riskID, color);

    }
    public void UpdateLikelihood(string riskID, int likelihood)
    {
        CmdUpdateLikelihood(riskID, likelihood);
        Debug.Log("likelinessCHangedone");
    }
    [Command]
    public void CmdUpdateLikelihood(string riskID, int likelihood)
    {
        if (riskLikelihood.ContainsKey(riskID))
        {
            riskLikelihood[riskID] = likelihood;
        }
        else
        {
            Debug.LogWarning("risk not found");
        }
    }
    public void UpdateImpact(string riskID, int impact)
    {
        CmdUpdateImpact(riskID, impact);
    }
    [Command]
    public void CmdUpdateImpact(string riskID, int impact)
    {
        if (riskImpact.ContainsKey(riskID))
        {
            riskImpact[riskID] = impact;
        }
        else
        {
            Debug.LogWarning("risk not found");
        }
    }
    public void UpdateMonitor(string riskID, bool monitor)
    {
        CmdUpdateMonitor(riskID, monitor);
    }
    [Command]
    public void CmdUpdateMonitor(string riskID, bool monitor)
    {
        if (riskMonitor.ContainsKey(riskID))
        {
            riskMonitor[riskID] = monitor;
        }
        else
        {
            Debug.LogWarning("risk not found");
        }
    }

    //HOOKS
    public void OnLikelihoodChange(SyncDictionaryStringInt.Operation op, string riskID, int likelihood)
    { if (riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
        {
            AddRiskToList(riskID);
        }
    }
    public void OnImpactChange(SyncDictionaryStringInt.Operation op, string riskID, int impact)
    {
        if (riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
        {
            AddRiskToList(riskID);
        }

    }
    public void OnRisksChange(SyncListString.Operation op, int index, string riskID)
    {
        //Debug.Log("like" + riskLikelihood.ContainsKey(riskID) + "," + riskImpact.ContainsKey(riskID) + "," + riskMonitor.ContainsKey(riskID));
        if (riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
        {
            //Debug.Log("updating points" + risks.Count);
           // Debug.Log(riskLikelihood.Values.ToString());
            riskUIHandler.UpdateGraphPoints();
        }

    }
    public void OnColorChange(SyncDictionaryStringColor.Operation op, string riskID, Color color)
    {
        if (riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
        {
            //Debug.Log(riskColor.ContainsKey(riskID));
            AddRiskToList(riskID);
        }
    }

    public void AddRiskToList(string riskID)
    {
        CmdAddRiskToList(riskID);
        //Debug.Log("added to list");
    }
    [Command]
    public void CmdAddRiskToList(string riskID)
    {
        if (risks.Contains(riskID))
        {
            risks.Remove(riskID);
            risks.Add(riskID);
        }
        else
        {
            risks.Add(riskID);

        }

    }

    // NEXT QUARTER EVALUATION METHODS...
    [Server]
    public void MoveToTheNextQuarter()
    {
        SetNewReferences();
        LoadNextQuarterData(currentQuarter);
    }

    [Server]
    public void SaveCurrentQuarterData()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound(); 

        risk1DescriptionQuarters.Insert(currentQuarter, risk1Description);
        risk1ImpactActionQuarters.Insert(currentQuarter,  risk1ImpactAction);
        risk2DescriptionQuarters.Insert(currentQuarter, risk2Description);
        risk2ImpactActionQuarters.Insert(currentQuarter, risk2ImpactAction);
        risk3DescriptionQuarters.Insert(currentQuarter, risk3Description);
        risk3ImpactActionQuarters.Insert(currentQuarter, risk3ImpactAction);

        if (currentQuarter == 3)
        {
            foreach (string risk in risks)
            {
                risksQ3.Add(risk);
            }
            foreach (KeyValuePair<string, int> likelihood in riskLikelihood)
            {
                riskLikelihoodQ3.Add(likelihood);
            }
            foreach (KeyValuePair<string, int> impact in riskImpact)
            {
                riskImpactQ3.Add(impact);
            }
            foreach (KeyValuePair<string, bool> monitor in riskMonitor)
            {
                riskMonitorQ3.Add(monitor);
            }
            foreach (KeyValuePair<string, Color> color in riskColor)
            {
                riskColorQ3.Add(color);
            }
        }
        if (currentQuarter == 4)
        {
            foreach (string risk in risks)
            {
                risksQ4.Add(risk);
            }
            foreach (KeyValuePair<string, int> likelihood in riskLikelihood)
            {
                riskLikelihoodQ4.Add(likelihood);
            }
            foreach (KeyValuePair<string, int> impact in riskImpact)
            {
                riskImpactQ4.Add(impact);
            }
            foreach (KeyValuePair<string, bool> monitor in riskMonitor)
            {
                riskMonitorQ4.Add(monitor);
            }
            foreach (KeyValuePair<string, Color> color in riskColor)
            {
                riskColorQ4.Add(color);
            }
        }
    }

    public void SetNewReferences()
    {
        RpcSetNewReferences();
    }

    [ClientRpc]
    public void RpcSetNewReferences()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if (riskUIHandler != null)
        {
            riskUIHandler.EnableCorrespondingUI(currentQuarter + 1);
        }
    }

    public void LoadNextQuarterData(int currentQuarter)
    {
        LoadQuarterData(currentQuarter + 1 );
    }

    private void Start()
    {

    }
}
