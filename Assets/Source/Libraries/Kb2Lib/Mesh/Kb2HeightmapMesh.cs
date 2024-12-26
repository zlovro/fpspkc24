using System;
using System.IO;

namespace Source.Libraries.KBLib2.Mesh
{
    public class Kb2HeightmapMesh : IKb2BakeableMesh
    {
        public Kb2HeightmapChunkMesh[] CachedChunkMap;

        public ushort ChunkCountX, ChunkCountZ, VerticesPerChunk;
        public uint[] OffsetMap;
        public int    LodLevels;

        private BinaryReader mUncompressedReader;

        public UnityEngine.Mesh Bake()
        {
            throw new Exception("Cannot bake Kb2HeightmapMesh");
        }

        public void Deserialize(string pFilePath)
        {
            mUncompressedReader = new BinaryReader(File.OpenRead(pFilePath));

            ChunkCountX = mUncompressedReader.ReadUInt16();
            ChunkCountZ = mUncompressedReader.ReadUInt16();

            CachedChunkMap = new Kb2HeightmapChunkMesh[ChunkCountX * ChunkCountZ];

            VerticesPerChunk = mUncompressedReader.ReadUInt16();
            LodLevels        = Kb2HeightmapChunkMesh.BitSize(VerticesPerChunk) - 1;

            OffsetMap = new uint[ChunkCountX * ChunkCountZ];
            for (var i = 0; i < OffsetMap.Length; i++)
            {
                OffsetMap[i] = mUncompressedReader.ReadUInt32();
            }
        }

        private int GetChunkIndex(int pX, int pZ) => pZ * ChunkCountX + pX;

        private int GetChunkByteSize(int pX, int pZ)
        {
            var idx = GetChunkIndex(pX, pZ);
            if (idx == ChunkCountX * ChunkCountZ - 1)
            {
                return (int)(mUncompressedReader.BaseStream.Length - OffsetMap[idx]);
            }

            return (int)(OffsetMap[idx + 1] - OffsetMap[idx]);
        }

        public Kb2HeightmapChunkMesh LoadChunk(int pX, int pZ)
        {
            var chunk = new Kb2HeightmapChunkMesh(VerticesPerChunk);

            mUncompressedReader.BaseStream.Position = OffsetMap[GetChunkIndex(pX, pZ)];
            chunk.Deserialize(mUncompressedReader.ReadBytes(GetChunkByteSize(pX, pZ)));

            CachedChunkMap[GetChunkIndex(pX, pZ)] = chunk;

            return chunk;
        }

        public UnityEngine.Mesh ProvideChunk(int pX, int pZ, int pLod, float pDisplaySize)
        {
            Kb2HeightmapChunkMesh chunk;
            if ((chunk = CachedChunkMap[GetChunkIndex(pX, pZ)]) != null)
            {
                return chunk.CachedLodMeshes[pLod] != null ? chunk.CachedLodMeshes[pLod] : chunk.Bake(pLod);
            }

            chunk             = LoadChunk(pX, pZ);
            chunk.DisplaySize = pDisplaySize;
            return chunk.Bake(pLod);
        }
    }
}