using UnityEngine;

namespace DefaultNamespace
{
    public class PointController : MonoBehaviour
    {
        private void Start()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh.SetIndices(meshFilter.mesh.GetIndices(0), MeshTopology.Points, 0);
        }
    }
}