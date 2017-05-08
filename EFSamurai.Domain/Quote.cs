using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EFSamurai.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public string QuoteText { get; set; }
        public bool IsReallyDeep { get; set; }
        public virtual Samurai Samurai { get; set; }
    }
}
