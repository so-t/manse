using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerControls.Inventory
{
    public class InventoryDisplay
    {
        private const int CenterIndex = 0;
        private const int RotatingLeft = 1;
        private const int RotatingRight = -1;
        private const int NotRotating = 0;
        
        private const float RotationTimeSeconds = 0.5f;
        
        private readonly float _rotationAngle;
        private readonly float _rotationSpeed;
        
        private readonly GameObject _object;
        private readonly Mesh _mesh;
        private readonly List<GameObject> _itemList;

        private int _rotationDirection;
        private Vector3 _previousUpVector;

        public InventoryDisplay(List<GameObject> itemList, GameObject parentObject, float radiusLength=0.5f)
        {
            _itemList = itemList;
            _mesh = CreateMesh(_itemList.Count, radiusLength);
            _object = CreateObject(_mesh, parentObject);
            
            _rotationAngle = RotationAngleFromMesh(_mesh);
            _rotationSpeed = _rotationAngle / RotationTimeSeconds;
            _previousUpVector = _object.transform.up;
        }
        
        public void Close()
        {
            // Disable renderers and remove parents from gameObjects in _itemList
            // so they do not get destroyed along with _object
            foreach (var item in _itemList)
            {
                item.GetComponent<MeshRenderer>().enabled = false;
                item.transform.SetParent(null);
            }
            Object.Destroy(_object);
        }

        private static Mesh CreateMesh(int itemCount, float radiusLength)
        {
            if (itemCount < 2) itemCount = 2; // TODO: Code specific case for 1

            var circleVertices = new List<Vector3>();
            var triangleVertices = new List<int>();
            var arcBetweenPoints = 360.0f / itemCount;
        
            // Quaternion representing the rotation between two points on the circles circumference
            var quaternion = Quaternion.Euler(0.0f, 0.0f, arcBetweenPoints);

            // Create the center of the circle, and first triangle
            var center = new Vector3(0.0f, 0.0f, 0.0f);
            circleVertices.Add(center);
            circleVertices.Add(new Vector3(0.0f, radiusLength, 0.0f));
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
        
            return new Mesh
            {
                vertices = circleVertices.ToArray(),
                triangles = triangleVertices.ToArray(),
                name = "Inventory Mesh"
            };
        }
        
        private GameObject CreateObject(Mesh mesh, GameObject parentObject)
        {
            var obj = new GameObject("Empty")
            {
                name = "Inventory Display"
            };
            var meshFilter = obj.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;

            // Add the game objects from _itemList as children of obj
            // Position one at each boundary point around the edge of the circle
            //    
            // i = 1 as mesh.vertices[0] is the center vertex of the circle
            for (var i = 1; i < _itemList.Count; i++)
            {
                var item = _itemList[i];
                item.transform.rotation = Quaternion.identity;
                item.transform.SetParent(obj.transform);
                item.transform.position = mesh.vertices[i]; 
                
                // The parent object's 'up' is pointing away from it's parent
                // Rotate the display items so that they face the parent's parent
                item.transform.RotateAround(item.transform.position, obj.transform.right, 90); 
                var localScale = item.transform.localScale;
                var maxDimension = new[] {localScale.x, localScale.y, localScale.z }.Max();
                var ratio = 0.1f/maxDimension;
                localScale *= ratio;
                item.transform.localScale = localScale;
                item.GetComponent<MeshRenderer>().enabled = true;
            }
            
            PositionObject(obj,parentObject);
            
            return obj;
        }
        
        private void PositionObject(GameObject obj, GameObject parentObj, bool tiltObject=true)
        {
            obj.transform.SetParent( parentObj.transform);
        
            var position = 1.25f *  parentObj.transform.forward;
            obj.transform.localPosition = position;

            // Ensure facing: towards player camera, with the circle oriented
            // so that the closest part of the circle to the camera is a boundary point
            var itemCount = _mesh.vertices.Length / 3;
            var rotation = itemCount / 3 % 2 != 0
                ? Quaternion.Euler(0, 180, 0)
                : Quaternion.Euler(0, 180, 360.0f / itemCount / 2);
            obj.transform.rotation = rotation;

            if (!tiltObject) return;
            
            // Tilt the circle to give it an oblique appearance
            var tilt = Quaternion.Euler(-100, 0, 0);
            obj.transform.rotation = tilt;
        }

        private static float RotationAngleFromMesh(Mesh mesh)
        {
            var side1 = mesh.vertices[1] - mesh.vertices[CenterIndex];
            var side2 = mesh.vertices[2] - mesh.vertices[CenterIndex];
            var angle = Vector3.Angle(side1, side2);
            
            return angle;
        }

        public bool IsRotating() => _rotationDirection != NotRotating;

        public bool HasFinishedRotating()
        {
            var ang = Vector3.Angle(_object.transform.up, _previousUpVector);
            return Math.Abs(ang - Math.Abs(_rotationAngle)) < 0.6f;
        }

        public void SetRotationDirection(float direction)
        {
            if (direction == 0.0f) _rotationDirection = NotRotating;
            else _rotationDirection = direction > 0.0f? RotatingLeft : RotatingRight;
        }

        public void Rotate()
        {
            _object.transform.RotateAround(
                _object.transform.position, 
                _rotationDirection * _object.transform.forward,
                _rotationSpeed * Time.fixedDeltaTime
            );
        }

        public void StopRotating()
        {
            _rotationDirection = NotRotating;
            _previousUpVector = _object.transform.up;
        }
        
        public static void RotateDisplayObject(GameObject obj)
        {
            // obj.transform.RotateAround(
            //     obj.transform.position,
            //     obj.transform.up,
            //     100 * Time.fixedDeltaTime
            // );
        }
    }
}