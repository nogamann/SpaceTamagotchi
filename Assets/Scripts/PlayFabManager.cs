using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;


public class PlayFabManager : MonoBehaviour {
	public string PlayFabId;
	public string mySharedGroupId;
	public string titleID = "3067";

	// Use this for initialization
	void Start () {
		Login (titleID);
		Debug.Log ("hhhh");



	}
	
	// Update is called once per frame
	void Update () {


		CreateSharedGroupRequest request = new CreateSharedGroupRequest(){
			SharedGroupId = "367"
		};
		PlayFabClientAPI.CreateSharedGroup (request, (result) => {
			mySharedGroupId = result.SharedGroupId;
			Debug.Log ("Got SharedGroupId: " + mySharedGroupId);

		},
			(error) => {
				Debug.Log ("Error");
			});

		GetSharedGroupDataRequest getSharedGroupRequest = new GetSharedGroupDataRequest () {
			SharedGroupId = "367",
			GetMembers = true
		};
		PlayFabClientAPI.GetSharedGroupData (getSharedGroupRequest, (result) => {
			Debug.Log ("in group: " + result.Members);
		}, (error) => {
			Debug.Log ("members error");
		});


		AddSharedGroupMembersRequest request2 = new AddSharedGroupMembersRequest(){
			SharedGroupId = "367",
			PlayFabIds = new List<String>() {"827019C949ED0449"}
		};
		PlayFabClientAPI.AddSharedGroupMembers(request2, (result) => {
			Debug.Log("Added ");
		},
			(error) => {
				Debug.Log("error");
			});

		Dictionary<string,string> data = new Dictionary<string,string> ();
		data.Add ("lo", "shoa");
		data.Add ("ken", "shoa");

		UpdateSharedGroupDataRequest updateShardGriupDataRequest = new UpdateSharedGroupDataRequest () {
			SharedGroupId = "367",
			Data = data
		};
		PlayFabClientAPI.UpdateSharedGroupData(updateShardGriupDataRequest, (result) => {
			Debug.Log("yeshhhh");
		},
			(error)=>{
				Debug.Log("looooo");
			});

		GetSharedGroupDataRequest getSharedGroupDataRequest = new GetSharedGroupDataRequest () {
			SharedGroupId = "367"
		};
		PlayFabClientAPI.GetSharedGroupData(getSharedGroupDataRequest, (result) => {
			Debug.Log(result.Data["lo"].ToString() + " " + result.Data["ken"].ToString());
		},
			(error)=> {
				Debug.Log("basa");});
		
	}



	void Login(string titleId)
	{
		

			

		LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
		{
			TitleId = titleId,
			CreateAccount = true,
			CustomId = SystemInfo.deviceUniqueIdentifier
		};
		PlayFabClientAPI.LoginWithCustomID(request, (result) => {
			PlayFabId = result.PlayFabId;
			Debug.Log("Got PlayFabID: " + PlayFabId);

			if(result.NewlyCreated)
			{
				Debug.Log("(new account)");
			}
			else
			{
				Debug.Log("(existing account)");
			}
		},
			(error) => {
				Debug.Log("Error logging in player with custom ID:");
				Debug.Log(error.ErrorMessage);
			});
	}


}
