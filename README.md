# Weather Test

4Com technical test for prospective .NET developers.

## Prerequisites

* [Visual Studio 2017](https://www.microsoft.com/net/core#windowsvs2017) with ".NET Core cross-platform development" workload installed
* [Git](https://git-scm.com/downloads)

## Brief

Create an application or website which given a location, will query the APIs provided in this repo and then display an aggregated weather result.

We find this challenge normally takes a few hours to complete, however you may spend as much time as you would like to produce a result you are happy with and believe meets all the requirements.
Your code should be written in a way that shows you have a good understanding of common software design patterns and SOLID principles.
Should there be any aspects, which given more time you would have refactored, please add a README, or comments explaining what you would like to have done.

You should submit a link to your repository (GitHub, GitLab, BitBucket, etc), rather than a compiled application / link to the deployed website.

If you believe the brief is unclear in anyway or you have any questions please ask. 

### Requirements

Your application/website should:

* Display the aggregated (average) result, showing both temperature and wind speed, from any APIs it has queried
* Allow the user to choose which measurement unit they want results displayed in. Wind should be MPH or KPH and temperature should be Celsius or Fahrenheit
* Allow for more APIs to be easily added in the future
* Allow for other units of measure to be easily added in the future
* Gracefully handle one or more of the APIs being down or slow to respond

It should pass the following tests:

*	Given temperatures of 10c from bbc and 68f from accuweather when searching then display either 15c or 59f depending on what the user has chosen
*	Given wind speeds of 8kph from bbc and 10mph from accuweather when searching then display either 12kph or 7.5mph depending on what the user has chosen