# The backend of Nutriscan

### To run:

`docker compose -f config/docker/docker-compose.yml up -d` <br>
`dotnet run --project src/Adria.Main` <br>
Visit `http://localhost:8000/swagger/index.html`

Remove the example server container if you get any errors. <br>
Database credentials are in Discord under "Database credentials"

### To test:

`dotnet test`