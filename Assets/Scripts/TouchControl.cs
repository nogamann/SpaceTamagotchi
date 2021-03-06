﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Networking;

public class TouchControl : NetworkBehaviour
{

    private bool isDragged;
	public bool isTickled;
    Vector3 offset;
    Vector2 vec2Position;
    float zAxisPos;
    RaycastHit2D hit;
    Touch draggingTouch;
    Vector3 touchPosition;
    public Camera mainCamera;
	public bool disabled = false;

    Creature creature;


    //TODO change to in enabled
    void Awake()
    {
        GameInputCapture.OnTouchDown += this.OnTouchDown;
        GameInputCapture.OnTouchDrag += this.OnTouchDrag;
        GameInputCapture.OnTouchUp += this.OnTouchUp;

		mainCamera = FindObjectOfType<Camera>();
    }


    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        isDragged = false;
        creature = FindObjectOfType<Creature>();
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
		if (isDragged & hasAuthority) {
			Vector3 touchWorldPos = mainCamera.ScreenToWorldPoint (obj.position);

			this.transform.position = touchWorldPos + offset;
		}
    }

    private void OnTouchDown(PointerEventData obj)
    {
		int creatureLayer = LayerMask.GetMask ("creature");
		Collider2D hitCreature = Physics2D.OverlapPoint (mainCamera.ScreenToWorldPoint (obj.position), creatureLayer);
		if (hitCreature != null) {
//			Debug.Log ("touched creature");
			isTickled = true;
//			creature.tickled ();
		}

		// ignore disable items - food that's been eaten etc.
		if (!disabled){
			// Original code
	        Collider2D hit = Physics2D.OverlapPoint(mainCamera.ScreenToWorldPoint(obj.position));
	        if (hit != null && hit == this.GetComponent<Collider2D>())
	        {
				// set authority to the local player that touched the item
				GameObject[] objects = GameObject.FindGameObjectsWithTag ("Finger");
				PlayerController local = null;
				foreach (GameObject go in objects){
					PlayerController pc = go.GetComponent<PlayerController>();
					if (pc.isLocalPlayer) {
						local = pc;
					}
				}
				Debug.Assert (gameObject != null);
				Debug.Assert (local != null);
				local.GrabItem (gameObject);


	            offset = (this.transform.position - mainCamera.ScreenToWorldPoint(obj.position));
	            isDragged = true;

				this.GetComponent<SpriteRenderer> ().color = new Color (.5f, .5f, .5f, .5f);
	        }

		}
    }

    private void OnTouchUp(PointerEventData obj)
    {
		

		if (!disabled){
			if (isDragged) {
				// release client authority
				GameObject[] objects = GameObject.FindGameObjectsWithTag ("Finger");
				PlayerController local = null;
				foreach (GameObject go in objects) {
					PlayerController pc = go.GetComponent<PlayerController> ();
					if (pc.isLocalPlayer) {
						local = pc;
					}
				}

				local.FreeItem (gameObject);
				isDragged = false;
				this.GetComponent<SpriteRenderer> ().color = new Color (1,1,1,1);

				int creatureLayer = LayerMask.GetMask ("creature");
				Collider2D hit = Physics2D.OverlapPoint (mainCamera.ScreenToWorldPoint (obj.position), creatureLayer);
				if (hit != null) {
					Debug.Log ("hit creature");
					Debug.Assert (this.GetComponent<ThingObject> () != null);
					//            creature.ChooseAction(this.GetComponent<ThingObject>());
					creature.ChooseAction (gameObject);
					return;
				}

			}
					

			if (isTickled) {
				int creatureLayer = LayerMask.GetMask ("creature");
				Collider2D hit = Physics2D.OverlapPoint (mainCamera.ScreenToWorldPoint (obj.position), creatureLayer);
				if (hit != null) {
					Debug.Log ("tickled creature");
					creature.tickled ();
				}
				isTickled = false;
			}
		}
    }

}
