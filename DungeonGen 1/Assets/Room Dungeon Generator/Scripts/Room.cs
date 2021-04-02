using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ooparts.dungen;

namespace ooparts.dungen
{

    public class Room : MonoBehaviour
    {

        public Corridor CorridorPrefab;
        public IntVector2 Size;
        public IntVector2 Coordinates;
        public int Num;

        private GameObject _tilesObject;
        private GameObject _wallsObject;
        private GameObject _InWallObject;
        private GameObject _lightObject;
        private GameObject _subjectObject;

        public Tile TilePrefab;
        private Tile[,] _tiles;
        public GameObject WallPrefab;
        public RoomSetting Setting;
        public GameObject InWallPrefab;
        public GameObject LightPrefab;
        public int InWallCount;
        public int MaxSubjectCount;
        public GameObject[] SubjectsPref;

        public Dictionary<Room, Corridor> RoomCorridor = new Dictionary<Room, Corridor>();

        private Map _map;


        public GameObject PlayerPrefab;
        private GameObject[] InWalls;
        private GameObject[] Lights;
        private GameObject[] Subjects;

        public void Init(Map map)
        {
            _map = map;
        }

        public IEnumerator Generate()
        {

            _tilesObject = new GameObject("Tiles");
            _tilesObject.transform.parent = transform;
            _tilesObject.transform.localPosition = Vector3.zero;

            _tiles = new Tile[Size.x, Size.z];
            for (int x = 0; x < Size.x; x++)
            {
                for (int z = 0; z < Size.z; z++)
                {
                    _tiles[x, z] = CreateTile(new IntVector2((Coordinates.x + x), Coordinates.z + z));
                }
            }
            yield return null;
        }

        private Tile CreateTile(IntVector2 coordinates)
        {
            if (_map.GetTileType(coordinates) == TileType.Empty)
            {
                _map.SetTileType(coordinates, TileType.Room);
            }
            Tile newTile = Instantiate(TilePrefab);
            newTile.Coordinates = coordinates;
            newTile.name = "Tile " + coordinates.x + ", " + coordinates.z;
            newTile.transform.parent = _tilesObject.transform;
            newTile.transform.localPosition = RoomMapManager.TileSize * new Vector3(coordinates.x - Coordinates.x - Size.x * 0.5f + 0.5f, 0f, coordinates.z - Coordinates.z - Size.z * 0.5f + 0.5f);
            newTile.transform.GetChild(0).GetComponent<Renderer>().material = Setting.floor;
            return newTile;
        }

        public Corridor CreateCorridor(Room otherRoom)
        {
            //Не создавать если уже соединено 
            if (RoomCorridor.ContainsKey(otherRoom))
            {
                return RoomCorridor[otherRoom];
            }

            Corridor newCorridor = Instantiate(CorridorPrefab);
            newCorridor.name = "Corridor (" + otherRoom.Num + ", " + Num + ")";
            newCorridor.transform.parent = transform.parent;
            newCorridor.Coordinates = new IntVector2(Coordinates.x + Size.x / 2, otherRoom.Coordinates.z + otherRoom.Size.z / 2);
            newCorridor.transform.localPosition = new Vector3(newCorridor.Coordinates.x - _map.MapSize.x / 2, 0, newCorridor.Coordinates.z - _map.MapSize.z / 2);
            newCorridor.Rooms[0] = otherRoom;
            newCorridor.Rooms[1] = this;
            newCorridor.Length = Vector3.Distance(otherRoom.transform.localPosition, transform.localPosition);
            newCorridor.Init(_map);
            otherRoom.RoomCorridor.Add(this, newCorridor);
            RoomCorridor.Add(otherRoom, newCorridor);

            return newCorridor;
        }

        public IEnumerator CreateWalls()
        {
            _wallsObject = new GameObject("Walls");
            _wallsObject.transform.parent = transform;
            _wallsObject.transform.localPosition = Vector3.zero;

            IntVector2 leftBottom = new IntVector2(Coordinates.x - 1, Coordinates.z - 1);
            IntVector2 rightTop = new IntVector2(Coordinates.x + Size.x, Coordinates.z + Size.z);
            for (int x = leftBottom.x; x <= rightTop.x; x++)
            {
                for (int z = leftBottom.z; z <= rightTop.z; z++)
                {
                    // Если это центр или угол или не стена то выполнить
                    if ((x != leftBottom.x && x != rightTop.x && z != leftBottom.z && z != rightTop.z) ||
                        ((x == leftBottom.x || x == rightTop.x) && (z == leftBottom.z || z == rightTop.z)) ||
                        (_map.GetTileType(new IntVector2(x, z)) != TileType.Wall))
                    {
                        continue;
                    }
                    Quaternion rotation = Quaternion.identity;
                    if (x == leftBottom.x)
                    {
                        rotation = MapDirection.East.ToRotation();
                    }
                    else if (x == rightTop.x)
                    {
                        rotation = MapDirection.West.ToRotation();
                    }
                    else if (z == leftBottom.z)
                    {
                        rotation = MapDirection.North.ToRotation();
                    }
                    else if (z == rightTop.z)
                    {
                        rotation = MapDirection.South.ToRotation();
                    }
                    else
                    {
                        Debug.LogError("Wall is not on appropriate location!!");
                    }

                    GameObject newWall = Instantiate(WallPrefab);
                    newWall.name = "Wall (" + x + ", " + z + ")";
                    newWall.transform.parent = _wallsObject.transform;
                    newWall.transform.localPosition = RoomMapManager.TileSize * new Vector3(x - Coordinates.x - Size.x * 0.5f + 0.5f, 0f, z - Coordinates.z - Size.z * 0.5f + 0.5f);
                    newWall.transform.localRotation = rotation;
                    newWall.transform.localScale *= RoomMapManager.TileSize;
                    newWall.transform.GetChild(0).GetComponent<Renderer>().material = Setting.wall;
                }
            }
            yield return null;
        }
        //Далее создание объектов, внутренних стен, игрока
        public IEnumerator CreateInWalls()
        {
            _InWallObject = new GameObject("Objects");
            _InWallObject.transform.parent = transform;
            _InWallObject.transform.localPosition = Vector3.zero;

            InWalls = new GameObject[InWallCount];
            IntVector2 leftBottom = new IntVector2(Coordinates.x - 1, Coordinates.z - 1);
            IntVector2 rightTop = new IntVector2(Coordinates.x + Size.x, Coordinates.z + Size.z);

            _lightObject = new GameObject("Light");
            _lightObject.transform.parent = transform;
            _lightObject.transform.localPosition = Vector3.zero;
            
            for (int i = 0; i < InWallCount; i++)
            {
                GameObject newInWall = Instantiate(InWallPrefab);
                newInWall.name = "Object " + (i + 1);
                newInWall.transform.parent = transform;
                newInWall.transform.localScale *= RoomMapManager.TileSize;
                newInWall.transform.localPosition = RoomMapManager.TileSize * new Vector3(Random.Range(-1 * _map.lensize.x + 1, _map.lensize.x) / 2, 0, Random.Range(-1 * _map.lensize.z + 1, _map.lensize.z) / 2);
                newInWall.transform.GetChild(0).GetComponent<Renderer>().material = Setting.wall;
                if (newInWall.transform.localPosition == Vector3.zero)
                {
                    Destroy(newInWall);
                }
                if (Random.value <= 0.05)
                {
                    GameObject newLight = Instantiate(LightPrefab);
                    newLight.transform.parent = transform;
                    newLight.transform.localScale *= RoomMapManager.TileSize / 2;
                    newLight.transform.localPosition = newInWall.transform.localPosition + RoomMapManager.TileSize * (new Vector3(0, 1.5f, 1) / 2f);
                    if (newLight.transform.localPosition == RoomMapManager.TileSize * new Vector3(0, 1.5f, 1f) / 2f)
                    {
                        Destroy(newLight);
                    }
                }
                if (Random.value <= 0.05)
                {
                    GameObject newLight = Instantiate(LightPrefab);
                    newLight.transform.parent = transform;
                    newLight.transform.localScale *= RoomMapManager.TileSize / 2;
                    newLight.transform.localPosition = newInWall.transform.localPosition + RoomMapManager.TileSize * (new Vector3(0, 1.5f, -1) / 2f);
                    newLight.transform.rotation = Quaternion.Euler(180, 0, 180);
                    if (newLight.transform.localPosition == RoomMapManager.TileSize * new Vector3(0, 1.5f, -1f) / 2f)
                    {
                        Destroy(newLight);
                    }
                }
                if (Random.value <= 0.05)
                {
                    GameObject newLight = Instantiate(LightPrefab);
                    newLight.transform.parent = transform;
                    newLight.transform.localScale *= RoomMapManager.TileSize / 2;
                    newLight.transform.localPosition = newInWall.transform.localPosition + RoomMapManager.TileSize * (new Vector3(1, 1.5f, 0) / 2f);
                    newLight.transform.rotation = Quaternion.Euler(0, 90, 0);
                    if (newLight.transform.localPosition == RoomMapManager.TileSize * new Vector3(1, 1.5f, 0) / 2f)
                    {
                        Destroy(newLight);
                    }
                }
                if (Random.value <= 0.05)
                {
                    GameObject newLight = Instantiate(LightPrefab);
                    newLight.transform.parent = transform;
                    newLight.transform.localScale *= RoomMapManager.TileSize / 2;
                    newLight.transform.localPosition = newInWall.transform.localPosition + RoomMapManager.TileSize * (new Vector3(-1, 1.5f, 0) / 2f);
                    newLight.transform.rotation = Quaternion.Euler(0, -90, 0);
                    if (newLight.transform.localPosition == RoomMapManager.TileSize * new Vector3(-1, 1.5f, 0) / 2f)
                    {
                        Destroy(newLight);
                    }
                }
                InWalls[i] = newInWall;
            }
            yield return null;
        }

        public IEnumerator CreateSubjects()
        {
            _subjectObject = new GameObject("Subjects");
            _subjectObject.transform.parent = transform;
            _subjectObject.transform.localPosition = Vector3.zero;

            Subjects = new GameObject[MaxSubjectCount];
            IntVector2 leftBottom = new IntVector2(Coordinates.x - 1, Coordinates.z - 1);
            IntVector2 rightTop = new IntVector2(Coordinates.x + Size.x, Coordinates.z + Size.z);

            {
                for (int s = 0; s < MaxSubjectCount; s++)
                {
                    GameObject newSubject = Instantiate(SubjectsPref[Random.Range(0, SubjectsPref.Length)]);
                    newSubject.name = "Subject " + (s + 1);
                    newSubject.transform.parent = transform;
                    newSubject.transform.localScale *= RoomMapManager.TileSize;
                    newSubject.transform.localPosition = RoomMapManager.TileSize * new Vector3(Random.Range(-1 * _map.lensize.x + 2, _map.lensize.x - 1) / 2, 0.3f, Random.Range(-1 * _map.lensize.z + 2, _map.lensize.z - 1) / 2);
                    newSubject.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90, 0);
                    if (newSubject.transform.localPosition == Vector3.zero)
                    {
                        Destroy(newSubject);
                    }
                    Subjects[s] = newSubject;
                }
            }
            yield return null;
        }

        public IEnumerator CreatePlayer()
		{
			GameObject player = Instantiate((PlayerPrefab));
			player.name = "Player";
			player.transform.parent = transform.parent;
			player.transform.localPosition = transform.localPosition;
			yield return null;
		}
	}
}