using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace censusapp.Models
{
    public enum Gender { Male, Female };
    public class Family
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string LastName { get; set; }
        public Parent[] Parents { get; set; }
        public Child[] Children { get; set; }
        public Address Address { get; set; }
        public bool IsRegistered { get; set; }
        public string DataOrigin { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class Parent
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
    }
    public class Child
    {
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public int Grade { get; set; }
        public Pet[] Pets { get; set; }
    }
    public class Pet
    {
        public string GivenName { get; set; }
    }
    public class Address
    {
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
