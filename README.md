PerformanceDsl
==============

This README is lacking a lot of info however it should be enough to get the intrepid going.

TGPs experiments in C# performance testing DSL


How to
======

1. Set up both websites in a local IIS. 
2. Set up reporting site
3. install agent service on a windows machine
4. post json test config objects at the service
5. see the results

of course you need to write the tests :)

Performance Dsl
===============
Dsl to describe performance tests pretty easy to use needs work to add PUT/DELETE.

PerformanceTests
================
Decorate Test Methods with [PerformanceTest] attribute in order for Agent to understand they are tests

MvcTestApp
==========

Needs .Net 4.5 and MVC 4 installing.
Set it up in IIS with hostname www.testmvcapp.dev

WebformsTestApp
===============

Needs .Net 4.5 and MVC 4 installing.
Set it up in IIS with hostname www.testwebformsapp.dev
You will need SQL Server installed locally, a user called TestUser with the password "password" and they need to be a sysadmin.

TestResultsStore
================

There is now a WebAPI to store the test results from the tests

This is .NET4.5 IIS, just needs setting up.
I talks to an MS SQL database called TestResultsStoreDb and there is a database project for this in the main solution.
The web config needs to point at this.

Agent
=====

There is now an agent service, install using installutil, post json at it to run tests.
Example can be seen in the Agent project. 


Server
======
Console app to kick off tests on agents, this isnt finished and will probably read json from somewhere as a config or I'll let you pass json into it. This json will describe a Test Suite

The idea is you set up a Test Suite that contains Tests that contain Test Runs and an agent hostname/ip. These Test Runs are then posted at an agent. The agent then runs the Test Configurations on the Test Run. Test Runs also hold a Guid which is used to identify the test run (for reporting purposes) and the path to the DLL on the agent machine that contains the methods tagged with the [PerformanceTest] attribute that are relevant to the Test Runs Test Configurations. At the moment a TestRun can only point to one DLL, I might change this going forward. 

A Test Run can contain multiple Test Configurations that is instructions to the agent how to run a method that has been decorated with the Performance Test Attribute. Test Configurations are executed in Parallel. So I could describe two Test Configurations (one calls TestBBCHomePageGet and the other calls TestITVHomePageGet) and they are executed by the agent at the same time.

In the one Test you can have multiple Test Runs
