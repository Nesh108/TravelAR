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
	private SpriteRenderer spriteImage;

	public GameObject TextPrefab;
	public GameObject ImagePrefab;
	public GameObject AudioPrefab;
	public GameObject EmojiPrefab;

	// Use this for initialization
	void Start ()
	{
		textbox = GameObject.FindGameObjectWithTag ("Label");
		droppedBy = GetComponentInChildren<TextMesh> ();
		spriteImage = GetComponentInChildren<SpriteRenderer> ();
		textboxImage = textbox.GetComponent<UnityEngine.UI.Image> ();
		StartCoroutine (DroppableFetcher.FetchSpecificDroppables (this, gameObject.name, interactType.Equals (InteractableType.MAIN)));
		label = GameObject.FindObjectOfType<UnityEngine.UI.Text> ();

		if (spriteImage != null && interactType.Equals (InteractableType.DROPPABLE) && Drops [0].Dt.Equals (DroppableType.IMAGE)) {
			byte[] img = System.Convert.FromBase64String (Drops [0].Content);
			Texture2D tex = new Texture2D (400, 400);
			tex.LoadImage (img);
			Sprite s = Sprite.Create (tex, new Rect (0, 0, tex.width, tex.height), new Vector2 ());
			spriteImage.sprite = s;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) || Input.GetMouseButtonDown (0)) {
			#if UNITY_EDITOR
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			#elif UNITY_ANDROID
				Ray ray = Camera.main.ScreenPointToRay (Input.GetTouch(0).position);
			#endif

			RaycastHit hit;
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {
				if (hit.collider.name.Equals (gameObject.name)) {
					StartCoroutine (HandleClick (hit.collider.name));
				} else {
					activated = false;
					if (interactType.Equals (InteractableType.DROPPABLE)) {
						if (Drops [0].Dt.Equals (DroppableType.TEXT) && !hit.collider.name.Contains ("TEXT#")) {
							Debug.LogWarning ("Disabling shit");
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
				label.text = textboxImage.enabled ? Drops [0].UserName + ": " + SpliceText (Drops [0].Content, 20) : "";
			} else if (Drops.Count > 0 && Drops [0].Dt.Equals (DroppableType.IMAGE)) {
				droppedBy.text = activated ? Drops [0].UserName : "";
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
				switch (d.Dt) {
				case DroppableType.TEXT:
					go = Instantiate (TextPrefab, d.Position, Quaternion.identity, DroppableGO.transform);
					break;
				case DroppableType.IMAGE:
					go = Instantiate (ImagePrefab, d.Position, Quaternion.identity, DroppableGO.transform);
					break;
				case DroppableType.AUDIO:
					go = Instantiate (AudioPrefab, d.Position, Quaternion.identity, DroppableGO.transform);
					break;
				case DroppableType.EMOJI:
					go = Instantiate (EmojiPrefab, d.Position, Quaternion.identity, DroppableGO.transform);
					break;
				}

				if (go != null) {
					go.name = d.Marker;
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