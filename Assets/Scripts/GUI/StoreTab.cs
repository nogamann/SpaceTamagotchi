using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreTab : MonoBehaviour
{
    Animator storeTabsAnimator;
    int LastStoreTabIndex = 2;
    Animation closeStore;
    Animation OpenStore;
    public void OnClick()
    {
        int parentIndex = this.transform.parent.GetSiblingIndex();
        if (parentIndex == LastStoreTabIndex)
        {

       
        }
        
    }
}