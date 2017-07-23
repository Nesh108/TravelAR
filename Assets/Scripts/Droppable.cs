using System;
using UnityEngine;

public class Droppable
{	
	public static string IP = "192.168.3.28";//"10.2.12.162";
	public string Parent;
	public string Marker;
	public DroppableType Dt;
	public string UserName;
	public DateTime DateDropped;
	public string Content;
	public Vector3 Position;

	public Droppable (string p, string m, DroppableType drop, string username, DateTime date, string c, Vector3 pos)
	{
		Parent = p;
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
