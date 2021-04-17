namespace RaytracerNet
{
    using System.Numerics;

    public struct Mesh
    {
        public Vector3[] Vertices;
        public Vector3[] Normals;
        public Vector3[] Textures;
        public Face[] Faces;

        public Trig[] Trigs;

        public Mesh(Vector3[] vertices, Vector3[] normals, Vector3[] textures, Face[] faces) : this()
        {
            Vertices = vertices;
            Normals = normals;
            Textures = textures;
            Faces = faces;
            
            SetupTrigs();
        }

        public void SetupTrigs()
        {
            Trigs = new Trig[Faces.Length];
            for (var i = 0; i < Faces.Length; i++)
            {
                var face = Faces[i];
                
                var aIndex = face[0].VertexIndex;
                var bIndex = face[1].VertexIndex;
                var cIndex = face[2].VertexIndex;
                
                var anIndex = face[0].NormalIndex;
                var bnIndex = face[1].NormalIndex;
                var cnIndex = face[2].NormalIndex;

                Trigs[i] = new Trig(Vertices[aIndex], Vertices[bIndex], Vertices[cIndex],
                Normals[anIndex], Normals[bnIndex], Normals[cnIndex]);
            }
        }
        public void Transform(Matrix4x4 translateM, Matrix4x4 rotateM, Matrix4x4 scaleM)
        {
            TransformPoints(Vertices, translateM * rotateM * scaleM);
            TransformPoints(Normals,  rotateM);
            SetupTrigs();
        }

        private void TransformPoints(Vector3[] array, Matrix4x4 matrix)
        {
            for (var i = 0; i < array.Length; i++)
            {
                var v3 = array[i];
                var v4 = new Vector4(v3, 1);
            
                var vout = matrix.MultiplyBy(v4);
                array[i] = new Vector3(vout.X, vout.Y, vout.Z);
            }
        }
    }

    public class Face
    {
        public PointData[] Values = new PointData[3];

        public PointData this[int index]
        {
            get => Values[index];
            set => Values[index] = value;
        }
    }

    public struct PointData
    {
        public int VertexIndex;
        public int TextureIndex;
        public int NormalIndex;
    }
}