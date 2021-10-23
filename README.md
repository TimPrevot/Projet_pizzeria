#The Pizzeria Project

###Before starting

- The database we chose to use is postgreSQL
- To create the database, run the sql script `create_db.sql`
- To populate the database, run the sql script `populate.sql`
- Change the `postgreConStr` value in the `App.cs` with your postgreSQL credentials

###

#####To create a new root user

- The first time you run the app, immediately select "add a new user", then choose `entity = 2` to be asked to enter a username and a password.
- Then exit the app and un-comment the lines 955 to 958 in the `App.cs` file, they look like this:
```
while (!isConnected)
{
    isConnected = myApp.connectClerk();
}
```
- You can now login with your newly created credentials !
