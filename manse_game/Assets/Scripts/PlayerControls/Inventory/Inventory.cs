using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEditor;
using UnityEngine;

namespace PlayerControls.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private const int CenterIndex = 0;
        private const float RadiusLength = 0.5f;
        
        [SerializeField]
        private List<Item> inventory = new List<Item>();

        private GameObject _displayObject;
        private Mesh _displayMesh;
        private float _targetRotation;
        private Vector3 _previousUp;
        private int _displayIndex = 1;
        [SerializeField]
        private UnityEngine.Camera displayCamera;
        
        public void Add(Item item) { inventory.Add(item); }

        public bool Remove(Item item) { return inventory.Remove(item); }

        public bool Contains(string itemName) { return inventory.Any(item => itemName == item.itemName); }

        public void Display()
        {
            CreateInventoryLayout(6);
            PositionInventoryLayout(_displayObject);
            _previousUp = _displayObject.transform.up;
        }

        public void DestroyDisplay()
        {
            Destroy(_displayObject);
        }

        private void CreateInventoryLayout(int itemCount)
        {
            if (itemCount < 2) itemCount = 2;
            
            var circleVertices = new List<Vector3>();
            var triangleVertices = new List<int>();
            var arcBetweenPoints = 360.0f / itemCount;
            
            // Quaternion representing the rotation between two points on the circles circumference
            var quaternion = Quaternion.Euler(0.0f, 0.0f, arcBetweenPoints);

            // Create the center of the circle, and first triangle
            var center = new Vector3(0.0f, 0.0f, 0.0f);
            circleVertices.Add(center);
            circleVertices.Add(new Vector3(0.0f, RadiusLength, 0.0f));
            circleVertices.Add(quaternion * circleVertices[1]);
            triangleVertices.Add(0);
            triangleVertices.Add(1);
            triangleVertices.Add(2);
            
            // Create the rest of the points/triangles
            for (var i = 0; i < itemCount - 1; i++)
            {
                triangleVertices.Add(CenterIndex);
                triangleVertices.Add(circleVertices.Count - 1);
                triangleVertices.Add(circleVertices.Count);
                circleVertices.Add(quaternion * circleVertices[circleVertices.Count - 1]);
            }
            Debug.Log(triangleVertices.Count);
            Debug.Log(circleVertices.Count);

            // Create the mesh from circle and triangle vertices
            // and attach it to a new GameObject
            _displayMesh = new Mesh
            {
                vertices = circleVertices.ToArray(),
                triangles = triangleVertices.ToArray(),
                name = "Inventory Mesh"
            };
            _displayObject = new GameObject("Empty")
            {
                name = "Inventory Display"
            };
            
            var meshRenderer = _displayObject.AddComponent<MeshRenderer>();
            var meshFilter = _displayObject.AddComponent<MeshFilter>();
            meshFilter.mesh = _displayMesh;
            
            meshRenderer.enabled = false;
            if (!Application.isEditor) return;
            
            foreach (var vertex in _displayMesh.vertices)
            {
                if (vertex == _displayMesh.vertices[0]) continue;
                    
                meshRenderer.material = AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
                var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                marker.transform.SetParent(_displayObject.transform);
                marker.transform.position = vertex;
                marker.transform.localScale /= 10;
            }
        }
        
        private void PositionInventoryLayout(GameObject obj)
        {
            obj.transform.SetParent(displayCamera.gameObject.transform);
            
            var position = 1.25f * displayCamera.gameObject.transform.forward;
            obj.transform.localPosition = position;

            // Ensure facing: towards player camera, with the circle oriented
            // so that the closest part of the circle to the camera is a boundary point
            var itemCount = _displayMesh.vertices.Length / 3;
            var rotation = itemCount / 3 % 2 != 0
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.Euler(0, 180, 360.0f / itemCount / 2);
            obj.transform.rotation = rotation;
            
            // Tilt the circle to give it an oblique appearance
            var tilt = Quaternion.Euler(-100, 0, 0);
            obj.transform.rotation = tilt;
        }

        private bool IsRotating() => _targetRotation != 0.0;

        private void Update()
        {
            if (_displayObject && Input.GetAxisRaw("Horizontal") != 0.0f && !IsRotating())
            {
                var direction = Input.GetAxisRaw("Horizontal") < 0.0f ? -1 : 1;
                var nextIndex = direction switch
                {
                    -1 => _displayIndex - 1 < 1 ? _displayMesh.vertices.Length - 2 : _displayIndex - 1,
                    1 => _displayIndex + 1 >= _displayMesh.vertices.Length - 2 ? 1 : _displayIndex + 1,
                    _ => throw new ArgumentOutOfRangeException()
                };
                Debug.Log("nextIndex set");
                Debug.Log("List length: " + _displayMesh.vertices.Length);
                Debug.Log("Horizontal input: " + Input.GetAxisRaw("Horizontal"));
                Debug.Log("nextIndex: " + nextIndex);

                // TODO: This angle is reusable
                var side1 = _displayMesh.vertices[_displayIndex] - _displayMesh.vertices[CenterIndex];
                var side2 = _displayMesh.vertices[nextIndex] - _displayMesh.vertices[CenterIndex];
                var angle = Vector3.Angle(side1, side2);
                _targetRotation = angle * direction;
                Debug.Log("Target rotation: " + _targetRotation);
                _previousUp = _displayObject.transform.up;
            }
        }

        private void FixedUpdate()
        {
            if (!_displayObject || !IsRotating()) return;

            var ang = Vector3.Angle(_displayObject.transform.up, _previousUp);
            if (Math.Abs(ang - Math.Abs(_targetRotation)) < 0.5f)
            {
                _targetRotation = 0.0f;
                _previousUp = _displayObject.transform.up;
            }
            else
            {
                _displayObject.transform.RotateAround(
                     _displayObject.transform.position, 
                    _targetRotation > 0 ? _displayObject.transform.forward : -1 * _displayObject.transform.forward, 
                    100 * Time.fixedDeltaTime
                    );
            }
        }
    }
}