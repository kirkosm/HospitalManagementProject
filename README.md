# Hospital Management Project

## Overview

This project is a C# solution for managing patients in a hospital environment. It consists of two separate applications that work together to simulate patient transport, triage, and hospital management.

### Projects

1. **PatientTransport**

   * Role: Used by the stretcher carrier to quickly enter a patient's situation and triage information.
   * Functionality:

     * Connects to the hospital database.
     * Allows the carrier to input a quick description of the patient.
     * Sends the patient data to the hospital system for processing.

2. **HospitalClient**

   * Role: Used by hospital staff to manage incoming patients and perform examinations.
   * Functionality:

     * Displays patients who have just arrived.
     * Admits patients into the hospital system.
     * Tracks medical examinations and maintains patient history.
     * Works with the same database as PatientTransport to ensure real-time updates.

## Technologies Used

* Programming Languages: C#, SQL
* Frameworks: .NET WinForms
* Database: Somee
* Tools: Visual Studio, Git, GitHub

## Solution Structure

```
HospitalManagementProject/     <- Root folder / GitHub repository
│
├─ HospitalManagementProject.sln  <- Visual Studio solution file
├─ .gitignore
├─ PatientTransport/                 <- Stretcher carrier app project
│   └─ All project files (except bin/obj)
└─ HospitalClient/             <- Hospital management app project
    └─ All project files (except bin/obj)
```

## How to Run

1. Open `HospitalManagementProject.sln` in Visual Studio.
2. To run **PatientTransport**:

   * Select `PatientTransport` as the startup project.
   * Press F5 to run.
3. To run **HospitalClient**:

   * Select `HospitalClient` as the startup project.
   * Press F5 to run.
4. Optionally, you can configure **Multiple Startup Projects** in the solution properties to run both apps simultaneously (for testing database integration).

## Usage

* **PatientTransport**: Stretcher carriers enter basic patient data and triage, then send it to the hospital.
* **HospitalClient**: Hospital staff admit patients, view their incoming status, perform examinations, and update medical history.

## GitHub Repository

* This repository contains both applications in one solution for easier management and sharing.
* Each project can run independently or together within the same solution.
