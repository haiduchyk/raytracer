namespace RaytracerNet
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Numerics;
    using System.Text;
    using System.Text.RegularExpressions;
    using Converter;

    public class ObjReader
    {
        private List<string> lines;

        private IFileWorker fileWorker;

        private readonly Regex whitespace = new Regex(@"\s+");

        public ObjReader(IFileWorker fileWorker)
        {
            this.fileWorker = fileWorker;
        }

        public Mesh Read(string path)
        {
            var file = fileWorker.Read(path);
            return Decode(file);
        }

        private Mesh Decode(byte[] file)
        {
            // to parse float with dots
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var data = Encoding.ASCII.GetString(file, 0, file.Length);
            SetupLines(data);

            return new Mesh
            (
                GetLinesBy("v").Select(GetVector).ToArray(),
                GetLinesBy("vn").Select(GetVector).ToArray(),
                // GetLinesBy("vt").Select(GetVector).ToArray(),
                null,
                GetLinesBy("f").Select(ParseFace).ToArray()
            );
        }

        private void SetupLines(string data)
        {
            lines = data.Split('\n').ToList();
        }

        private IEnumerable<string[]> GetLinesBy(string label)
        {
            return lines.Select(l => whitespace.Split(l.Trim())).Where(a => a[0] == label);
        }


        private Vector3 GetVector(string[] str)
        {
            if (str.Length != 4) throw new ArgumentException();
            var x = float.Parse(str[1]);
            var y = float.Parse(str[2]);
            var z = float.Parse(str[3]);

            return new Vector3(x, y, z);
        }

        private Face ParseFace(string[] str)
        {
            var face = new Face();

            for (var i = 1; i < str.Length; i++)
            {
                var data = str[i].Split('/');

                var pointData = new PointData
                {
                    VertexIndex = ParseIndex(data[0]),
                    TextureIndex = ParseIndex(data[1]),
                    NormalIndex = ParseIndex(data[2])
                };

                face[i - 1] = pointData;
            }

            return face;
        }


        private int ParseIndex(string str)
        {
            if (string.IsNullOrEmpty(str)) return -1;
            return int.Parse(str) - 1;
        }
    }
}