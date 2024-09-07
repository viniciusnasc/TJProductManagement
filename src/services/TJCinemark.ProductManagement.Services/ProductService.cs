using Amazon.Runtime;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using ServiceStack;
using ServiceStack.Redis;
using TJ.ProductManagement.Domain.Constants;
using TJ.ProductManagement.Domain.Entities;
using TJ.ProductManagement.Domain.Interfaces.Repositories;
using TJ.ProductManagement.Domain.Interfaces.Services;
using TJ.ProductManagement.Domain.Models;

namespace TJ.ProductManagement.Services
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly AmazonSQSClient _amazonSQSClient;
        private readonly RedisSettings _redisSettings;
        private readonly QueueUrl _queueUrl;

        public ProductService(IProductRepository productRepository, INotificator notificator
                            , IOptions<AmazonSQSSettings> awsSettings, IOptions<RedisSettings> redisSettings
                            , IOptions<QueueUrl> queueUrl) : base(notificator)
        {
            _productRepository = productRepository;
            _amazonSQSClient = new AmazonSQSClient(new BasicAWSCredentials(awsSettings.Value.AccessKey, awsSettings.Value.SecretKey), new AmazonSQSConfig
            {
                ServiceURL = awsSettings.Value.ServiceURL,
                UseHttp = true
            });
            _redisSettings = redisSettings.Value;
            _queueUrl = queueUrl.Value;
        }

        public async Task<IEnumerable<ProductViewModel>> GetAll()
        {
            using (var redisClient = new RedisClient(_redisSettings.Host))
            {
                var countDB = await _productRepository.CountItems();
                var countRedis = redisClient.DbSize;

                if (countDB == countRedis)
                {
                    var keys = redisClient.SearchKeys("product:*");
                    var results = redisClient.GetValues<Product>(keys);

                    return results.Select(x => new ProductViewModel
                    {
                        Id = x.Id.ToString(),
                        Price = x.Price,
                        Description = x.Description,
                        Name = x.Name,
                    });
                }

                var entities = _productRepository.GetAll();

                redisClient.RemoveAll(redisClient.SearchKeys("product:*"));
                var keyValuePairs = entities.ToDictionary(x => $"product:{x.Id}", x => x);
                redisClient.SetAll<Product>(keyValuePairs);

                return entities.Select(x => new ProductViewModel
                {
                    Id = x.Id.ToString(),
                    Price = x.Price,
                    Description = x.Description,
                    Name = x.Name,
                });
            }
        }

        public async Task<ProductViewModel> GetById(string id)
        {
            using (var redisClient = new RedisClient(_redisSettings.Host))
            {
                var entity = redisClient.Get<Product>(id);

                if (entity is not null)
                    return new ProductViewModel
                    {
                        Id = entity.Id.ToString(),
                        Description = entity.Description,
                        Name = entity.Name,
                        Price = entity.Price,
                    };

                entity = await _productRepository.GetById(id.ToString());
                if (entity is null)
                {
                    Notificate("Product not found!");
                    return null;
                }

                redisClient.Set<Product>($"product:{id.ToString()}", entity, new TimeSpan(0, 10, 0));

                return new ProductViewModel
                {
                    Id = entity.Id.ToString(),
                    Description = entity.Description,
                    Name = entity.Name,
                    Price = entity.Price,
                };
            }
        }

        public async Task Add(ProductInsertModel model)
        {
            Product entity = new Product(model.Name, model.Description, model.Price);
            await _productRepository.Add(entity);

            var request = new Amazon.SQS.Model.SendMessageRequest
            {
                QueueUrl = _queueUrl.Product,
                MessageBody = $"New product:id={entity.Id.ToString()}name={model.Name}"
            };
            await _amazonSQSClient.SendMessageAsync(request);

            using (var redisClient = new RedisClient(_redisSettings.Host))
            {
                redisClient.Set<Product>($"product:{entity.Id.ToString()}", entity, new TimeSpan(0, 10, 0));
            }
        }

        public async Task Update(string id, ProductInsertModel model)
        {
            using (var redisClient = new RedisClient(_redisSettings.Host))
            {
                var entity = redisClient.Get<Product>($"product:{id}");

                if (entity is not null)
                    redisClient.Remove(id);
                else
                    entity = await _productRepository.GetById(id);

                if (entity is null)
                {
                    Notificate("Product not found!");
                    return;
                }

                entity.Update(model.Name, model.Description, model.Price);
                redisClient.Set<Product>($"product:{id}", entity, new TimeSpan(0, 10, 0));
                await _productRepository.Update(entity);
            }
        }

        public async Task Delete(string id)
        {
            using (var redisClient = new RedisClient(_redisSettings.Host))
            {
                var entity = redisClient.Get<Product>($"product:{id}");

                if (entity is null)
                    entity = await _productRepository.GetById(id);
                else
                    redisClient.Remove($"product:{id}");

                if (entity is null)
                {
                    Notificate("Product not found!");
                    return;
                }

                await _productRepository.Remove(entity);
            }
        }
    }
}
