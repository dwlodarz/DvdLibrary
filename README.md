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

* AllLines = new string[MAX]; //only allocate memory here
* AllLines = File.ReadAllLines(fileName);
* Parallel.For(0, AllLines.Length, x =>
* {
*     TestReadingAndProcessingLinesFromFile_DoStuff(AllLines[x]);
* });

> *Time estimation*
> Assuming that we work on a average machine with no SSD drive, some benchmark indicate that with parallel processing 10k word file would be processed within 4-7 minutes. Processing
> time of 400k in unacceptable, therefore one has to consider either scaling up or out. The cheapest is to take advantage of existing cloud vendors and not only do parallel processing
> within a single machine but also in terms of multiple instance. The files should be placed on a shared network resource.
