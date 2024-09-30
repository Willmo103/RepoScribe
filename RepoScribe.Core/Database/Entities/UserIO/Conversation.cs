using System.ComponentModel.DataAnnotations;

namespace RepoScribe.Core.Database.Entities.UserIO
{
    internal class Conversation
    {
        [Key]
        public Guid ConversationId { get; set; } = Guid.NewGuid();
        public List<Query> Queries { get; set; } = new List<Query>();
        public string ConversationName { get; set; } = "";
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Provider { get; set; } = "";
        public string ConversationUrl { get; set; } = "";
        public List<string> ConversationTopics { get; set; } = new List<string>();
    }
}
