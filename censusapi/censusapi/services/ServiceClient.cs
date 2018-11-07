using censusapi.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

using Microsoft.Azure.Documents.Linq;

namespace censusapi.services
{
    public class ServiceClient
    {
        private DocumentClient client;
        private string _writeuri;
        private string _readuri;
        private async Task EstablishConnection()
        {
            ConnectionPolicy connectionPolicy = new ConnectionPolicy
            {
                UseMultipleWriteLocations = true,
            };
            connectionPolicy.PreferredLocations.Add(LocationNames.EastUS2);
            connectionPolicy.PreferredLocations.Add(LocationNames.SoutheastAsia);

            try
            {
                client = new DocumentClient(new Uri(GlobalParams.EndpointUri), GlobalParams.PrimaryKey, connectionPolicy);
                await client.OpenAsync().ConfigureAwait(false);
                //Console.WriteLine("The write URI (REMOTE CLIENT) used in the Connection is " + client.WriteEndpoint.AbsoluteUri);
                _writeuri = client.WriteEndpoint.AbsoluteUri;
                //string dataorigin = "https://censusdata-southeastasia.documents.azure.com/";
                string dataorigin2 = _writeuri.Substring(_writeuri.IndexOf('-') + 1);
                string dataorigin3 = dataorigin2.Substring(0, dataorigin2.IndexOf('.'));
                GlobalParams.dataOrigin = dataorigin3;
                _readuri = client.ReadEndpoint.AbsoluteUri;
                //Console.WriteLine("The read URI (REMOTE CLIENT) used in the Connection is " + client.ReadEndpoint.AbsoluteUri);
            }
            catch (DocumentClientException de)
            {
               // Some connection error 
            }

        }

        public async Task<Family> InsertFamilyData(Family family)
        {
            await EstablishConnection();

            try
            {
                await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(GlobalParams.databaseName, GlobalParams.collectionName, family.Id), new RequestOptions { PartitionKey = new PartitionKey(family.Id) });
                //Console.WriteLine("Found {0}", family.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    ResourceResponse<Document> response = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(GlobalParams.databaseName, GlobalParams.collectionName), family);
                    //Console.WriteLine("Created Family {0}" + family.Id + ", and the RUs consumed " + response.RequestCharge);
                }
                else
                {
                    return null;
                }
            }
            return family;
        }

        public async Task<List<Family>> GetAllCensusData()
        {
            await EstablishConnection();
            List<Family> censusdata = new List<Family>();
            try
            {
                IDocumentQuery<Family> query = client.CreateDocumentQuery<Family>(
                UriFactory.CreateDocumentCollectionUri(GlobalParams.databaseName, GlobalParams.collectionName),
                 new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                 .AsDocumentQuery();

                while (query.HasMoreResults)
                {
                    censusdata.AddRange(await query.ExecuteNextAsync<Family>());
                }
                return censusdata;
            }
            catch (DocumentClientException de)
            {
                Console.WriteLine("Error reading documents from Collection " + de.StackTrace + " , " + de.Message);
                return null;
            }
        }
    }
}
