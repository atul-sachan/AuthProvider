using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.DataAccess
{
    public class Entity : IEntity
    {
        public string Id { get; set; }
    }

    public interface IEntity
    {
        string Id { get; set; }
    }
}
