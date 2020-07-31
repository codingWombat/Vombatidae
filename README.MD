# Vombatidae

![.NET Core](https://github.com/codingWombat/Vombatidae/workflows/.NET%20Core/badge.svg?branch=develop)

## Intention

The intention of this project was to create an a REST mock server, that you can run yourself locally during the development of front- and backend applications.

## Usage

The idea of **Vombatidae** is that you can generate multiple endpoints (they are called burrow in the code) by asking the server for a new one. The server will return a `Guid` that identifies this particular endpoint. With this `Guid` you are able to prepare rest response messages with JSON bodies for the following REST verbs `GET`, `POST`, `PUT`, `DELETE`. At the moment it is only possible to prepare one response per verb and configure the status code that is used.

**Vombatidae** also allows to query this history of an endpoint. The history contains all requests and send responses for an complete endpoint.

### Demo system with UI
[Demo](https://www.codingwombat.dev)

### Configuration

**Vombatidae** supports two operations modes. You can either use .net core based in memory cache or a REDIS server to store the history and respons message per endpoint.

The needed configuration can either be done in the appsettings.json or by setting environment variables.

|name|data type|mandatory|description|
|---|---|---|---|
|UseRedis|boolea|y|indicates if Redis should be used or not|
|SlidingExpiration|int|y|The amount of seconds the data is kept in the cache. (Resets every time the end point is used.)|
|Host|string|n|The host of the Redis server, must only be set if UseRedis is true|
|Instance|string|n|The instance that is used in the Redis, must only be set if UseRedis is true|
|Origins|string []|n|If you want to use a frontend application that runs on a different URL you must configure them here|

Json:
```json
"Cache": {
    "UseRedis": false,
    "SlidingExpiration": 48200,
    "Host":"localhost",
    "Instance":"Foo"
  },
  "Cors": {
      "Origins": ["https://localhost:8080", "http://example.com"]
    }
```
Environment variables:
```
Cache__Host=localhost;
Cache__Instance=Foo;
Cache__SlidingExpiration=48200;
Cache__UseRedis=true
Cors__Origins__0=http://example.com
Cors__Origins__1=https://localhost:8080
```

### Docker

For **Vombatidae** there is also a docker container available that can be found [here]([https://link](https://hub.docker.com/r/codingwombat/vombatidae/)).

To run the container on your machine without Redis use the following command:

````Docker
docker run -p 8081:80 --env Cache__UseRedis=false --env Cache__SlidingExpiration=12600 --env Cors__Origins__0=http://example.com --env Cors__Origins__1=https://localhost:8080 --name vombatidae codingwombat/vombatidae:master  
````

To run **Vombatidae** with an attached Redis use `docker-compose -d up` with the `docker-compose.yml` that is located in the top level of the repo.

### Endpoints

To use **Vombatidae** as a mock server you need the following endpoints:

1. To create a new endpoint (burrow) send a `GET` request to `/Vombatidae/Burrow`.
The response will contain a `Guid` that will be used in all following URLs.

2. To save a response send a `PUT` request to `/Vombatidae/Feed/{{guid}}/{RESTverb}`.
The `Guid` defines what endpoint should be used and `RESTverb` defines for what requests the response should be used.
Allowed values are: `GET`, `POST`, `PUT`, `DELETE`.
The response itself will be send inside the body in the following format:
    ````JSON
        {
        "StatusCode": 200,
        "ResponseMessage": {
            "String": "this is a test string",
            "int": 123456789,
            "bool": true,
            "datetime":"2020-07-21T11:22:47.834561Z"
            }
        }
    ````
    `StatusCode` is the http response code used for the response.
    `ResponseMessage` is a wrapper for the response message it can contain any JSON you want.


3. The endpoint that will produce your stored response is reachable via `/Vombatidae/{{guid}}`.

4. To query the history send a `GET` request to `/Vombatidae/history/{{guid}}`.

You can find example requests under `dev.codingWombat.Vombatidae/Scripts/Vombatidae.http` the can either be used in Rider or with removed variables in VSCode with the REST Client extension.
