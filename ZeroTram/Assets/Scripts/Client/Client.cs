using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Client
{
    public class Client : MonoBehaviour
    {
        private const String ServerName = "http://golang-zerotramserver.rhcloud.com/";
        private const String SavedUseridString = "UserId";
		private const String SavedTokenString = "ServerToken";


        private static string UserId;
        private static string Token;

		private static bool IsClientDisabled;

		public void DisableClient() {
			IsClientDisabled = true;
		}

        public bool IsUserIdSaved()
        {
            return PlayerPrefs.HasKey(SavedUseridString);
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
            UserId = userName;
            POST("user/register", new Dictionary<string, string> { { "uuid", userName } }, onComplete);
        }

		public void UpdateToken(String token){
			Token = token;
			PlayerPrefs.SetString(SavedTokenString, token);
		}

		public void AuthorizeUser(Action<JSONObject> onComplete)
        {
			if(Token == null) {
				if(PlayerPrefs.HasKey (SavedTokenString)) {
					Token = PlayerPrefs.GetString (SavedTokenString);
				} else {
					String response = "{\"error\": \"no token found\"}";
					JSONObject jsonResponse = new JSONObject (response);
					jsonResponse.Bake ();
					onComplete (jsonResponse);
					return;
				}
			}
            if (UserId == null)
            {
                if (PlayerPrefs.HasKey(SavedUseridString))
                {
                    UserId = PlayerPrefs.GetString(SavedUseridString);
                }
                else
                {
					String response = "{\"error\": \"no userid found\"}";
					JSONObject jsonResponse = new JSONObject (response);
					jsonResponse.Bake ();
					onComplete (jsonResponse);
					return;   
                }
            }
			POST ("user/authorize", new Dictionary<string, string> { { "token", Token }, { "uuid", UserId } }, onComplete);
        }

        public void BindUser(string newUsername, Action<JSONObject> onComplete)
        {
            if (!HandleUnsetToken(onComplete))
            {
                return;
            }
            POST("user/bind", new Dictionary<string, string> { { "token", Token }, {"bindid", newUsername} }, onComplete);
        }

        public void UnlockEvent(string eventName, string stringParameter, int intParameter, Action<JSONObject> onComplete)
        {
            if (!HandleUnsetToken(onComplete))
            {
                return;
            }
            POST("user/authorize", new Dictionary<string, string> { { "token", Token }, {"eventname", eventName}, {"stringparameter", stringParameter}, {"intparameter", intParameter.ToString()} }, onComplete);
        }

        public void GetEvents(Action<JSONObject> onComplete)
        {
            if (!HandleUnsetUserid(onComplete))
            {
                return;
            }
            GET("event/"+ UserId, onComplete);
        }

        public void GetBonuses(Action<JSONObject> onComplete)
        {
            if (!HandleUnsetUserid(onComplete))
            {
                return;
            }
            GET("bonus/" + UserId, onComplete);
        }

        public void UseBonus(string bonusName, Action<JSONObject> onComplete)
        {
            if (!HandleUnsetToken(onComplete))
            {
                return;
            }
            POST("bonus/use/" + bonusName, new Dictionary<string, string> { {"token", Token} },  onComplete);
        }

        public void DecreaseTramLives(Action<JSONObject> onComplete)
        {
            if (!HandleUnsetToken(onComplete))
            {
                return;
            }
            POST("tramlives/decrease", new Dictionary<string, string> { { "token", Token } }, onComplete);
        }

        public void GetTramLives(Action<JSONObject> onComplete)
        {
            if (!HandleUnsetUserid(onComplete))
            {
                return;
            }
            GET("tramlives/get/" + UserId, onComplete);
        }

        public void GetResources(Action<JSONObject> onComplete)
        {
            if (!HandleUnsetUserid(onComplete))
            {
                return;
            }
            GET("resources/" + UserId, onComplete);
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
            if (UserId == null)
            {
                if (PlayerPrefs.HasKey(SavedUseridString))
                {
                    UserId = PlayerPrefs.GetString(SavedUseridString);
                    return true;
                }
                JSONObject response = new JSONObject();
                response.AddField("error", "null userid passed");
                onComplete(response);
                return false;
            }
            return true;
        }

        private bool HandleUnsetToken(System.Action<JSONObject> onComplete)
        {
            if (Token == null)
            {
                JSONObject response = new JSONObject();
                response.AddField("error", "null token passed");
                onComplete(response);
                return false;
            }
            return true;
        }
    }
}
