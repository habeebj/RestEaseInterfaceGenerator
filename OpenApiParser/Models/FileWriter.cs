using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenApiParser.Models
{
    public class FileWriter : IOutputWriter
    {
        private readonly string _path;
        
        public FileWriter(string path)
        {
            _path = path;
        }
        public void Save(string content)
        {
            File.WriteAllText(_path, content);
        }

        public async Task SaveAsync(string content)
        {
            await File.WriteAllTextAsync(_path, content);
        }
    }
}