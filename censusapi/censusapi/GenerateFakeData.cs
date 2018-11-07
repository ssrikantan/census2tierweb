using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.Bson;
using censusapi.entities;

namespace censusapi
{
    public class GenerateFakeData
    {
       private enum _Gender
        {
            Male, 
            Female
        }

        private enum Petname
        {
            Ben,
            Billy,
            Giny
        }

        public Family ReturnNewObject(string id)
        {

            var faker = new Faker<Family>()
                  .RuleFor(m => m.Id, f => id)
                  .RuleFor(m => m.LastName, f => f.Name.LastName())
                  .RuleFor(m => m.Address, f => new Address() { State = f.Address.State(), Country = f.Address.Country(), City = f.Address.City() })
                  .RuleFor(m => m.Parents, f => new Parent[] {
                      new Parent { FirstName = f.Name.FirstName(), FamilyName = f.Name.LastName()},
                      new Parent { FirstName = f.Name.FirstName(), FamilyName=f.Name.LastName()}
                  })
                  .RuleFor(m=> m.DataOrigin,GlobalParams.dataOrigin)
                  .RuleFor(m => m.IsRegistered, f=> f.Random.Bool())
                  .RuleFor(m => m.Children, f => new Child[] {
                      new Child {
                          FirstName = f.Name.FirstName(),
                          Gender = f.Random.Enum<_Gender>().ToString(),
                          Grade = f.Random.Int(0, 10),
                          Pets = new Pet[]
                          {
                              new Pet {GivenName = f.Random.Enum<Petname>().ToString()}
                          }
                      }
                  });

            return faker.Generate();
        }

    }
}
