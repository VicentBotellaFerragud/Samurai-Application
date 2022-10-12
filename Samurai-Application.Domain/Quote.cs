using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai_Application.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Explanation { get; set; } 
        public Samurai? Samurai { get; set; }    
        public int SamuraiId { get; set; }    
    }
}
