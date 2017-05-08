using System;
using System.Collections.Generic;
using System.Text;

namespace EFSamurai.Domain
{
    public class BattleLog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BattleEvent> BattleEvents { get; set; }
    }
}
