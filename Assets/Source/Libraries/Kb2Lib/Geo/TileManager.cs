using System;
using System.Collections;
using Mapbox.VectorTile.Geometry;
using Source.Libraries.KBLib2;
using Source.Libraries.KBLib2.Mesh;
using UnityEngine;

namespace Source.Libraries.KbLib2.Geo
{
    public class TileManager : Kb2Behaviour
    {
        public Vector3Int topLeftTile, bottomRightTile;

        public void ClearTiles()
        {
            foreach (var child in GetChildren())
            {
                DestroyImmediate(child);
            }
        }

        public IEnumerator DownloadTiles()
        {
            // var zoom = topLeftTile.z;
            //
            // var elevationMap = new float[bottomRightTile.x - topLeftTile.x + 1, bottomRightTile.y - topLeftTile.y + 1, MapTilerService.CTileSizeVertex, MapTilerService.CTileSizeVertex];
            //
            // for (int x = topLeftTile.x, xnm = 0; x <= bottomRightTile.x; x++, xnm++)
            // {
            //     for (int y = topLeftTile.y, ynm = 0; y <= bottomRightTile.y; y++, ynm++)
            //     {
            //         var staticTile = MapTilerService.ProvideStaticTile(x, y, zoom);
            //         var elevTile = MapTilerService.ProvideElevationTile(x, y, zoom);
            //         for (var j = 0; j < elevTile.GetLength(0); j++)
            //         {
            //             for (var k = 0; k < elevTile.GetLength(1); k++)
            //             {
            //                 elevationMap[xnm, ynm, j, k] = elevTile[j, k];
            //             }
            //         }
            //
            //         var mesh = new Kb2HeightmapMesh(MapTilerService.CTileSizeVertex, MapTilerService.CTileSizeVertex, true);
            //         for (var meshY = 0; meshY < MapTilerService.CTileSizeVertex; meshY++)
            //         {
            //             for (var meshX = 0; meshX < MapTilerService.CTileSizeVertex; meshX++)
            //             {
            //                 // var elev = i * 2;
            //                 // var elev = Mathf.Sin((meshX / (float)MapTilerService.CTileSizeDisplay) * Mathf.PI * 5) * 25;
            //                 var elev = elevTile[meshX, meshY];
            //                 mesh.SetVertex(meshX, meshY, new Kb2HeightmapMesh.Vertex(new Vector3(meshX, elev, meshY), staticTile[meshX, meshY]));
            //             }
            //         }
            //
            //         IKb2BakeableMesh.InstantiateAndBakeMesh(mesh, new Vector3(xnm * MapTilerService.CTileSizeDisplay, 0, ynm * MapTilerService.CTileSizeDisplay));
            //     }
            // }
            //
            // // for (int x = topLeftTile.x, xnm = 0; false; x++, xnm++)
            // for (int x = topLeftTile.x, xnm = 0; x <= topLeftTile.x; x++, xnm++)
            // {
            //     for (int y = topLeftTile.y, ynm = 0; y <= bottomRightTile.y; y++, ynm++)
            //     {
            //         var vectorTile = MapTilerService.ProvideVectorTile(x, y, zoom);
            //         var buildingsLayer = vectorTile.GetLayer("building");
            //
            //         if (buildingsLayer != null)
            //         {
            //             for (var i = 0; i < buildingsLayer.FeatureCount(); i++)
            //             {
            //                 var feature = buildingsLayer.GetFeature(i);
            //                 switch (feature.GeometryType)
            //                 {
            //                     case GeomType.POLYGON:
            //                     {
            //                         foreach (var ring in feature.Geometry<float>())
            //                         {
            //                             for (var j = 1; j < ring.Count; j++)
            //                             {
            //                                 var p1 = ring[j - 1];
            //
            //                                 var x1nm = (int)Mathf.Clamp(p1.X / buildingsLayer.Extent * MapTilerService.CTileSizeDisplay, 0, MapTilerService.CTileSizeDisplay);
            //                                 var z1nm = (int)Mathf.Clamp(p1.Y / buildingsLayer.Extent * MapTilerService.CTileSizeDisplay, 0, MapTilerService.CTileSizeDisplay);
            //
            //                                 var x1 = xnm * MapTilerService.CTileSizeDisplay + x1nm;
            //                                 var z1 = ynm * MapTilerService.CTileSizeDisplay + z1nm;
            //
            //                                 var pos1 = new Vector3(x1, elevationMap[xnm, ynm, x1nm, z1nm], z1);
            //
            //                                 var p2 = ring[j - 0];
            //
            //                                 var x2nm = (int)Mathf.Clamp(p2.X / buildingsLayer.Extent * MapTilerService.CTileSizeDisplay, 0, MapTilerService.CTileSizeDisplay);
            //                                 var z2nm = (int)Mathf.Clamp(p2.Y / buildingsLayer.Extent * MapTilerService.CTileSizeDisplay, 0, MapTilerService.CTileSizeDisplay);
            //
            //                                 var x2 = xnm * MapTilerService.CTileSizeDisplay + x2nm;
            //                                 var z2 = ynm * MapTilerService.CTileSizeDisplay + z2nm;
            //
            //                                 var pos2 = new Vector3(x2, elevationMap[xnm, ynm, x2nm, z2nm], z2);
            //
            //                                 Debug.DrawLine(pos1, pos2, Color.black, Mathf.Infinity);
            //                             }
            //                         }
            //
            //                         break;
            //                     }
            //
            //                     default:
            //                     {
            //                         break;
            //                     }
            //                 }
            //             }
            //         }
            //     }
            // }
            //
         
            yield return null;
        }

        private void Start()
        {
            ClearTiles();
            StartCoroutine(DownloadTiles());
        }
    }
}