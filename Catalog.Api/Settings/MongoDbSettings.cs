using System;

namespace Catalog.Api.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongodb://{User}:{Password}@{Host}:{Port}";
            }
        }
    }
}
// docker commands to spin up a mongo db container without and with authentication
// docker run -d --rm --name mongo -d -p 27017:27017 -v mongodbdata:/data/db mongo
//docker run -d --rm --name mongo -d -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=password --network=net5tutorial mongo