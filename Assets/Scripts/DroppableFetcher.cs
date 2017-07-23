using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public class DroppableFetcher: MonoBehaviour
{
	static public IEnumerator FetchSpecificDroppables (InteractionHandler ihscript, string id, bool isParent)
	{
		string url = "http://" + Droppable.IP + "/travelar/drop.php";
		WWW www = new WWW (url);
		yield return www;

		// check for errors
		if (www.error == null) {
			ihscript.Drops = processData (www.text, id, isParent);
			ihscript.SpawnDroppables ();

		} else {
			Debug.Log ("WWW Error: " + www.error);
		}    
	}

	static List<Droppable> processData (string data, string id, bool isParent)
	{
		JsonData js = JsonMapper.ToObject (data);
		List<Droppable> allDrops = new List<Droppable> ();
		List<Droppable> drops = new List<Droppable> ();
		for (int i = 0; i < js.Count; i++) {
			allDrops.Add (new Droppable (js [i] ["parent"].ToString (), js [i] ["marker"].ToString (), convertStringToType (js [i] ["type"].ToString ()),
				js [i] ["username"].ToString (), DateTime.Now, js [i] ["content"].ToString (), convertStringToVector3 (js [i] ["position"].ToString ())));
		}
		foreach (Droppable d in allDrops) {
			if (!isParent && d.Marker.Equals (id)) {
				drops.Add (d);
			} else if (isParent && d.Parent.Equals (id)) {
				drops.Add (d);
			}
		}

		return drops;
	}

	static DroppableType convertStringToType (string type)
	{
		switch (type) {
		case "text":
			return DroppableType.TEXT;
		case "image":
			return DroppableType.IMAGE;
		case "audio":
			return DroppableType.AUDIO;
		case "emoji":
			return DroppableType.EMOJI;
		}
		return DroppableType.TEXT;
	}

	static Vector3 convertStringToVector3 (string v3)
	{
		string[] vs = v3.Split (',');
		return new Vector3 (float.Parse (vs [0].Trim ()), float.Parse (vs [1].Trim ()), float.Parse (vs [2].Trim ()));
	}
}

