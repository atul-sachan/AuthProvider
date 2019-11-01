using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthProvider.Authentication.DataAccess
{
    public class EntityClassMap
    {
        public static void Instance()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(Entity)) == false)
            {
                BsonClassMap.RegisterClassMap<Entity>(map =>
                {
                    //BsonSerializer.RegisterIdGenerator(typeof(Guid), new GuidGenerator());
                    map.AutoMap();
                    map.SetIgnoreExtraElements(true);
                    //map.MapIdProperty(x => x.Id)
                    //    .SetIdGenerator(new GuidGenerator())
                    //    .SetIgnoreIfDefault(true);
                    map.MapIdMember(c => c.Id)
                        .SetSerializer(new StringSerializer(BsonType.ObjectId))
                        .SetIdGenerator(StringObjectIdGenerator.Instance);


                });
            }
        }
    }
}
