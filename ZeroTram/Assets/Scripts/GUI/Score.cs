using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Assets
{

public class Score : MonoBehaviour {

	public int score;
	
	void Start () {
	
	}
	
	void Update ()
	{
		if (score / 10 == 0) {
			GetComponent<Text> ().text = "000" + score;
		}
		if (score / 10 > 0 && score / 10 < 10) {
			GetComponent<Text> ().text = "00" + score;
		}
		if (score / 10 > 10 && score / 10 < 100) {
			GetComponent<Text> ().text = "0" + score;
		}
		if (score / 10 > 100 && score / 10 < 1000) {
			GetComponent<Text> ().text = "" + score;
		}
	}
}
}