PerformanceDsl
==============

This README is lacking a lot of info however it should be enough to get the intrepid going.

TGPs experiments in C# performance testing DSL


1. Set up both websites in a local IIS. 


MvcTestApp
Needs .Net 4.5 and MVC 4 installing.
Set it up in IIS with hostname www.testmvcapp.dev

WebformsTestApp
Needs .Net 4.5 and MVC 4 installing.
Set it up in IIS with hostname www.testwebformsapp.dev
You will need SQL Server installed locally, a user called TestUser with the password "password" and they need to be a sysadmin.

TestResultsStore
There is now a WebAPI to store the test results from the tests

This is .NET4.5 IIS, just needs setting up.
I talks to an MS SQL database called TestResultsStoreDb and there is a database project for this in the main solution.
The web config needs to point at this.


Agent
There is now an agent service, install using installutil, post json at it to run tests.
Example can be seen in the Agent project. 

TestRunner
The test runner is a console app of the Agent but the json object is hardcoded in the main method.