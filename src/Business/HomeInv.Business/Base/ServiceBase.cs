using HomeInv.Common.Interfaces.Services;

namespace HomeInv.Business.Base
{
    public abstract class ServiceBase : IServiceBase
    {
        //protected DocumentClient client;
        //protected CosmosSettings cosmosSettings;
        //private Uri collectionUri;

        //public ServiceBase(IOptions<CosmosSettings> _cosmosSettings)
        //{
        //    cosmosSettings = _cosmosSettings.Value;
        //    client = new DocumentClient(new Uri(cosmosSettings.EndpointUri), cosmosSettings.PrimaryKey);
        //    CreateDatabaseIfNotExists(cosmosSettings.DatabaseName).Wait();

        //    collectionUri = UriFactory.CreateDocumentCollectionUri(cosmosSettings.DatabaseName, GetCollectionName());
        //    CreateDocumentCollectionIfNotExists(cosmosSettings.DatabaseName, GetCollectionName()).Wait();
        //}

        public abstract string GetCollectionName();

        //protected async Task CreateDatabaseIfNotExists(string databaseName)
        //{
        //    try
        //    {
        //        await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
        //    }
        //    catch (DocumentClientException de)
        //    {
        //        // If the database does not exist, create a new database
        //        if (de.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            await client.CreateDatabaseAsync(new Database { Id = databaseName });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //protected async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        //{
        //    try
        //    {
        //        await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
        //    }
        //    catch (DocumentClientException de)
        //    {
        //        // If the document collection does not exist, create a new collection
        //        if (de.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            DocumentCollection collectionInfo = new DocumentCollection();
        //            collectionInfo.Id = collectionName;

        //            collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

        //            await client.CreateDocumentCollectionAsync(
        //                UriFactory.CreateDatabaseUri(databaseName),
        //                new DocumentCollection { Id = collectionName },
        //                new RequestOptions { OfferThroughput = 400 });
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        //protected async Task<T> CreateDocument<T>(T document)
        //{
        //    var createResponse = await client.CreateDocumentAsync(collectionUri, document);

        //    if (createResponse.StatusCode != HttpStatusCode.Created)
        //    {
        //        throw new HICosmosException($"Belge yaratılırken hata oluştu. Durum kodu: {createResponse.StatusCode.ToString()}");
        //    }

        //    string x = createResponse.Resource.ToString();
        //    T y = JsonConvert.DeserializeObject<T>(x);

        //    return y;
        //}

        //private IOrderedQueryable<T> GetAllDocuments<T>()
        //{
        //    return client.CreateDocumentQuery<T>(collectionUri);
        //}

        //public IQueryable<T> GetDocumentListByPredicateAsQueryable<T>(Expression<Func<T, bool>> predicate)
        //{
        //    var queryableList = GetAllDocuments<T>();

        //    var filteredList = queryableList.Where(predicate);

        //    return filteredList;
        //}

        //public T GetDocumentByPredicate<T>(Expression<Func<T, bool>> predicate)
        //{
        //    var queryableList = GetAllDocuments<T>();

        //    if(queryableList != null)
        //    {
        //        var filteredList = queryableList.Where(predicate).AsEnumerable();
        //        if (filteredList != null && filteredList.Count() > 0)
        //        {
        //            return filteredList.First();
        //        }
        //    }

        //    return default(T);
        //}

        //public async Task<T> GetDocumentById<T>(string id)
        //{
        //    try
        //    {
        //        T item = await client.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(cosmosSettings.DatabaseName, GetCollectionName(), id).ToString());
        //        return item;
        //    }
        //    catch (AggregateException ae)
        //    {
        //        if (ae.InnerException is DocumentClientException &&
        //            (ae.InnerException as DocumentClientException).StatusCode == System.Net.HttpStatusCode.NotFound)
        //        {
        //            return default(T);
        //        }
        //        throw;
        //    }
        //    catch(DocumentClientException dce)
        //    {
        //        if(dce.StatusCode == HttpStatusCode.NotFound)
        //        {
        //            return default(T);
        //        }
        //        throw;
        //    }
        //}
    }
}
