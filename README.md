# Platform API

.NET Core API is part of insurance product and responsible for 3rd party integrations and serve web platform requests.

## Deployment

Recommended IDE Jet Brains Rider or Visual studio for c# projects  

- Clone or download project to your machine
- Create Dabase (we recomend MySQL) and deploy scripts from */Aigang.Platform.API/DBScripts*
- Update configuration file in destination */Aigang.Platform.API* remove **DRAFT** prefix:
  - *appsettings.json* - local development configuration  
  - *appsettings.qa.json* - qa environment configuration  
  - *appsettings.production.json* - production environment configuration  

- Review configuration and update with your values  

Build project:  

- click build and run button in IDE


## Contributing

Everyone is welcome to contribute this repository, just make sure your code does not have validation issues and builds without errors. Create your feature branch and send us pull request to master branch.

