# Kantar Assessment - Setup Instructions

Follow these steps to get the application running locally using Docker.

## Pre requisites

- [Git](https://git-scm.com/downloads) installed
- [Docker](https://www.docker.com/get-started) installed and running
- Ensure ports 3306, 8080, and 8081 are available and not blocked by other processes

## Steps

1. Create a folder named `kantar-assessment` on your machine.

2. Open a command prompt inside the `kantar-assessment` folder.

3. Clone the project repository:

   ```bash
   git clone https://github.com/alessandro-feliz/kantar-assessment.git

4. Change into the project directory:

   ```bash
   cd kantar-assessment

5. Build and start the Docker containers:

   ```bash
   docker-compose up --build

6. Once the containers are running, access the following:

- Web Application: http://localhost:8081/
- Web API Swagger UI: http://localhost:8080/swagger

##  Stopping the containers
To stop the running containers, press Ctrl + C in the terminal and run:

   ```bash
   docker-compose down
