using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTargetRecognized : MonoBehaviour
{
	public InteractionHandler ihscript;
	public bool initialized = false;
	private MeshRenderer _mr;

	// Use this for initialization
	void Start ()
	{
		ihscript = GetComponent<InteractionHandler> ();
		StartCoroutine (StartGPS ());
		_mr = GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!initialized && _mr.enabled) {
			ihscript.enabled = true;
			initialized = true;
		} else if (initialized && !_mr.enabled) {
			ihscript.enabled = false;
			initialized = false;
		}
	}

	IEnumerator StartGPS ()
	{
		// First, check if user has location service enabled
		if (!Input.location.isEnabledByUser)
			yield break;

		// Start service before querying location
		Input.location.Start ();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			Debug.Log ("Timed out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.Log ("Unable to determine device location");
			yield break;
		}

	}

	void OnDisable ()
	{
		Input.location.Stop ();
		ihscript.enabled = false;
		initialized = false;
	}
}
