UniFirst Corporation
Software Developer Coding Assessment
Overview
Using SOLID principles, create the business layer of a vehicle transfer application that adheres to the
requirements below. Optionally, you may stub out user interface and data layers to show how the layers would
interact with each other.
The goal of this test is become familiar with how you solve business problems within an application, not for you
to build a functional application from scratch. We do not expect you to spend more than 1 or 2 hours on it. The
application does not need to be functional, but it should be testable. You may optionally include a few unit tests
to show how you would test it.
Requirements:
 A vehicle has a make, model, year, and VIN.
 A year must be four numeric characters.
 Vin must be 24 alphanumeric characters with a minimum of 8 alphas, ending with 5 numeric.
 There are three types of vehicles a truck, van, and semi.
 Vehicles have four status types, stand-by, in transit, in service, and in repair.
 A location can be a distribution center or a branch.
 A distribution center has branches and vehicles.
 A branch has vehicles, but doesn’t have too.
 A truck or van can be transferred between branches.
 A semi can be transferred between distribution centers, but not to branches.
 Only vehicles in stand-by can be transferred.
Using SOLID principles, create an app that supports the above requirements.
Architecture:
Your application can feature any architectural pattern and use any technology stack that you wish. It should be
as simple or as complex as you feel is necessary to meet the requirements.
Extensibility and Testability are key ingredients in good software development, so be prepared to extend your
application as part of our on-site interview process.
Happy Coding!