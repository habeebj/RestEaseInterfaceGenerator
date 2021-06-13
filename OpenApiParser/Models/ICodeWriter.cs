using System.Collections.Generic;
using System.Text;

namespace OpenApiParser.Models
{
    public interface ICodeWriter
    {
        string GetPropertyType(PropertySchema propertySchema);
        StringBuilder WriteInterface(IEnumerable<InterfaceActionModel> actionModels);
        StringBuilder WriteClassModel(IEnumerable<ClassModel> classModels);
    }
}