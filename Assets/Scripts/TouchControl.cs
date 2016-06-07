using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class TouchControl : MonoBehaviour
{

    private bool isDragged;
    Vector3 offset;
    Vector2 vec2Position;
    float zAxisPos;
    RaycastHit2D hit;
    Touch draggingTouch;
    Vector3 touchPosition;
    public Camera mainCamera;
    


    //TODO change to in enabled
    void Awake()
    {
        GameInputCapture.OnTouchDown += this.OnTouchDown;
        GameInputCapture.OnTouchDrag += this.OnTouchDrag;
        GameInputCapture.OnTouchUp += this.OnTouchUp;
    }


    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        isDragged = false;
    }

    void Update()
    {
       
//#if UNITY_EDITOR

//        if (Input.GetMouseButtonDown(0))
//        {
//            isDragged = true;
//            offset = (this.transform.position - Input.mousePosition);
//        }
//        if (Input.GetMouseButtonUp(0))
//        {
//            isDragged = false;
//        }

//        if (isDragged)
//        {
//            Vector3 newPos = new Vector3(Input.mousePosition.x + offset.x, Input.mousePosition.y + offset.y, zAxisPos);
//            this.transform.position = newPos;
//        }

//#endif



        //foreach (Touch touch in Input.touches)
        //{
        //    switch (touch.phase)
        //    {
        //        case (TouchPhase.Began):
        //            Debug.Log("touch");
        //            Vector3 pos = Camera.main.ScreenToRay(touch.position);
        ////
        //            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        //            if (hit.collider != null && hit.collider == this.GetComponent<Collider>())
        //            {
        //                Debug.Log("I'm hitting "); //+ hit.collider.name);

        //                draggingTouch = touch;
        //                isDragged = true;
        //                Debug.Log("isDragged");

        //            }
        //            break;

        //        case (TouchPhase.Moved):
        //            if (isDragged && touch.Equals(draggingTouch))
        //            {
        //                Vector3 deltaPositionVec3 = new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0);
        //                this.transform.position += deltaPositionVec3;
        //            }
        //            break;

        //        case (TouchPhase.Ended):
        //            if (touch.Equals(draggingTouch))
        //            {
        //                isDragged = false;
        //                Debug.Log("not Dragged");
        //            }

        //            break;
        //    }
        //}
    }

    private void OnTouchDrag(PointerEventData obj)
    {
        if (isDragged)
        {
            Vector3 touchWorldPos = mainCamera.ScreenToWorldPoint(obj.position);
            this.transform.position = touchWorldPos + offset;
        }
    }

    private void OnTouchDown(PointerEventData obj)
    {
        Collider2D hit = Physics2D.OverlapPoint(mainCamera.ScreenToWorldPoint(obj.position));
        if (hit == this.GetComponent<Collider2D>())
        {
            offset = (this.transform.position - mainCamera.ScreenToWorldPoint(obj.position));
            isDragged = true;
        }
    }


    private void OnTouchUp(PointerEventData obj)
    {
        isDragged = false;
    }

}

