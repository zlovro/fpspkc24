using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using ImageMagick;
using Mapbox.VectorTile;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Source.Libraries.KbLib2.Geo
{
    public class MapTilerService
    {
        public const int TILE_SIZE_VERTEX  = 512;
        public const int TILE_SIZE_DISPLAY = TILE_SIZE_VERTEX - 1;

        public static VectorTile ProvideVectorTile(int pX, int pY, int pZoom)
        {
            var path = $"tileCache/{pX}.{pY}.{pZoom}.pbf";
            if (File.Exists(path))
            {
                return new VectorTile(File.ReadAllBytes(path));
            }

            using var client = new HttpClient();
            var       task   = client.GetByteArrayAsync($"https://api.maptiler.com/tiles/v3/{pZoom}/{pX}/{pY}.pbf?key=NmzQvzb9NURd2TpyG1Mh");
            task.Wait();

            if (task.Result.Length > 80)
            {
                File.WriteAllBytes(path, task.Result);
            }
            return new VectorTile(task.Result);
        }

        public static float[,] ProvideElevationTile(int pX, int pY, int pZ)
        {
            using var client = new HttpClient();

            var exp = (int)Mathf.Pow(2, pZ - 12);
            var tx  = pX / exp;
            var ty  = pY / exp;

            var url  = $"https://api.maptiler.com/tiles/terrain-rgb/12/{tx}/{ty}.png?key=NmzQvzb9NURd2TpyG1Mh";
            var task = client.GetByteArrayAsync(url);
            task.Wait();

            var img = new Bitmap(new MemoryStream(task.Result));

            // var img = new MagickImage(webpStream);
            // webpStream.Close();
            //
            // var newSz = img.Width / exp;
            // img.Crop(new MagickGeometry(pX % exp * newSz, pY % exp * newSz, newSz, newSz));
            // img.Resize(CTileSizeVertex, CTileSizeVertex);
            //
            // img.Write($"Tiles/{pZ}-{pX}-{pY}-og.png");

            var ret = new float[TILE_SIZE_VERTEX, TILE_SIZE_VERTEX];
            for (var x = 0; x < TILE_SIZE_VERTEX; x++)
            {
                for (var y = 0; y < TILE_SIZE_VERTEX; y++)
                {
                    var color = img.GetPixel(x, y);
                    ret[x, y] = -10000 + (color.R * 256 * 256 + color.G * 256 + color.B) * 0.1F;
                }
            }

            // var retList = ret.ToList();
            //
            // var elevMax = retList.Max();
            // var elevMin = retList.Min();
            // var elevDelta = elevMax - elevMin;
            //
            // var img2Data = new byte[3 * CTileSizeVertex * CTileSizeVertex];
            // var k = 0;
            // foreach (var elev in ret)
            // {
            //     var y = (byte)((elev - elevMin) / elevDelta * byte.MaxValue);
            //     img2Data[k++] = y;
            //     img2Data[k++] = y;
            //     img2Data[k++] = y;
            // }
            //
            // var img2 = new MagickImage(MagickColors.Black, CTileSizeVertex, CTileSizeVertex);
            // img2.ImportPixels(img2Data.ToArray(), new PixelImportSettings(CTileSizeVertex, CTileSizeVertex, StorageType.Char, PixelMapping.RGB));
            // img2.Write($"Tiles/{pZ}-{pX}-{pY}-elev.png");

            return ret;
        }

        public static Color[,] ProvideStaticTile(int pX, int pY, int pZoom)
        {
            using var client = new HttpClient();
            var       task   = client.GetByteArrayAsync($"https://api.maptiler.com/maps/streets-v2/{pZoom}/{pX}/{pY}.png?key=NmzQvzb9NURd2TpyG1Mh");
            task.Wait();

            var webpData   = task.Result;
            var webpStream = new MemoryStream(webpData);

            var bitmap = new Bitmap(webpStream);

            var ret    = new Color[TILE_SIZE_VERTEX, TILE_SIZE_VERTEX];
            var factor = bitmap.Width / TILE_SIZE_VERTEX;
            for (int x = 0, xnm = 0; x < bitmap.Width; x += factor, xnm++)
            {
                for (int y = 0, ynm = 0; y < bitmap.Height; y += factor, ynm++)
                {
                    int g, b;
                    var r = g = b = 0;

                    for (var i = 0; i < factor; i++)
                    {
                        var px = bitmap.GetPixel(x + i, y);

                        r += px.R;
                        g += px.G;
                        b += px.B;
                    }

                    var color = new Color(r / 255.0F, g / 255.0F, b / 255.0F);
                    ret[xnm, ynm] = color;
                }
            }

            webpStream.Close();

            return ret;
        }
    }
}