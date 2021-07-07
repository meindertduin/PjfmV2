
#Introduction

This project is the forntend and the backend for the new and better version of PJFM. 

## Setting up the backend

### Databse
- Create a new local database in microsoft SQL on localhost. 
  
- Add with [dotnet user-secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?tabs=windows#secret-manager) (of vanuit Visual Studio) in the API project directory the connectionstring naar jouw lokale database zetten.
  The key is `ConnectionStrings:ApplicationDb`. In the user-secrets map it will look something like this:
```json
{
  "ConnectionStrings:Database": "data source=.;initial catalog=polderjongensfm;persist security info=True;Integrated Security=SSPI;"
}
```


## Setting up the frontend

### Installing dependencies
Go to the ClientApp directory in the Bff project and simply run ``npm install``.

## Running the frontend and backend

### running the frontend
The frontend can be run with the command ``npm start`` in the ClientApp directory located in the Bff project folder. 

### Running the backend
The backend can be run with the command ``dotnet watch run`` in the Api project folder.

