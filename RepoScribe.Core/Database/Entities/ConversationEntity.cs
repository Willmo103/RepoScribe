using System.ComponentModel.DataAnnotations;

namespace RepoScribe.Core.Database.Entities
{
    internal class ConversationEntity
    {
        [Key]
        public Guid ConversationId { get; set; } = Guid.NewGuid();
        public List<QueryEntity> Queries { get; set; } = new List<QueryEntity>();
        public string ConversationName { get; set; } = "";
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string Provider { get; set; } = "";
        public string ConversationUrl { get; set; } = "";
        public List<string> ConversationTopics { get; set; } = new List<string>();
    }
}
