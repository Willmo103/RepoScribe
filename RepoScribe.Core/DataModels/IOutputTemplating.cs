using System.Text;

namespace RepoScribe.Core.DataModels.Markdown
{
    public interface IOutputTemplating
    {
        StringBuilder _templateBuilder { get; set; }
       
    }
}
