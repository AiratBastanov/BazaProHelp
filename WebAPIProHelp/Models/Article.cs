using System;
using System.Collections.Generic;

namespace WebAPIProHelp.Models
{
    public partial class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public byte[]? Image { get; set; }
    }
}
