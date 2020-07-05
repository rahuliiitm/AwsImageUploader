using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageUploader.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using ImageUpload.Services;
using ImageUpload.Model;
using Amazon;

namespace ImageUpload.Services
{
    public class DynamoDBStorageService : IImageUploaderStorageService
    {
        private readonly IMapper _mapper;

        public DynamoDBStorageService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> AddAsync(ImageUploadModel model)
        {
            var dbModel = _mapper.Map<DBModel>(model);

            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreationDateTime = DateTime.UtcNow;
            dbModel.Status = ImageUploaderStatus.Pending;

            using (var client = new AmazonDynamoDBClient(RegionEndpoint.APSouth1))
            {
                var table = await client.DescribeTableAsync("ImageMetaData");

                using (var context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(dbModel);
                }
            }

            return dbModel.Id;
        }

        public async Task<bool> CheckHealthAsync()
        {
            Console.WriteLine("Health checking...");
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.APSouth1))
            {
                var tableData = await client.DescribeTableAsync("ImageMetaData");
                return string.Compare(tableData.Table.TableStatus, "active", true) == 0;
            }
        }

        public async Task ConfirmAsync(ConfirmModel model)
        {
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.APSouth1))
            {
                using (var context = new DynamoDBContext(client))
                {
                    var record = await context.LoadAsync<DBModel>(model.Id);
                    if (record == null) throw new KeyNotFoundException($"A record with ID={model.Id} was not found.");
                    if (model.Status == ImageUploaderStatus.Active)
                    {
                        record.FilePath = model.FilePath;
                        record.Status = ImageUploaderStatus.Active;
                        await context.SaveAsync(record);
                    }
                    else
                    {
                        await context.DeleteAsync(record);
                    }
                }
            }
        }

        public async Task<List<ImageUploadModel>> GetAllAsync()
        {
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.APSouth1))
            {
                using (var context = new DynamoDBContext(client))
                {
                    var scanResult =
                        await context.ScanAsync<DBModel>(new List<ScanCondition>()).GetNextSetAsync();
                    return scanResult.Select(item => _mapper.Map<ImageUploadModel>(item)).ToList();
                }
            }
        }

        public async Task<ImageUploadModel> GetByIdAsync(string id)
        {
            using (var client = new AmazonDynamoDBClient(RegionEndpoint.APSouth1))
            {
                using (var context = new DynamoDBContext(client))
                {
                    var dbModel = await context.LoadAsync<DBModel>(id);
                    if (dbModel != null) return _mapper.Map<ImageUploadModel>(dbModel);
                }
            }

            throw new KeyNotFoundException();
        }
    }
}
