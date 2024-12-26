using System;
using Mapbox.VectorTile;

namespace Source.Libraries.KBLib2.Mesh
{
    public class Kb2VectorTileMesh : IKb2BakeableMesh
    {
        public static Kb2VectorTileMesh FromMapboxTile(VectorTile pTile)
        {
            throw new NotImplementedException();
        }
        
        public UnityEngine.Mesh Bake()
        {
            throw new NotImplementedException();
        }
        public byte[] Serialize()
        {
            throw new NotImplementedException();
        }
        public void Deserialize(byte[] pSerialized)
        {
            throw new NotImplementedException();
        }
        public string GetWatermark()
        {
            throw new NotImplementedException();
        }
    }
}