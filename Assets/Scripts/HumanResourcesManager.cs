using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanResourcesManager : MonoBehaviour
{   
    //VARIABLES
    private int programmersCount;
    private int UISpecialistsCount;
    private int IntegrabilitySpecialistsCount;
    private int QASpecialistsCount;

    //GETTERS & SETTERS
    public void SetProgrammersCount(int count) { programmersCount = count; }
    public void SetUISpecialistsCount(int count) { UISpecialistsCount = count; }
    public void SetIntegrabilitySpecialistsCount(int count) { IntegrabilitySpecialistsCount = count; }
    public void SetQASpecialistsCount(int count) { QASpecialistsCount = count; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
