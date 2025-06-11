This is a baseball-themed web api using dotnet 8, C#, and two Docker containers (MySql and postgreSQL). 

It uses Docker for running MySql and PostgreSQL containers. 

The baseball tables are stored in MySql DB.

PostgreSQL container is used for authentication using Asp.Net Core Identity library and entity framework.


It utilizes the MySql.Data library to access the container instance, create tables, and retrieve data via controller API endpoints.

Check out db_resources.txt for table diagrams and some seed data so you can get started with learning docker in a terminal. 

The project was created in Ubuntu Linux 22.04/24.04 and Mac OS. Should work fine using Windows as well. I've been able to replicate it in VS code and Rider IDEs. 

<bold>INSTRUCTIONS FOR USE</bold>

You need to install: Docker, dotnet 8 SDK, and IDE of choice (vs code, Rider, etc.)

<p>Follow instructions in <a href="https://github.com/developer-seb-v/baseballAPI_verson1.0/blob/main/db_resources.txt">HERE</a> to create the necessary Docker volume for data persistence and containers for running your MySQL queries, etc. </p>
<br>
<p>
  If you want to add Authentication/Authorization, then follow the instructions <a href="https://github.com/developer-seb-v/baseballAPI_verson1.0/blob/main/db_resources_postgreSQL">HERE</a> to get the PostgreSQL container running for ASP DOT NET CORE Identity. 

  
</p>
