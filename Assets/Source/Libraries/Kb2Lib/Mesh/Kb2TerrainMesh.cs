using System.Collections.Generic;
using UnityEngine;

namespace Source.Libraries.KBLib2.Mesh
{
    public class Kb2TerrainMesh : IKb2BakeableMesh
    {
        public Vector2Int TopLeftTile;

        public Dictionary<Vector2Int, Kb2HeightmapMesh> ChunkMeshes = new();

        public UnityEngine.Mesh Bake()
        {
            var bakedMesh = new UnityEngine.Mesh();

            // foreach (var (tilePos, value) in ChunkMeshes)
            // {
            //     var pos = new Vector3((TopLeftTile.x - tilePos.x) * value.VertexWidth, 0, (TopLeftTile.y - tilePos.y) * value.VertexHeight);
            //     IKb2BakeableMesh.InstantiateAndBakeMesh(value, pos);
            // }

            return bakedMesh;
        }
        public byte[] Serialize()
        {
            throw new System.NotImplementedException();
        }
        public void Deserialize(byte[] pSerialized)
        {
            throw new System.NotImplementedException();
        }
        public string GetWatermark()
        {
            throw new System.NotImplementedException();
        }
    }
}