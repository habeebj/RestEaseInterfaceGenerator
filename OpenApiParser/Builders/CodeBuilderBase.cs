using Newtonsoft.Json.Linq;
using OpenApiParser.Models;

namespace OpenApiParser.Builders
{
    public abstract class CodeBuilderBase<TModel> where  TModel: class 
    {
        // protected TModel Models;
        protected readonly ICodeWriter CodeWriter;

        protected CodeBuilderBase(ICodeWriter codeWriter)
        {
            CodeWriter = codeWriter;
        }
        protected abstract TModel Build(JObject jObject);
    }
}