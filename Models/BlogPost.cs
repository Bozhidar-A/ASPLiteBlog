using ASPLiteBlog.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPLiteBlog.Models
{
    public class BlogPost
    {
        public int ID { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string previewPicLocation { get; set; }
        public DateTime timeCreated { get; set; } = DateTime.UtcNow;
        public DateTime? timeLastEdited { get; set; }
        public DateTime? timePublished { get; set; }
        public DateTime? timeLastEditedPublished { get; set; }
        public bool draft { get; set; }

        public string userID { get; set; }
        [ForeignKey("userID")]
        public virtual ApplicationUser user { get; set; }
    }
}