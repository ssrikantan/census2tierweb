using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using censusapi.entities;
using censusapi.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace censusapi.Controllers
{
    [Produces("application/json")]
    [Route("api/Family")]
    public class FamilyController : Controller
    {
        // GET: api/Family
        [HttpGet]
        public async Task<IEnumerable<Family>> Get()
        {
            List<Family> families = null;
            ServiceClient client = new ServiceClient();
            families = await client.GetAllCensusData();
            return families;
        }

        // GET: api/Family/5
        [HttpGet("{id}", Name = "Get")]
        public Family Get(int id)
        {
            return new Family();
        }

        // POST: api/Family
        [HttpPost]
        public async Task<Family> Post([FromBody]Family value)
        {
            GenerateFakeData datagen = new GenerateFakeData();
            Family family = datagen.ReturnNewObject(value.Id);
            ServiceClient client = new ServiceClient();
            Family fam= await client.InsertFamilyData(family);
            if(fam==null)
            {
                return null;
            }
            else
            {
                return family;
            }

        }
        
        // PUT: api/Family/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
