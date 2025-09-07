# Campus Event Management System

## About the Project

I made a Campus Event Management system. In this, colleges can create events like workshops, hackathons, tech talks, and fests. Students can see the events, register for them, and mark their attendance.

## Features

* Login for students and colleges
* Colleges can create events and see reports
* Students can browse events and register
* Track attendance
* Students can give feedback
* Reports for event popularity and student participation
* Top 3 most active students report

## Tech Used

* Backend: .NET 8 Web API
* Database: SQLite

## How to Run

1. Open the backend project in Visual Studio
2. In Package Manager Console, run:

   ```
   add-migration InitialCreate
   update-database```
3. Run the backend project. It will run at `https://localhost:7161`

## Most Challenging part

* To think what are Endpoints required and how to Achive it
* Testing the Api takes more time and it is more annoying
* in my project, if a student has registerd for that event and then attended only he can give feedback to that particular Event

## Good Things 

* I enjoyes it a lot and i learnt a lot from this

* i have never use sqlite, i know MSSMS, i learnt how to use sqlite

* Finally i made it
