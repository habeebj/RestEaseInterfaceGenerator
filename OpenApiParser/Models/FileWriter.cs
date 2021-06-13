using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenApiParser.Models
{
    public class FileWriter : IOutputWriter
    {
        private readonly string _path;
        
        public FileWriter(string path, string filename)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            _path = Path.Combine(path, filename);
        }

        private void PrintInfo()
        {
            Console.WriteLine("Done.");
            Console.WriteLine($"File Created: {Path.GetFullPath(_path)}");
        }
        
        public void Save(string content)
        {
            File.WriteAllText(_path, content);
            PrintInfo();
        }

        public async Task SaveAsync(string content)
        {
            await File.WriteAllTextAsync(_path, content);
            PrintInfo();
        }
    }
}