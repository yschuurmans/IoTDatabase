using IoT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IoT.Controllers
{

    [Route("/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IotDbContext dbContext;

        public ValuesController(IotDbContext context)
        {
            this.dbContext = context;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "usage:\n" +
                   "For getting values: \n" +
                   "GET /1/[ApiKey]/[collection]/[password]/[key]\n" + // /1/testapikey/testcollection/testpassword/testkey1
                   "or\n" +
                   "GET /[ApiKey]/[collection]/[password]?key1&key2&key3\n" + // /testapikey/testcollection/testpassword?testkey1&testkey2
                   "\n\n" +
                   "For settings values: \n" +
                   "GET /[ApiKey]/[collection]/[password]/[key]/[newValue]\n" +  // /testapikey/testcollection/testpassword/testkey1/abc
                   "or\n" +
                   "POST /[ApiKey]/[collection]/[password]?key=value&key2=value2\n"; // /testapikey/testcollection/testpassword?testkey1=abc&testkey2=abcd
        }





        /// <summary>
        /// GET /get/[collection]/[key]
        /// Gets the value, "" if not found
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/{collection}/{key}")]
        public ActionResult<string> GetOne([FromQuery] string apiKey, string collection, string key)
        {
            if (!IsApiKeyValidForGet(apiKey)) return new StatusCodeResult(401);

            return dbContext.Collections.Include(collection => collection.Items)
                .FirstOrDefault(x => x.Name == collection)?
                .Items.FirstOrDefault(x => x.Key == key)?.Value ?? "";
        }

        /// <summary>
        /// GET /get/[collection]
        /// Gets the entire collection, [] if no result
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpGet("get/{collection}")]
        public ActionResult<List<string>> GetAll([FromQuery] string apiKey, string collection)
        {
            ApiKey thisKey = null;
            if (!IsApiKeyValidForGet(apiKey)) return new StatusCodeResult(401);

            var test = dbContext.Collections.Include(collection => collection.Items)
                .FirstOrDefault(x => x.Name == collection)
                ;
            var test2 = test.Items;
            var test3 = test.Items.Select(x => x.Value).ToList();


            return dbContext.Collections.Include(collection => collection.Items)
                .FirstOrDefault(x => x.Name == collection)?
                .Items.Select(x => x.Value).ToList() ?? new List<string>();
        }

        /// <summary>
        /// 
        /// GET /set/[collection]/[key]?value=[value]
        /// Sets the value of [key], creating the collection if it doesn't already exist
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("set/{collection}/{key}")]
        public ActionResult<string> SetValue([FromQuery] string apiKey, string collection, string key, [FromQuery] string value)
        {
            ApiKey thisKey = null;
            if (!IsApiKeyValidForSet(apiKey, out thisKey)) return new StatusCodeResult(401);

            Collection coll = GetOrCreateCollection(thisKey, collection);
            SetOrCreateItem(thisKey, coll, key, value);
            dbContext.SaveChanges();
            return "OK";
        }

        /// <summary>
        /// GET /set/[collection]?value=[value]
        /// Generates a random key and sets its value, creating the collection if it doesn't already exist
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="collection"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet("set/{collection}")]
        public ActionResult<string> AddValue([FromQuery] string apiKey, string collection, [FromQuery] string value)
        {
            ApiKey thisKey = null;
            if (!IsApiKeyValidForSet(apiKey, out thisKey)) return new StatusCodeResult(401);

            Collection coll = GetOrCreateCollection(thisKey, collection);
            SetOrCreateItem(thisKey, coll, new Guid().ToString(), value);

            dbContext.SaveChanges();
            return "OK";
        }

        /// <summary>
        /// GET /remove/[collection]/[key]
        /// Removes the [key] in [collection], if it exists
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("remove/{collection}/{key}")]
        public ActionResult<string> RemoveItem([FromQuery] string apiKey, string collection, string key)
        {
            ApiKey thisKey = null;
            if (!IsApiKeyValidForSet(apiKey, out thisKey)) return new StatusCodeResult(401);

            Collection coll = dbContext.Collections.FirstOrDefault(x => x.Name == collection);
            if (coll == null) return "OK";

            Item item = coll.Items.FirstOrDefault(x => x.Key == key);
            if (item == null) return "OK";

            coll.Items.Remove(item);
            if (coll.Items.Count == 0) dbContext.Collections.Remove(coll);

            dbContext.SaveChanges();

            return "OK";
        }

        /// <summary>
        /// GET /remove/[collection]
        /// Removes the entire collection, including all items in it
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpGet("remove/{collection}")]
        public ActionResult<string> RemoveCollection([FromQuery] string apiKey, string collection)
        {
            ApiKey thisKey = null;
            if (!IsApiKeyValidForSet(apiKey, out thisKey)) return new StatusCodeResult(401);

            Collection coll = dbContext.Collections.FirstOrDefault(x => x.Name == collection && x.Owner == thisKey);
            if (coll == null) return "OK";

            coll.Items.Clear();
            dbContext.Collections.Remove(coll);

            dbContext.SaveChanges();

            return "OK";
        }




        private void SetOrCreateItem(ApiKey thisKey, Collection coll, string key, string value)
        {
            var item = coll.Items.FirstOrDefault(x => x.Key == key);
            if (item == null)
            {
                item = new Item { Key = key, Collection = coll };
                dbContext.Items.Add(item);
            }
            item.Value = value;
            item.LastUpdateTime = DateTime.Now;
            item.LastUpdater = thisKey;
        }

        private Collection GetOrCreateCollection(ApiKey thisKey, string collection)
        {
            var coll = dbContext.Collections.FirstOrDefault(x => x.Name == collection);
            if (coll == null)
            {
                coll = new Collection { Name = collection, Items = new List<Item>(), Owner = thisKey };
                dbContext.Collections.Add(coll);
            }

            return coll;
        }

        private bool IsApiKeyValidForGet(string apiKey)
        {
            return dbContext.ApiKeys.Any(x => x.Key == apiKey);
        }
        private bool IsApiKeyValidForSet(string apiKey, out ApiKey thisKey)
        {
            thisKey = dbContext.ApiKeys.FirstOrDefault(x => x.Key == apiKey && x.CanWrite);
            return thisKey != null;
        }
    }
}
