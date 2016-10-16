# DVD Library
[![Build status](https://ci.appveyor.com/api/projects/status/x7pr6aw8un4558i1?svg=true)](https://ci.appveyor.com/project/dwlodarz/dvdlibrary)

This site includes answers to the the test question as well as high-level description of the application and its implementation.

## 1. Basic CS

### A
The design consists of 4 tables:
1. Client -> Holds the users of the DVD library
2. Movies -> List of available movies with their names.
3. CopiesOfMovie -> Table that stores the current stock status.
4. Order -> Joins the client with the movie to become an order

### B
The main problem of this task is the runtime memory allocation. Having that many files in a single folder makes it impossible to load it all and then merge. 
Even if I'd pick a single name and then search for it, it doesn't make me certain that the app wouldn't crash.

**Solution** would be to hand over the memory management to some other entity and base on its API. What I mean by this, is that I'd take advantage of
a database (no matter if it's relational or NoSQL one). The NoSQL (e.g. MongoDB) might even behave slightly better due to lack of the overhead of relations and the fact
that it's meant to deal with large volumes of data of this kind.

> **Steps**
> - Load the files one by one into a stream and insert in the same manner into the DB of our choice (e.g. 2 tables - one to hold the names, the other to store the content of the file).
>    There's a small chance that this might overflow the memory. One can also take some precautions against it.
> - When finished. Perform a SQL/NoSQL query that return a single file content at a time of a picked user and append it to a file. When all the records for this user are already 
>	 written back to a file, take another name from table 1.
> **Side remark**
> In order to further increase the performance, one can consider writing a stored procedure that operates on the local file system (if possible) and does the same as above
> with the exception of communication via ADO.NET.

### C
As previously, the main issue is again the amount of memory one can use in order not to crash the application. On average a simple text file containing 10k words is 60-80kB. 
This is fine in terms of loading a single file at a time but not all of them.

If the method was meant to be executed once, it'd be enough to open files one by one and with the use of regular expressions (Regex.Matches( input,  "searchedWord" ).Count) add up 
all the values for all the files. 
> **Side remark**
> Regular expressions are DOS attack prone

If the method should perform statistics of every single word across all files. I'd approach it similarly as previously. So the first run of a method would populate a DB with
a single entry for a given word and corresponding appearance count. 
> In English there are up to 10k words that are used daily so there shouldn't be too many records in the table (otherwise one can consider using Partitions).
Every following call to a method would just query the database for a corresponding value. So only the first call would be computationally heavy or rather I/O heavy. 
One can also consider reading the files in parallel what shout increase the general throughput:

```
 AllLines = new string[MAX]; //only allocate memory here
 AllLines = File.ReadAllLines(fileName);
 Parallel.For(0, AllLines.Length, x =>
 {
     TestReadingAndProcessingLinesFromFile_DoStuff(AllLines[x]);
 });
```

> **Time estimation**
> Assuming that we work on a average machine with no SSD drive, some benchmark indicate that with parallel processing 10k word file would be processed within 4-7 minutes. Processing
> time of 400k in unacceptable, therefore one has to consider either scaling up or out. The cheapest is to take advantage of existing cloud vendors and not only do parallel processing
> within a single machine but also in terms of multiple instance. The files should be placed on a shared network resource.

## 2. Build a web interface that Oscar will use to manage his DVD rental store.

### A
The current implementation is a poor man's solution to the problem due to the time constraints given. It'd have to be further expanded with following features and refactorings:
1. Finish implementing the Facebook JWT authentication (BackEnd + FrontEnd) : 2MD
2. Add DI in order to separate the layers: 5MH
3. Introduce UnitTests for basic functionalities: 2MD
4. Add a business logic level: 1MD
5. Change css it less + gulp task for compilation: 1MD
6. Add FrontEnd functionalities like: add order, delete order, see what have I a rented: 3MD
> *Nice to have* 7. introduce TypeScript instead of plain JS: 1MD
> *Nice to have* 8. introduce lazy javascript load - on demand.

### C
I've implemented a single page application. The FrontEnd functionalities that are made available to the user had to be cut due to the time restrictions. I tried to deliver
a potentially shippable piece of software. All the foundations where prepared to further development (at least to a certain extend). 
Database structure and access covers at leasta couple of points from **A**. Cross-cutting concerns between DAL and Endpoint are also taken care of with the help of different
data models. Unfortunately, I haven't managed to move the DB's connection string up to Azure configuration management.

The business logic itself is placed in the WebAPI's actions which may lead to a question if I wouldn't be better of having it in a separate layer. The fact is that EntityFramework gives
us a repository patter out of the box but frankly I'd feel better having WebAPI expose only data (REST resource) and not have the logic inside. The decision to simplify it was
made due to lack of time. 

FrontEnd application is hosted under a different url and composes a separate entity on itself. It communicates with IIS to retrieve plain html markup needed for angular navigation and
WebAPI's endpoint for feeding data.

The technology stack consists of the following:

- ASP.NET WebAPI
- AngularJS 1.x
- Azure Database
- EntityFramework 6.x
- Automapper - for rewriting the models
- OWIN + Facebook authentication (not finished)
- Bootstrap - components + responsive UI + navigation

Everything is hosted under App service in azure:
> **FrontEnd application** {[ttp://dvdlibrarywebapp.azurewebsites.net/](http://dvdlibrarywebapp.azurewebsites.net/)
> **WebAPI** [http://dvdlibrary.azurewebsites.net/api/](http://dvdlibrary.azurewebsites.net/api/movie/AvailableMovies)

Having this setup I'm able to bind the continuous integration that I have configured [![Build status](https://ci.appveyor.com/api/projects/status/x7pr6aw8un4558i1?svg=true)](https://ci.appveyor.com/project/dwlodarz/dvdlibrary)
with the deployment and also if the load increases to auto-scale.