using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HospitalRescue.Infrastructure.Services
{
    /// <summary>
    /// Generates a prototype hospital blockout for testing
    /// </summary>
    public class HospitalBlockout : MonoBehaviour
    {
        [Header("Room Settings")]
        [SerializeField] private int roomsX = 3;
        [SerializeField] private int roomsZ = 3;
        [SerializeField] private float roomWidth = 8f;
        [SerializeField] private float roomDepth = 8f;
        [SerializeField] private float roomHeight = 3f;
        [SerializeField] private float wallThickness = 0.2f;
        [SerializeField] private float doorWidth = 1.2f;
        [SerializeField] private float doorHeight = 2.2f;
        
        [Header("Materials")]
        [SerializeField] private Material floorMaterial;
        [SerializeField] private Material wallMaterial;
        [SerializeField] private Material ceilingMaterial;
        
        private void Awake()
        {
            // Placeholder materials if none assigned
            if (floorMaterial == null)
                floorMaterial = CreatePlaceholderMaterial("FloorMat", new Color(0.3f, 0.3f, 0.35f));
            if (wallMaterial == null)
                wallMaterial = CreatePlaceholderMaterial("WallMat", new Color(0.8f, 0.8f, 0.85f));
            if (ceilingMaterial == null)
                ceilingMaterial = CreatePlaceholderMaterial("CeilingMat", new Color(0.9f, 0.9f, 0.9f));
        }
        
        [ContextMenu("Generate Hospital Blockout")]
        public void Generate()
        {
            ClearExisting();
            
            for (int x = 0; x < roomsX; x++)
            {
                for (int z = 0; z < roomsZ; z++)
                {
                    CreateRoom(x, z);
                }
            }
            
            CreateCorridors();
            
            Debug.Log($"Hospital blockout generated: {roomsX}x{roomsZ} rooms");
        }
        
        [ContextMenu("Clear Hospital")]
        public void ClearExisting()
        {
            while (transform.childCount > 0)
            {
                #if UNITY_EDITOR
                if (!UnityEngine.Application.isPlaying)
                    DestroyImmediate(transform.GetChild(0).gameObject);
                else
                    Destroy(transform.GetChild(0).gameObject);
                #else
                Destroy(transform.GetChild(0).gameObject);
                #endif
            }
        }
        
        private void CreateRoom(int x, int z)
        {
            Vector3 roomCenter = new Vector3(
                x * roomWidth,
                0,
                z * roomDepth
            );
            
            // Create room parent
            GameObject room = new GameObject($"Room_{x}_{z}");
            room.transform.SetParent(transform);
            room.transform.position = roomCenter;
            
            // Floor
            CreateFloor(room.transform, roomWidth, roomDepth);
            
            // Ceiling
            CreateCeiling(room.transform, roomWidth, roomDepth);
            
            // Walls
            CreateWalls(room.transform, x, z);
        }
        
        private void CreateFloor(Transform parent, float width, float depth)
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Floor";
            floor.transform.SetParent(parent);
            floor.transform.localPosition = new Vector3(0, 0.05f, 0);
            floor.transform.localScale = new Vector3(width, 0.1f, depth);
            floor.GetComponent<Renderer>().material = floorMaterial;
            
            // Remove collider for performance in blockout
            #if UNITY_EDITOR
            if (!UnityEngine.Application.isPlaying)
                DestroyImmediate(floor.GetComponent<Collider>());
            else
                Destroy(floor.GetComponent<Collider>());
            #endif
        }
        
        private void CreateCeiling(Transform parent, float width, float depth)
        {
            GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ceiling.name = "Ceiling";
            ceiling.transform.SetParent(parent);
            ceiling.transform.localPosition = new Vector3(0, roomHeight, 0);
            ceiling.transform.localScale = new Vector3(width, 0.1f, depth);
            ceiling.GetComponent<Renderer>().material = ceilingMaterial;
            
            #if UNITY_EDITOR
            if (!UnityEngine.Application.isPlaying)
                DestroyImmediate(ceiling.GetComponent<Collider>());
            else
                Destroy(ceiling.GetComponent<Collider>());
            #endif
        }
        
        private void CreateWalls(Transform parent, int roomX, int roomZ)
        {
            float halfWidth = roomWidth / 2f;
            float halfDepth = roomDepth / 2f;
            
            // North wall (positive Z)
            if (roomZ >= roomsZ - 1 || roomZ == 0 && roomX == 1)
            {
                CreateWall(parent, new Vector3(0, roomHeight/2f, halfDepth), 
                    new Vector3(roomWidth, roomHeight, wallThickness), "Wall_North", true);
            }
            
            // South wall (negative Z) 
            if (roomZ == 0 || roomZ < roomsZ - 1)
            {
                bool hasDoor = roomX == 1 && roomZ == 0;
                CreateWall(parent, new Vector3(0, roomHeight/2f, -halfDepth), 
                    new Vector3(roomWidth, roomHeight, wallThickness), "Wall_South", hasDoor);
            }
            
            // East wall (positive X)
            if (roomX >= roomsX - 1)
            {
                bool hasDoor = roomX == 2 && roomZ == 1;
                CreateWall(parent, new Vector3(halfWidth, roomHeight/2f, 0), 
                    new Vector3(wallThickness, roomHeight, roomDepth - (hasDoor ? doorWidth : 0)), "Wall_East", false);
                
                if (hasDoor)
                {
                    CreateDoorway(parent, new Vector3(halfWidth, 0, 0), "Doorway_East");
                }
            }
            
            // West wall (negative X)
            if (roomX == 0)
            {
                bool hasDoor = roomX == 0 && roomZ == 1;
                CreateWall(parent, new Vector3(-halfWidth, roomHeight/2f, 0), 
                    new Vector3(wallThickness, roomHeight, roomDepth - (hasDoor ? doorWidth : 0)), "Wall_West", false);
                
                if (hasDoor)
                {
                    CreateDoorway(parent, new Vector3(-halfWidth, 0, 0), "Doorway_West");
                }
            }
        }
        
        private void CreateWall(Transform parent, Vector3 position, Vector3 scale, string name, bool hasDoor)
        {
            GameObject wall;
            
            if (hasDoor)
            {
                // Wall with door opening - create multiple pieces
                float doorCenterOffset = doorWidth / 2f + 0.5f;
                
                // Left part
                GameObject wallLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wallLeft.name = name + "_Left";
                wallLeft.transform.SetParent(parent);
                wallLeft.transform.localPosition = position + new Vector3(-doorCenterOffset, 0, 0);
                wallLeft.transform.localScale = new Vector3(scale.x - doorWidth/2f - 0.5f, scale.y, scale.z);
                wallLeft.GetComponent<Renderer>().material = wallMaterial;
                
                // Right part
                GameObject wallRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wallRight.name = name + "_Right";
                wallRight.transform.SetParent(parent);
                wallRight.transform.localPosition = position + new Vector3(doorCenterOffset, 0, 0);
                wallRight.transform.localScale = new Vector3(scale.x - doorWidth/2f - 0.5f, scale.y, scale.z);
                wallRight.GetComponent<Renderer>().material = wallMaterial;
                
                // Top part (above door)
                GameObject wallTop = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wallTop.name = name + "_Top";
                wallTop.transform.SetParent(parent);
                wallTop.transform.localPosition = position + new Vector3(0, doorHeight + (roomHeight - doorHeight)/2f, 0);
                wallTop.transform.localScale = new Vector3(doorWidth, roomHeight - doorHeight, scale.z);
                wallTop.GetComponent<Renderer>().material = wallMaterial;
                
                #if UNITY_EDITOR
                foreach (var obj in new[] { wallLeft, wallRight, wallTop })
                {
                    if (!UnityEngine.Application.isPlaying)
                        DestroyImmediate(obj.GetComponent<Collider>());
                    else
                        Destroy(obj.GetComponent<Collider>());
                }
                #endif
            }
            else
            {
                wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.name = name;
                wall.transform.SetParent(parent);
                wall.transform.localPosition = position;
                wall.transform.localScale = scale;
                wall.GetComponent<Renderer>().material = wallMaterial;
                
                #if UNITY_EDITOR
                if (!UnityEngine.Application.isPlaying)
                    DestroyImmediate(wall.GetComponent<Collider>());
                else
                    Destroy(wall.GetComponent<Collider>());
                #endif
            }
        }
        
        private void CreateDoorway(Transform parent, Vector3 position, string name)
        {
            // Door frame
            GameObject doorFrame = new GameObject(name);
            doorFrame.transform.SetParent(parent);
            doorFrame.transform.localPosition = position;
            
            // Left frame
            GameObject leftFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftFrame.name = "Frame_Left";
            leftFrame.transform.SetParent(doorFrame.transform);
            leftFrame.transform.localPosition = new Vector3(0, doorHeight/2f, -doorWidth/2f);
            leftFrame.transform.localScale = new Vector3(0.1f, doorHeight, 0.1f);
            
            // Right frame
            GameObject rightFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightFrame.name = "Frame_Right";
            rightFrame.transform.SetParent(doorFrame.transform);
            rightFrame.transform.localPosition = new Vector3(0, doorHeight/2f, doorWidth/2f);
            rightFrame.transform.localScale = new Vector3(0.1f, doorHeight, 0.1f);
            
            #if UNITY_EDITOR
            foreach (var obj in doorFrame.GetComponentsInChildren<Renderer>())
            {
                if (!UnityEngine.Application.isPlaying)
                    DestroyImmediate(obj.GetComponent<Collider>());
                else
                    Destroy(obj.GetComponent<Collider>());
            }
            #endif
        }
        
        private void CreateCorridors()
        {
            // Main corridor connecting rooms
            GameObject corridor = new GameObject("MainCorridor");
            corridor.transform.SetParent(transform);
            corridor.transform.position = new Vector3(roomWidth / 2f, 0, roomDepth / 2f);
            
            // Floor
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Floor";
            floor.transform.SetParent(corridor.transform);
            floor.transform.localPosition = new Vector3(0, 0.05f, 0);
            floor.transform.localScale = new Vector3(roomWidth * 2f, 0.1f, roomDepth);
            floor.GetComponent<Renderer>().material = floorMaterial;
            
            #if UNITY_EDITOR
            if (!UnityEngine.Application.isPlaying)
                DestroyImmediate(floor.GetComponent<Collider>());
            else
                Destroy(floor.GetComponent<Collider>());
            #endif
        }
        
        private Material CreatePlaceholderMaterial(string name, Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.name = name;
            mat.color = color;
            return mat;
        }
    }
}
