using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepoScribe.Core.Database.Entities
{
    abstract class QueryEntity
    {
        [Key]
        Guid QueryId { get; set; } = Guid.NewGuid();
        Guid ConversationId { get; set; }
        string Source { get; set; } = "";
        string QueryString { get; set; } = "";
        string Response { get; set; } = "";
        DateTime TimeStamp { get; set; } = DateTime.Now;
        bool IsResponse { get; set; } = false;
        bool IsQuery { get; set; } = false;
        bool IsUser { get; set; } = false;
        bool IsBot { get; set; } = false;
        bool HasResponse { get; set; } = false;
        List<QueryEntity> Responses { get; set; } = new List<QueryEntity>();
        bool IsBestResponse { get; set; } = false;
        string Provider { get; set; } = "";
        ConversationEntity Conversation { get; set; }
    }
}
