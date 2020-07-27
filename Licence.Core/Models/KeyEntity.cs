using System;
using Licence.Abstraction.Model;

namespace Licence.Core.Models
{
    public class KeyEntity : IKeyEntity
    {
        public Guid Id { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
    }
}
