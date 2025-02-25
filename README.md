# Gourmet Shop Application
## Summary
Create the customer WinForms appl, and update the DataAccess library. 

## Setup
To create the database, run the script `GourmetShop.sql` under the `SQL` folder. 

Place your connection string in `Config/ConnectionString.txt'. In Visual Studio, under `Git->Open in command prompt`, type in `git update-index --skip-worktree "./Gourmet Shop/Config/ConnectionString.txt"`, `git update-index --skip-worktree "./Gourmet Shop/GourmetShop.WebApp/appsettings.json"` and `git update-index --skip-worktree "./Gourmet Shop/GourmetShop.WebApp/appsettings.Development.json"`. The files remains in the repository and GitHub, but your local changes are ignored. Place your connection string in that file. Set the startup project as `GourmetShop.WebApp`.

## Assignment Instructions
