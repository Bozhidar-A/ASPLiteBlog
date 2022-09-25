# ASPLiteBlog

A simple and lite blogging engine created with ASP.NET

A live version can be found at [aspliteblog.duckdns.org](https://aspliteblog.duckdns.org)

## Install
It is recommended to use the Docker and docker-compose.

* Copy the contents of docker-compose.yaml to your own file
* Change the database password by updating the ```DB_PASS``` environment variable
* Run with ```docker-compsoe up```
* By default there will be one admin account with the passwrod ```Admin123!```, it can be changed by clicking in the top right and changing the password from settings

## Working with the project
Both ```Visual Studio``` (2022 preferably) and ```.NET 6``` are needed

If you are on Windows that should be all that's needed to run the project, if you are on Linux try to use Docker instead

### Pull requests are WELCOME

## Future
There are many things I wish to add and fix, including, but not limited to:

* Adding a comment section
* Making it multi level
* Adding proper email handling
* Updating the look of the site
* Adding dark mode
* Making SEO friendly URLs
* Adding previews
