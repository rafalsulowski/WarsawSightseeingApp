﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektZespolowy.Models.DTO
{
    public class DeleteCommentDTO
    {
        public int PostId { get; set; }
        public int CommentId { get; set; }
    }
}
