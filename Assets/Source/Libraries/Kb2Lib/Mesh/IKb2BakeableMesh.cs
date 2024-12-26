using UnityEngine;

namespace Source.Libraries.KBLib2.Mesh
{
    public interface IKb2BakeableMesh
    {
        public static void InstantiateMesh(Transform pParent, UnityEngine.Mesh pMesh, Vector3 pPos, Material pMaterial, bool pAddCollision, out GameObject pGameObject, out UnityEngine.Mesh pOutMesh, out MeshFilter pMeshFilter, out MeshRenderer pMeshRenderer)
        {
            pGameObject = new GameObject($"Kb2BakeableMesh @{pPos}");
            
            pGameObject.transform.SetParent(pParent);
            pGameObject.transform.position = pPos;

            pMeshFilter = pGameObject.AddComponent<MeshFilter>();
            pMeshFilter.sharedMesh = pMesh;

            pMeshRenderer = pGameObject.AddComponent<MeshRenderer>();
            pMeshRenderer.sharedMaterial = pMaterial;

            if (pAddCollision)
            {
                var meshCollider = pGameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = pMesh;
            }

            pOutMesh = pMesh;
        }

        public UnityEngine.Mesh Bake();
    }
}