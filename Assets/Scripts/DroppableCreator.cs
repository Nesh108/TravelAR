using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DroppableCreator: MonoBehaviour
{
	public GameObject CreatorBox;

	public IEnumerator CreateDroppable (string parent, string marker, string type, string username, string content, string position)
	{
		string url = "http://10.2.12.162/travelar/create-drop.php";
		string paramters = "?parent=" + parent + "&marker=" + marker + "&username=" + username + "&position=" + position;
		WWW www = new WWW (url + paramters);
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("Success! " + www.text);
		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
	}

	public void CreateDroppable() {
		//StartCoroutine (CreateDroppable ());

	}

	public void ShowCreator() {
		CreatorBox.SetActive (!CreatorBox.activeSelf);
	}
}