using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

public class InteractionHandler : MonoBehaviour
{

	public InteractableType interactType;
	public List<Droppable> Drops;
	public GameObject DroppableGO;
	private UnityEngine.UI.Text label;
	private GameObject textbox;
	private UnityEngine.UI.Image textboxImage;
	private bool activated = false;
	private TextMesh droppedBy;

	public GameObject TextPrefab;
	public GameObject ImagePrefab;
	public GameObject AudioPrefab;
	public GameObject EmojiPrefab;

	public AudioSource player;
	public AudioClip[] audios;

	public List<String> droppables;

	// Use this for initialization
	void Start ()
	{
		droppables = new List<String> ();
		textbox = GameObject.FindGameObjectWithTag ("Label");
		droppedBy = GetComponentInChildren<TextMesh> ();
		textboxImage = textbox.GetComponent<UnityEngine.UI.Image> ();
		StartCoroutine (DroppableFetcher.FetchSpecificDroppables (this, gameObject.name, interactType.Equals (InteractableType.MAIN)));
		label = textbox.GetComponentInChildren<UnityEngine.UI.Text> ();
		player = FindObjectOfType<AudioSource> ();
	}

	// Update is called once per frame
	void Update ()
	{
		Ray ray = new Ray();
		bool clicked;
			#if UNITY_EDITOR
				clicked = Input.GetMouseButtonDown (0);
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			#elif UNITY_ANDROID
				clicked = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
				if(Input.touchCount > 0){
					ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
				}
			#endif
		if (clicked) {
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.name.Equals (gameObject.name)) {
					StartCoroutine (HandleClick (hit.collider.name));
				} else {
					if (interactType.Equals (InteractableType.DROPPABLE)) {
						activated = false;
						if (Drops [0].Dt.Equals (DroppableType.TEXT) && !hit.collider.name.Contains ("TEXT#")) {
							textboxImage.enabled = false;
							label.text = "";
						} else if (Drops [0].Dt.Equals (DroppableType.IMAGE)) {
							droppedBy.text = "";
						}
					}
				}
			}
		}
	}

	IEnumerator HandleClick (string name)
	{
		if (interactType.Equals (InteractableType.MAIN)) {
			DroppableGO.SetActive (!DroppableGO.activeSelf);
		} else if (interactType.Equals (InteractableType.DROPPABLE)) {
			activated = !activated;
			if (Drops.Count > 0 && Drops [0].Dt.Equals (DroppableType.TEXT)) {
				textboxImage.enabled = activated;
				label.text = textboxImage.enabled ? "<b>" + Drops [0].UserName + "</b>: " + SpliceText (Drops [0].Content, 35) : "";
			} else if (Drops.Count > 0 && Drops [0].Dt.Equals (DroppableType.IMAGE)) {
				droppedBy.text = activated ? Drops [0].UserName : "";
			} else if (Drops.Count > 0 && Drops [0].Dt.Equals (DroppableType.AUDIO)) {
				AudioClip a;
				if (player.isPlaying) {
					player.Stop ();
				} else {
					if (Drops [0].Content.Contains ("helloangelhack")) {
						a = audios [0];
					} else {
						a = null;
					}
					player.Play ();
				}
			}
		}
		yield break;
	}

	public static string SpliceText (string text, int lineLength)
	{
		return Regex.Replace (text, "(.{" + lineLength + "})", "$1" + Environment.NewLine);
	}

	void OnDisable ()
	{
		if (textbox != null) {
			textboxImage.enabled = false;
		}
		label.text = "";
		if (DroppableGO != null) {
			DroppableGO.SetActive (false);
		}
	}

	public void SpawnDroppables ()
	{
		if (interactType.Equals (InteractableType.MAIN)) {
			GameObject go = null;
			foreach (Droppable d in Drops) {
				if (!droppables.Contains (d.Marker)) {
					droppables.Add (d.Marker);
					switch (d.Dt) {
					case DroppableType.TEXT:
						go = Instantiate (TextPrefab, d.Position, new Quaternion(180,0,0,0), DroppableGO.transform);
						break;
					case DroppableType.IMAGE:
						go = Instantiate (ImagePrefab, d.Position, Quaternion.identity, DroppableGO.transform);
						byte[] img = System.Convert.FromBase64String (d.Content);
						Texture2D tex = new Texture2D (400, 400);
						tex.LoadImage (img);
						Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 ());
						go.GetComponentInChildren<SpriteRenderer> ().sprite = s;
						break;
					case DroppableType.AUDIO:
						go = Instantiate (AudioPrefab, d.Position, Quaternion.identity, DroppableGO.transform);
						break;
					case DroppableType.EMOJI:
						go = Instantiate (EmojiPrefab, d.Position, Quaternion.identity, DroppableGO.transform);
						if (d.Content.Equals ("smiley")) {
							Debug.Log ("Got Smiley");
						}
						break;
					}

					if (go != null) {
						go.transform.localPosition = d.Position;
						if (!d.Dt.Equals (DroppableType.TEXT)) {
							go.transform.localRotation = new Quaternion (0, 0, 0, 0);
						}

						go.name = d.Marker;
					}
				}

			}
		}
	}
}

public enum InteractableType
{
	MAIN,
	DROPPABLE
}