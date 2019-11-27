using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndividualDecisionsFirmStrategiesHandler : MonoBehaviour
{

    public Toggle marketSize1;
    public Toggle marketSize2;
    public Toggle marketSize3;

    public Toggle competitivePosture1;
    public Toggle competitivePosture2;
    public Toggle competitivePosture3;
    public Toggle competitivePosture4;
    public Toggle competitivePosture5;

    public Toggle distictiveComp1;
    public Toggle distictiveComp2;
    public Toggle distictiveComp3;
    public Toggle distictiveComp4;
    public Toggle distictiveComp5;

    public Toggle businesspartnerDiversity1;
    public Toggle businesspartnerDiversity2;
    public Toggle businesspartnerDiversity3;

    public Toggle contractPriorities1;
    public Toggle contractPriorities2;
    public Toggle contractPriorities3;
    public Toggle contractPriorities4;

    public Toggle accountingStrategies1;
    public Toggle accountingStrategies2;
    public Toggle accountingStrategies3;

    public Toggle growthStrategies1;
    public Toggle growthStrategies2;
    public Toggle growthStrategies3;

    public Toggle developmentStrategies1;
    public Toggle developmentStrategies2;
    public Toggle developmentStrategies3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMarketsize((bool ms1, bool ms2, bool ms3) marketSize)
    {
        this.marketSize1.isOn = marketSize.ms1;
        this.marketSize2.isOn = marketSize.ms2;
        this.marketSize3.isOn = marketSize.ms3;
    }

    public void SetCompetitivePosture((bool cp1, bool cp2, bool cp3, bool cp4, bool cp5) cp)
    {
        competitivePosture1.isOn = cp.cp1;
        competitivePosture2.isOn = cp.cp2;
        competitivePosture3.isOn = cp.cp3;
        competitivePosture4.isOn = cp.cp4;
        competitivePosture5.isOn = cp.cp5;
    }

    public void SetDistinctiveCompetencies((bool dc1, bool dc2, bool dc3, bool dc4, bool dc5) dc)
    {
        distictiveComp1.isOn = dc.dc1;
        distictiveComp2.isOn = dc.dc2;
        distictiveComp3.isOn = dc.dc3;
        distictiveComp4.isOn = dc.dc4;
        distictiveComp4.isOn = dc.dc5;
    }

    public void SetBusinessPartnerDiversity((bool bpd1, bool bpd2, bool bpd3) bp)
    {
        businesspartnerDiversity1.isOn = bp.bpd1;
        businesspartnerDiversity2.isOn = bp.bpd2;
        businesspartnerDiversity3.isOn = bp.bpd3;
    }

    public void SetContractPriorities((bool cp1,bool cp2, bool cp3, bool cp4) cp)
    {
        contractPriorities1.isOn = cp.cp1;
        contractPriorities2.isOn = cp.cp2;
        contractPriorities3.isOn = cp.cp3;
        contractPriorities4.isOn = cp.cp4;
    }

    public void SetAccountingStrategies((bool as1, bool as2, bool as3) ast)
    {
        accountingStrategies1.isOn = ast.as1;
        accountingStrategies2.isOn = ast.as2;
        accountingStrategies3.isOn = ast.as3;
    }

    public void SetGrowthStrategies((bool gs1, bool gs2, bool gs3)gs)
    {
        growthStrategies1.isOn = gs.gs1;
        growthStrategies2.isOn = gs.gs2;
        growthStrategies3.isOn = gs.gs3;
    }

    public void SetDevelopmentStrategies((bool ds1, bool ds2, bool ds3)ds)
    {
        developmentStrategies1.isOn = ds.ds1;
        developmentStrategies2.isOn = ds.ds2;
        developmentStrategies3.isOn = ds.ds3;
    }

}
