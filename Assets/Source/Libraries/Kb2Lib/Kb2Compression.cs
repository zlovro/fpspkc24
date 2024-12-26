using System.IO;
using System.IO.Compression;

namespace Source.Libraries.KBLib2
{
    public class Kb2Compression
    {
        public static byte[] GzipCompress(byte[] pUncompressed) => GzipCompress(new MemoryStream(pUncompressed));
        public static byte[] GzipDecompress(byte[] pCompressed) => GzipDecompress(new MemoryStream(pCompressed));
        
        public static byte[] GzipCompress(MemoryStream pUncompressed)
        {
            using var outStream = new MemoryStream();
            using var gzStream  = new GZipStream(outStream, CompressionLevel.Fastest);
            gzStream.Write(pUncompressed.ToArray());
            gzStream.Close();
            return outStream.ToArray();
        }

        public static byte[] GzipDecompress(MemoryStream pCompressed)
        {
            using var outStream = new MemoryStream();
            using var gzStream  = new GZipStream(pCompressed, CompressionMode.Decompress);
            gzStream.CopyTo(outStream);
            gzStream.Close();
            return outStream.ToArray();
        }
    }
}