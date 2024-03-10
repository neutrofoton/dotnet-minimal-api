- Swagger URL : http://localhost:5193/swagger/index.html

# Adding Security LocalUser
1. Add class <code>LocalUser</code>
2. Add migration
    ```bash
    cd src/Lab.MinilaApi

    # generate local user database code creation
    dotnet ef migrations add Security-LocalUser

    # execute generated database migration/update
    dotnet ef database update
    ```

# References
- https://aka.ms/aspnetcore/swashbuckle