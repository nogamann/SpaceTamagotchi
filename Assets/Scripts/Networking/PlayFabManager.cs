using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour {

	public string myPlayFabId;
	public string friendsPlayFabId;
	public string mySharedGroupId;
	public string titleID = "3067";

	public NetworkManager networkManager;

	public bool invited;

	// EVENTS
	public static event Action OnLoginRecievedEvent;
	public static event Action<string, Action<string>> OnReadFromSharedGroupEvent;
	public static event Action OnCreateSharedGroupEvent;
	//public static event Action OnWriteToSharedGroupEvent;
	public static event Action OnEnterCodeEvent;
	public static event Action OnAddFriendEvent;
	public static event Action<string> OnUserDataRecievedEvent;

	void Start(){
		// first time - inviting
//		OnLoginRecievedEvent += OnLoginRecievedInviting;
//		Login(titleID);

//		// first time - invited
		OnLoginRecievedEvent += OnLoginRecievedInvited;
		Login(titleID);
//
//		// not first time
//		OnLoginRecievedEvent += OnLoginRecieved;
//		Login (titleID);

	}

	void Update(){
//		if (justLoggedIn) {
////			numOfFriends = GetNumOfFriends();
////			EnterCode();
//			FriendEntered ();
//			justLoggedIn = false;
//		}
//
//		// if it's the first time
//		// TODO check only at certain intervals
//		if (waitToBeAdded) {
//			if (GetNumOfFriends() > numOfFriends) {
//				FriendEntered ();
//			}
//		}
			
		// not the first time
		// read friendsplayfabid from playfab TODO
		friendsPlayFabId = "827019C949ED0449";
		mySharedGroupId = myPlayFabId + friendsPlayFabId;

		// check if other player is connected ('playfabid+connected' true)


		// if so, read ip, write connected and join as client

		// if not, read game data, write connected, write ip and join as host
	}

	void createSharedGroupWithFriend(string sharedGroupId, string friendsPlayfabId){
		// create
		CreateSharedGroupRequest createSharedGroupRequest = new CreateSharedGroupRequest () {
			SharedGroupId = sharedGroupId
		};
		PlayFabClientAPI.CreateSharedGroup (createSharedGroupRequest, (result) => {
			mySharedGroupId = result.SharedGroupId;
			Debug.Log ("Got SharedGroupId: " + mySharedGroupId);
			if (OnCreateSharedGroupEvent != null){
				OnCreateSharedGroupEvent();
			}
		},
			(error) => {
				Debug.Log ("Error creating group");
			});

		// add friend
		AddSharedGroupMembersRequest addSharedGroupMembersRequest = new AddSharedGroupMembersRequest(){
			SharedGroupId = sharedGroupId,
			PlayFabIds = new List<String>() {friendsPlayFabId}
		};
		PlayFabClientAPI.AddSharedGroupMembers(addSharedGroupMembersRequest, (result) => {
			Debug.Log("Added " + friendsPlayFabId);

		},
			(error) => {
				Debug.Log("error adding " + friendsPlayFabId);
			});
	}

	public void WriteToSharedGroup(string sharedGroupId, Dictionary<string,string> data){
		UpdateSharedGroupDataRequest updateSharedGroupRequest = new UpdateSharedGroupDataRequest () {
			SharedGroupId = sharedGroupId,
			Data = data
		};
		PlayFabClientAPI.UpdateSharedGroupData (updateSharedGroupRequest, (result) => {
			Debug.Log ("wrote " + result.CustomData);
		}, (error) => {
			Debug.Log (error.ErrorMessage);
		});

	}
		
	public void ReadFromSharedGroup(string sharedGroupId, string key, Action<string> func){
		GetSharedGroupDataRequest getSharedGroupDataRequest = new GetSharedGroupDataRequest () {
			SharedGroupId = sharedGroupId
		};
		PlayFabClientAPI.GetSharedGroupData(getSharedGroupDataRequest, result => {
			var resultData = result.Data;
			if (OnReadFromSharedGroupEvent != null){
				OnReadFromSharedGroup(resultData[key].Value, func);
			}
		},
			error => {
				Debug.LogError(error.ErrorMessage);
			});
	}
		
	void UpdateUserData(Dictionary<string, string> data){
		UpdateUserDataRequest updateUserDataRequest = new UpdateUserDataRequest () {
			Data = data
		};
		PlayFabClientAPI.UpdateUserData(updateUserDataRequest, (result) => {
		}, (error) => {
			Debug.Log ("couldn't write user data");
		});
	}

	//TODO event
	void ReadUserData(string key){
		List<string> keys = new List<string>();
		keys.Add (key);
		GetUserDataRequest getUserDataRequest = new GetUserDataRequest () {
			Keys = keys
		};
		PlayFabClientAPI.GetUserData(getUserDataRequest, (result) => {
			if (OnUserDataRecievedEvent!=null){
				//playfabfrinedid
				OnUserDataRecieved(result.Data[key].Value);
			}
		}, (error) => {
			Debug.Log ("couldn't write user data");
		});
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
			myPlayFabId = result.PlayFabId;
			Debug.Log("Got PlayFabID: " + myPlayFabId);
			if (OnLoginRecievedEvent != null) {
				OnLoginRecievedEvent();
			}
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

	public void AddFriend(string friendsPlayfabId){
		AddFriendRequest addFriendRequest = new AddFriendRequest () {
			FriendPlayFabId = friendsPlayFabId
		};
		PlayFabClientAPI.AddFriend (addFriendRequest, (result) => {
			Debug.Log ("friend " + friendsPlayFabId + " was added");
			if (OnAddFriendEvent != null){
				OnAddFriend();
			}
		}, (error) => {
			Debug.Log (error.ErrorMessage);
		});
	}

//	public int GetNumOfFriends(){
//		List<FriendInfo> friendsInfo = new List<FriendInfo>();
//		PlayFabClientAPI.GetFriendsList(new GetFriendsListRequest(), (result) => {
//			friendsInfo = result.Friends;
//		}, (error) => {
//			Debug.Log(error.ErrorMessage);
//		});
//
//		//TODO test
//		return friendsInfo.Count;
//	}

//	public void InviteFriend(){
//
//		// show code to send to friend
//
//		// turn on bool to wait for friend
//		waitToBeAdded = true;
//	}

	// enetred after added as a friend on playfab and we know his id
//	public void FriendEntered(){
//		waitToBeAdded = false;
//
//		//TODO delete
//		friendsPlayFabId = "827019C949ED0449";
//		mySharedGroupId = myPlayFabId + friendsPlayFabId;
//
//		//save friendsplayfabid to playfab
//		Dictionary<string,string> friendData = new Dictionary<string,string>();
//		friendData.Add ("friendsplayfabid", friendsPlayFabId);
//		UpdateUserData(friendData);
//
//		// read ip from shared group
//		Dictionary<string,SharedGroupDataRecord> resultData = new Dictionary<string, SharedGroupDataRecord>();
//		ReadFromSharedGroup(mySharedGroupId);
////		Debug.Log (resultData.Keys.Count);
////		SharedGroupDataRecord val = resultData["ip"];
////		Debug.Log("data: " + val.Value);
////		string IP = data ["ip"].Value;
//
//
//	}
//		
//
//	public void EnterCode(){
//		
//		// TODO get input from user
//		friendsPlayFabId = "827019C949ED0449";
//
//		// add friend on playfab
//		AddFriend(friendsPlayFabId);
//
//		// create shared group and add friend
//		mySharedGroupId = myPlayFabId + friendsPlayFabId;
//		Debug.Log ("mySharedGroupId: " + mySharedGroupId);
//		createSharedGroupWithFriend(mySharedGroupId, friendsPlayFabId);
//
//		//save friendsplayfabid to playfab
//		Dictionary<string,string> friendData = new Dictionary<string,string>();
//		friendData.Add ("friendsplayfabid", friendsPlayFabId);
//		UpdateUserData(friendData);
//
//		// write ip to shared group 
//		Dictionary<string,string> data = new Dictionary<string, string>();
//		string myIP = Network.player.ipAddress;
//		data.Add ("ip", myIP);
//		WriteToSharedGroup (mySharedGroupId, data);
//
//		// update ip and enter game as host
//		networkManager.networkAddress = myIP;
//		networkManager.StartHost ();
//	}		

	public void OnLoginRecievedInviting() {
		Debug.Log ("OnLoginRecievedInviting");
		OnLoginRecievedEvent -= OnLoginRecievedInviting;

		// print code

		// waiting for friend to enter code
		OnEnterCodeEvent += OnEnterCode;
	}

	public void OnLoginRecievedInvited() {
		Debug.Log ("OnLoginRecievedInvited");
		OnLoginRecievedEvent -= OnLoginRecievedInvited;

		// enter code TODO
		friendsPlayFabId = "827019C949ED0449";
		if (OnEnterCodeEvent != null) {
			OnEnterCode ();
		}

		// add friend on playfab
		OnAddFriendEvent += OnAddFriend;
		AddFriend(friendsPlayFabId);
	}

	public void OnEnterCode(){
		Debug.Log ("OnEnterCode");
		OnEnterCodeEvent -= OnEnterCode;

		//TODO delete
		friendsPlayFabId = "827019C949ED0449";

		mySharedGroupId = myPlayFabId + friendsPlayFabId;

		// read ip from shared group
		OnReadFromSharedGroupEvent += OnReadFromSharedGroup;
		ReadFromSharedGroup(mySharedGroupId, "ip", ConnectAsClient);
	}

	public void OnReadFromSharedGroup(string val, Action<string> func){
		Debug.Log ("OnReadFromSharedGroup");
		// enter game as client
		func(val);
	}

	public void ConnectAsClient(string ip){
		Debug.Log ("ConnectAsClient");
		networkManager.networkAddress = ip;
		networkManager.StartClient ();
	}

	public void IsOtherConnected(string connected){
		Debug.Log ("IsOtherConnected");
		// if he's connected
		// read ip
		OnReadFromSharedGroupEvent += OnReadFromSharedGroup;
		ReadFromSharedGroup (mySharedGroupId, "ip", ConnectAsClient);

		// if he's not
		// write ip
		string ip = Network.player.ipAddress;
		Dictionary<string, string> data = new Dictionary<string, string> ();
		data.Add ("ip", ip);
		data.Add (myPlayFabId, "connected");
		WriteToSharedGroup(mySharedGroupId, data);

		// TODO read madadim
		// connect as host
		networkManager.networkAddress = ip;
		networkManager.StartHost ();
	}

	public void OnAddFriend(){
		Debug.Log ("OnAddFriend");
		OnAddFriendEvent -= OnAddFriend;

		// create shared group and add friend
		mySharedGroupId = myPlayFabId + friendsPlayFabId;
		OnCreateSharedGroupEvent += OnCreateSharedGroupEvent;
		createSharedGroupWithFriend(mySharedGroupId, friendsPlayFabId);
//
	}

	public void OnCreateSharedGroup(){
		Debug.Log ("OnCreateSharedGroup");
		OnCreateSharedGroupEvent -= OnCreateSharedGroup;

		// write ip to shared group 
		Dictionary<string,string> data = new Dictionary<string, string>();
		string myIP = Network.player.ipAddress;
		data.Add ("ip", myIP);
		WriteToSharedGroup (mySharedGroupId, data);

		// update ip and enter game as host
		networkManager.networkAddress = myIP;
		networkManager.StartHost ();
	}

	public void OnLoginRecieved(){
		Debug.Log ("OnLoginRecieved");
		OnLoginRecievedEvent -= OnLoginRecieved;
		 
		// get friendsPlayFabId
		OnUserDataRecievedEvent += OnUserDataRecieved;
		ReadUserData ("friendsPlayFabId");
	}

	public void OnUserDataRecieved(string friendPlayfabId){
		Debug.Log ("OnUserDataRecieved");
		friendsPlayFabId = friendPlayfabId;
		OnUserDataRecievedEvent -= OnUserDataRecieved;

		mySharedGroupId = myPlayFabId + friendsPlayFabId;

		OnReadFromSharedGroupEvent += OnReadFromSharedGroup;
		ReadFromSharedGroup (mySharedGroupId, friendsPlayFabId, IsOtherConnected);
	}

}