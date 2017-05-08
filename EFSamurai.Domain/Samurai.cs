using System;
using System.ComponentModel.DataAnnotations;

namespace EFSamurai.Domain
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public bool HasKatana { get; set; }
    }
}
