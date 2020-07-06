# TestTasks
This is test task for "Developer C# junior".

## Koshelek.TestTask
Main solution be changed from [this project](https://github.com/DanWahlin/AspNetCorePostgreSQLDockerApp) after some problems with docker.
 
## prev_Koshelek.TestTask
This solution has some problems with docker containers.

This compose file is the solution of test task.
It's include postgres 

# Using:
1) Download `docker-compose.yml` from Koshelek.TestTask directory
2) Use `docker-compose up`
3) Pages
   - `localhost:5000` - web aplication
   - `localhost:5000/Home/Sender` - send text messages client
   - `localhost:5000/Home/Recipient` -  receive text messages client
   - `localhost:5000/Home/Journal` - clientm what can see messages for last minute or last 10 minutes
   - `localhost:5001` - api to Database
   - `localhost:5001/swagger/index.html` - swagger for api testing
