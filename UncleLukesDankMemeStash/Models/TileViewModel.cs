using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace UncleLukesDankMemeStash.Models
{
    public class TileViewModel
    {
        public IDisplayable Displayable { get; set; }
        public bool CanEdit { get; set; }
    }
}
