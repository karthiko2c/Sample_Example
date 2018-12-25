using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace SubQuip.Entity.Models
{
    public class Comment
    {

        public string Id { get; set; }

		public ObjectId CreatedById { get; set; }

        public string CreatedBy { get; }

        public string CommentText { get; set; }

        public string CreatedOn { get; private set; }
    }
}
