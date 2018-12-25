# logiteam-subquip

Longiteam-subquip backend, API

**This Architecture comes with the "battery-pack included"**:

* Multi-layer structure
* Comes with IOC (Inversion of Control) i.e. Dependency Injection.
* Comes with your favorite swagger template with authorize feature
* Comes with the JWT token authorization


## Layers :

#### SubQuip.Common

Layer contains common and shared features.
That can be use in the project, across any layer

#### SubQuip.Data

Layer will intract with the db entities and perform various db operations.
Here Interfaces and there implementation is present, that in turn send data
back to service layer i.e. business.
Migration history is also maintained here.

NOTE : Data sent back from this layer should be of Entity type.

#### SubQuip.Entity

In this layer database entities are managed.

#### SubQuip.Service

Service layer i.e. also called business layer; data sent from data layer managed and 
manipulated here and sent data back to web layer. 

NOTE : Data sent back from here should be of Viewmodel type.

#### SubQuip.ViewModel

This layer keeps the list of Data transfer objects

#### SubQuip.Web

This Layer will intract with outside world. :-)

## How to use :

* Mongo db should be installed on the machine, installation link : https://docs.mongodb.com/manual/installation/
* Run the mongodb service, so that one can connect to mongo database.
* Create test db, command "use database". This case command should be "use test".
* Create one document in test db, like db.role.insert([{'name': 'Alice', 'age': 30}]);
* Check for inserted document.
* Now, Git clone the repo at system drive.
* Build the project to download the required packages and dependencies
* Change the mongodb connection string in appsetting.json, only if mongo db services running at different port.
* Run the project using f5 button.
* Browser will display the swagger template for Login, role feature.
* Authorize swagger by providing token like as "Bearer {token}".
* To generate token login in swagger with username/password as test/test.

## How to create and run Docker Build for WEB API

### To create docker build
docker build -t subquip.webapi .

### To run Docker build
docker run -p 3002:80 subquip.webapi:latest

## How to create and run Docker Build for Mongo DB

### To create and run the docker build

#### Step-1: 
Check if JSON files for all the collection is available in mongo-seed folder
#### Step-2:
Check if invidual commands for importing all JSON files is written in dockerfile inside mongo-seed folder
#### Step-3:
Open the terminal where docker-compose.yml file is placed and run below command in terminal to create and run the docker build for mongoDB,

docker-compose up -d


### How to export the mongoDB collection to JSON files

mongoexport --db test --collection traffic --out traffic.json