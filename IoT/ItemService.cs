using IoT.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT
{
    public class ItemService
    {
        private readonly IotDbContext dbContext;

        public ItemService(IotDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public string GetValue(string apiKey, string collection, string password, string key)
        {
            return GetValues(apiKey, collection, password, key)[key];
        }
        public Dictionary<string, string> GetValues(string apiKey, string collectionName, string password, params string[] keys)
        {
            Dictionary<string, string> resultDictionary = new Dictionary<string, string>();
            foreach (var key in keys)
            {
                resultDictionary[key] = "";
            }
            if (!CheckApiKeyGet(apiKey))
            {
                resultDictionary["ERROR"] = "NO VALID API KEY";
                return resultDictionary;
            }


            List<Item> relevantItems = new List<Item>();



            if (String.IsNullOrEmpty(password))
            {
                Collection collection = dbContext.Collections.FirstOrDefault(x => x.Name == collectionName);
                relevantItems = dbContext.Items.Where(x=>x.CollectionFK == collection.Id && x.IsPublic).ToList();
            }
            else
            {
                Collection collection = dbContext.Collections.FirstOrDefault(x => x.Name == collectionName && x.Password == password);
                relevantItems = dbContext.Items.Where(x => x.CollectionFK == collection.Id).ToList();

            }

            if (relevantItems == null || relevantItems.Count <=0)
            {
                resultDictionary["ERROR"] = "COLLECTION NAME OR PASSWORD INVALID";
                return resultDictionary;
            }

            foreach (var relevantItem in relevantItems)
            {
                if (keys.Any(x => x.Equals(relevantItem.Key, StringComparison.InvariantCultureIgnoreCase)))
                {
                    resultDictionary[relevantItem.Key] = relevantItem.Value;
                }
            }


            return resultDictionary;
        }


        public bool SetValues(string apiKey, string collectionName, string password, Dictionary<string, string> keyValues)
        {
            if (!CheckApiKeyWrite(apiKey))
            {
                return false;
            }

            Collection collection = dbContext.Collections.FirstOrDefault(x => x.Name == collectionName && x.Password == password);
            if (collection == null)
            {
                collection = new Collection() { Name = collectionName, Password = password };
                dbContext.Collections.Attach(collection);
                dbContext.SaveChanges();
            }

            foreach (var keyValue in keyValues)
            {
                Item currentItem = dbContext.Items.FirstOrDefault(x => x.CollectionFK == collection.Id && x.Key == keyValue.Key);
                if (currentItem == null)
                {
                    collection.Items.Add(new Item()
                    {
                        IsPublic = false,
                        Key = keyValue.Key,
                        Value = keyValue.Value
                    });
                }
                else
                {
                    dbContext.Items.Attach(currentItem);
                    currentItem.Value = keyValue.Value;
                    currentItem.IsPublic = false;
                }
            }

            dbContext.Collections.Attach(collection);
            dbContext.SaveChanges();
            return true;

        }

        public bool SetPublicValues(string apiKey, string collectionName, string password, Dictionary<string, string> keyValues)
        {
            if (!CheckApiKeyWrite(apiKey))
            {
                return false;
            }

            Collection collection = dbContext.Collections.FirstOrDefault(x => x.Name == collectionName && x.Password == password);
            if (collection == null)
            {
                return false;
            }

            foreach (var keyValue in keyValues)
            {
                Item currentItem = dbContext.Items.FirstOrDefault(x => x.CollectionFK == collection.Id && x.Key == keyValue.Key);
                if (currentItem == null)
                {
                    collection.Items.Add(new Item()
                    {
                        IsPublic = true,
                        Key = keyValue.Key,
                        Value = keyValue.Value,
                        Collection = collection
                    });
                }
                else
                {
                    currentItem.Value = keyValue.Value;
                    currentItem.IsPublic = true;
                }
            }

            dbContext.Collections.Attach(collection);
            dbContext.SaveChanges();
            return true;

        }

        private bool CheckApiKeyGet(string apiKey)
        {
            return dbContext.ApiKeys.Any(x => x.Key == apiKey);
        }
        private bool CheckApiKeyWrite(string apiKey)
        {
            return dbContext.ApiKeys.Any(x => x.Key == apiKey && x.CanWrite);
        }

    }
}
