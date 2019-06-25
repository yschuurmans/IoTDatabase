using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Controllers
{

    [Route("/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ItemService itemService;

        public ValuesController(ItemService itemService)
        {
            this.itemService = itemService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "usage:\n" +
                   "For getting values: \n" +
                   "/[ApiKey]/[collection]/[password]/[key]\n" + // /testapikey/testcollection/testpassword/testkey1
                   "or\n" +
                   "/[ApiKey]/[collection]/[password]?key1&key2&key3\n" + // /testapikey/testcollection/testpassword?testkey1&testkey2
                   "\n\n" +
                   "For settings values: \n" +
                   "/[ApiKey]/[collection]/[password]/[key]/[newValue]\n" +  // /testapikey/testcollection/testpassword/testkey1/abc
                   "or\n" +
                   "/[ApiKey]/[collection]/[password]?key=value&key2=value2\n"; // /testapikey/testcollection/testpassword?testkey1=abc&testkey2=abcd
        }

        // GET api/values/5
        [HttpGet("1/{apiKey}/{collection}/{password}/{key}")]
        [HttpGet("1/{apiKey}/{collection}/{key}")]
        public ActionResult<string> GetOne(string apiKey, string collection, string key, string password = "")
        {
            return itemService.GetValue(apiKey, collection, password, key);
        }

        [HttpGet("{apiKey}/{collection}/{password}")]
        [HttpGet("{apiKey}/{collection}")]
        public ActionResult<Dictionary<string,string>> GetMultiple(string apiKey, string collection, string password = "")
        {
            string[] keyStrings = Request.Query.Keys.ToArray();
            return itemService.GetValues(apiKey, collection, password, keyStrings);
        }


        // GET api/values/5
        [HttpPost("{apiKey}/{collection}/{password}/{key}/{newValue}")]
        public ActionResult<string> SetMultiple(string apiKey, string collection, string password, string key, string newValue)
        {
            return itemService.SetValues(apiKey, collection, password, new Dictionary<string, string>()
            {
                { key, newValue }
            }) ? "OK" : "NOT OK";
        }

        // GET api/values/5
        [HttpPost("{apiKey}/{collection}/{password}")]
        public ActionResult<string> SetMultiple(string apiKey, string collection, string password, string key)
        {

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            foreach (var queryKey in Request.Query.Keys)
            {
                keyValues[queryKey] = Request.Query[queryKey];
            }
            return itemService.SetValues(apiKey, collection, password, keyValues) ? "OK" : "NOT OK";
        }
    }
}
