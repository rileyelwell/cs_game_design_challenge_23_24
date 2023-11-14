# Software Development Process (SDP)

## Contents

- Principles
- Process
- Roles
- Tooling
- Definition of Done (DoD)
- Release Cycle
- Environments

## Principles

- We are to answer questions on our discord within 24 hours
- We are to show up on time to meetings
- We will use GitHub Project and Issues
- We will create issues in the project backlog
- Each field in an Issue should be filled out as completely as possible
- We are to update the status of our issues as we deal with them
- All changes to the project are made in their respective branches
- Our pull requests need to be appropriately linked with its respective issues
- Do not allow force-pushing on branches
- Non-Milestone Issues must be achievable in a single week
- Milestone Issues will get their own iteration

## Process

We will…

- Be using a method very much like Scrum.
- Keep a large project backlog that contains everything we can think of to complete our projects.
- Assign items from our project backlog to our weekly iteration backlogs a week before the iteration begins.
- Work on the things in our current iteration during the week and update their status as we work on them. We will continue this process until we have completed our project backlog.
- We will meet with the TA every Monday at 2:15 to discuss our progress and ask for advice.
- Meet as a group every Wednesday at 2:15 to discuss our progress and do group assignments that require better communication.

## Roles

In our group, we will have the following roles:

- Primary Artist
  - Riley
- Primary Audio Designer
  - Connor
- Primary UI / UI Designer
  - Ethan
- General Physics Developers
  - Split Among Everyone Case-to-Case
- AI Driving Developers
  - Split Among Everyone Case-to-Case
- Internal Vehicle Physics Developers
  - Split Among Everyone Case-to-Case
- ADAS Simulation Developers
  - Split Among Everyone Case-to-Case

## Tooling

**Version Control**\
GitHub and Unreal Source Control\

**Project Management**\
GitHub Issues and Projects\

**Documentation**\
Google Drive\

**Test Framework**\
Unreal Engine 5\

**Linting and Formatting**\
Visual Studio Code Extension\

**CI/CD**\
GitHub Actions\

**IDE**\
Visual Studio Code\

**Graphic Design**\
Google Slides\

**Others**\
Blender

## Definition of Done (DoD)

- All group members approve of the issues' completeness
- The associated feature branch is merged with the working branch
- The issue has its todo items complete and has been thoroughly tested
- The progress report has been updated to reflect the completion

## Release Cycle

Deploy the working branch to an appropriate version branch and then to the release branch.\
Our versioning schematic (Major.minor.patch) goes as follows:

- Increment Major when the game has an entirely new feel
- Increment minor when a new feature is added, reset when Major increments
- Increment patch when a bug has been fixed, reset when minor increments or resets

Version 0 is our testable project\
Version 1 is our minimal viable product\
Version 2 is our completed stretch goal project

## Environments

| **Environment** | **Infrastructure** | **Deployment** | **What is it for?** | **Monitoring** |
|----|---|---|---|----|
| Production | AWS through CI/CD | Release | Sleeping well at night | Prometheus, Grafana, Sentry |
| Staging (Test) | Render through CI/CD | PR | New unreleased features and integration tests | Sentry |
| Dev | Local (macOS and Windows) | Commit | Development and unit tests | N/A |
