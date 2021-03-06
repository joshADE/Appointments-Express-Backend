# Appointments-Express-Backend
> The backend server needed to run [Appointments-Express](https://github.com/joshADE/appointments-express-frontend)


# Technologies used
* [.NET Core(Web API)](https://dotnet.microsoft.com/apps/aspnet/apis) (REST API)
* [PostgreSQL](https://www.postgresql.org/) (Open Source Relational Database)
* [Cloudinary](https://cloudinary.com/) (Cloud based image and video management service)

# Requirements

* GIT (version 2.x and above)
* Visual Studio (version 2019 and above) (configured with the .NET package, community edition is fine) (go [here](https://visualstudio.microsoft.com/vs/features/net-development/) to install visual studio with .NET)
* PostgreSQL (latest version would be fine) (click [here](https://www.postgresqltutorial.com/install-postgresql/) for installation instructions)
* Cloudinary Account (with the cloud name, api key, and api secret) 
* Gmail Account (You will need a gmail account just for sending emails from this application. I recommend that you create a new one. The account will need to be configured a bit to send emails from an application)

# Getting Started

## Clone Repository

Clone the repository to your computer.

```
git clone https://github.com/joshADE/Appointments-Express-Backend.git
```

## Installation

1. Open the project directory.
2. Open the .sln (solution) file with Visual Studio to restore the necessary NuGet packages required for the project

## Setup PostgreSQL Database

1. Once the project is open in Visual Studio and you have PostgreSQL installed, open the file appsettings.json.
2. In appsettings.json, change the credentials of the database for the DefaultConnection ConnectionStrings to you credentials for pgAdmin for PostgreSQL. 

Example:
```
    "ConnectionStrings": {
      "Dev": "Server=DESKTOP-LJG3CL3; Database=AppointmentsExpressDB; Trusted_Connection=True; MultipleActiveResultSets=true",
      "Prod": "Server=(localdb)\\MSSQLLocalDB; Database=AppointmentsExpressDB; Trusted_Connection=True; MultipleActiveResultSets=true",
      "DefaultConnection": "Server=localhost;Port=5432;User Id=(your postgresql user id here);Password=(your postgresql user password here);Database=AppointmentsExpressDB;sslmode=Prefer;Trust Server Certificate=true"
    },
```

3. Open the NuGet package manager console for Visual Studio by navigating to Tools > NuGet Package Manager > Package Manager Console or by pressing (Alt + =) on the keyboard.
4. Run the command in the package manager console `` UPDATE-DATABASE ``. This command will setup the PostgreSQL Database for you and run the necessary migrations.

## Setup Cloudinary

1. Create an account with cloudinary using the link above.
2. Once you have an account, navigate to the dashboard page.
3. At the top of the dashboard page you will see the cloud details.
4. You need to copy over the cloud name, api key, and api secret into the appsettings.json file in the project. 

Example:
```
  "AccountSettings": {
    "CloudName": "(your cloud name here)",
    "ApiKey": "(your api key here)",
    "ApiSecret": "(your api secret here)",
    "ApiBaseAddress": "https://api.cloudinary.com"
  }
```

## Setup GMail Email Sending Account
1. Create an new email account with google.
2. Navigate to the security account settings for the google account. 
3. Either (1) create an app password to use for this app (requires the 2-step verification to be turned on) or (2) turn on Less secure app access. You can follow the instructions of the answer on this stackoverflow thread https://stackoverflow.com/questions/32260/sending-email-in-net-through-gmail?answertab=oldest#tab-top . Just scroll down to the part where it says 'Additionally go to the Google Account > Security page' and start following those instructions except copy the app password into the appsettings.json file as below.
4. If you chose to use an app password (which is the option I decided on) you will have to copy the account email and account app password to the appsettings.json file in the project.
5. If you chose to enable Less secure app access you will have to copy the account email and account password to the appsettings.json file.

Example:
```
  "Email": {
    "Smtp": {
      "Name": "Appointments Express",
      "Host": "smtp.gmail.com",
      "Port": 587,
      "Username": "(your gmail email here)",
      "Password": "(your app password here or the account password)"
    }
  }
```

## Running project locally

1. Build the project by navigating to Build > Build Solution and make sure that there are no errors.
2. Press the green play button (IIS Express)

Open up https://localhost:44371/api/stores in a browser to see the app. (There should be no stores yet in the database, but you can connect the app to the front-end linked above). You can also visit https://localhost:44371/swagger to see list of all the endpoints and interact with them. (Note that you will not be allowed to interact with most of the endpoints as they require an authentication token header added to the request and swagger UI doesn't allow this).

# Deploying to Heroku

(If the above instructions don't work for you or you don't want to install the necessary tools that are required, you can deploy your own copy to heroku)

1. Make sure you know how to create a heroku app and deploy the files from your local machine over to the app. There are several videos on how to do this online, and I don't want to list the instruction here.
2. Heroku doesn't officially support .NET applications, but there is a buildpack that allows you to get the applications running on Heroku. The buildpack we will be using is https://github.com/jincod/dotnetcore-buildpack.
3. When you create a heroku app navigate to the settings tab and scroll to the section on buildpacks and add a new buildpack, setting the url of the buildpack to `` https://github.com/jincod/dotnetcore-buildpack ``.
4. While you are on the settings tab, scroll to the config vars section and add a config variable with the key of `` ASPNETCORE_ENVIRONMENT `` and a value of `` Production ``.
5. Also add the following config vars from your cloudinary account: `` ApiKey `` set to your api key, `` ApiSecret `` set to your api secret, and `` CloudName `` set to the cloudinary cloud name.
6. Follow the instruction above in the - Setup GMail Email Sending Account - section of this README to create a gmail account with either an app password or the Less secure app access setting turned on (you don't have to copy the account details into the appsettings.json file so you can ignore steps (4) and (5) of those instructions)
7. Create the following config vars from your gmail account: `` SmtpHost `` set to `` smtp.gmail.com ``, `` SmtpPort `` set to `` 587 ``, `` SmtpName `` set to `` Appointments Express `` (or any name you want to appear on the from field in emails), `` SmtpUsername `` set to your google email, and `` SmtpPassword `` set to your google email password or app password.
8. Next is to add the heroku postgresql addon to the app. Navigate to the resources tab and in the add-ons section, search for the Heroku Postgres addon and add it to your app (it should be free).
9. Once you add the Postgres add-on to the app it should add another config variable called DATABASE_URL, which is used to get the connection string for the postgresql database.
10. I've already setup the necessary code to automatically detect that the app is in production and create the connection string to connect to the Postgres database on Heroku and also run the necessary migrations on the database to keep it up to date.
11. You can now deploy the app as you would a normal heroku app by following the instructions on the deploy tab in the heroku dashboard.

# Diagrams
![Database diagram](Screenshots/DBDiagram.png)

