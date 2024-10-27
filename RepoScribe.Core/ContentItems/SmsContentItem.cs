using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoScribe.Core.ContentItems
{
    internal class SmsContentItem : ContentItem
    {
        public SmsContentItem() { }

        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public override void Ingest()
        {
            // Implement SMS ingestion logic here
            // For example, parse message, extract metadata, etc.
        }
    }
}
