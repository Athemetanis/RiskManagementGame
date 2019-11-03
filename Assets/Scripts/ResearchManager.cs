using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ResearchManager : NetworkBehaviour
{   
    //STORED VALUES FOR Q0, Q1, Q2, Q3, Q4, on corresponding indexes  // Q0 are default values when the game begins. 
    private SyncListInt programmersSalaryAverage = new SyncListInt() { };
    private SyncListInt programmersSalaryMin = new SyncListInt() { };
    private SyncListInt programmersSalaryMax = new SyncListInt() { };

    private SyncListInt uiSpecialistsSalaryAverage = new SyncListInt() { };
    private SyncListInt uiSpecialistsSalaryMin = new SyncListInt() { };
    private SyncListInt uiSpecialistsSalaryMax = new SyncListInt() { };

    private SyncListInt integrabilitySpecialistsSalaryAverage = new SyncListInt() { };
    private SyncListInt integrabilitySpecialistsSalaryMin = new SyncListInt() { };
    private SyncListInt integrabilitySpecialistsSalaryMax = new SyncListInt() { };

    private SyncListInt reliabilityAverage = new SyncListInt() { };
    private SyncListInt reliabilityMin = new SyncListInt() { };
    private SyncListInt reliabilityMax = new SyncListInt() { };

    private SyncListInt enterprisePriceAverage = new SyncListInt() { };
    private SyncListInt enterprisePriceMin = new SyncListInt() { };
    private SyncListInt enterprisePriceMax = new SyncListInt() { };

    private SyncListInt businessPriceAverage = new SyncListInt() { };
    private SyncListInt businessPriceMin = new SyncListInt() { };
    private SyncListInt businessPriceMax = new SyncListInt() { };

    private SyncListInt individualPriceAverage = new SyncListInt() { };
    private SyncListInt individualPriceMin = new SyncListInt() { };
    private SyncListInt individualPriceMax = new SyncListInt() { };

    private SyncListInt advertisementAverage = new SyncListInt() { };
    private SyncListInt advertisementMin = new SyncListInt() { };
    private SyncListInt advertisementMax = new SyncListInt() { };

    /*   GET THIS INFORMATIONS FROM OTHER MANAGERS WHERE HISTORY IS ALSO AVAILABLE
    //   ----------< firmName, Count>-----------
    public SyncDictionaryStringInt developersEmployees = new SyncDictionaryStringInt() { };    
    //   ----------< firmName, Value>-----------
    public SyncDictionaryStringInt providerProductFunctionality = new SyncDictionaryStringInt() { };
    public SyncDictionaryStringInt providerProductIntegrability = new SyncDictionaryStringInt() { };
    public SyncDictionaryStringInt providerProductUI = new SyncDictionaryStringInt() { };

    public SyncDictionaryStringInt partnersReliabilities = new SyncDictionaryStringInt() { };
    */

    [SyncVar (hook = "OnChangeBuyCompetitorsResearch")]
    private bool buyCompetitorsResearch;
    [SyncVar(hook = "OnChangeBuyPossiblePartnersResearch")]
    private bool buyPossiblePartnersResearch;

    private SyncListBool buyCompetitorsResearchQuarters = new SyncListBool() { };
    private SyncListBool buyPossiblePartnersResearchQuarters = new SyncListBool() { };
    private string gameID;
    private PlayerData playerData;
    private int currentQuarter;
    private ProviderResearchUIHandler providerResearchUIHandler;
    private DeveloperResearchUIHandler developerResearchUIHandler;
    
    //GETTERS & SETTERS
    public void SetProviderResearchUIHandler(ProviderResearchUIHandler providerResearchUIHandler) { this.providerResearchUIHandler = providerResearchUIHandler; }
    public void SetDeveloperResearchUIHandler(DeveloperResearchUIHandler developerResearchUIHandler) { this.developerResearchUIHandler = developerResearchUIHandler; }

    public bool GetBuyCompertitorsResearchQuarter(int quarter) { return buyCompetitorsResearchQuarters[quarter]; }
    public bool GetBuyPossiblePartnersResearchQuarter(int quarter) { return buyPossiblePartnersResearchQuarters[quarter]; }


    // Start is called before the first frame update
    void Start() { }
    public override void OnStartServer()
    {  
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = playerData.GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        SetUpDefaultValues();
        LoadDefaultValues(currentQuarter);
    }

    public override void OnStartClient()
    {
        playerData = this.gameObject.GetComponent<PlayerData>();
        gameID = playerData.GetGameID();
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
    }

    [Server]
    public void SetUpDefaultValues()
    {
        programmersSalaryAverage.Insert(0, 0);
        programmersSalaryMax.Insert(0, 0);
        programmersSalaryMin.Insert(0, 0);

        uiSpecialistsSalaryAverage.Insert(0, 0);
        uiSpecialistsSalaryMax.Insert(0, 0); ;
        uiSpecialistsSalaryMin.Insert(0, 0);

        integrabilitySpecialistsSalaryAverage.Insert(0, 0); ;
        integrabilitySpecialistsSalaryMax.Insert(0, 0);
        integrabilitySpecialistsSalaryMin.Insert(0, 0);

        reliabilityAverage.Insert(0, 0); ;
        reliabilityMax.Insert(0, 0);
        reliabilityMin.Insert(0, 0);

        enterprisePriceAverage.Insert(0, 0);
        enterprisePriceMax.Insert(0, 0);
        enterprisePriceMin.Insert(0, 0);

        businessPriceAverage.Insert(0, 0);
        businessPriceMax.Insert(0, 0);
        businessPriceMin.Insert(0, 0);

        individualPriceAverage.Insert(0, 0);
        individualPriceMax.Insert(0, 0);
        individualPriceMin.Insert(0, 0);

        advertisementAverage.Insert(0, 0);
        advertisementMax.Insert(0, 0);
        advertisementMin.Insert(0, 0);

        buyCompetitorsResearchQuarters.Insert(0, false);
        buyPossiblePartnersResearchQuarters.Insert(0, false);

        buyCompetitorsResearch = false;
        buyPossiblePartnersResearch = false;

    }

    [Server]
    public void LoadDefaultValues(int quarter)
    {
        if(programmersSalaryAverage.Count != quarter)
        {
            for (int i = programmersSalaryAverage.Count + 1; i < quarter; i++)
            {
                programmersSalaryAverage.Insert(i, 0);
                programmersSalaryMax.Insert(i, 0);
                programmersSalaryMin.Insert(i, 0);

                uiSpecialistsSalaryAverage.Insert(i, 0);
                uiSpecialistsSalaryMax.Insert(i, 0); ;
                uiSpecialistsSalaryMin.Insert(i, 0);

                integrabilitySpecialistsSalaryAverage.Insert(i, 0); ;
                integrabilitySpecialistsSalaryMax.Insert(i, 0);
                integrabilitySpecialistsSalaryMin.Insert(i, 0);

                reliabilityAverage.Insert(i, 0); ;
                reliabilityMax.Insert(i, 0);
                reliabilityMin.Insert(i, 0);

                enterprisePriceAverage.Insert(i, 0);
                enterprisePriceMax.Insert(i, 0);
                enterprisePriceMin.Insert(i, 0);

                businessPriceAverage.Insert(i, 0);
                businessPriceMax.Insert(i, 0);
                businessPriceMin.Insert(i, 0);

                individualPriceAverage.Insert(i, 0);
                individualPriceMax.Insert(i, 0);
                individualPriceMin.Insert(i, 0);

                advertisementAverage.Insert(i, 0);
                advertisementMax.Insert(i, 0);
                advertisementMin.Insert(i, 0);

                buyCompetitorsResearchQuarters.Insert(0, false);
                buyPossiblePartnersResearchQuarters.Insert(0, false);
            }
        }
        buyCompetitorsResearch = false;
        buyPossiblePartnersResearch = false;
    }


    public (int enterprisePriceAverage, int enterprisePriceMin, int enterprisePriceMax, int businessPriceAverage, int businessPriceMin, int businessPriceMax, int individualPriceAverage, int individualPriceMin, int individualPriceMax, int advertisementAverage, int advertisementMin, int advertisementMax) GetCorrespondingQuarterDataProvider(int correspondingQuarter) { return (enterprisePriceAverage[correspondingQuarter], enterprisePriceMin[correspondingQuarter], enterprisePriceMax[correspondingQuarter], businessPriceAverage[correspondingQuarter], businessPriceMin[correspondingQuarter], businessPriceMax[correspondingQuarter], individualPriceAverage[correspondingQuarter], individualPriceMin[correspondingQuarter], individualPriceMax[correspondingQuarter], advertisementAverage[correspondingQuarter], advertisementMin[correspondingQuarter], advertisementMax[correspondingQuarter]); }

    public (int programmersSalaryAverage, int programmersSalaryMin, int programmersSalaryMax, int integrabilitySpecialistsSalaryAverage, int integrabilitySpecialistsSalaryMin, int integrabilitySpecialistsSalaryMax, int uiSpecialistsSalaryAverage, int uiSpecialistsSalaryMin, int uiSpecialistsSalaryMax, int reliabilityAverage, int reliabilityMin, int reliabilityMax) GetCorrespondingQuarterDataDeveloper(int correspondingQuarter)
    {
        return (programmersSalaryAverage[correspondingQuarter], programmersSalaryMin[correspondingQuarter], programmersSalaryMax[correspondingQuarter], integrabilitySpecialistsSalaryAverage[correspondingQuarter], integrabilitySpecialistsSalaryMin[correspondingQuarter], integrabilitySpecialistsSalaryMax[correspondingQuarter], uiSpecialistsSalaryAverage[correspondingQuarter], uiSpecialistsSalaryMin[correspondingQuarter], uiSpecialistsSalaryMax[correspondingQuarter], reliabilityAverage[correspondingQuarter], reliabilityMin[correspondingQuarter], reliabilityMax[correspondingQuarter]);
    }

    public void SetBuyCompetitorsResearch(bool buyCompetitorsResearch) { CmdSetBuyCompetitorsResearch(buyCompetitorsResearch); }
    public void SetBuyPossiblePartnersResearch(bool buyPossiblePartnersResearch) { CmdSetBuyPossiblePartnersResearch(buyPossiblePartnersResearch); }
    [Command]
    public void CmdSetBuyCompetitorsResearch(bool buyCompetitorsResearch)
    {
        this.buyCompetitorsResearch = buyCompetitorsResearch;
    }
    [Command]
    public void CmdSetBuyPossiblePartnersResearch(bool buyPossiblePartnersResearch)
    {
        this.buyPossiblePartnersResearch = buyPossiblePartnersResearch;
    }

    [Server]
    public List<GameObject> GetAllDevelopers(string gameID)
    {
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);
        return developers;
    }
    [Server]
    public void ComputeSalaryValues()
    {
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);

        int developersCount = developers.Count;

        int programmersSalaryAverage = 0;
        int programmersSalarySum = 0;
        int programmersSalaryMin = 1000000;
        int programmersSalaryMax = 0;

        int uiSpecialistsSalaryAverage = 0;
        int uiSpecialistsSalarySum = 0;
        int uiSpecialistsSalaryMin = 1000000;
        int uiSpecialistsSalaryMax = 0;

        int integrabilitySpecialistsSalaryAverage = 0;
        int integrabilitySpecialistsSalarySum = 0;
        int integrabilitySpecialistsSalaryMin = 1000000;
        int integrabilitySpecialistsSalaryMax = 0;
                              
        foreach ( GameObject developer in developers)
        {   
            int programmerSalary = developer.GetComponent<HumanResourcesManager>().GetProgrammerSalaryPerQurter();
            int uiSpecialistsSalary = developer.GetComponent<HumanResourcesManager>().GetUISpecialistSalaryPerQuarter();
            int integrabilitySpecialistsSalary = developer.GetComponent<HumanResourcesManager>().GetIntegrabilitySpecialistSalaryPerQuarter();

            programmersSalarySum += programmerSalary;
            uiSpecialistsSalarySum += uiSpecialistsSalary;
            integrabilitySpecialistsSalarySum += integrabilitySpecialistsSalary;

            if(programmerSalary > programmersSalaryMax)
            {
                programmersSalaryMax = programmerSalary;
            }
            if(programmerSalary < programmersSalaryMin)
            {
                programmersSalaryMin = programmerSalary;
            }
            if(uiSpecialistsSalary > uiSpecialistsSalaryMax)
            {
                uiSpecialistsSalaryMax = uiSpecialistsSalary;
            }
            if(uiSpecialistsSalary < uiSpecialistsSalaryMin)
            {
                uiSpecialistsSalaryMin = uiSpecialistsSalary;
            }
            if(integrabilitySpecialistsSalary > integrabilitySpecialistsSalaryMax)
            {
                integrabilitySpecialistsSalaryMax = integrabilitySpecialistsSalary;
            }
            if(integrabilitySpecialistsSalary < integrabilitySpecialistsSalaryMin)
            {
                integrabilitySpecialistsSalaryMin = integrabilitySpecialistsSalary;
            }
        }
        programmersSalaryAverage = programmersSalarySum / developersCount;
        uiSpecialistsSalaryAverage = uiSpecialistsSalarySum / developersCount;
        integrabilitySpecialistsSalaryAverage = integrabilitySpecialistsSalarySum / developersCount;
        this.programmersSalaryAverage.Insert(currentQuarter, programmersSalaryAverage);
        this.uiSpecialistsSalaryAverage.Insert(currentQuarter, uiSpecialistsSalaryAverage);
        this.integrabilitySpecialistsSalaryAverage.Insert(currentQuarter, integrabilitySpecialistsSalaryAverage);
        this.programmersSalaryMax.Insert(currentQuarter, programmersSalaryMax);
        this.uiSpecialistsSalaryMax.Insert(currentQuarter, uiSpecialistsSalaryMax);
        this.integrabilitySpecialistsSalaryMax.Insert(currentQuarter, integrabilitySpecialistsSalaryMax);
        this.programmersSalaryMin.Insert(currentQuarter, programmersSalaryMin);
        this.uiSpecialistsSalaryMin.Insert(currentQuarter, uiSpecialistsSalaryMin);
        this.integrabilitySpecialistsSalaryMin.Insert(currentQuarter, integrabilitySpecialistsSalaryMin);
    }
    [Server]
    public void ComputeReliability()
    {
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);

        int developersCount = developers.Count;

        int reliabilityAverage = 0;
        int reliabilitySum = 0;
        int reliabilityMin = 100;
        int reliabilityMax = 0;

        foreach (GameObject developer in developers)
        {
            int reliability = developer.GetComponent<ContractManager>().GetReliabilityQuater(currentQuarter);
            reliabilitySum += reliability;
            if(reliability > reliabilityMax)
            {
                reliabilityMax = reliability;
            }
            if(reliability < reliabilityMin)
            {
                reliabilityMin = reliability;
            }
        }

        reliabilityAverage = reliabilitySum / developersCount;
        this.reliabilityAverage.Insert(currentQuarter, reliabilityAverage);
        this.reliabilityMin.Insert(currentQuarter, reliabilityMin);
        this.reliabilityMax.Insert(currentQuarter, reliabilityMax);
    }
    /*[Server]
    public void ComputeDevelopersEmployees()
    {
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);
        foreach (GameObject developer in developers)
        {
            int employeeCount = developer.GetComponent<HumanResourcesManager>().GetEmployeesCountQuater(currentQuarter);
            string firmName = developer.GetComponent<FirmManager>().GetFirmName();
            developersEmployees.Add(firmName, employeeCount);
        }

    }*/
    [Server]
    public void ComputeAdvertisement()
    {
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> providers = new List<GameObject>(GameHandler.allGames[gameID].GetProviderList().Values);
        int providerCount = providers.Count;

        int advertisementAverage = 0;
        int advertisementSum = 0;
        int advertisementMin = 100;
        int advertisementMax = 0;
              
        foreach (GameObject provider in providers)
        {
            int advertisement = provider.GetComponent<MarketingManager>().GetAdvertisementCoverage();
            advertisementSum += advertisement;
            if(advertisement > advertisementMax)
            {
                advertisementMax = advertisement;
            }
            if (advertisement < advertisementMin)
            {
                advertisementMin = advertisement;
            }
        }

        advertisementAverage = advertisementSum / providerCount;
        this.advertisementAverage.Insert(currentQuarter, advertisementAverage);
        this.advertisementMax.Insert(currentQuarter, advertisementMax);
        this.advertisementMin.Insert(currentQuarter, advertisementMin);
    }
    [Server]
    public void ComputePrice()
    {
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> providers = new List<GameObject>(GameHandler.allGames[gameID].GetProviderList().Values);
        int providerCount = providers.Count;

        int enterprisePriceAverage = 0;
        int enterprisePriceSum = 0;
        int enterprisePriceMin = 1000000;
        int enterprisePriceMax = 0;

        int businessPriceAverage = 0;
        int businessPriceSum = 0;
        int businessPriceMin = 100000;
        int businessPriceMax = 0;

        int individualPriceAverage = 0;
        int individualPriceSum = 0;
        int individualPriceMin = 100000;
        int individualPriceMax = 0;

        foreach (GameObject provider in providers)
        {
            int enterprisePrice = provider.GetComponent<MarketingManager>().GetEnterprisePrice();
            int businessPrice = provider.GetComponent<MarketingManager>().GetBusinessPrice();
            int individualPrice = provider.GetComponent<MarketingManager>().GetIndividualsPrice();

            enterprisePriceSum += enterprisePrice;
            businessPriceSum += businessPrice;
            individualPriceSum += individualPrice;

            if(enterprisePrice > enterprisePriceMax)
            {
                enterprisePriceMax = enterprisePrice;
            }
            if (enterprisePrice < enterprisePriceMin)
            {
                enterprisePriceMin = enterprisePrice;
            }
            if (businessPrice > businessPriceMax)
            {
                businessPriceMax = businessPrice;
            }
            if (businessPrice < businessPriceMin)
            {
                businessPriceMin = businessPrice;
            }
            if (individualPrice > individualPriceMax)
            {
                individualPriceMax = individualPrice;
            }
            if (individualPrice < individualPriceMin)
            {
                individualPriceMin = individualPrice;
            }
        }
        enterprisePriceAverage = enterprisePriceSum / providerCount;
        businessPriceAverage = businessPriceSum / providerCount;
        individualPriceAverage = individualPriceSum / providerCount;

        this.enterprisePriceAverage.Insert(currentQuarter, enterprisePriceAverage);
        this.enterprisePriceMax.Insert(currentQuarter, enterprisePriceMax);
        this.enterprisePriceMin.Insert(currentQuarter, enterprisePriceMin);
        this.businessPriceAverage.Insert(currentQuarter, businessPriceAverage);
        this.businessPriceMax.Insert(currentQuarter, businessPriceMax);
        this.businessPriceMin.Insert(currentQuarter, businessPriceMin);
        this.individualPriceAverage.Insert(currentQuarter, individualPriceAverage);
        this.individualPriceMax.Insert(currentQuarter, individualPriceMax);
        this.individualPriceMin.Insert(currentQuarter, individualPriceMin);
    }

    /*[Server]
   public void ComputeProductStats()
   {
       int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
       List<GameObject> providers = new List<GameObject>(GameHandler.allGames[gameID].GetProviderList().Values);
       int providerCount = providers.Count;


       foreach (GameObject provider in providers)
       {         
           int productFunctionality = provider.GetComponent<ProductManager>().GetFunctionality();
           int productIntegrability = provider.GetComponent<ProductManager>().GetIntegrability();
           int productUI = provider.GetComponent<ProductManager>().GetUserFrienliness();
           string firmName = provider.GetComponent<FirmManager>().GetFirmName();

           providerProductFunctionality.Add(firmName, productFunctionality);
           providerProductIntegrability.Add(firmName, productIntegrability);
           providerProductUI.Add(firmName, productUI);
       }
    }*/
    /* public void ComputePartnersReliabilities()
    {
        int currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        List<GameObject> developers = new List<GameObject>(GameHandler.allGames[gameID].GetDeveloperList().Values);
        int developersCount = developers.Count;

        foreach (GameObject developer in developers)
        {
            int reliability = developer.GetComponent<ContractManager>().GetReliabilityQuater(currentQuarter);
            string firmName = developer.GetComponent<FirmManager>().GetFirmName();
            partnersReliabilities.Add(firmName, reliability);
        }

    }*/

    public void OnChangeBuyPossiblePartnersResearch( bool buyPossiblePartnersResearch)
    {
    }
    public void OnChangeBuyCompetitorsResearch(bool buyCompetitorsResearch)
    {
    }

    //NEXT QUARTER METHODS
    [Server]
    public void SaveCurrentQuaterData()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        ComputeSalaryValues();
        ComputeReliability();
        ComputeAdvertisement();
        ComputePrice();

        buyPossiblePartnersResearchQuarters.Insert(currentQuarter, buyPossiblePartnersResearch);
        buyCompetitorsResearchQuarters.Insert(currentQuarter, buyCompetitorsResearch);
    }
    [Server]
    public void MoveToNextQuarter()
    {
        RpcEnableCorrespondingUI();
    }
    [ClientRpc]
    public void RpcEnableCorrespondingUI()
    {
        currentQuarter = GameHandler.allGames[gameID].GetGameRound();
        if (playerData.GetPlayerRole() == PlayerRoles.Provider)
        {
            if (providerResearchUIHandler != null)
            {
                providerResearchUIHandler.EnableCorrespondingQuarterUI(currentQuarter + 1);
            }
        }
        if (playerData.GetPlayerRole() == PlayerRoles.Developer)
        {
            if (developerResearchUIHandler != null)
            {
                developerResearchUIHandler.EnableCorrespondingQuarterUI(currentQuarter + 1);
            }
        }
    }
}
