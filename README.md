# Acme Activity Signup
## Frontend
- SPA built in Angular 10

#### Testing
- Unit tests focused on SignUpDialogComponent and can be found in ```src/app/_shared/dialogs/sign-up-dialog/sign-up-dialog.component.spec.ts```
- The rest of the spec.ts files have been commented out (mostly to avoid having to go through and set up all the missing providers in order to cleanly run the small subset of tests)

## Backend
- .NET Core 3.1 Web API
- runs on default ports 5000/5001
- Swagger UI available at http://localhost:5001/api/docs
- Configured to use a MySQL database
-- Connection string goes in ```appsettings.json```
-- Run migrations after placing connection string in ```ApplicationDbContext.cs``` line 27
- To seed activities, POST the following body to http://localhost:5001/api/activities
```[
  {
    "title": "Seagull Chasing",
    "description": "Get in some good cardio, and have some fun with friends while pointlessly chasing birds around",
    "imageUrl": "https://upload.wikimedia.org/wikipedia/commons/9/9a/Gull_portrait_ca_usa.jpg",
    "date": "2020-08-15"
  },
{
    "title": "Movie Night",
    "description": "Sit back and relax with friends while watching everyone's favourite star wars episode",
    "imageUrl": "https://gottalovethemmovies.files.wordpress.com/2015/07/img_59811.png",
    "date": "2020-08-17"
  },
{
    "title": "Video Game Night",
    "description": "Sit back and relax with friends while playing some totally non-violent and safe for work video game titles such as Doom, or Doom Eternal",
    "imageUrl": "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8c/Doom_%E2%80%93_Game%E2%80%99s_logo.svg/500px-Doom_%E2%80%93_Game%E2%80%99s_logo.svg.png",
    "date": "2020-08-20"
  },
{
    "title": "Work Day",
    "description": "Have the most fun you've ever had in one day. Activities include: sitting at your desk, completing tasks, being productive",
    "imageUrl": "https://previews.123rf.com/images/vgstudio/vgstudio1704/vgstudio170400386/76265956-i-have-done-it-very-happy-businessman-in-black-suit-working-with-laptop-computer-at-office-success-i.jpg",
    "date": "2020-08-21"
  }
]
```

#### Testing
- Unit tests can be found in the AcmeActivitySignupTests project
- Uses InMemory database for test data store


