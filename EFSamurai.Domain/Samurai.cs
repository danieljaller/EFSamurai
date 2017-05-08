using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFSamurai.Domain
{
    public class Samurai
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Age { get; set; }
        public bool HasKatana { get; set; }
        public Haircut Haircut { get; set; }
        public virtual SecretIdentity SecretIdentity { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
    }

    public enum Haircut
    {
        Chonmage,
        Oicho,
        Western
    }
}
