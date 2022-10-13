using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samurai_Application.Data
{
    public class SamuraiContextNoTracking : SamuraiContext
    {
        public SamuraiContextNoTracking()
        {
            base.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;    
        }   
    }
}
