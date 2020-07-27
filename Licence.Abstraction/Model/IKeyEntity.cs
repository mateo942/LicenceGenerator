using System;
namespace Licence.Abstraction.Model
{
    public interface IKeyEntity
    {
        Guid Id { get; set; }
        string PrivateKey { get; set; }
        string PublicKey { get; set; }
    }
}
