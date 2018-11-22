# Building and deploying highly available and resilient applications
A sample application that showcases how highly available and resilient services can be deployed to Azure. can be used to deploy highly available and reliable services. Refer to it in the context of the article published in the MSDN Magazine <to be added later>, that describes the use case and the architecture of the Solution.

## Developing the Application
There are two ASP.NET 2.0 Core Applications, one an Mvc Project **censusapp.sln** and the other, a Web API Project **censusapi.sln**. The Mvc project accepts user input and calls the Web API to store the data in Azure Cosmos DB. The applications are packaged using Docker containers for Linux using the built-in features in Visual Studio 2017. The source files, associated Dockerfile and docker-compose files of these Projects are available in this Repository. Minor edits were done to the docker-compose files of the two projects to add port mapping between the Node and container.

### About the Web API Project
The Application is deployed to two Azure regions (US East 2 and Southeast Asia). The Web API Project leverages the Multi-region write capability in Azure Cosmos DB to write data into a Collection local to that region. The Web API deployed to East US 2 region adds the Cosmos DB endpoint in this region as priority 1, and that in Southeast Asia as priority 2. This ensures that in the event of an issue connecting to the local collection, the SDK fails over the connection to Southeast Asia, preventing Application downtime. The snippet below shows how Multi-region write is enabled and how multiple connection endpoints are added.
~~~
ConnectionPolicy connectionPolicy = new ConnectionPolicy
{
    UseMultipleWriteLocations = true,
};
connectionPolicy.PreferredLocations.Add(LocationNames.EastUS2);
connectionPolicy.PreferredLocations.Add(LocationNames.SoutheastAsia);
~~~
The API takes minimal data from the user and generates fictitious data for the remaining attributes, using the Bogus Nuget Package.

## Source control and CI pipeline
The Source files from the two Projects are checked into a Git Repository in Azure Devops Service. A Continuous integration Pipeline uses the built-in activities to run docker-compose. This triggers a build of the Projects and generate the Container images for the Mvc and Web API Projects. The container images are tagged & suffixed with the CI Build Run Id, and pushed to the Docker Hub registry.

<img src="./images/CIpipeline.png" alt="drawing" height="350px"/>

The steps above are repeated after making code changes to the Web API Project. The order of priority of the Cosmos DB connection endpoints are modified for deployment to Azure Region in Southeast Asia. The container image for the Mvc Project would be the same in both the regions.

## Packaging the Application for deployment to Service Fabric
The Yeoman tool creates an Application and adds two Service types for the Mvc and Web API applications. At the prompt, provide the names of the container images uploaded to Docker Hub and the number of instances of containers of each Service type to deploy in Service Fabric. See below:

<img src="./images/yeomanGen.PNG" alt="drawing" height="700px"/>
