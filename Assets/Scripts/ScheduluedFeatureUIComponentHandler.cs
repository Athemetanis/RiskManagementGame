using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScheduluedFeatureUIComponentHandler : MonoBehaviour
{
    public RectTransform developmentTimeImageRT;
    public Image featureImage;
    public Text featureLabel;

    public RectTransform GetDevelopmentTimeRectangleRT() { return developmentTimeImageRT; }
    public Image GetFeatureImage() { return featureImage; }
    public Text GetFeatureLabel() { return featureLabel; }
    
   

    

}
