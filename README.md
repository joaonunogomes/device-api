# Device API

REST API service that supports the management of a smartphone device database.

## Represented entities

Device

- Device name
- Device brand
- Creation time

## Supported operations

1. Add device
2. Get device by identifier
3. List all devices
4. Update device (full and partial)
5. Delete a device
6. Search device by brand

# Project Structure

The structure of this project follows Domain Driven Design patterns, devided into several layers, each one of them will sever its own responsability.

## Layers

This architecture relies on the Dependency Inversion Principle. The Presentation Layer communicates to Application Layer through interfaces, and so on.
It has four layers and a common crosscutting extra layer, as shown in next figure.

![dependency-layer-diagram](https://github.com/joaonunogomes/device-api/blob/main/documentation/dependency-layer-diagram.png)

- Presentation -> responsible by exposing the service to the clients
- Application -> Contains Data Tranfer Objects that will be returned to the client and used by the client to do requests to this service. Also here is where we try to coordinate the lower level layers, like a facade.
- Domain Model -> Contains all business/data models
- Data Repository -> Contains all database access implementations and interfaces.
- Infrastructure Crosscutting -> Contains helpers, extensions and other tools, that should be accessible by all layers.

## Implementation notes

This service contains two resources:

1. Devices
2. Brands

Since the main goal of the exercise was to manage devices, Brands resource was created only to have a brand associated to a specific device, because of that, the CRUD operations of Brands resource are not complete.

## Integration tests

A postman collection with some happy path tests was created and is available inside `./postman`.

To run postman tests, the newman command line tool can be used as shown bellow:

```
newman run ./postman/Devices.postman_collection.json -e ./postman/Test.postman_environment.json
```

# How to run

## Testing environment

In order to help testing this service, a testing environment was created using _Google Cloud Run_ serverless deployment and _Mongo Atlas_ free tier cluster.

Due to the nature of this exercice, the service domain is public and no authorization strategies were applied, but the thresholds of the servers are very low, so a lot of concurrent requests will shutdown the service.

The service is available at:

```
https://device-api-tmi2mv3z7a-ew.a.run.app/swagger/index.html
```

Example of a request path to this endpoint (get all devices):

```
https://device-api-tmi2mv3z7a-ew.a.run.app/devices
```

## With docker

To run this project with docker, a docker-compose file was provided in order to make run both the DeviceApi and mongo db.
To do so, open a terminal and go to the project root folder. Once there, just execute the following command:

```
docker-compose -f "docker-compose.yml" up -d --build
```

### **Notes**

Ensure that ports `9199` and `27017` are free on the server you will run this project.

### Custom configuration

If you want to use a custom mongo db instance, set `MongoSettings__MongoConnectionString` upon the docker run of DeviceApi.

## With Visual Studio

To run this project with Visual Studio follow the follwing steps:

1.  `Presentation.Api` is set as _Startup Project_
2.  Set the correct `MongoConnectionString` on `appsettings.json`
    - If you want to use the provided mongodb run the follwogin command on the root folder of the project: `docker-compose -f "docker-compose.yml" up -d --build mongodb` (this will start a mongodb dockerized instance)
    - If you want to use a custom mongodb instance, set the connection string on `appsettings.json`
3.  Click on the green play button and the project should build and start running

### **Port**

If running with docker or Visual Studio, this service will listen on port `9199`.

# Pull Requests

Every pull request will run the following actions:

1. Docker build + unit test run
2. Deploy to google cloud run specific PR environment
3. When steps 1 and 2 finish, run Newman test collection targeting the previously deployed version on cloud run PR environment

#### **Notes**

- Steps 1 and 2 are possible due to Google Cloud Build integration with Github
- Step 3 is possibel using the following Github actions available on Github marketplace:
  - https://github.com/marketplace/actions/newman-action
  - https://github.com/marketplace/actions/wait-for-check
