### Running Locally
1. Create an empty SQL database named `Hangfire`
2. Create an empty SQL database named `Shows`
3. Set environment variable `SQLAZURECONNSTR_Hangfire` with the connection string of the `Hangfire` database 
4. Set environment variable `SQLAZURECONNSTR_Shows` with the connection string of the `Shows` database 
5. Open `ShowsService.sln` in Visual Studio
6. In the Package Manager Console of Visual Studio, select the project `ShowsService.Data`
7. In the Package Manager Console, run `Add-Migration Initial`
8. In the Package Manager Console, run `Update-Database`
9. Hit F5
10. The ingestion process will be kicked off and start retrieving the required data from the TVmaze API (this will take some time)
11. The Shows API will be usable immediately using the data available from the ingestion process

### Improvement Ideas
* Instead of environment variables and hardcoded constants, create proper settings and secrets using standard ASP.NET Core mechanism  
* Instead of public access, configure proper authentication for the Hangfire dashboard  
* Create more unit tests  
* Create integration tests using `TestServer` and `WebApplicationFactory`  
* Reduce Hangfire latency by switching to Redis  
* In `ShowIngestionBackgroundService`, find a better way to determine the ingestion process has been kicked off  
* Wrap the static Hangfire `BackgroundJob` class in an interface to improve testability 