using JuggleWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JuggleWebApp.ViewModels
{
    public class DetailPostViewModel
    {
        public Post Post { get; set; }
        public Comment? Comment { get; set; }
        public IEnumerable<Comment>? CommentsList { get; set; }
    }
}
