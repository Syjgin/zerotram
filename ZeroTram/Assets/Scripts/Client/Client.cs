using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
	private const string ServerName = "http://golang-zerotramserver.rhcloud.com/";//"http://127.0.0.1:8080/"
	private const string SavedUseridString = "UserId";
	private const string SavedTokenString = "ServerToken";
	private const string TicketsRecordKey = "TicketsRecordKey";

	private int _currentRecord;
	private bool _isRecordLoaded;
	private string _userId;
	private string _token;

	private string GetUserid() {
		if(_userId == null) {
			_userId = EncryptedPlayerPrefs.GetString (SavedUseridString, "");
		}
		return _userId;
	}

	private void SetUserid(string newid) {
		_userId = newid;
		EncryptedPlayerPrefs.SetString (SavedUseridString, newid);
	}

	private string GetToken() {
		if(_token == null) {
			_token = EncryptedPlayerPrefs.GetString (SavedTokenString, "");
		}
		return _token;
	}

	public void UpdateToken(string newToken) {
		_token = newToken;
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
		if(_token == null) {
			_token = GetToken ();
		}
		if(_token == "") {
			String response = "{\"error\": \"no token found\"}";
			JSONObject jsonResponse = new JSONObject (response);
			jsonResponse.Bake ();
			onComplete (jsonResponse);
			return;
		}
		if (_userId == null) {
			_userId = GetUserid ();
		}
		if(_userId == "") {
			String response = "{\"error\": \"no userid found\"}";
			JSONObject jsonResponse = new JSONObject (response);
			jsonResponse.Bake ();
			onComplete (jsonResponse);
			return;   
		}    
		POST ("user/authorize", new Dictionary<string, string> { { "token", _token }, { "uuid", _userId } }, onComplete);
	}

	public void BindUser(string newUsername, Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("user/bind", new Dictionary<string, string> { { "token", _token }, {"bindid", newUsername} }, onComplete);
	}

	public void UnlockEvent(string eventName, string stringParameter, int intParameter, Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("event/unlock", new Dictionary<string, string> { { "token", _token }, {"eventname", eventName}, {"stringparameter", stringParameter}, {"intparameter", intParameter.ToString()} }, onComplete);
	}

	public void GetEvents(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("event/"+ _userId, onComplete);
	}

	public void GetBonuses(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("bonus/" + _userId, onComplete);
	}

	public void UseBonus(string bonusName, Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("bonus/use/" + bonusName, new Dictionary<string, string> { {"token", _token} },  onComplete);
	}

	public void DecreaseTramLives(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("tramlives/decrease", new Dictionary<string, string> { { "token", _token } }, onComplete);
	}

	public void StartCombination(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("combination/start", new Dictionary<string, string> { { "token", _token } }, onComplete);
	}

	public void SendCombination(Action<JSONObject> onComplete, List<string> passengerNames)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		string joinedNames = string.Join (",", passengerNames.ToArray ());
		POST("combination/send", new Dictionary<string, string> { { "token", _token }, {"passengersArray", joinedNames} }, onComplete);
	}

	public void GetTramLives(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("tramlives/get/" + _userId, onComplete);
	}

	public void GetResources(Action<JSONObject> onComplete)
	{
		if (!HandleUnsetUserid(onComplete))
		{
			return;
		}
		GET("resources/" + _userId, onComplete);
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
		www.Dispose ();
	}

	private bool HandleUnsetUserid(System.Action<JSONObject> onComplete)
	{
		if (_userId == null)
		{
			_userId = GetUserid ();
			if (_userId == "") {
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
		if (_token == null)
		{
			_token = GetToken ();
			if(_token == "") {
				JSONObject response = new JSONObject();
				response.AddField("error", "null token passed");
				onComplete(response);
				return false;	
			}
		}
		return true;
	}

	public int GetRecord() {
		if(!_isRecordLoaded) {
			_isRecordLoaded = true;
			_currentRecord = EncryptedPlayerPrefs.GetInt (TicketsRecordKey, 0);
		}
		return _currentRecord;
	}

    public void SendDoorBonusTime(int bonusTime, System.Action<JSONObject> onComplete)
    {
        if (!HandleUnsetToken(onComplete))
        {
            return;
        }
        POST("event/unlock", new Dictionary<String, String>{
                {"eventName", "doorsBonusTime"},
                {"intParameter", bonusTime.ToString()},
                {"token", _token}
            }, onComplete);
    }

    public void SendRecord(int newRecord, bool withFriends, System.Action<JSONObject> onComplete) {
		if(newRecord > _currentRecord) {
			_currentRecord = newRecord;
			EncryptedPlayerPrefs.SetInt (TicketsRecordKey, _currentRecord);
			if(!HandleUnsetToken (onComplete)) {
				return;
			}
			POST ("event/unlock", new Dictionary<String, String>{
				{"eventName", withFriends ? "friendsRecord" : "ticketRecord"}, 
				{"intParameter", newRecord.ToString()}, 
				{"token", _token}
			}, onComplete);
		}
	}

	public void SendPacifistRecord(int stationCount, System.Action<JSONObject> onComplete) {
		if(!HandleUnsetToken (onComplete)) {
			return;
		}
		POST ("event/unlock", new Dictionary<String, String>{{"eventName", "pacifist"}, {"intParameter", stationCount.ToString ()}, {"token", _token}}, onComplete);
	}

	public void SendTruckerRecord(int stationCount, System.Action<JSONObject> onComplete) {
		if(!HandleUnsetToken (onComplete)) {
			return;
		}
		POST ("event/unlock", new Dictionary<String, String>{{"eventName", "trucker"}, {"intParameter", stationCount.ToString ()}, {"token", _token}}, onComplete);
	}

	public void SendDangerRecord(int beatenCount, String passengerName, bool isBoss, System.Action<JSONObject> onComplete) {
		if(!HandleUnsetToken (onComplete)) {
			return;
		}
		POST ("event/unlock", new Dictionary<String, String>{
			{"eventName", isBoss ? "dangerBoss" : "danger"}, 
			{"intParameter", beatenCount.ToString ()}, 
			{"stringParameter", passengerName}, 
			{"token", _token}
		}, onComplete);
	}

    public void SendAntiStickRecord(int savedCount, System.Action<JSONObject> onComplete)
    {
        if (!HandleUnsetToken(onComplete))
        {
            return;
        }
        POST("event/unlock", new Dictionary<String, String>{
            {"eventName", "antistick"},
            {"intParameter", savedCount.ToString ()},          
            {"token", _token}
        }, onComplete);
    }

	public void SendLivesaverRecord(int stationNumber, System.Action<JSONObject> onComplete)
	{
		if (!HandleUnsetToken(onComplete))
		{
			return;
		}
		POST("event/unlock", new Dictionary<String, String>{
			{"eventName", "livesaver"},
			{"intParameter", stationNumber.ToString ()},          
			{"token", _token}
		}, onComplete);
	}
}
