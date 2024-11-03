This is a baseball-themed web api using dotnet 8, C#, and two Docker containers (MySql and postgreSQL). 

It uses Docker for running MySql and PostgreSQL containers. The baseball tables are stored in MySql DB 
and PostgreSQL container is used for authentication using Asp.Net Core Identity library. This uses entity framework
with PostgreSQL.

It utilizes the MySql.Data library to access the container instance and EF for using Asp.Net core Idenity
library with postgreSQL. 

Check out db_resources.txt for table diagrams and some seed data so you can get started with learning docker in a unix terminal. 

The project was created in Ubuntu Linux 22.04 and Mac OS.
