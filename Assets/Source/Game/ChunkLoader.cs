using System.Collections;
using System.Diagnostics;
using Source.Libraries.KBLib2;
using Source.Libraries.KBLib2.Mesh;
using TMPro;
using UnityEngine;

namespace Source.Game
{
    public class ChunkLoader : Kb2Behaviour
    {
        public Material        chunkMaterial;
        public TextMeshProUGUI debugText;

        public float chunkDisplaySize;
        public int   renderDistance;
        // public float lod0Distance;

        public Transform chunkParent;

        private float mDebugTextUpdateTimer = 0;

        private struct ChunkObject
        {
            public MeshFilter   Filter;
            public MeshRenderer Renderer;

            public ChunkObject(MeshFilter pFilter, MeshRenderer pRenderer)
            {
                Filter   = pFilter;
                Renderer = pRenderer;
            }
        }

        private ChunkObject?[,]  mChunkMeshes;
        private Kb2HeightmapMesh mMesh;

        private void Start()
        {
            mMesh = new Kb2HeightmapMesh();
            mMesh.Deserialize(@"C:\Users\Made\Documents\src\cs\cross\srtm\workdir\chunks.kchk");

            mChunkMeshes = new ChunkObject?[mMesh.ChunkCountX, mMesh.ChunkCountZ];
        }

        private int BitSize(int pBits)
        {
            var size = 0;

            for (; pBits != 0; pBits >>= 1)
            {
                size++;
            }

            return size;
        }

        private void Update()
        {
            var pos = tf.position;

            var chX = (int)(pos.x / chunkDisplaySize);
            var chZ = (int)(pos.z / chunkDisplaySize);

            var text = "";
            text += $"FPS: {1 / Time.deltaTime:0.00}\n";
            text += $"chunk: {chX}, {chZ}\n";
            text += $"renderDistance: {renderDistance}\n";
            text += $"xyz: {pos}\n";

            if (chX >= 0 && chZ >= 0)
            {
                var startX = Mathf.Max(0, chX - renderDistance);
                var startZ = Mathf.Max(0, chZ - renderDistance);

                var endX = Mathf.Min(mMesh.ChunkCountX, chX + renderDistance + 1);
                var endZ = Mathf.Min(mMesh.ChunkCountZ, chZ + renderDistance + 1);


                var processedChunks = new BitArray(mMesh.ChunkCountX * mMesh.ChunkCountZ);

                var watch = new Stopwatch();
                watch.Start();
                for (var x = startX; x < endX; x++)
                {
                    for (var z = startZ; z < endZ; z++)
                    {
                        processedChunks.Set(z * mMesh.ChunkCountX + x, true);

                        var distance = (int)Vector2Int.Distance(new Vector2Int(x, z), new Vector2Int(startX, startZ));
                        // var lod      = Mathf.Min(BitSize(distance), mMesh.LodLevels);
                        var lod      = (int)Mathf.Min(Mathf.Log(Mathf.Max(distance, 1), 1.5F), mMesh.LodLevels);
                        var chunk    = mMesh.ProvideChunk(x, z, lod, chunkDisplaySize);

                        ChunkObject? chunkObject;
                        if ((chunkObject = mChunkMeshes[x, z]) != null)
                        {
                            var tmpChk = chunkObject.Value;
                            tmpChk.Filter.sharedMesh = chunk;

                            mChunkMeshes[x, z] = tmpChk;
                        }
                        else
                        {
                            IKb2BakeableMesh.InstantiateMesh(chunkParent, chunk, new Vector3(x * chunkDisplaySize, 0, z * chunkDisplaySize), chunkMaterial, false, out _, out _, out var filter, out var meshRenderer);
                            mChunkMeshes[x, z] = new ChunkObject(filter, meshRenderer);
                        }
                    }
                }
                watch.Stop();

                text += $"Providing chunks took {watch.Elapsed.TotalMilliseconds:0.00000} ms\n";

                watch.Restart();
                for (int z = 0, i = 0; z < mMesh.ChunkCountZ; z++)
                {
                    for (var x = 0; x < mMesh.ChunkCountX; x++)
                    {
                        if (mChunkMeshes[x, z] is { } chunk)
                        {
                            chunk.Renderer.enabled = processedChunks.Get(i);
                        }
                        i++;
                    }
                }
                watch.Stop();

                text += $"Hiding chunks took {watch.Elapsed.TotalMilliseconds:0.00000} ms\n";
            }

            // if (mDebugTextUpdateTimer >= 0.5F)
            // {
            //     mDebugTextUpdateTimer = 0;
            //     debugText.text        = text;
            // }
            debugText.text        =  text;
            mDebugTextUpdateTimer += Time.deltaTime;
        }
    }
}