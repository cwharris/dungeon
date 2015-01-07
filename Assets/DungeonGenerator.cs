using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour {

	public Room roomPrefab;
	public Room startRoomPrefab;
	public int totalRooms = 10;

	// Use this for initialization
	void Start () {
		var blah = Mathf.FloorToInt (Mathf.Sqrt (totalRooms));
		var rooms = new Dictionary2D<int, int, Room> ();
		var x = 0;
		var y = 0;
		int xInc;
		int yInc;
		int count = 0;
		Room currentRoom, previousRoom, startRoom = (Room)GameObject.Instantiate (startRoomPrefab, new Vector3 (x, y, 0), Quaternion.identity);
		rooms.Set (x, y, startRoom);
		previousRoom = startRoom;
		while (count < totalRooms) {
			GetNextDirection(out xInc, out yInc);
			x += xInc;
			y += yInc;
			if (x > blah || x < -blah || y > blah || y < -blah) {
				x = xInc;
				y = yInc;
				previousRoom = startRoom;
			}
			if (!rooms.TryGet(x, y, out currentRoom)) {
				// generate a new room
				currentRoom = (Room)GameObject.Instantiate(roomPrefab, new Vector3(x, y, 0), Quaternion.identity);
				rooms.Set(x, y, currentRoom);
				count++;
			}
			// connect with previous room
			if (xInc > 0) {
				currentRoom.doorLeft.enabled = true;
				previousRoom.doorRight.enabled = true;
			}
			if (xInc < 0) {
				currentRoom.doorRight.enabled = true;
				previousRoom.doorLeft.enabled = true;
			}
			if (yInc > 0) {
				currentRoom.doorBottom.enabled = true;
				previousRoom.doorTop.enabled = true;
			}
			if (yInc < 0) {
				currentRoom.doorTop.enabled = true;
				previousRoom.doorBottom.enabled = true;
			}
			previousRoom = currentRoom;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GetNextDirection(out int x, out int y) {
		switch (Random.Range (0, 4)) {
		case 0:
			x = 1; y = 0;
			break;
		case 1:
			x = -1; y = 0;
			break;
		case 2:
			x = 0; y = 1;
			break;
		case 3:
			x = 0; y = -1;
			break;
		default:
			throw new System.Exception();
		}
	}

	private class Dictionary2D<X, Y, T>
	{
		private Dictionary<X, Dictionary<Y, T>> items = new Dictionary<X, Dictionary<Y, T>> ();

		public void Set(X x, Y y, T obj) {
			if (!items.ContainsKey (x)) {
				items.Add(x, new Dictionary<Y, T>());
			}
			items [x] [y] = obj;
  		}

		public T Get(X x, Y y) {
			return Get (x, y, default(T));
		}			
					
		public T Get(X x, Y y, T def) {
			return items.ContainsKey (x) && items [x].ContainsKey (y)
				? items [x] [y]
				: def;
		}

		public bool TryGet(X x, Y y, out T obj) {
			if (items.ContainsKey (x) && items [x].ContainsKey (y)) {
				obj = items [x] [y];
				return true;
			} else {
				obj = default(T);
				return false;
			}
		}
	}
}
