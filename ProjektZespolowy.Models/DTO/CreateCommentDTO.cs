using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class CreateCommentDTO
    {
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
    }
}
