using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkUIHandler : MonoBehaviour
{
    //public GameObject networkManagerObject;
    private NetworkManager manager;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
