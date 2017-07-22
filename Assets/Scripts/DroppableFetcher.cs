using System;
using System.Collections.Generic;

public class DroppableFetcher
{
	public static List<Droppable> FetchDroppables ()
	{
		List<Droppable> drops = new List<Droppable> ();
		drops.Add (
			new Droppable (
				"TEXT#00000000000000",
				DroppableType.TEXT,
				"Nesh",
				DateTime.Now,
				"Hello Peoples!!", new UnityEngine.Vector3(2, 5, 2)));
		drops.Add (
			new Droppable (
				"TEXT#00000000000001",
				DroppableType.TEXT,
				"Lily",
				DateTime.Now,
				"Imanoob!!", new UnityEngine.Vector3(2, 5, 2)));

		drops.Add (
			new Droppable (
				"TEXT#00000000000002",
				DroppableType.TEXT,
				"Badooo",
				DateTime.Now,
				"YOooo!!", new UnityEngine.Vector3(2, 5, 2)));
		drops.Add (
			new Droppable (
				"IMAGE#0000000000003",
				DroppableType.IMAGE,
				"Badooo",
				DateTime.Now,
				"", new UnityEngine.Vector3(2, 5, 2)));
		

		return drops;
	}

	public static List<Droppable> FetchSpecificDroppables (string id)
	{
		List<Droppable> allDrops = FetchDroppables ();
		List<Droppable> drops = new List<Droppable> ();

		foreach (Droppable d in allDrops) {
			if (d.Marker.Equals (id)) {
				drops.Add (d);
			}
		}

		return drops;
	}
}

