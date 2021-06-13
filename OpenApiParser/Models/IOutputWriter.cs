using System.Threading.Tasks;

namespace OpenApiParser.Models
{
    public interface IOutputWriter
    {
        void Save(string content);
        Task SaveAsync(string content);
    }
}