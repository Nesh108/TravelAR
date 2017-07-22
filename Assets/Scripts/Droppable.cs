using System;
using UnityEngine;

public class Droppable
{	
	public string Marker;
	public DroppableType Dt;
	public string UserName;
	public DateTime DateDropped;
	public string Content;
	public Vector3 Position;

	public Droppable (string m, DroppableType drop, string username, DateTime date, string c, Vector3 pos)
	{
		Marker = m;
		Dt = drop;
		UserName = username;
		DateDropped = date;
		Content = c;
		Position = pos;
	}
}

public enum DroppableType {
	TEXT,
	AUDIO,
	IMAGE,
	EMOJI
}
