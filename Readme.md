NoSql Blog
==========

A Blog Engine Implemented with Multiple Data Storage Products
-------------------------------------------------------------

This is a project created to learn about various NoSql products and 
their use withink the Microsoft .NET framework, especially with
regard to use in a web application built in ASP.NET MVC. 

Currently, most applications in the Microsoft world are built on 
top of SQL Server. While relational databases are very useful and 
widely understood, they do have their drawbacks, especially when
it comes to mapping between objects created within the context of
code and rows retrieved from an RDBMS.

Using this project as a framework, I will evaluate several
"alternative" data storage mechanisms on the following attributes:

* Ease of Development

As discussed above, one of the major drawbacks to traditional DMBS
is that transferring data between the data store and the application
is a chore pervading much of the application's architecture and 
slowing development considerably. A seamless integration between the
two would be a major benefit to any application.

* Ease of Deployment

Because of the wealth of knowledge and experience with SQL Server
among administrators on the MS stack, there will be a considerable
amount of resistance to any suggestion of changing such a 
fundamental building block of software development projects in that
environment. A replacement product which is not difficult to install
and get running should help ease those concerns.

* Ease of Administration

In the same vein as the above point, a system which is simple to 
monitor, back up, restore and otherwise care for will help any
team's stakeholders feel more comfortable in making a change.

* Flexibility

I'm not sure what I meant by this!

* Performance

As someone whom I greatly resepect, but I can't remember who, once said:

> Performance is a feature.

SQL Server can undeniably perform either extremely well or very poorly,
depending on the configuration and how it's used by an application. A
replacement product which can perform well regardless of poor or naive
application design would be very well received by both developers and
administrators.

* Source Control Integration

Currently, integrating SQL Server schema changes with source control and
easily moving between database versions is not a task which is simple,
intuitive or effortless. A system which improved on this workflow would
make deployments less stressful.

* Tooling

An area in which SQL Server shines is the tooling which has grown up around
it over the years, including many third-party tools, but specifically
Microsoft's own SQL Server Management Studio. Its ease of use in the areas of
quickly creating and debugging databases, procedures and the like add
tremendous value to the implementation of SQL Server in applications.

* Cost

As I've said many, many, many times,

> You get what you pay for.

Now, this doesn't necessarily refer to monetary transactions. In the world of
open source, pull requests are the currency of choice. Sweat equity, if you will.
It is definitely possible to "freeload" and use an open source tool without
contributing, but in most cases there is no support available, so anyone
using a community tool will need to have appropriate talent in-house to handle
difficulties which will most certainly arise.

Also, how much money is it?

The Candidates
--------------

This list will most likely grow and shrink as I learn more about what's available
and which tools meet the requirements I feel are important to most of the projects
I work on. Currently, there are only a couple which I know anything at all about,
a couple more of which I know only the name, and many many remaining to be 
investigated, added or discarded.

### RavenDB

### Redis

### MongoDB

I hope you've enjoyed this production. Be sure to tune in next week. Same bat time.
Same bat channel.