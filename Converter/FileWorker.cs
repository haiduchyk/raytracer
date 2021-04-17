namespace Converter
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public interface IFileWorker
    {
        void Write(byte[] bytes, string path, string ppm);
        byte[] Read(string path);
    }
    
    public class FileWorker : IFileWorker
    {
        public void Write(byte[] bytes, string path, string extension)
        {
            var rgx = new Regex(@"\.[^.]*$");
            path = rgx.Replace(path, $"_result_{DateTime.Now.Hour}_{DateTime.Now.Minute}-{DateTime.Now.Second}.{extension}");
            
            path = path.Replace('/', Path.DirectorySeparatorChar);
            File.WriteAllBytes(path, bytes);
        }
        
        public byte[] Read(string path)
        {
            var file = File.ReadAllBytes(path);
            return file;
        }
    }
}