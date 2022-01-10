# Device API

REST API service that supports the management of a device database.

# Project Structure

The structure of this project follows Domain Driven Design patterns, devided into several layers, each where each one will sever its own responsability.

- Presentation -> responsible by exposing the servise to the clients
- Application -> Contains Data Tranfer Object that will be retruned to the client and used by the client to do requests to this service
- Domain -> Contains all business/data models
- Data -> Contains all database access implementations
- Infrastructure Crosscutting -> Contaisn helpers, extensions and other tools, that should be accessible by all other projects.

# How to run

## With docker

To run this project with docker, a docker-compose file was provided in order to make run both the DeviceApi and mongo db.
To do so, open a terminal and go to the project root folder. Once there, just execute the following command:

```
docker-compose -f "docker-compose.yml" up -d --build
```

_NOTE: ensure that ports `9199` and `27017` are free on the server you will run this project._

### Custom configuration

If you want to use a custom mongo db instance, set `MongoSettings__MongoConnectionString` upon the docker run of DeviceApi.

## With Visual Studio

To run this project with Visual Studio follow the follwing steps:

1.  `Presentation.Api` is set as _Startup Project_
2.  Set the correct `MongoConnectionString` on `appsettings.json`
    - If you want to use the provided mongodb run the follwogin command on the root folder of the project: `docker-compose -f "docker-compose.yml" up -d --build mongodb` (this will start a mongodb dockerized instance)
    - If you want to use a custom mongodb instance, set the connection string on `appsettings.json`
3.  Click on the green play button and the project should build and start running
