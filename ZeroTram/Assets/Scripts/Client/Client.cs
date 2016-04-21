using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
	private const String ServerName = "http://www.golang-zerotramserver.rhcloud.com/";
	private const String SavedUseridString = "UserId";
	private const String SavedTokenString = "ServerToken";
	private const String TicketsRecordKey = "TicketsRecordKey";

	private int _currentRecord;
	private bool recordLoaded;
	private String userId;
	private String token;

	private String GetUserid() {
		if(userId == null) {
			userId = EncryptedPlayerPrefs.GetString (SavedUseridString, "");
		}
		return userId;
	}

	private void SetUserid(String newid) {
		userId = newid;
		EncryptedPlayerPrefs.SetString (SavedUseridString, newid);
	}

	private String GetToken() {
		if(token == null) {
			token = EncryptedPlayerPrefs.GetString (SavedTokenString, "");
		}
		return token;
	}

	public void UpdateToken(String newToken) {
		token = newToken;
		EncryptedPlayerPrefs.SetString (SavedTokenString, newToken);
	}

	public void DisableClient() {

	}

	public bool IsUserIdSaved()
	{
		return EncryptedPlayerPrefs.HasKey (SavedUseridString);
	}

	public void LoadConfig(Action<JSONObject> onComplete)
	{
		GET("config/get", onComplete);
	}

	public void CheckConfig(Action<JSONObject> onComplete)
	{
		GET("config/version", onComplete);
	}

	public void RegisterUser(string userName, Action<JSONObject> onComplete)
	{
		PlayerPrefs.SetString(SavedUseridString, userName);
		SetUserid (userName);
		POST("user/register", new Dictionary<string, string> { { "uuid", userName } }, onComplete);
	}

	public void AuthorizeUser(Action<JSONObject> onComplete)
	{
		if(token == null) {
			token = GetToken ();
		}
		if(token == "") {
			String response = "{\"error\": \"no token found\"}";
			JSONObject jsonResponse = new JSONObject (response);
			jsonResponse.Bake ();
			onComplete (jsonResponse);
			return;
		}
		if (userId == null) {
			userId = GetUserid ();
		}
		if(userId == "") {
			String response = "{\"error\": \"no userid found\"}";
			JSONObject jsonResponse = new JSONObject (response);
			jsonResponse.Bake ();
			onComplete (jsonResponse);
			return;   
		}    
		POST ("user/authorize", new Dictionary<string, string> { { "token", token }, { "uuid", userId } }, onComplete);
	}

	public void BindUser(string newUsername, Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("user/bind", new Dictionary<string, string> { { "token", token }, {"bindid", newUsername} }, onComplete);
	}

	public void UnlockEvent(string eventName, string stringParameter, int intParameter, Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("user/authorize", new Dictionary<string, string> { { "token", token }, {"eventname", eventName}, {"stringparameter", stringParameter}, {"intparameter", intParameter.ToString()} }, onComplete);
	}

	public void GetEvents(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("event/"+ userId, onComplete);
	}

	public void GetBonuses(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("bonus/" + userId, onComplete);
	}

	public void UseBonus(string bonusName, Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("bonus/use/" + bonusName, new Dictionary<string, string> { {"token", token} },  onComplete);
	}

	public void DecreaseTramLives(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("tramlives/decrease", new Dictionary<string, string> { { "token", token } }, onComplete);
	}

	public void GetTramLives(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("tramlives/get/" + userId, onComplete);
	}

	public void GetResources(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("resources/" + userId, onComplete);
	}

	private WWW GET(string url, System.Action<JSONObject> onComplete)
	{

		WWW www = new WWW(ServerName + url);
		StartCoroutine(WaitForRequest(www, onComplete));
		return www;
	}

	private WWW POST(string url, Dictionary<string, string> post, System.Action<JSONObject> onComplete)
	{
		WWWForm form = new WWWForm();

		foreach (KeyValuePair<string, string> post_arg in post)
		{
			form.AddField(post_arg.Key, post_arg.Value);
		}

		WWW www = new WWW(ServerName + url, form);

		StartCoroutine(WaitForRequest(www, onComplete));
		return www;
	}

	private IEnumerator WaitForRequest(WWW www, System.Action<JSONObject> onComplete)
	{
		yield return www;
		// check for errors
		if (www.error == null)
		{
			JSONObject result = new JSONObject(www.text);
			result.Bake();
			onComplete(result);
		}
		else {
			Debug.Log(www.error);
			JSONObject result = new JSONObject();
			result.AddField("error", www.error);
			result.Bake();
			onComplete(result);
		}
	}

	private bool HandleUnsetUserid(System.Action<JSONObject> onComplete)
	{
		if (userId == null)
		{
			userId = GetUserid ();
			if (userId == "") {
				JSONObject response = new JSONObject();
				response.AddField("error", "null userid passed");
				onComplete(response);
				return false;	
			}
		}
		return true;
	}

	private bool HandleUnsetToken(System.Action<JSONObject> onComplete)
	{
		if (token == null)
		{
			token = GetToken ();
			if(token == "") {
				JSONObject response = new JSONObject();
				response.AddField("error", "null token passed");
				onComplete(response);
				return false;	
			}
		}
		return true;
	}

	public int GetRecord() {
		if(!recordLoaded) {
			recordLoaded = true;
			_currentRecord = EncryptedPlayerPrefs.GetInt (TicketsRecordKey, 0);
		}
		return _currentRecord;
	}

	public void SaveRecord(int newRecord, System.Action<JSONObject> onComplete) {
		if(newRecord > _currentRecord) {
			_currentRecord = newRecord;
			EncryptedPlayerPrefs.SetInt (TicketsRecordKey, _currentRecord);
			if(!HandleUnsetToken (onComplete)) {
				return;
			}
			POST ("/event/unlock/", new Dictionary<String, String>{{"eventName", "ticketRecord"}, {"intValue", newRecord.ToString()}}, onComplete);
		}
	}
}
