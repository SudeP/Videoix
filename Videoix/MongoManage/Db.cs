using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDK.DB.MONGODB;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using MongoDB.Driver.Core.Authentication.Sspi;
using MongoDB.Driver.Core.Authentication.Vendored;
using MongoDB.Driver.Core.Bindings;
using MongoDB.Driver.Core.Clusters;
using MongoDB.Driver.Core.Clusters.ServerSelectors;
using MongoDB.Driver.Core.Compression;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Core.ConnectionPools;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Core.Events.Diagnostics;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Core.Operations;
using MongoDB.Driver.Core.Operations.ElementNameValidators;
using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.Core.WireProtocol;
using MongoDB.Driver.Core.WireProtocol.Messages;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders.BinaryEncoders;
using MongoDB.Driver.Core.WireProtocol.Messages.Encoders.JsonEncoders;
using MongoDB.Driver.Encryption;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.GeoJsonObjectModel.Serializers;
using MongoDB.Driver.Linq;
using MongoDB.Libmongocrypt;

namespace Videoix.MongoManage
{
    public static class Db
    {
        public static void Init()
        {
            MC = new MongoClient("mongodb://-g304gk34g-304kg:6tf7u43jmnvf2g-*s0923@213.238.178.34:27017/?authSource=admin&readPreference=primary&appname=MongoDB%20Compass&ssl=false");
            MD = MC.GetDatabase("vidoix") as MongoDatabaseBase;
            MD.GetCollection<Users>(nameof(Users));
        }
        public static MongoClient MC { get; set; }
        public static MongoDatabaseBase MD { get; set; }
        public static MongoCollectionBase<Users> Users { get; set; }
    }
    public class Users : ObjectUniqe<Users>
    {

    }
}
