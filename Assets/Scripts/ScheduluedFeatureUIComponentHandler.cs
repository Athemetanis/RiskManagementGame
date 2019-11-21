using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScheduluedFeatureUIComponentHandler : MonoBehaviour
{
    public RectTransform developmentTimeImageRT;
    public Image featureImage;
    public TextMeshProUGUI featureLabel;

    public RectTransform GetDevelopmentTimeRectangleRT() { return developmentTimeImageRT; }
    public Image GetFeatureImage() { return featureImage; }
    public TextMeshProUGUI GetFeatureLabel() { return featureLabel; }
    
   

    

}
