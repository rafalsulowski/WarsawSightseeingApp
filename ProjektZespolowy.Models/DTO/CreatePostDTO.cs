using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class CreatePostDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public PostType Type { get; set; }
        public int VotesFor { get; set; }
        public int VotesAgainst { get; set; }
    }
}
