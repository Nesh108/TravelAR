using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DroppableCreator: MonoBehaviour
{
	public GameObject CreatorBox;
	public UnityEngine.UI.InputField message;
	public MeshRenderer mrSign;
	public MeshRenderer mrWelcome;
	
	public IEnumerator CreateDroppable (string parent, string marker, string type, string username, string content, string position)
	{
		string url = "http://" + Droppable.IP + "/travelar/create-drop.php?";
		string paramters = "parent=" + WWW.EscapeURL(parent) + "&marker=" + WWW.EscapeURL(marker)+ "&content=" + WWW.EscapeURL(content) + "&type=" + WWW.EscapeURL(type) + "&username=" + WWW.EscapeURL(username) + "&position=" + WWW.EscapeURL(position);
		WWW www = new WWW (url + paramters);
		yield return www;

		// check for errors
		if (www.error == null) {
			Debug.Log ("Success! " + www.text);
			InteractionHandler[] ihscripts = GameObject.FindObjectsOfType<InteractionHandler> ();
			foreach (InteractionHandler hs in ihscripts) {
				if (hs.enabled && hs.gameObject.name.Equals(parent)) {
					StartCoroutine (DroppableFetcher.FetchSpecificDroppables (hs, parent, true));
				}
			}


			ShowCreator ();

		} else {
			Debug.Log ("WWW Error: " + www.error);
		}
	}

	public void CreateDroppable() {
		string parent = "";
		Vector3 pos = Vector3.zero;

		if(mrSign.enabled) {
			pos = mrSign.gameObject.transform.parent.transform.position;
			parent = "sign";
		} else if (mrWelcome.enabled) {
			pos = mrWelcome.gameObject.transform.parent.transform.position;
			parent = "welcome";
		}

		if (!parent.Equals ("")) {
			StartCoroutine (CreateDroppable (parent, "TEXT#" + DateTime.Now, "text", "Nesh", message.text, pos.x + "," + pos.y + "," + (pos.z -1)));	
		}
	}

	public void ShowCreator() {
		CreatorBox.SetActive (!CreatorBox.activeSelf);
	}
}