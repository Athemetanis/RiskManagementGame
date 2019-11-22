using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductUIHandler : MonoBehaviour
{
    //VARIABLES
    
    public TextMeshProUGUI functionalityText;
    public TextMeshProUGUI userFriendlinessText;
    public TextMeshProUGUI integrabilityText;


    private GameObject myPlayerDataObject;
    private ProductManager productManager;



    // Start is called before the first frame update
    void Start()
    {
        myPlayerDataObject = GameHandler.singleton.GetLocalPlayer().GetMyPlayerObject();
        productManager = myPlayerDataObject.GetComponent<ProductManager>();
        productManager.SetProductUIHandler(this);
    }

    //METHODS

    public void UpdateUIFunctionalityText(int functionality)
    {
        this.functionalityText.text = functionality.ToString();
    }

    public void UpdateUIUserFrienlinessText(int userFriendliness)
    {
        this.userFriendlinessText.text = userFriendliness.ToString();
    }

    public void UpdateUIIntegrabilityText(int integrability)
    {
        this.integrabilityText.text = integrability.ToString();
    }

    
}
