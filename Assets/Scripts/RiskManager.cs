using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncDictionaryStringInt : SyncDictionary<string, int> { };
public class SyncDictionaryStringBool : SyncDictionary<string, bool> { };
public class SyncDictionaryStringColor : SyncDictionary<string, Color> { };
public class SyncListString : SyncList<string> { }

public class RiskManager : NetworkBehaviour
{

    private SyncListString risks = new SyncListString();
    //----------------------riskID, value----------------------
    private SyncDictionaryStringInt riskLikelihood = new SyncDictionaryStringInt();
    private SyncDictionaryStringInt riskImpact = new SyncDictionaryStringInt();
    private SyncDictionaryStringBool riskMonitor = new SyncDictionaryStringBool();
    private SyncDictionaryStringColor riskColor = new SyncDictionaryStringColor();

    private RiskUIHandler riskUIHandler;

    //GETTERS & SETTERS
    public void SetRiskUIHandler(RiskUIHandler riskUIHandler) { this.riskUIHandler = riskUIHandler; }
    public List<string> GetRisks() { return new List<string>(risks); }

   

    public override void OnStartClient()
    {
        // Equipment is already populated with anything the server set up
        // but we can subscribe to the callback in case it is updated later on
        riskLikelihood.Callback += OnLikelihoodChange;
        riskImpact.Callback += OnImpactChange;
        riskMonitor.Callback += OnMonitorChange;
        riskColor.Callback += OnColorChange;
        risks.Callback += OnRisksChange;
     
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

    public void OnLikelihoodChange(SyncDictionaryStringInt.Operation op, string riskID, int likelihood)
    {   if(riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
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
    public void OnMonitorChange(SyncDictionaryStringBool.Operation op, string riskID, bool monitor)
    {
       //NOTHING(already created default tree slots) or generating description slots for every risk with mitigation/monitoring on
    }
    public void OnRisksChange(SyncListString.Operation op, int index, string riskID)
    {
        Debug.Log("like" + riskLikelihood.ContainsKey(riskID) + "," + riskImpact.ContainsKey(riskID) + "," + riskMonitor.ContainsKey(riskID));
        if (riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
        {
            Debug.Log("updating points" + risks.Count);
            Debug.Log(riskLikelihood.Values.ToString());
            riskUIHandler.UpdateGraphPoints();
        }
       
    }

    public void OnColorChange(SyncDictionaryStringColor.Operation op, string riskID, Color color)
    {
        if (riskUIHandler != null & riskLikelihood.ContainsKey(riskID) & riskImpact.ContainsKey(riskID) & riskColor.ContainsKey(riskID))
        {
            Debug.Log(riskColor.ContainsKey(riskID));
            AddRiskToList(riskID);
        }
    }

    public void HighlightRisk(string riskID)
    {
        if (riskUIHandler != null)
        {
            riskUIHandler.HighlightRisk(riskID);
        }
       
    }

    public void AddRiskToList(string riskID)
    {
        CmdAddRiskToList(riskID);
        Debug.Log("added to list");
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
}
