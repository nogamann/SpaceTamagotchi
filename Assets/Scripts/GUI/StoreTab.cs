using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StoreTab : MonoBehaviour
{
    public Animator storeTabsAnimator;
    int LastStoreTabIndex;// = 2;
    //Animation closeStore;
    //Animation OpenStore;
    public RectTransform panelRectTransform;
    void Start()
    {
        LastStoreTabIndex = this.transform.parent.childCount;
        Debug.Log("child count: " + LastStoreTabIndex);
    }

    public void OnClick()
    {
        //if the store is open, bring this tab forward
        if (storeTabsAnimator.GetCurrentAnimatorStateInfo(0).IsName("StoreIsOpen"))
        {
            Debug.Log("storeOpen");
            int parentIndex = this.transform.parent.GetSiblingIndex();
            
            if (parentIndex != LastStoreTabIndex)
            {
                panelRectTransform.SetAsLastSibling();
            }
            //if the player clicks on a store tab that is already open, it will close the store
            else
            {
                storeTabsAnimator.SetTrigger("playStoreAnim");
                Debug.Log("closing store");
            }
        }
        //opens the store when it's closed
        else
        {
            panelRectTransform.SetAsLastSibling();
            storeTabsAnimator.SetTrigger("playStoreAnim");
            Debug.Log("opening store");
        }

        //    if (parentIndex == LastStoreTabIndex)
        //{
        //    if (storeTabsAnimator.GetCurrentAnimatorStateInfo(0).IsName("StoreISOpen"))
        //    {
        //        int closeStoreHash = Animator.StringToHash("CloseStore");
        //        storeTabsAnimator.Play(closeStoreHash);
        //    }
        //    else
        //    {
        //        int openStoreHash = Animator.StringToHash("OpenStore");
        //        storeTabsAnimator.Play(openStoreHash);
        //    }
            
        
        
        
    }
}