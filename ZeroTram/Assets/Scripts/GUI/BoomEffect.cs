using UnityEngine;
using System.Collections;

public class BoomEffect : MonoBehaviour
{

    [SerializeField] private Animator _animator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("boom"))
	    {
	        Destroy(gameObject);
	    }
	}
}
