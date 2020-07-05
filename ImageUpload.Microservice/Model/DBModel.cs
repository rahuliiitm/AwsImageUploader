using Amazon.DynamoDBv2.DataModel;
using ImageUploader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageUpload.Model
{
    [DynamoDBTable("ImageMetaData")]
    public class DBModel
    {
        [DynamoDBHashKey] public string Id { get; set; }

        [DynamoDBProperty] public string Title { get; set; }

        [DynamoDBProperty] public string Description { get; set; }

        [DynamoDBProperty] public double Price { get; set; }

        [DynamoDBProperty] public DateTime CreationDateTime { get; set; }

        [DynamoDBProperty] public ImageUploaderStatus Status { get; set; }

        [DynamoDBProperty] public string FilePath { get; set; }

        [DynamoDBProperty] public string UserName { get; set; }
    }
}
