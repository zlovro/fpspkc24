using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Source.Libraries.KBLib2.Mesh
{
    public class Kb2HeightmapChunkMesh : IKb2BakeableMesh
    {
        public UnityEngine.Mesh[] CachedLodMeshes;
        public ushort[][]           HeightData;

        public int   Size;
        public float DisplaySize;
        public int   LodLevels;

        private int mCurrentLod;

        public static int BitSize(int pBits)
        {
            var size = 0;

            for (; pBits != 0; pBits >>= 1)
            {
                size++;
            }

            return size;
        }

        public Kb2HeightmapChunkMesh(int pSize) : this(pSize, pSize)
        {

        }
        public Kb2HeightmapChunkMesh(int pSize, float pDisplaySize)
        {
            Size        = pSize;
            DisplaySize = pDisplaySize;

            LodLevels  = BitSize(Size) - 1;
            HeightData = new ushort[LodLevels][];

            for (var i = 0; i < LodLevels; i++)
            {
                var sz = Size >> i;
                HeightData[i] = new ushort[sz * sz];
            }

            CachedLodMeshes = new UnityEngine.Mesh[LodLevels];
        }

        public void ReloadLodMesh()
        {
            Array.Clear(CachedLodMeshes, 0, CachedLodMeshes.Length);
        }

        public UnityEngine.Mesh Bake(int pLod)
        {
            mCurrentLod = pLod;
            return Bake();
        }

        public UnityEngine.Mesh Bake()
        {
            var mesh = new UnityEngine.Mesh
            {
                indexFormat = IndexFormat.UInt16
            };

            var lodSize = Size >> mCurrentLod;
            
            var vtxCount  = lodSize * lodSize;
            var triangles = new int[(lodSize - 1) * (lodSize - 1) * 6];
            var vertices  = new Vector3[vtxCount];
            var colors    = new Color[vtxCount];

            var scalar = DisplaySize / (lodSize - 1);

            for (int y = 0, i = 0; y < lodSize; y++)
            {
                for (var x = 0; x < lodSize; x++)
                {
                    var t = HeightData[mCurrentLod][i] / 65535.0F;

                    vertices[i] = new Vector3(x * scalar, t * 1000, y * scalar);
                    colors[i]   = new Color(t, t, t);

                    i++;
                }
            }

            for (int y = 0, triIdx = 0, i = 0; y < lodSize - 1; y++)
            {
                for (var x = 0; x < lodSize - 1; x++)
                {
                    triangles[triIdx++] = i;
                    triangles[triIdx++] = i + lodSize;
                    triangles[triIdx++] = i + 1;

                    triangles[triIdx++] = i + 1;
                    triangles[triIdx++] = i + lodSize;
                    triangles[triIdx++] = i + 1 + lodSize;

                    i++;
                }

                i++;
            }

            mesh.vertices  = vertices;
            mesh.colors    = colors;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();

            CachedLodMeshes[mCurrentLod] = mesh;

            return mesh;
        }

        public void Deserialize(byte[] pSerialized)
        {
            var uncompressed = Kb2Compression.GzipDecompress(pSerialized);

            Buffer.BlockCopy(uncompressed, 0, HeightData[0], 0, uncompressed.Length);

            for (var lod = 1; lod < LodLevels; lod++)
            {
                    var sizeLod = Size >> lod;
                for (int y = 0, i = 0; y < sizeLod; y++)
                {
                    for (var x = 0; x < sizeLod; x++)
                    {
                        var yLod0 = y << lod;
                        var xLod0 = x << lod;

                        if (x == sizeLod - 1)
                        {
                            xLod0 = Size - 1;
                        }
                        if (y == sizeLod - 1)
                        {
                            yLod0 = Size - 1;
                        }

                        HeightData[lod][i] = HeightData[0][yLod0 * Size + xLod0];

                        i++;
                    }
                }
            }
        }
    }
}