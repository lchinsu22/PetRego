# PetRego
.NET Rest Api for PetRego service that maintains and provide a list of owners, their pets and attributes

GitHub Url - https://github.com/lchinsu22/PetRego


Help Page :

	Run the application in the server, which automatically opens the Help Page - http://<HostName>:52516/Help.
	The Page provides the information on Request and Response format of all the Controller methods.

Application peoperties:

    .Net Framework - 4.5.2
    Port Number - 52516
    Url - http://localhost:52516/

Dependency Frameworks:

	Entity Framework version 6.0.0
	Unity COntainer version 5.11.1.0
	WebAPi Core version 5.2.3

Database Configuration:

	The database configuration is configured as connectionstring with connectionstring Name - "PetRegoContext"
	Database used - Microsoft Sql Server. 
	
	Changes required in database configuration and add respective database provider in Web.Config.
