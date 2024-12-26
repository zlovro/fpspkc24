using System.Diagnostics;
using Source.Libraries.KBLib2;
using Source.Libraries.KBLib2.Mesh;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Input = UnityEngine.Input;

namespace Source.Game
{
    public class HeightmapImporter : Kb2Behaviour
    {
        [Range(0, 8)]
        public int lod;
        
        private void Start()
        {
            LoadTerrain();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LoadTerrain();
            }
        }

        private void LoadTerrain()
        {
            // foreach (Transform child in tf)
            // {
            //     Destroy(child.gameObject);
            // }
            //
            // var mesh = new Kb2HeightmapMesh();
            // mesh.Deserialize(@"C:\Users\Made\Documents\src\cs\cross\srtm\workdir\chunks.kchk");
            //
            // var watch = new Stopwatch();
            //
            // var tookChunks = 0.0;
            // var tookMeshes = 0.0;
            //
            // var chunksX = 12;
            // var chunksZ = 12;
            //
            // // var chunksX = mesh.ChunkCountX;
            // // var chunksZ = mesh.ChunkCountZ;
            //
            // var chCount = chunksX * chunksZ;
            //
            // for (var chZ = 0; chZ < chunksZ; chZ++)
            // {
            //     for (var chX = 0; chX < chunksX; chX++)
            //     {
            //         watch.Restart();
            //         var chunk = mesh.LoadChunk(chX, chZ, lod);
            //         watch.Stop();
            //
            //         tookChunks += watch.Elapsed.TotalMilliseconds;
            //
            //         watch.Restart();
            //         IKb2BakeableMesh.InstantiateAndBakeMesh(chunk, tf, new Vector3(chX * chunk.DisplayWidth, 0, chZ * chunk.DisplayHeight),  out var bakedObject, out var bakedMesh);
            //         watch.Stop();
            //
            //         bakedObject.name = $"X {chX} Z {chZ}";
            //
            //         tookMeshes += watch.Elapsed.TotalMilliseconds;
            //
            //         // var child = new GameObject($"Chunk {chX:00000}:{chZ:00000}");
            //         // child.transform.SetParent(tf);
            //         //
            //         // child.AddComponent<MeshRenderer>().sharedMaterial = meshMaterial;
            //         //
            //         // var meshFilter = child.AddComponent<MeshFilter>();
            //         // meshFilter.sharedMesh = chunk.Bake();
            //     }
            // }
            //
            // Debug.Log($"Loading chunks took {tookChunks} ms ({1000 * tookChunks / chCount} us per chunk, total average {(1000 * (tookChunks + tookMeshes)) / (chCount * 2)} us)");
            // Debug.Log($"Loading meshes took {tookMeshes / 1000} s ({tookMeshes / chCount} ms per mesh)");
        }
    }
}