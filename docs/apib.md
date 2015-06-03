FORMAT: 1A
HOST: https://localhost:8080

# Bridge Resource API's
This API documentiong outlines the basic set of test controller resource available across the bridge.

## Bridge Controller
The test controller can be invoked using the API's listed below. These include capabilities like  
+ dynamic invoke of  the ```Invoke()``` method on well known initializable type.
+ central logging so that client and server logs can viewed together.  

# Group Config

## Config Api's [/config]

The resources/types can be initialized by bridge provided the path to the assemblies are known and types can be inistantiated. 
* `resourcesDirectory` - This is the assembly probe path that would be used to initialize a case insenitive dictionary of fully quailified class name. The assembly name is not used to make the resource[PUT] command easier.   

### Update directory [POST]

+ Request (application/json)

		{
			resourcesDirectory:"c:\testdir\",
		}

+ Response 200 (application/json)

		{
			"config": {	"resourcesDirectory": "C:\git\bifrost\src\tests\Bridge.Tests\bin\Debug" },
			"types": {	
				"Bridge.Commands.Hostname": "Bridge.Commands.Hostname, Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
				...
		}

# Group Resources
Group of all resource initialization & update methods.

## Resource Api's [/resource/{resourcename}]

+ The initialization framework would be responsible for executing the ```Invoke()``` method a given class. 

+ Parameters
    + resourcename: 'ResourceNamespace.ResourceClass' (required, string) - A unique type name identifier. 

### Initialize Resource [PUT]

+ Request (application/json)

		{
			name:"resourcename",
			type:"FullyQualifiedTypeName"
			parameters : [{
					baseurl: "",
					other: { ... }
			}]
		}

+ Response 201 (application/json)
	
		{
			id:"correlation-guid"
			details : {
			  baseUrl: "..." /* The data returned by the invoke method */
			}
		}

+ Response 500 (application/json)
	
		{
			id:"correlation-guid"
			details : { 
				error: "Internal server error exception or stack trace."
			}
		}

### Update status [POST]

+ Request (application/json)

		{
			status: "success|failure",
			message: "Additional messages or timing info."
			error: "Exception or stack trace"
		}

+ Response 200 (text/plain)

# Group Diagnostics

## Logs Entries [/log]

### View All Logs [GET]

+ Request (text/plain)

+ Response 200 (application/json)

		{
			level : 'info|error',
			message : 'Entry string'
			dateTime: 'ISO encoded time'
		}

## Resource Logs [/log/{resourcename}{?level}]

Api's which enable retrieving or adding log entries for a specific resource.

+ Parameters
    + level (enum[string], optional) - Level of the log message to indicate if it an error just info
    	+ Default: info
        + Members
            + `error` - Indicate that the log entry is an error
            + `info`  - Indicates that the log entry is just informational.

### View Resource Logs [GET]

Returns logs of a specified level including all entires of higher criticality than the one specified. For example if the level is ```info``` then the response woulc contain all entries for the given resource which have level ```info``` and ```error```

+ Request (text/plain)

+ Response 200 (application/json)

		{
			level : 'info|error',
			message : 'Entry string'
			dateTime: 'ISO encoded time'
		}

### Add Log Entry [POST]

+ Request (application/json)
   
		{ 
			message:"the message to log."
		}

+ Response 200 (text/plain)

## Purge all logs [/purge]

### Purge all. [DELETE]

+ Request (text/plain)

+ Response 200 (text/html)
