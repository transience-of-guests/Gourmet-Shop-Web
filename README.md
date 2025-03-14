# Gourmet Shop Application
## Summary
Create the customer WinForms appl, and update the DataAccess library. 

## Setup
Place your connection string in `Config/ConnectionString.txt'`. In Visual Studio, under `Git->Open in command prompt`, type in `git update-index --skip-worktree "./Gourmet Shop/Config/ConnectionString.txt"`, `git update-index --skip-worktree "./Gourmet Shop/GourmetShop.WebApp/appsettings.json"` and `git update-index --skip-worktree "./Gourmet Shop/GourmetShop.WebApp/appsettings.Development.json"`. The files remains in the repository and GitHub, but your local changes are ignored. Under `Package Manager Console`, set the default project to `GourmetShop.DataAccess` and set the startup project as `GourmetShop.DataAccess`. Input the command `Update-Database` to apply the existing migrations. Afterwards, set the startup project as `GourmetShop.WebApp` or `Admin.WebApp` depending on what side you want to run.

## Assignment Instructions
