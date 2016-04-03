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
        private string result;

        private const String ServerName = "http://golang-zerotramserver.rhcloud.com/";

        public WWW GET(string url, System.Action<String> onComplete)
        {

            WWW www = new WWW(ServerName + url);
            StartCoroutine(WaitForRequest(www, onComplete));
            return www;
        }

        public WWW POST(string url, Dictionary<string, string> post, System.Action<String> onComplete)
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

        private IEnumerator WaitForRequest(WWW www, System.Action<String> onComplete)
        {
            yield return www;
            // check for errors
            if (www.error == null)
            {
                result = www.text;
                onComplete(result);
            }
            else {
                Debug.Log(www.error);
            }
        }
    }
}
