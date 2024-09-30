using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepoScribe.Core.Database.Entities.UserIO
{
    abstract class Query
    {
        [Key]
        Guid QueryId { get; set; } = Guid.NewGuid();
        Guid ConversationId { get; set; }
        String Source { get; set; } = "";
        String QueryString { get; set; } = "";
        String Response { get; set; } = "";
        DateTime TimeStamp { get; set; } = DateTime.Now;
        Boolean IsResponse { get; set; } = false;
        Boolean IsQuery { get; set; } = false;
        Boolean IsUser { get; set; } = false;
        Boolean IsBot { get; set; } = false;
        Boolean HasResponse { get; set; } = false;
        List<Query> Responses { get; set; } = new List<Query>();
        Boolean IsBestResponse { get; set; } = false;
        String Provider { get; set; } = "";
        
        [ForeignKey("ConversationId")];
        Conversation Conversation { get; set; }
    }
}
